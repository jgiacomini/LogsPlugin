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

### Created By: [@JamesMontemagno](http://twitter.com/jamesmontemagno)
* Twitter: [@JamesMontemagno](http://twitter.com/jamesmontemagno)
* Blog: [MotzCod.es](http://motzcod.es), [Micro Blog](http://motz.micro.blog)
* Podcasts: [Merge Conflict](http://mergeconflict.fm), [Coffeehouse Blunders](http://blunders.fm), [The Xamarin Podcast](http://xamarinpodcast.com)
* Video: [The Xamarin Show on Channel 9](http://xamarinshow.com), [YouTube Channel](https://www.youtube.com/jamesmontemagno) 

## License
MIT © JGI
