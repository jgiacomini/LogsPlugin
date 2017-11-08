using System.Reflection;
using Android.App;
using Android.OS;
using Plugin.Logs.Test;
using Xamarin.Android.NUnitLite;

namespace Plugin.Logs.Droid
{
    [Activity(Label = "Plugin.Logs.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            //AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            AddTest (typeof (LogServiceTest).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }
}

