using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CCSXamarin.Droid
{
	public class MainApplication : Application
	{
		public MainApplication(IntPtr handle, JniHandleOwnership transer)
		  : base(handle, transer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this);
		}
	}
}