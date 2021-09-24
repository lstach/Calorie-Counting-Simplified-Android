using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

using CCSXamarin.Droid.Implementations;
using CCSXamarin.Controls;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;
using Android.Widget;


[assembly: ExportRenderer(typeof(AdmobControl),typeof(AdMobRenderer))]
namespace CCSXamarin.Droid.Implementations
{

    public class AdMobRenderer : ViewRenderer<AdmobControl, AdView>
    {
        public AdMobRenderer(Context context) : base(context)
        {
        }

        string adUnitID = "ca-app-pub-4377533152176814/6876823095";

        AdSize adSize = AdSize.SmartBanner;
        AdView adView;

        private AdView CreateAdView()
        {

            if (adView != null)
            {
                return adView;
            }

            adView = new AdView(Context)
            {
                AdSize = adSize,
                AdUnitId = adUnitID
            };

            var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            adView.LayoutParameters = adParams;
            adView.LoadAd(new AdRequest.Builder().Build());

            return adView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdmobControl> e)
        {
            base.OnElementChanged(e);
            if (Control == null && e.NewElement != null)
            {
                SetNativeControl(CreateAdView());
                e.NewElement.HeightRequest = GetSmartBannerDpHeight();
            }
        }

        private int GetSmartBannerDpHeight()
        {
            var dpHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;

            if (dpHeight <= 400) return 32;
            if (dpHeight > 400 && dpHeight <= 720) return 50;
            return 90;
        }


    }
}