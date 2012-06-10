using System.Collections.Generic;
using System.Management;
using Microsoft.Win32;
using System.Diagnostics;

namespace dnscrypt_winclient
{
	class NetworkManager
	{
		/// <summary>
		/// Sets the DNS server address(es) for the specified NIC
		/// </summary>
		/// <param name="NIC">NIC name</param>
		/// <param name="DNS">DNS server addresses, comma separated</param>
		public static void setDNS(string NIC, string DNS)
		{
			ManagementClass NetworkConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection NICcollection = NetworkConfig.GetInstances();

			foreach (ManagementObject NICconfig in NICcollection)
			{
				if ((bool)NICconfig["IPEnabled"])
				{
					if (NICconfig["Description"].Equals(NIC))
					{
						ManagementBaseObject DNSconfig = NICconfig.GetMethodParameters("SetDNSServerSearchOrder");
						DNSconfig["DNSServerSearchOrder"] = DNS.Split(',');
						NICconfig.InvokeMethod("SetDNSServerSearchOrder", DNSconfig, null);
					}
				}
			}
		}

		/// <summary>
		/// Sets the DNS server address(es) for the specified NIC
		/// </summary>
		/// <param name="NIC">NIC name</param>
		/// <param name="DNS">DNS server addresses</param>
		public static void setDNS(string NIC, List<string> IPs)
		{
			ManagementClass NetworkConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection NICcollection = NetworkConfig.GetInstances();

			foreach (ManagementObject NICconfig in NICcollection)
			{
				if ((bool)NICconfig["IPEnabled"])
				{
					if (NICconfig["Description"].Equals(NIC))
					{
						ManagementBaseObject DNSconfig = NICconfig.GetMethodParameters("SetDNSServerSearchOrder");
						DNSconfig["DNSServerSearchOrder"] = IPs.ToArray();
						NICconfig.InvokeMethod("SetDNSServerSearchOrder", DNSconfig, null);
					}
				}
			}
		}

		/// <summary>
		/// Sets the DNS server address(es) for the specified NIC
		/// Since this calls an external program, you should only do this if you absolutely have to
		/// </summary>
		/// <param name="NIC">NIC name</param>
		/// <param name="DNS">DNS server addresses</param>
		public static void setDNSv6(string NICName, List<string> IPs)
		{
			//netsh interface ipv6 delete dns "Interface Name" all
			Process p = new Process();
			ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface ipv6 delete dns \"" + NICName + "\" all");
			psi.WindowStyle = ProcessWindowStyle.Minimized;
			p.StartInfo = psi;
			p.Start();

			foreach (string ip in IPs)
			{
				//netsh interface ipv6 add dns "Interface Name" 127.0.0.1 validate=no
				psi = new ProcessStartInfo("netsh", "interface ipv6 add dns \"" + NICName + "\" " + ip + " validate=no");
				psi.WindowStyle = ProcessWindowStyle.Minimized;
				p.StartInfo = psi;
				p.Start();
			}
		}

		/// <summary>
		/// Gets the DNS server address(es) for the specified NIC
		/// </summary>
		/// <param name="NIC">NIC ID</param>
		/// <returns>A list of DNS server IPs</returns>
		public static List<string> getDNS(string adapterID)
		{
			List<string> ServerAddresses = new List<string>();

			object regAddress = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + adapterID, "NameServer", "");
			if (regAddress != null && regAddress.ToString().Length > 0)
			{
				ServerAddresses = new List<string>(((string)regAddress).Split(new string[] { "," }, System.StringSplitOptions.None));
			}

			return ServerAddresses;
		}

		/// <summary>
		/// Gets the DNS server address(es) for the specified NIC's IPv6 protocol
		/// </summary>
		/// <param name="NIC">NIC ID</param>
		/// <returns>A list of DNS server IPs</returns>
		public static List<string> getDNSv6(string adapterID)
		{
			List<string> ServerAddresses = new List<string>();

			object regAddress = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\TCPIP6\\Parameters\\Interfaces\\" + adapterID, "NameServer", "");
			if (regAddress != null && regAddress.ToString().Length > 0)
			{
				ServerAddresses = new List<string>(((string)regAddress).Split(new string[] { "," }, System.StringSplitOptions.None));
			}

			return ServerAddresses;
		}
	}
}
