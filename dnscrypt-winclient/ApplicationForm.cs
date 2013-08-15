using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace dnscrypt_winclient
{
	public partial class ApplicationForm : Form
	{
		public const int SW_SHOWMINIMIZED = 2;
		public const int SW_HIDE = 0;

        public Dictionary<string, List<String>> dnsServerslist = new Dictionary<string, List<String>>();
        public Dictionary<string, List<String>> dnsServersv6list = new Dictionary<string, List<String>>();

        private bool shouldIgnoreCheckEvent = false;

		private ProcessStartInfo CryptProc = null;
		private Process CryptHandle = null;
		private bool CryptProcRunning = false;


		public ApplicationForm()
		{
			InitializeComponent();
            setdnsServers();
			GetNICs(false,false);
			this.portBox.SelectedIndex = 1;	//Port 443
            this.serverBox.SelectedIndex = 0; // CloudNS.com.au
		}

        private void setdnsServers()
        {
            int i = 0;
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
		    foreach (NetworkInterface adapter in adapters)
		    {
			    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                
                List<string> dnsServers = new List<string>();
                List<string> dnsServersv6 = new List<string>();

			    //Determine if the DNS addresses are obtained automatically or specified
			    //We don't want to add them when the program is closed if they weren't actually set
			    //Unfortunately, neither WMI nor the NetworkInterface class provide information about this setting
			    dnsServers = NetworkManager.getDNS(adapter.Id);
			    dnsServersv6 = NetworkManager.getDNSv6(adapter.Id);
                dnsServerslist.Add(adapter.Id, dnsServers);
                dnsServersv6list.Add(adapter.Id, dnsServersv6);
                i++;
            }

        }

        /// <summary>
		/// Fills the ListBox with information about each NIC
		/// </summary>
		private void GetNICs(Boolean showHidden, Boolean refresh)
		{
			this.ipv6Radio.Enabled = false;

            int i=0;
			NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in adapters)
			{
				IPInterfaceProperties adapterProperties = adapter.GetIPProperties();

                List<string> dnsServers = new List<string>();
                List<string> dnsServersv6 = new List<string>();

                //Determine if the DNS addresses are obtained automatically or specified
                //We don't want to add them when the program is closed if they weren't actually set
                //Unfortunately, neither WMI nor the NetworkInterface class provide information about this setting
                dnsServers = NetworkManager.getDNS(adapter.Id);
                dnsServersv6 = NetworkManager.getDNSv6(adapter.Id);

                NetworkListItem item = new NetworkListItem(adapter.Id,adapter.Name, adapter.Description, dnsServers, dnsServersv6, adapter.Supports(NetworkInterfaceComponent.IPv4), adapter.Supports(NetworkInterfaceComponent.IPv6));
                if (!item.hidden || showHidden)
                {
                    if(refresh == false)
                    { 
                        if( dnsServers.Count != 0)
                        { 
                            if(dnsServers[0]=="127.0.0.1")
                            {
                                DNSlistbox.Items.Add(item, true);
                            }
                            else
                            {
                                DNSlistbox.Items.Add(item, false);
                            }
                        }
                        else
                        {
                            DNSlistbox.Items.Add(item, false);
                        }
                        

                        //See if the device supports IPv6 and enable the option if so
                        if (item.IPv6)
                        {
                            this.ipv6Radio.Enabled = true;
                        }
                    } 
                    else
                    {
                        if(DNSlistbox.Items.Count != 0)
                        { 
                            DNSlistbox.Items[i] = item;

                            //See if the device supports IPv6 and enable the option if so
                            if (item.IPv6)
                            {
                                this.ipv6Radio.Enabled = true;
                            }
                        }
                    }
                }

                

                i++;

				/*
				IPAddressCollection dnsServers2 = adapterProperties.DnsAddresses;
				if (dnsServers2.Count > 0)
				{
					//NetworkListItem item = new NetworkListItem(adapter.Description, dnsServers2);
					//DNSlistbox.Items.Add(item);

					Console.WriteLine(adapter.Description + ": " + adapter.Id);
					foreach (IPAddress dns in dnsServers2)
					{
					    Console.WriteLine("  DNS Servers ............................. : {0}", dns.ToString());
					}
					Console.WriteLine();
				}*/

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
            if (this.shouldIgnoreCheckEvent != true) 
            {
			    try
                {

                    //Restore the user's old settings
				    if (e.CurrentValue == CheckState.Checked)
				    {
					    if (((NetworkListItem)DNSlistbox.Items[e.Index]).IPv4)
					    {
                            List<string> DNSservers = dnsServerslist[((NetworkListItem)DNSlistbox.Items[e.Index]).Id];
						    NetworkManager.setDNS(((NetworkListItem)DNSlistbox.Items[e.Index]).NICDescription, DNSservers);
					    }

					    //Since we don't set an IPv6 server, don't run this if they didn't have any to begin with
					    if (((NetworkListItem)DNSlistbox.Items[e.Index]).DNSserversv6.Count > 0)
					    {
                            List<string> DNSserversv6 = dnsServerslist[((NetworkListItem)DNSlistbox.Items[e.Index]).Id];
                            NetworkManager.setDNSv6(((NetworkListItem)DNSlistbox.Items[e.Index]).NICName, DNSserversv6);
					    }
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
				    }
			    }
			    catch (Exception exception)
			    {
				    MessageBox.Show("There was an error changing the device's DNS server: " + exception.Message);
                }
                this.GetNICs(this.hideAdaptersCheckbox.Checked, true);
            }
		}

		/// <summary>
		/// Button to start and stop the DNSCrypt Proxy application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void service_button_Click(object sender, EventArgs e)
		{
			// Start the DNSCrypt process
			if (!this.CryptProcRunning)
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

				if (this.protoTCP.Checked)
				{
					this.CryptProc.Arguments = "-T";
				}

                if (this.serverBox.SelectedItem.ToString() == "CloudNS.com.au")
                {
                    if (this.ipv6Radio.Checked)
                    {
                        this.CryptProc.Arguments += " -r 2405:5000:2:1e5:250:56ff:fe9a:35b --provider-name=2.dnscrypt-cert.cloudns.com.au --provider-key=1971:7C1A:C550:6C09:F09B:ACB1:1AF7:C349:6425:2676:247F:B738:1C5A:243A:C1CC:89F4";
                    }
                    else
                    {
                        this.CryptProc.Arguments += "-r 113.20.6.2:443 --provider-name=2.dnscrypt-cert.cloudns.com.au --provider-key=1971:7C1A:C550:6C09:F09B:ACB1:1AF7:C349:6425:2676:247F:B738:1C5A:243A:C1CC:89F4";
                    }
                }
                else { 
				    if (this.ipv6Radio.Checked)
				    {
					    this.CryptProc.Arguments += " --resolver-address=[2620:0:ccd::2]";
				    }
				    else if (this.parentalControlsRadio.Checked)
				    {
					    this.CryptProc.Arguments += " --resolver-address=208.67.220.123";
				    }
				    else
				    {
					    this.CryptProc.Arguments += " --resolver-address=208.67.220.220";
				    }
                }
				this.CryptProc.Arguments += ":" + this.portBox.SelectedItem.ToString();

				if (this.gatewayCheckbox.Checked)
				{
					this.CryptProc.Arguments += " --local-address=0.0.0.0";
				}

				this.CryptHandle = Process.Start(this.CryptProc);

				this.service_button.Text = "Stop";
				this.service_label.Text = "DNSCrypt is running";
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

				this.service_button.Text = "Start";
				this.service_label.Text = "DNSCrypt is NOT running";
				this.CryptProcRunning = false;
			}
		}

        private void revert_dns()
        {
            //Revert all of the DNS server settings
            foreach (NetworkListItem item in this.DNSlistbox.CheckedItems)
            {
                try
                {
                    List<string> DNSservers = dnsServerslist[item.Id];
                    NetworkManager.setDNS(item.NICDescription, DNSservers);

                    if (item.DNSserversv6.Count > 0)
                    {
                        List<string> DNSserversv6 = dnsServerslist[item.Id];
                        NetworkManager.setDNSv6(item.NICName, DNSserversv6);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("There was an error reverting your DNS settings: " + exception.Message);
                }
            }
            //this.DNSlistbox.Items.Clear();
            this.GetNICs(this.hideAdaptersCheckbox.Checked,true);
        }

		/// <summary>
		/// Called right before the form closes.
		/// Resets all DNS information back to their previous values and stops the DNSCrypt Proxy application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void form_closing(object sender, FormClosingEventArgs e)
		{
            revert_dns();

			//Kill the DNSCrypt process if it's running
			if (this.CryptProcRunning && !this.CryptHandle.HasExited)
			{
				this.CryptHandle.Kill();
				this.CryptProc = null;

				this.service_button.Text = "Start";
				this.service_label.Text = "DNSCrypt is NOT running";
				this.CryptProcRunning = false;
			}
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
            this.shouldIgnoreCheckEvent = true;
			this.DNSlistbox.Items.Clear();
			this.GetNICs(this.hideAdaptersCheckbox.Checked,false);
            this.shouldIgnoreCheckEvent = false;
		}

        private void serverListChanged(object sender, EventArgs e)
        {
            if (this.serverBox.SelectedItem.ToString() == "CloudNS.com.au")
            {
                this.parentalControlsRadio.Enabled = false;
            }
            else
            {
                this.parentalControlsRadio.Enabled = true;
            }
        }
	}

	/// <summary>
	/// Custom list item entry for the DNS list box. Allows us to store information about the NIC device.
	/// </summary>
	public class NetworkListItem
	{
        public String Id;
		public String NICName;
		public String NICDescription;
		public List<string> DNSservers;
		public List<string> DNSserversv6;
		public Boolean IPv4 = false;
		public Boolean IPv6 = false;
		public Boolean hidden = false;

		public NetworkListItem(String Id, String Name, String Description, List<string> IPs, List<string> IPsv6, Boolean IPv4, Boolean IPv6)
		{
            this.Id = Id;
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
	}
}
