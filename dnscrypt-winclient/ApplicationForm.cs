using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;


namespace dnscrypt_winclient
{
	public partial class ApplicationForm : Form
	{
		public const int SW_SHOWMINIMIZED = 2;
		public const int SW_HIDE = 0;
		public const UInt32 BCM_SETSHIELD = 0x160C;

		private ProcessStartInfo CryptProc = null;
		private Process CryptHandle = null;
		private ServiceController CryptService = null;
		private bool CryptProcRunning = false;
		private bool ServiceInstalled = false;
		private Thread ServiceCheck = null;
		private int LastPluginTooltipIndex = -1;
		private string registryRoot = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\dnscrypt-proxy\\Parameters\\";

		//A map of names to list entries so that we can rebuild the listbox and remember which items were checked
		private Dictionary<String, Pair<NetworkListItem, bool>> NICitems = new Dictionary<string, Pair<NetworkListItem, bool>>();

		private BindingList<DNSCryptProvider> Providers = new BindingList<DNSCryptProvider>();
		private List<PluginListItem> Plugins = new List<PluginListItem>();

		[DllImport("user32.dll")]
		public static extern UInt32 SendMessage (IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

		//
		// Function pointers for plugins
		//
		//Short description
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr dcplugin_description(IntPtr dcPlugin);

		//Long description
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr dcplugin_long_description(IntPtr dcPlugin);


		public ApplicationForm()
		{
			InitializeComponent();
			SendMessage(this.buttonInstall.Handle, BCM_SETSHIELD, 0, 1);

			//Fetch the NIC information once
			NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in adapters)
			{
				//Skip over adapters that aren't connected to a network
				if (adapter.OperationalStatus != OperationalStatus.Up)
				{
					continue;
				}

				IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
				List<string> dnsServers = new List<string>();	//More accurate than IPInterfaceProperties.DnsAddresses
				List<string> dnsServersv6 = new List<string>();

				//Determine if the DNS addresses are obtained automatically or specified
				//We don't want to add them when the program is closed if they weren't actually set
				//Unfortunately, neither WMI nor the NetworkInterface class provide information about this setting
				dnsServers = NetworkManager.getDNS(adapter.Id);
				dnsServersv6 = NetworkManager.getDNSv6(adapter.Id);

				NetworkListItem item = new NetworkListItem(adapter.Name, adapter.Description, dnsServers, dnsServersv6, adapter.Supports(NetworkInterfaceComponent.IPv4), adapter.Supports(NetworkInterfaceComponent.IPv6));
				NICitems.Add(adapter.Name, new Pair<NetworkListItem, bool>(item, false));
			}

			GetNICs(false);

			// Start thread to monitor process/service status
			this.ServiceCheck = new Thread(new ThreadStart(this.update_Service_Info));
			this.ServiceCheck.Start();

			LoadPlugins();

			this.combobox_provider.DataSource = Providers;
			this.combobox_provider.DisplayMember = "displayName";
			LoadConfig();
		}

		/// <summary>
		/// Searches for and lists available plugins in the \plugins directory
		/// </summary>
		private void LoadPlugins()
		{
			this.Plugins.Clear();
			this.pluginListBox.Items.Clear();

			string pluginDirectory = Directory.GetCurrentDirectory() + "\\plugins";
			if (Directory.Exists(pluginDirectory))
			{
				string[] fileEntries = Directory.GetFiles(pluginDirectory);
				foreach (string fileName in fileEntries)
				{
					IntPtr pDll = NativeMethods.LoadLibrary(fileName);
					IntPtr descFunc = NativeMethods.GetProcAddress(pDll, "dcplugin_description");
					IntPtr longDescFunc = NativeMethods.GetProcAddress(pDll, "dcplugin_long_description");
					string short_description = "";
					string long_description = "";

					if (descFunc != IntPtr.Zero)
					{
						dcplugin_description dcplugin_description = (dcplugin_description)Marshal.GetDelegateForFunctionPointer(descFunc, typeof(dcplugin_description));

						IntPtr strPtr = dcplugin_description(IntPtr.Zero);
						short_description = Marshal.PtrToStringAnsi(strPtr);
					}

					if (longDescFunc != IntPtr.Zero)
					{
						dcplugin_long_description dcplugin_long_description = (dcplugin_long_description)Marshal.GetDelegateForFunctionPointer(longDescFunc, typeof(dcplugin_long_description));

						IntPtr strPtr = dcplugin_long_description(IntPtr.Zero);
						long_description = Marshal.PtrToStringAnsi(strPtr);
					}

					if (descFunc != IntPtr.Zero || longDescFunc != IntPtr.Zero)
					{
						this.Plugins.Add(new PluginListItem(Path.GetFileName(fileName), short_description, long_description));
						this.pluginListBox.Items.Add(Path.GetFileName(fileName));
					}
				}
			}
		}

		/// <summary>
		/// Reads the application settings file and populates the Provider list
		/// </summary>
		private void LoadConfig()
		{
			try
			{
				// Make sure the file exists before trying to read it
				if (!File.Exists(Directory.GetCurrentDirectory() + "\\dnscrypt-resolvers.csv"))
				{
					MessageBox.Show("dnscrypt-resolvers.csv was not found. It should be placed in the same directory as this program. If you do not have this file, you can find it in the download packages at http://download.dnscrypt.org/dnscrypt-proxy/", "File not found");
					return;
				}

				CSVReader reader = new CSVReader("dnscrypt-resolvers.csv", true);
				foreach (CSVRow CSVprovider in reader)
				{
					DNSCryptProvider provider = new DNSCryptProvider();
					provider.name = CSVprovider["Name"];
					provider.displayName = CSVprovider["Full name"];
					provider.description = CSVprovider["Description"];
					provider.location = CSVprovider["Location"];
					provider.server = CSVprovider["Resolver address"];
					provider.fqdn = CSVprovider["Provider name"];
					provider.key = CSVprovider["Provider public key"];

					this.Providers.Add(provider);
				}
			}
			catch (Exception err)
			{
				MessageBox.Show("There was an error parsing dnscrypt-resolvers.csv: " + err.ToString());
			}

			// Load the last used resolver
			string resolver = (string)Registry.GetValue(this.registryRoot, "ResolverName", "");
			int resolverIndex = -1;
			if (resolver != null && resolver.Length > 0)
			{
				resolverIndex = this.combobox_provider.FindString(resolver);
			}

			if (resolverIndex != -1)
			{
				populate_Config_Form(resolverIndex);
			}
			else
			{
				populate_Config_Form(0);
			}
		}

		/// <summary>
		/// Fills the ListBox with information about each NIC
		/// </summary>
		private void GetNICs(Boolean showHidden)
		{
			//this.ipv6Radio.Enabled = false;

			//NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			foreach (KeyValuePair<String, Pair<NetworkListItem, bool>> adapterEntry in this.NICitems)
			{
				if (!adapterEntry.Value.First.hidden || showHidden)
				{
					int idx = DNSlistbox.Items.Add(adapterEntry.Value.First, adapterEntry.Value.Second);
					
					//See if the device supports IPv6 and enable the option if so
					if (adapterEntry.Value.First.IPv6)
					{
						//this.ipv6Radio.Enabled = true;
					}
				}
			}
		}

		/// <summary>
		/// This callback is triggered when a checkbox's state changes in the NIC list box
		/// When the box is checked, the target NIC's DNS settings are changed to 127.0.0.1
		/// When the box is unchecked, the settings are restored to their original value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DNSlisbox_itemcheck_statechanged(object sender, ItemCheckEventArgs e)
		{
			//Checking the "Show hidden adapters" box triggers this event, so ignore it
			if (((CheckedListBox)sender).SelectedIndex == -1)
			{
				return;
			}

			try
			{
				//Restore the user's old settings
				if (e.CurrentValue == CheckState.Checked)
				{
					if (((NetworkListItem)DNSlistbox.Items[e.Index]).IPv4)
					{
						NetworkManager.setDNS(((NetworkListItem)DNSlistbox.Items[e.Index]).NICDescription, ((NetworkListItem)DNSlistbox.Items[e.Index]).DNSservers);
					}

					//Since we don't set an IPv6 server, don't run this if they didn't have any to begin with
					if (((NetworkListItem)DNSlistbox.Items[e.Index]).DNSserversv6.Count > 0)
					{
						NetworkManager.setDNSv6(((NetworkListItem)DNSlistbox.Items[e.Index]).NICName, ((NetworkListItem)DNSlistbox.Items[e.Index]).DNSserversv6);
					}

					//Mark that the box was unchecked so we can restore the state if we have to rebuild the list to show hidden adapters
					this.NICitems[((NetworkListItem)DNSlistbox.Items[e.Index]).NICName].Second = false;
				}
				else //Set it to loopback for DNSCrypt
				{
					if (((NetworkListItem)DNSlistbox.Items[e.Index]).IPv4)
					{
						NetworkManager.setDNS(((NetworkListItem)DNSlistbox.Items[e.Index]).NICDescription, "127.0.0.1");
					}

					//Only do this if there were custom servers set to begin with
					if (((NetworkListItem)DNSlistbox.Items[e.Index]).DNSserversv6.Count > 0)
					{
						List<string> ip = new List<string>();
						//ip.Add("::1");
						NetworkManager.setDNSv6(((NetworkListItem)DNSlistbox.Items[e.Index]).NICName, ip);
					}

					//Mark that the box was checked so we can restore the state if we have to rebuild the list to show hidden adapters
					this.NICitems[((NetworkListItem)DNSlistbox.Items[e.Index]).NICName].Second = true;
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("There was an error changing the device's DNS server: " + exception.Message);
			}
		}

		/// <summary>
		/// Button to start and stop the DNSCrypt Proxy application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void startstop_button_Click(object sender, EventArgs e)
		{
			// Start the DNSCrypt process
			if (!this.CryptProcRunning)
			{
				if (this.CryptService == null)
				{
					// Make sure the file exists before trying to launch it
					if (!File.Exists(Directory.GetCurrentDirectory() + "\\dnscrypt-proxy.exe"))
					{
						MessageBox.Show("dnscrypt-proxy.exe was not found. It should be placed in the same directory as this program. If you do not have this file, you can download it from http://download.dnscrypt.org/dnscrypt-proxy/", "File not found");
						return;
					}

					this.CryptProc = new ProcessStartInfo();
					this.CryptProc.FileName = "dnscrypt-proxy.exe";
					this.CryptProc.WindowStyle = ProcessWindowStyle.Minimized;
					this.CryptProc.Arguments += " --resolver-address=" + this.textbox_server_addr.Text;

					if (this.gatewayCheckbox.Checked)
					{
						this.CryptProc.Arguments += " --local-address=0.0.0.0";
					}

					this.CryptProc.Arguments += " --provider-key=" + this.textbox_key.Text;
					this.CryptProc.Arguments += " --provider-name=" + this.textbox_fqdn.Text;

					this.CryptHandle = Process.Start(this.CryptProc);
				}
				else
				{
					bool success = this.updateRegistry();
					if (!success)
					{
						return;
					}

					if (this.CryptService.Status != ServiceControllerStatus.Running && this.CryptService.Status != ServiceControllerStatus.StartPending)
					{
						this.CryptService.Start();
					}
				}

				this.startstop_button.Text = "Stop";
				this.CryptProcRunning = true;

				//Don't allow plugin changes
				this.pluginListBox.Enabled = false;
				this.button_plugins_refresh.Enabled = false;
				this.textbox_plugin_params.Enabled = false;
			}
			else
			{
				if (this.CryptHandle != null)
				{
					// Make sure the proxy wasn't terminated by another application/user
					if (!this.CryptHandle.HasExited)
					{
						this.CryptHandle.Kill();
					}
				}
				else if (this.CryptService != null)
				{
					if (this.CryptService.Status != ServiceControllerStatus.Stopped && this.CryptService.Status != ServiceControllerStatus.StopPending)
					{
						this.CryptService.Stop();
					}
				}

				this.CryptProc = null;

				this.startstop_button.Text = "Start";
				this.CryptProcRunning = false;

				//Allow plugin changes
				this.pluginListBox.Enabled = true;
				this.button_plugins_refresh.Enabled = true;
				this.textbox_plugin_params.Enabled = true;
			}
		}

		private bool updateRegistry()
		{

			try
			{
				Registry.SetValue(this.registryRoot, "Plugins", new string[] {}, RegistryValueKind.MultiString);
				Registry.SetValue(this.registryRoot, "LocalAddress", "127.0.0.1", RegistryValueKind.String);
				Registry.SetValue(this.registryRoot, "ProviderKey", this.textbox_key.Text, RegistryValueKind.String);
				Registry.SetValue(this.registryRoot, "ProviderName", this.textbox_fqdn.Text, RegistryValueKind.String);
				Registry.SetValue(this.registryRoot, "ResolverAddress", this.textbox_server_addr.Text, RegistryValueKind.String);
				Registry.SetValue(this.registryRoot, "ResolverName", this.Providers[this.combobox_provider.SelectedIndex].name, RegistryValueKind.String);
				Registry.SetValue(this.registryRoot, "ResolverList", Directory.GetCurrentDirectory() + "\\dnscrypt-resolvers.csv", RegistryValueKind.String);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Called right before the form closes.
		/// Resets all DNS information back to their previous values and stops the DNSCrypt Proxy application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void form_closing(object sender, FormClosingEventArgs e)
		{
			//Do not revert settings if installed as a service and it is running
			if (this.ServiceInstalled && this.CryptProcRunning)
			{
				return;
			}

			//Revert all of the DNS server settings
			foreach (NetworkListItem item in this.DNSlistbox.CheckedItems)
			{
				try
				{
					NetworkManager.setDNS(item.NICDescription, item.DNSservers);

					if (item.DNSserversv6.Count > 0)
					{
						NetworkManager.setDNSv6(item.NICName, item.DNSserversv6);
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show("There was an error reverting your DNS settings: " + exception.Message);
				}
			}

			//Kill the DNSCrypt process if it's running
			if (this.CryptHandle != null && this.CryptProcRunning && !this.CryptHandle.HasExited && this.CryptService == null)
			{
				this.CryptHandle.Kill();
				this.CryptProc = null;

				this.startstop_button.Text = "Start";
				this.CryptProcRunning = false;
			}

			this.ServiceCheck.Abort();
		}

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		/// <summary>
		/// The callback for double clicking the system tray icon
		/// Restores the main application window and shows the DNSCrypt Proxy application in a minimized state
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.ShowInTaskbar = true;
			this.notifyIcon.Visible = false;

			if (this.CryptProcRunning)
			{
				ShowWindow(this.CryptHandle.MainWindowHandle, SW_SHOWMINIMIZED);
			}
		}

		/// <summary>
		/// The callback for clicking "Open" on the system tray
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyIcon_Open(object sender, EventArgs e)
		{
			this.notifyIcon_MouseDoubleClick(sender, null);
		}

		/// <summary>
		/// Callback for when the window size is modified. Used to hijack the minimize action to place it in the system tray
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void form_resized(object sender, EventArgs e)
		{
			//The application only needs to be hidden away when the proxy is launched as a standalone process
			if (this.ServiceInstalled || !this.CryptProcRunning)
			{
				return;
			}

			if (WindowState == FormWindowState.Minimized)
			{
				this.Hide();
				this.ShowInTaskbar = false;
				this.notifyIcon.Visible = true;

				if (this.CryptProcRunning)
				{
					ShowWindow(this.CryptHandle.MainWindowHandle, SW_HIDE);
				}
			}
		}

		/// <summary>
		/// The callback for clicking "Exit" on the system tray
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyIcon_Exit(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Refreshes the NIC listing box with the hidden devices either shown or hidden
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void refreshNICList(object sender, EventArgs e)
		{
			this.DNSlistbox.Items.Clear();
			this.GetNICs(this.hideAdaptersCheckbox.Checked);
		}

		/// <summary>
		/// Shows a tooltip when hovered over the (Un)Install button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonInstall_MouseHover(object sender, EventArgs e)
		{
			if (this.ServiceInstalled)
			{
				this.serviceTooltip.Show("Uninstalls the DNSCrypt Windows service.", (Control)sender);
			}
			else
			{
				this.serviceTooltip.Show("Installs DNSCrypt as a Windows service.", (Control)sender);
			}
		}

		/// <summary>
		/// (Un)Installs DNSCrypt as a Windows service
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonInstall_Click(object sender, EventArgs e)
		{
			ProcessStartInfo proc = new ProcessStartInfo();
			proc.FileName = "dnscrypt-proxy.exe";
			proc.UseShellExecute = true;

			// Elevate process permissions on Vista and higher
			if (System.Environment.OSVersion.Version.Major >= 6)
			{
				proc.Verb = "runas";
			}

			if (this.ServiceInstalled)
			{
				proc.Arguments = "--uninstall";

				//Stop the service before uninstalling it
				ServiceController sc = new ServiceController("dnscrypt-proxy");
				try
				{
					if (sc.Status == ServiceControllerStatus.Running)
					{
						sc.Stop();
					}
				}
				catch (InvalidOperationException) { }
			}
			else
			{
				proc.Arguments = "--install";

				//Set up the keys that DNSCrypt wants
				if (!this.updateRegistry())
				{
					return;
				}
			}

			try
			{
				Process p = Process.Start(proc);
				p.WaitForExit();
			}
			catch (Exception exc)
			{
				//User pressed Cancel during the elevation request
				//return;
				MessageBox.Show(exc.Message);
			}
		}

		/// <summary>
		/// Updates the buttons with that status of the service
		/// </summary>
		private void update_Service_Info()
		{
			while (true)
			{
				bool serviceExists = false;
				ServiceController sc = new ServiceController("dnscrypt-proxy");
				try
				{
					string ServiceName = sc.DisplayName;	//Just need to access some property to ensure it exists

					if (this.CryptService == null)
					{
						this.CryptService = new ServiceController("dnscrypt-proxy");
					}
					else
					{
						this.CryptService.Refresh();
					}

					serviceExists = true;
				}
				catch (InvalidOperationException)
				{
					this.CryptService = null;
				}

				if(serviceExists && sc != null)
				{
					this.ServiceInstalled = true;
					if (sc.Status == ServiceControllerStatus.Running)
					{
						this.CryptProcRunning = true;
						this.startstop_button.Text = "Stop";
					}
					else
					{
						this.CryptProcRunning = false;
						this.startstop_button.Text = "Start";
					}
					this.buttonInstall.Text = "Uninstall";
				}
				else
				{
					this.ServiceInstalled = false;
					this.buttonInstall.Text = "Install";

					//See if it's a standalone process running
					if (this.CryptHandle != null)
					{
						if (this.CryptHandle.HasExited)
						{
							this.CryptProcRunning = false;
							this.startstop_button.Text = "Start";
						}
						else
						{
							this.CryptProcRunning = true;
							this.startstop_button.Text = "Stop";
						}
					}
					else
					{
						this.CryptProcRunning = false;
						this.startstop_button.Text = "Start";
					}
				}
				sc.Dispose();
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Fires when the provider dropdown is changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void combobox_provider_SelectionChangeCommitted(object sender, EventArgs e)
		{
			int index = this.combobox_provider.SelectedIndex;
			populate_Config_Form(index);
		}

		/// <summary>
		/// Populates the config tab's fields with information about the selected provider
		/// </summary>
		/// <param name="index">Index into the provider array</param>
		private void populate_Config_Form(int index)
		{
			this.combobox_provider.SelectedIndex = index;

			this.textbox_server_addr.ReadOnly = false;
			this.textbox_server_addr.Text = "";
			this.textbox_fqdn.ReadOnly = false;
			this.textbox_fqdn.Text = "";
			this.textbox_key.ReadOnly = false;
			this.textbox_key.Text = "";

			this.textbox_server_addr.Text = this.Providers[index].server;
			this.textbox_server_addr.ReadOnly = true;

			this.textbox_fqdn.Text = this.Providers[index].fqdn;
			this.textbox_fqdn.ReadOnly = true;

			this.textbox_key.Text = this.Providers[index].key;
			this.textbox_key.ReadOnly = true;
		}

		/// <summary>
		/// Updates the description box with the selected plugin's long description
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pluginListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string text = "";
			if (this.Plugins[this.pluginListBox.SelectedIndex].longDecription != "")
			{
				text = this.Plugins[this.pluginListBox.SelectedIndex].longDecription;
			}
			else
			{
				text = this.Plugins[this.pluginListBox.SelectedIndex].shortDescription;
			}

			this.pluginDescriptionTextBox.Text = text.Replace("\n", "\r\n");
			this.textbox_plugin_params.Text = this.Plugins[this.pluginListBox.SelectedIndex].parameters;
		}

		/// <summary>
		/// Save the plugin parameters to disk
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textbox_plugin_params_Leave(object sender, EventArgs e)
		{
			this.Plugins[this.pluginListBox.SelectedIndex].parameters = this.textbox_plugin_params.Text;
		}

		private void pluginListBox_MouseMove(object sender, MouseEventArgs e)
		{
			int index = pluginListBox.IndexFromPoint(this.pluginListBox.PointToClient(MousePosition));
			if (index != this.LastPluginTooltipIndex && index < this.Plugins.Count)
			{
				tooltip_plugin.SetToolTip(this.pluginListBox, this.Plugins[index].shortDescription);
				this.LastPluginTooltipIndex = index;
			}
		}
	}

	/// <summary>
	/// Win32 API methods for loading DLLs and accessing their functions
	/// </summary>
	static class NativeMethods
	{
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		[DllImport("kernel32.dll")]
		public static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
	}

	/// <summary>
	/// Custom list item entry for the DNS list box. Allows us to store information about the NIC device.
	/// </summary>
	public class NetworkListItem
	{
		public String NICName;
		public String NICDescription;
		public List<string> DNSservers;
		public List<string> DNSserversv6;
		public Boolean IPv4 = false;
		public Boolean IPv6 = false;
		public Boolean hidden = false;

		public NetworkListItem(String Name, String Description, List<string> IPs, List<string> IPsv6, Boolean IPv4, Boolean IPv6)
		{
			this.NICName = Name;
			this.NICDescription = Description;
			this.DNSservers = IPs;
			this.DNSserversv6 = IPsv6;
			this.IPv4 = IPv4;
			this.IPv6 = IPv6;

			if (this.shouldHide(Description))
			{
				this.hidden = true;
			}
		}

		public override string ToString()
		{
			String message = this.NICDescription;

			if (this.DNSservers.Count > 0 || this.DNSserversv6.Count > 0)
			{
				message += " - ";
			}
			
			message += String.Join(", ", DNSservers.ToArray());
			message += String.Join(", ", DNSserversv6.ToArray());
			
			return message;
		}

		/// <summary>
		/// Returns if a device should be flagged as hidden or not.
		/// Adapters such as Hamachi or virtual machines typically have their own settings
		/// which you should rarely ever need to change.
		/// </summary>
		/// <param name="Name">The name of the NIC</param>
		private Boolean shouldHide(String Description)
		{
			string[] blacklist = { 
				"Microsoft Virtual",
				"Hamachi Network",
				"VMware Virtual",
				"VirtualBox",
				"Software Loopback",
				"Microsoft ISATAP",
				"Teredo Tunneling Pseudo-Interface"
			};

			foreach (string entry in blacklist)
			{
				if (Description.Contains(entry))
				{
					return true;
				}
			}

			return false;
		}
	}

	/// <summary>
	/// Custom list item entry for the plugin list box. Allows us to store information about each plugin.
	/// </summary>
	public class PluginListItem
	{
		public string name { get; set; }
		public string shortDescription = "";
		public string longDecription = "";
		public string parameters = "";

		public PluginListItem(string name, string shortDescription, string longDescription)
		{
			this.name = name;
			this.shortDescription = shortDescription;
			this.longDecription = longDescription;
		}

		//public override string ToString()
		//{
		//	return name + ": " + shortDescription;
		//}
	}

	/// <summary>
	/// Stores information about DNSCrypt providers
	/// </summary>
	public class DNSCryptProvider
	{
		public string name;
		public string displayName { get; set; }
		public string description;
		public string location;
		public string server;
		public string ipv6;
		public string parentalControls;
		public List<int> ports = new List<int>();
		public string protos;
		public string fqdn;
		public string key;

		public void setPorts(string portString)
		{
			string[] portStrings = portString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string port in portStrings)
			{
				ports.Add(Int32.Parse(port));
			}
		}

	}

	/// <summary>
	/// A templated pair class since Tuples aren't available until .NET 4
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="U"></typeparam>
	public class Pair<T, U>
	{
		public Pair()
		{
		}

		public Pair(T first, U second)
		{
			this.First = first;
			this.Second = second;
		}

		public T First { get; set; }
		public U Second { get; set; }
	};
}
