![image](https://raw.github.com/Noxwizard/dnscrypt-winclient/master/screenshot.png)

About
=====
The purpose of this application is to allow the user to have a better experience controlling the DNSCrypt Proxy on Windows. It was primarily created because the proxy cannot run in the background yet, so I needed a way to minimize it out of sight. It is targeted at .NET 2.0 to reach a wider audience, is built on Visual Studio 2010, and is released under the MIT license.

Requirements
============
DNSCrypt Proxy 1.1.0 or greater (http://download.dnscrypt.org/dnscrypt-proxy/)  
Microsoft .NET Framework 2.0 or greater (http://www.microsoft.com/net/download)

Running
=======
Zip archives containing the necessary files can be found in the /binaries directory. Unless you have issues, use the Release files.  
Simply place the DNSCrypt proxy binaries (https://github.com/opendns/dnscrypt-proxy/downloads) in the same directory as this binary and execute the Windows client.

Required DNSCrypt Proxy files
-----------------------------
- dnscrypt-proxy.exe
- libldns-1.dll
- libsodium-4.dll


Features
========
- Enable DNSCrypt on multiple adapters via a checkbox
- Specify a port and protocol to send on
- Start/Stop the DNSCrypt proxy
- Connect to OpenDNS via IPv6
- Connect to OpenDNS with Parental Controls enabled
- Enable the proxy to act as a gateway device
- Install/Uninstall/Start/Stop DNSCrypt as a Windows service
- Choose from multiple DNSCrypt providers


When launched as a standalone process and a box is unchecked or the application is closed, all DNS server settings are reverted to their original state. This is so that browsing doesn't break if the proxy isn't restarted on the next system start. The "(Automatic)" marker appears if no DNS servers were assigned and they are provided by the router/ISP.

Known Issues
============
- Does not remember which provider was selected after closing when using as a service
- Does not remember custom network settings after closing when using as a service
- Cannot specify the local address to bind on
- Plugin support incomplete

Possible future plans
=====================
- Have a background worker periodically check that the proxy application hasn't crashed.
- Pick up a few of the other parameters that the proxy accepts

Currently Supported DNSCrypt Providers
======================================
Add your own by editing dnscrypt-winclient.exe.config.

* [OpenDNS](http://www.opendns.com)
  - IPv4: 208.67.220.220
  - IPv6: 2620:0:ccd::2
  - Parental Controls: 208.67.220.123
  - Ports: 443, 5353, 53
  - Provider name: 2.dnscrypt-cert.opendns.com
  - Public key: B735:1140:206F:225D:3E2B:D822:D7FD:691E:A1C3:3CC8:D666:8D0C:BE04:BFAB:CA43:FB79

* [CloudNS](https://cloudns.com.au/) - No logs, DNSSEC
  * Canberra, Australia
    - IPv4: 113.20.6.2
	- Ports: 443
    - Provider name: 2.dnscrypt-cert.cloudns.com.au
    - Public key: 1971:7C1A:C550:6C09:F09B:ACB1:1AF7:C349:6425:2676:247F:B738:1C5A:243A:C1CC:89F4
  * Sydney, Australia
    - IPv4: 113.20.8.17
	- Ports: 443
    - Provider name: 2.dnscrypt-cert-2.cloudns.com.au
    - Public key: 67A4:323E:581F:79B9:BC54:825F:54FE:1025:8B4F:37EB:0D07:0BCE:4010:6195:D94F:E330

* [okTurtles](http://opendns.com)
  - IPv4: 23.226.227.93
  - Ports: 443
  - Provider name: 2.dnscrypt-cert.okturtles.com
  - Public key: 1D85:3953:E34F:AFD0:05F9:4C6F:D1CC:E635:D411:9904:0D48:D19A:5D35:0B6A:7C81:73CB

* [OpenNIC](http://www.opennicproject.org/) - No logs
  * Japan
    - IPv4: 106.186.17.181
	- Ports: 2053
    - Provider name: 2.dnscrypt-cert.ns2.jp.dns.opennic.glue
    - Public key: 8768:C3DB:F70A:FBC6:3B64:8630:8167:2FD4:EE6F:E175:ECFD:46C9:22FC:7674:A1AC:2E2A
  * UK
    * NovaKing (ns8)
      - IPv4: 185.19.104.45
	  - Ports: 443
      - Provider name: 2.dnscrypt-cert.ns8.uk.dns.opennic.glue
      - Public key: A17C:06FC:BA21:F2AC:F4CD:9374:016A:684F:4F56:564A:EB30:A422:3D9D:1580:A461:B6A6
    * NovaKing (ns9)
      - IPv4: 185.19.105.6
	  - IPv6: 2a04:1400:1337:2000::6
	  - Ports: 443
      - Provider name: 2.dnscrypt-cert.ns9.uk.dns.opennic.glue
      - Public key: E864:80D9:DFBD:9DB4:58EA:8063:292F:EC41:9126:8394:BC44:FAB8:4B6E:B104:8C3B:E0B4
    * NovaKing (ns10)
      - IPv4: 185.19.105.14
	  - IPv6: 2a04:1400:1337:2000::14
	  - Ports: 443
      - Provider name: 2.dnscrypt-cert.ns10.uk.dns.opennic.glue
      - Public key: B1AB:7025:1119:9AEE:E42E:1B12:F2EF:12D4:53D9:CD92:E07B:9AF4:4794:F6EB:E5A4:F725
  * USA
    * Fremont, CA
      - IPv4: 173.230.156.28
	  - Ports: 443
      - Provider name: 2.dnscrypt-cert.ns17.ca.us.dns.opennic.glue
      - Public key: 2342:215C:409A:85A5:FB63:2A3B:42CD:5089:6BA8:551A:8BDC:2654:CF57:804F:B1B2:5019
    * Fremont, CA #2
      - IPv6: [2600:3c01::f03c:91ff:fe6e:1f6b]
	  - Ports: 443
      - Provider name: 2.dnscrypt-cert.ns18.ca.us.dns.opennic.glue
      - Public key: 689B:DAF2:6A9F:DB2D:42B4:AA15:1825:89E8:6FAE:0C2C:522A:D0AA:DD2B:80B4:8D61:0A43

* [DNSCrypt.eu](http://dnscrypt.eu/) - No logs, DNSSEC
  * Holland
    - IPv4: 176.56.237.171
	- IPv6: 2a00:d880:3:1::a6c1:2e89
	- Ports: 443
    - Provider name: 2.dnscrypt-cert.resolver1.dnscrypt.eu
    - Public key: 67C0:0F2C:21C5:5481:45DD:7CB4:6A27:1AF2:EB96:9931:40A3:09B6:2B8D:1653:1185:9C66

  * Denmark
    - IPv4: 77.66.84.233
	- Ports: 443
    - Provider name: 2.dnscrypt-cert.resolver2.dnscrypt.eu
    - Public key: 3748:5585:E3B9:D088:FD25:AD36:B037:01F5:520C:D648:9E9A:DD52:1457:4955:9F0A:9955

* [Soltysiak.com](http://dc1.soltysiak.com/) - No logs, DNSSEC
  * Poznan, Poland
    - IPv4: 178.216.201.222
	- IPv6: 2001:470:70:4ff::2
	- Ports: 2053
    - Provider name: 2.dnscrypt-cert.soltysiak.com
    - Public key: 25C4:E188:2915:4697:8F9C:2BBD:B6A7:AFA4:01ED:A051:0508:5D53:03E7:1928:C066:8F21