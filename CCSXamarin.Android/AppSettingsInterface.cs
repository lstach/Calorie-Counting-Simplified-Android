using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using CCSXamarin.Droid;

using Xamarin.Forms;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AppSettingsInterface))]
namespace CCSXamarin.Droid
{
    class AppSettingsInterface : IAppSettingsHelper
    {
        public void OpenAppSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionDateSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            
            Application.Context.StartActivity(intent);
        }
    }
}