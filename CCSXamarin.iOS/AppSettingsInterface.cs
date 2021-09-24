using System;
using CCSXamarin.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
[assembly: Dependency(typeof(AppSettingsInterface))]

namespace CCSXamarin.iOS
{
    public class AppSettingsInterface : IAppSettingsHelper
    {
        public void OpenAppSettings()
        {
            //TODO: test this on IOS
            var url = new NSUrl($"prefs:root=General&path=DATE_AND_TIME");
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}