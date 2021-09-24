using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace CCSXamarin.Controls
{
    public class AdmobControl : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
                      nameof(AdUnitId),
                      typeof(string),
                      typeof(AdmobControl),
                      string.Empty);

        public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard, SmartBannerPortrait }
        public Sizes Size { get; set; }

        public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }
    }
}
