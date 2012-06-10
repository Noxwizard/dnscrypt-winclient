![image](https://raw.github.com/Noxwizard/dnscrypt-winclient/metro/screenshot.png)

About
=====
The purpose of this application is to allow the user to have a better experience controlling the DNSCrypt Proxy on Windows. It was primarily created because the proxy cannot run in the background yet, so I needed a way to minimize it out of sight. It is targeted at .NET 4.5, is built on Visual Studio 2012 RC using C# and WPF, and is released under the MIT license.  
It is possible to run this on .NET 4.0, but it has slight higher memory usage, so 4.5 is what the pre-built binaries use. You can rebuild it targeting 4.0 if you would like. It will not work at less than that though.  
The Metro style was achieved through the use of https://github.com/MahApps/MahApps.Metro

Requirements
============
DNSCrypt Proxy 0.9.4 or greater (https://github.com/opendns/dnscrypt-proxy/downloads)  
Microsoft .NET Framework 4.5 (You can also build this under 4.0)

Running
=======
Archives can be found in the /binaries directory. Unless you have issues, use the Release files.  
Simply place the DNSCrypt proxy binary (https://github.com/opendns/dnscrypt-proxy/downloads) in the same directory as the contents of the archive and execute the Windows client.

Features
========
- Enable DNSCrypt on multiple adapters via a checkbox
- Specify a port and protocol to send on
- Start/Stop the DNSCrypt proxy
- Connect to OpenDNS via IPv6
- Connect to OpenDNS with Parental Controls enabled
- Enable the proxy to act as a gateway device

When a box is unchecked or the application is closed, all DNS server settings are reverted to their original state. This is so that browsing doesn't break if the proxy isn't restarted on the next system start. The "(Automatic)" marker appears if no DNS servers were assigned and they are provided by the router/ISP.

Possible future plans
=====================
- Have a background worker periodically check that the proxy application hasn't crashed.
- Pick up a few of the other parameters that the proxy accepts