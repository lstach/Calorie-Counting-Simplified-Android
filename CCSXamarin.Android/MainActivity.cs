using System;
using System.IO;

using Xamarin.Forms;
using CCSXamarin.Controls;
using CCSXamarin.Droid.Implementations;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;
using Android.Support.V7.App;
using Android.Content;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;

namespace CCSXamarin.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;


            base.OnCreate(savedInstanceState);
            MobileAds.Initialize(ApplicationContext, "ca-app-pub-4377533152176814~3052100481");


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            

            string fileName = "savedFoods_db.db";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string completePath = Path.Combine(folderPath, fileName);
            LoadApplication(new App(completePath));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }
    }
}