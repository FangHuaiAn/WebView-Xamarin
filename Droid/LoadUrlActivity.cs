using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Views.InputMethods;

using Java.Interop;
using static System.Console;

using AndroidHUD;

namespace WebViewInteraction.Droid
{
	[Activity (Label = "以 URL 連結網站")]
	public class LoadUrlActivity : Activity
	{
		private WebView MyWebView { get; set; }
		private EditText TxtUrl { get; set; }
		private Button BtnGo { get; set; }

		private InputMethodManager _InputMethodManager;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// 
			SetContentView (Resource.Layout.Main);

			var client = new ContentWebViewClient ();

			client.WebViewLocaitonChanged += (object sender, ContentWebViewClient.WebViewLocaitonChangedEventArgs e) => {
				WriteLine (e.CommandString);
			};

			client.WebViewLoadCompleted += (object sender, ContentWebViewClient.WebViewLoadCompletedEventArgs e) => {

				RunOnUiThread (() => {
					AndHUD.Shared.Dismiss (this);
				});

			};

			MyWebView = FindViewById<WebView> (Resource.Id.webview);

			MyWebView.SetWebViewClient (client);

			MyWebView.Settings.JavaScriptEnabled = true;
			MyWebView.Settings.UserAgentString = @"Android";

			#region EditText

			_InputMethodManager =
				(InputMethodManager)GetSystemService (Context.InputMethodService);



			TxtUrl = FindViewById<EditText> (Resource.Id.txtUrl);

			TxtUrl.TextChanged += (object sender,
				Android.Text.TextChangedEventArgs e) => {
					WriteLine (TxtUrl.Text + ":" + e.Text);

				};


			#endregion


			BtnGo = FindViewById<Button> (Resource.Id.btnGo);
			BtnGo.Click += (object sender, EventArgs e) => {

				// 隱藏鍵盤
				_InputMethodManager.HideSoftInputFromWindow (
					TxtUrl.WindowToken,
					HideSoftInputFlags.None);

				// 
				var url = TxtUrl.Text.Trim ();

				// 詢問是否要過去指定頁面
				AlertDialog.Builder alert = new AlertDialog.Builder (this);
				alert.SetTitle ("Info");
				alert.SetMessage ( $"請問是否前往 { url }" );
				alert.SetNegativeButton ("取消", (senderAlert, args) => {


				});
				alert.SetPositiveButton ("確認", (senderAlert, args) => {

					RunOnUiThread (
						() => {
							AndHUD.Shared.Show (this, "Status Message", -1, MaskType.Clear);
						}

					);

					MyWebView.LoadUrl (url);

				});

				RunOnUiThread (() => {
					alert.Show ();
				});



			};


		}
	}
}
