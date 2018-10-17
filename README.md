# LogsPlugin
[![Build status](https://ci.appveyor.com/api/projects/status/6b5nojsd4ex6gk70?svg=true)](https://ci.appveyor.com/project/jgiacomini/logsplugin)
[![NuGet](https://img.shields.io/nuget/v/Plugin.Logs.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Logs/)

Plugin to logs on .Net platforms.  
```cs
using Plugin.Logs;
ILoggerFactory loggerFactory = new LoggerFactory
{
   LogDirectoryPath = @"C:\logs\"
};

factory.LogDirectoryPath = "C:\logs\";

var logger = loggerFactory.GetLoggger("Log");

//Log a message 
logger.Info("Message");

//Log a warning message 
logger.Warning("Message");

// Log an exception
logger.Log(new NotImplementedException());

// Log an exception with message
logger.Log("message", new NotImplementedException());

```

## NuGet
* Available on NuGet: [Plugin.Logs](http://www.nuget.org/packages/Plugin.Logs) [![NuGet](https://img.shields.io/nuget/v/Plugin.Logs.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Logs/)

## Build
[![Build status](https://ci.appveyor.com/api/projects/status/6b5nojsd4ex6gk70?svg=true)](https://ci.appveyor.com/project/jgiacomini/logsplugin)

## Platform Support
|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|.NET Standard|Yes|2.0|

### Created By: [@JeromeGiacomini](https://twitter.com/jeromegiacomini)
* Twitter: [@JeromeGiacomini](http://twitter.com/jeromegiacomini)
* Blog: [Blog perso](http://jeromegiacomini.net/Blog/), [Blog pro](http://blogs.infinitesquare.com/users/jgiacomini)
* Book: [Xamarin French only](https://www.editions-eni.fr/supports-de-cours/livre/xamarin-developpez-vos-applications-multiplateformes-pour-ios-android-et-windows-9782409007477)

## License
MIT © JGI
