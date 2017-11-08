# LogsPlugin
Plugin to logs on .Net platforms.  
```cs
using Plugin.Logs;

var logService = new LogService("log","C:\logs\");
//Log a message 
logService.Log("Message");

//Log a warning message 
logService.Log("Message", LogLevel.Warning);

// Log an exception
logService.Log(new NotImplementedException());

// Log an exception with message
logService.Log("message", new NotImplementedException());

```

## NuGet
* Available on NuGet: [Plugin.Logs](http://www.nuget.org/packages/Plugin.Logs) [![NuGet](https://img.shields.io/nuget/v/Plugin.Logs.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Logs/)

## Build
[![Build status](https://ci.appveyor.com/api/projects/status/6b5nojsd4ex6gk70?svg=true)](https://ci.appveyor.com/project/jgiacomini/logsplugin)

## Platform Support
|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|No|8.0+|
|Windows Phone RT|No|8.1+|
|Windows Store RT|No|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|No|All|
|.NET 4.5|Yes|All|

### Created By: [@JeromeGiacomini](https://twitter.com/jeromegiacomini)
* Twitter: [@JeromeGiacomini](http://twitter.com/jeromegiacomini)
* Blog: [Blog perso](http://jeromegiacomini.net/Blog/), [Blog pro](http://blogs.infinitesquare.com/users/jgiacomini)
* Book: [Xamarin (French only)] (https://www.editions-eni.fr/supports-de-cours/livre/xamarin-developpez-vos-applications-multiplateformes-pour-ios-android-et-windows-9782409007477)

## License
MIT © JGI
