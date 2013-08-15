![image](https://raw.github.com/FivfBx2dOQTC3gc8YS4yMNo0el/dnscrypt-winclient/master/screenshot.png)

About
=====
The purpose of this application is to allow the user to have a better experience controlling the DNSCrypt Proxy on Windows. It was primarily created because the proxy cannot run in the background yet, so I needed a way to minimize it out of sight. It is targeted at .NET 2.0 to reach a wider audience, is built on Visual Studio 2010, and is released under the MIT license.

Requirements
============
DNSCrypt Proxy 0.9.4 or greater (http://download.dnscrypt.org/dnscrypt-proxy/)  
Microsoft .NET Framework 2.0 or greater (http://www.microsoft.com/net/download)

Running
=======
Executables can be found in the /binaries directory. Unless you have issues, use the Release binary.  
Simply place the DNSCrypt proxy binary (http://download.dnscrypt.org/dnscrypt-proxy/) in the same directory as this binary and execute the Windows client.

Features
========
- Enable DNSCrypt on multiple adapters via a checkbox
- Specify a port and protocol to send on
- Start/Stop the DNSCrypt proxy
- Select DNS resolver, OpenDNS or CloudNS.com.au
- Connect to OpenDNS via IPv6
- Connect to OpenDNS with Parental Controls enabled
- Enable the proxy to act as a gateway device

When a box is unchecked or the application is closed, all DNS server settings are reverted to their original state. This is so that browsing doesn't break if the proxy isn't restarted on the next system start. The "(Automatic)" marker appears if no DNS servers were assigned and they are provided by the router/ISP.

Possible future plans
=====================
- Have a background worker periodically check that the proxy application hasn't crashed.
- Pick up a few of the other parameters that the proxy accepts