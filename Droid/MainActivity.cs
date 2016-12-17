using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Android.OS;
using Android.App;
using Android.Widget;
using Android.Webkit;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Views.InputMethods;

using Java.Interop;

using AndroidHUD;
using Debug = System.Diagnostics.Debug ;

namespace WebViewInteraction.Droid
{
	[Activity (Label = "WebViewInteraction", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private WebView MyWebView { get; set;}
		private EditText TxtUrl { get; set;}
		private Button BtnGo { get; set;}



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.entryview);

			var btnLoadLocalHtml = FindViewById<Button> (Resource.Id.entryview_btnLoadLocalHtml);
			btnLoadLocalHtml.Click += (sender, e) => {
				StartActivity (typeof (LoadLocalHtmlActivity));
			};

			var btnLoadUrl = FindViewById<Button> (Resource.Id.entryview_btnLoadUrl);
			btnLoadUrl.Click += (sender, e) => {
				StartActivity (typeof (LoadUrlActivity));
			};

			var btnCallPlatformFeatures = FindViewById<Button> (Resource.Id.entryview_btnCallPlatformFeatures);
			btnCallPlatformFeatures.Click += (sender, e) => {
				StartActivity (typeof (CallPlatformFeaturesActivity));
			};

			var btnCallContentFunctions = FindViewById<Button> (Resource.Id.entryview_btnCallContentFunctions);
			btnCallContentFunctions.Click += (sender, e) => {
				StartActivity (typeof (CallContentFunctionsActivity));
			};

		}




		public override bool OnKeyDown (Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
		{
			if (keyCode == Keycode.Back ) {

				if (MyWebView.CanGoBack ()) {
					MyWebView.GoBack ();
				}

				return true;
			}

			return base.OnKeyDown (keyCode, e);
		}

		private bool IsUrl(string inputString){

			Regex urlchk = new Regex(@"((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,15})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			return urlchk.IsMatch (inputString);
		}
	}
}


