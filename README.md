![image](https://raw.github.com/Noxwizard/dnscrypt-winclient/master/screenshot.png)

About
=====
The purpose of this application is to allow the user to have a better experience controlling the DNSCrypt Proxy on Windows. It was primarily created because the proxy could not originally run in the background, so I needed a way to minimize it out of sight. It is targeted at .NET 2.0 to reach a wider audience, is built on Visual Studio 2012, and is released under the MIT license.

Requirements
============
DNSCrypt Proxy 1.4.0 or greater (http://download.dnscrypt.org/dnscrypt-proxy/)  
Microsoft .NET Framework 2.0 or greater (http://www.microsoft.com/net/download)

Running
=======
The necessary files can be found in the /binaries directory. Unless you have issues, use the Release files.  
Simply place the DNSCrypt proxy files (https://github.com/opendns/dnscrypt-proxy/downloads) in the same directory as this binary and execute the Windows client.

Required DNSCrypt Proxy files
-----------------------------
- dnscrypt-proxy.exe
- dnscrypt-resolvers.csv
- libldns-1.dll
- libsodium-4.dll


Features
========
- Enable DNSCrypt on multiple adapters via a checkbox
- Start/Stop the DNSCrypt proxy
- Enable the proxy to act as a gateway device
- Install/Uninstall/Start/Stop DNSCrypt as a Windows service
- Choose from multiple DNSCrypt providers


When launched as a standalone process and a box is unchecked or the application is closed, all DNS server settings are reverted to their original state. This is so that browsing doesn't break if the proxy isn't restarted on the next system start. The "(Automatic)" marker appears if no DNS servers were assigned and they are provided by the router/ISP.

Known Issues
============
- Does not remember custom network settings after closing when using as a service
- Cannot specify the local address to bind on
- Plugin support incomplete

Possible future plans
=====================
- Pick up a few of the other parameters that the proxy accepts
- Restore ability to add custom providers