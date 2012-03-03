using System.Collections.Generic;
using System.Management;

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
		/// Gets the DNS server address(es) for the specified NIC
		/// </summary>
		/// <param name="NIC">NIC name</param>
		/// <returns>A list of DNS server IPs</returns>
		public static List<string> getDNS(string NIC)
		{
			List<string> ServerAddresses = new List<string>();
			ManagementClass NetworkConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection NICcollection = NetworkConfig.GetInstances();

			foreach (ManagementObject NICconfig in NICcollection)
			{
				if ((bool)NICconfig["IPEnabled"])
				{
					if (NICconfig["Description"].Equals(NIC))
					{
						string[] result = (string[])NICconfig.GetPropertyValue("DNSServerSearchOrder");

						if (result != null)
						{
							ServerAddresses = new List<string>(result);
						}
					}
				}
			}

			return ServerAddresses;
		}
	}
}
