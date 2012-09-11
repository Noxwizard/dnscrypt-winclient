using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace dnscrypt_winclient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public const int SW_SHOWMINIMIZED = 2;
		public const int SW_HIDE = 0;

		private ProcessStartInfo CryptProc = null;
		private Process CryptHandle = null;
		private bool CryptProcRunning = false;

		private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
		private System.Windows.Forms.ContextMenuStrip notifyIconContextMenu;
		private System.Windows.Forms.ToolStripMenuItem notifyIconContextOpen;
		private System.Windows.Forms.ToolStripMenuItem notifyIconContextExit;

		public ObservableCollection<NetworkListItem> AdapterCheckList = new ObservableCollection<NetworkListItem>();

		public MainWindow()
		{
			InitializeComponent();
			GetNICs(false);

			//Set up the system tray icon
			using( Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/OpenDNSCrypt.ico")).Stream)
			{
				notifyIcon.Icon = new System.Drawing.Icon(iconStream);
			}
			notifyIcon.Text = "DNSCrypt Proxy Client";
			notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseDoubleClick);

			this.notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip();
			this.notifyIconContextOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIconContextExit = new System.Windows.Forms.ToolStripMenuItem();

			// 
			// notifyIconContextMenu
			// 
			this.notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyIconContextOpen,
            this.notifyIconContextExit});
			this.notifyIconContextMenu.Name = "notifyIconContextMenu";
			this.notifyIconContextMenu.Size = new System.Drawing.Size(181, 48);
			// 
			// notifyIconContextOpen
			// 
			this.notifyIconContextOpen.Name = "notifyIconContextOpen";
			this.notifyIconContextOpen.Size = new System.Drawing.Size(152, 22);
			this.notifyIconContextOpen.Text = "Open";
			this.notifyIconContextOpen.Click += new System.EventHandler(this.notifyIcon_Open);
			// 
			// notifyIconContextExit
			// 
			this.notifyIconContextExit.Name = "notifyIconContextExit";
			this.notifyIconContextExit.Size = new System.Drawing.Size(152, 22);
			this.notifyIconContextExit.Text = "Exit";
			this.notifyIconContextExit.Click += new System.EventHandler(this.notifyIcon_Exit);

			this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenu;
		}

		private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
		{
			AdapterListBox.ItemsSource = AdapterCheckList;
		}

		private void GetNICs(Boolean showHidden)
		{
			this.ipv6Radio.IsEnabled = false;

			NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in adapters)
			{
				IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
				List<string> dnsServers = new List<string>();	//More accurate than IPInterfaceProperties.DnsAddresses
				List<string> dnsServersv6 = new List<string>();

				//Determine if the DNS addresses are obtained automatically or specified
				//We don't want to add them when the program is closed if they weren't actually set
				//Unfortunately, neither WMI nor the NetworkInterface class provide information about this setting
				dnsServers = NetworkManager.getDNS(adapter.Id);
				dnsServersv6 = NetworkManager.getDNSv6(adapter.Id);

				NetworkListItem item = new NetworkListItem(adapter.Name, adapter.Description, dnsServers, dnsServersv6, adapter.Supports(NetworkInterfaceComponent.IPv4), adapter.Supports(NetworkInterfaceComponent.IPv6));
				if (!item.hidden || showHidden)
				{
					AdapterCheckList.Add(item);

					//See if the device supports IPv6 and enable the option if so
					if (item.IPv6)
					{
						this.ipv6Radio.IsEnabled = true;
					}
				}
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			// Start the DNSCrypt process
			if (!this.CryptProcRunning)
			{
				// Make sure the file exists before trying to launch it
				if (!File.Exists(Directory.GetCurrentDirectory() + "\\dnscrypt-proxy.exe"))
				{
					MessageBox.Show("dnscrypt-proxy.exe was not found. It should be placed in the same directory as this program. If you do not have this file, you can download it from https://github.com/opendns/dnscrypt-proxy/downloads", "File not found");
					return;
				}

				this.CryptProc = new ProcessStartInfo();
				this.CryptProc.FileName = "dnscrypt-proxy.exe";
				this.CryptProc.WindowStyle = ProcessWindowStyle.Minimized;

				if ((bool)this.protoTCP.IsChecked)
				{
					this.CryptProc.Arguments = "-T";
				}

				if ((bool)this.ipv6Radio.IsChecked)
				{
					this.CryptProc.Arguments += " --resolver-address=[2620:0:ccd::2]";
				}
				else if ((bool)this.parentalControlsRadio.IsChecked)
				{
					this.CryptProc.Arguments += " --resolver-address=208.67.220.123";
				}
				else
				{
					this.CryptProc.Arguments += " --resolver-address=208.67.220.220";
				}
				this.CryptProc.Arguments += ":" + ((ComboBoxItem)portBox.SelectedItem).Content;

				if ((bool)this.gatewayCheckbox.IsChecked)
				{
					this.CryptProc.Arguments += " --local-address=0.0.0.0";
				}

				try
				{
					this.CryptHandle = Process.Start(this.CryptProc);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}

				service_button.Content = "Stop";
				service_label.Content = "DNSCrypt is running";
				this.CryptProcRunning = true;
			}
			else
			{
				// Make sure the proxy wasn't terminated by another application/user
				if (!this.CryptHandle.HasExited)
				{
					this.CryptHandle.Kill();
				}

				this.CryptProc = null;

				service_button.Content = "Start";
				service_label.Content = "DNSCrypt is NOT running";
				this.CryptProcRunning = false;
			}
		}

		public static void AdpaterListBoxPropertyChange(NetworkListItem entry, Boolean NewValue)
		{
			try
			{
				//Restore the user's old settings
				if (NewValue == false)
				{
					if (entry.IPv4)
					{
						NetworkManager.setDNS(entry.NICDescription, entry.DNSservers);
					}

					//Since we don't set an IPv6 server, don't run this if they didn't have any to begin with
					if (entry.DNSserversv6.Count > 0)
					{
						NetworkManager.setDNSv6(entry.NICName, entry.DNSserversv6);
					}
				}
				else //Set it to loopback for DNSCrypt
				{
					if (entry.IPv4)
					{
						NetworkManager.setDNS(entry.NICDescription, "127.0.0.1");
					}

					//Only do this if there were custom servers set to begin with
					if (entry.DNSserversv6.Count > 0)
					{
						List<string> ip = new List<string>();
						//ip.Add("::1");
						NetworkManager.setDNSv6(entry.NICName, ip);
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("There was an error changing the device's DNS server: " + exception.Message);
			}
		}

		private void hideAdaptersCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			this.AdapterCheckList.Clear();
			this.GetNICs((bool)hideAdaptersCheckbox.IsChecked);
		}

		private void MetroWindow_Closing_1(object sender, CancelEventArgs e)
		{
			//Revert all of the DNS server settings
			foreach (NetworkListItem item in this.AdapterCheckList)
			{
				if ((bool)item.IsChecked == false)
				{
					continue;
				}

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
			if (this.CryptProcRunning && !this.CryptHandle.HasExited)
			{
				this.CryptHandle.Kill();
				this.CryptProc = null;

				service_button.Content = "Start";
				service_label.Content = "DNSCrypt is NOT running";
				this.CryptProcRunning = false;
			}
		}

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		private void MetroWindow_StateChanged_1(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
			{
				//this.Hide();
				this.ShowInTaskbar = false;
				this.notifyIcon.Visible = true;

				if (this.CryptProcRunning)
				{
					ShowWindow(this.CryptHandle.MainWindowHandle, SW_HIDE);
				}
			}
		}

		private void notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//this.Show();
			this.WindowState = WindowState.Normal;
			this.ShowInTaskbar = true;
			this.notifyIcon.Visible = false;

			if (this.CryptProcRunning)
			{
				ShowWindow(this.CryptHandle.MainWindowHandle, SW_SHOWMINIMIZED);
			}
		}

		private void notifyIcon_Open(object sender, EventArgs e)
		{
			this.notifyIcon_MouseDoubleClick(sender, null);
		}

		private void notifyIcon_Exit(object sender, EventArgs e)
		{
			this.Close();
		}
	}

	public class NetworkListItem : CheckBox
	{
		public String NICName;
		public String NICDescription;
		public List<string> DNSservers;
		public List<string> DNSserversv6;
		public Boolean IPv4 = false;
		public Boolean IPv6 = false;
		public Boolean hidden = false;
		public string ListName { get { return ToString(); } }

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

			this.IsChecked = false;
		}

		public override string ToString()
		{
			String message = this.NICDescription + " - ";

			if (this.IPv4)
			{
				message += "IPv4: ";
				if (this.DNSservers.Count > 0)
				{
					message += String.Join(", ", DNSservers.ToArray());
				}
				else
				{
					message += "(Automatic)";
				}
			}

			if (this.IPv6)
			{
				message += " IPv6: ";
				if (this.DNSserversv6.Count > 0)
				{
					message += String.Join(", ", DNSserversv6.ToArray());
				}
				else
				{
					message += "(Automatic)";
				}
			}

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

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//base.OnPropertyChanged(e);
			MainWindow.AdpaterListBoxPropertyChange(this, (Boolean)e.NewValue);
		}
	}
}
