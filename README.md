# ASP.NET Web Modules

## OriginalIP
Preserves the client's original IP address across load balancers or application gateways.
[![Build Status](https://added.visualstudio.com/Added/_apis/build/status/GitHub/Original-IP)](https://added.visualstudio.com/Added/_build/latest?definitionId=2)

First, install the assembly in the GAC with [Chocolatey](http://chocolatey.org/packages).

```
choco install ai-web-originalip

```

*If you have IIS installed, it will call iisreset*

Next, open up a command prompt in admin mode use appcmd to add a [managed module](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/cc754939%28v%3dws.10%29) to your site.

```
c:\windows\system32\inetsrv\appcmd add module /name:OriginalIP /type:"AI.Web.Core.Modules.OriginalIP, AI.Web.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=94825510554e74d3" /preCondition:runtimeVersionv4.0,managedHandler /app.name:"Default Web Site/"

```

Change the /app.name parameter to the web site you need.  If you leave the parameter off, it will apply it to all sites in IIS.