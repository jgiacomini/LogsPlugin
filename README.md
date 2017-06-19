# LogsPlugin
Plugin to logs on all  .Net platforms.  
```cs
using LLibrary;

L.Register("INFO");
L.Register("ERROR", "An exception just happened: {0}");
```



[![Build status](https://ci.appveyor.com/api/projects/status/6b5nojsd4ex6gk70?svg=true)](https://ci.appveyor.com/project/jgiacomini/logsplugin)

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
