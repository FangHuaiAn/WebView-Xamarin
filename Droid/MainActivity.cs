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

		private InputMethodManager _InputMethodManager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			#region WebView

			var client = new ContentWebViewClient ();

			client.WebViewLocaitonChanged += (object sender, ContentWebViewClient.WebViewLocaitonChangedEventArgs e) => {

				Debug.WriteLine(  e.CommandString );
			};

			client.WebViewLoadCompleted += (object sender, ContentWebViewClient.WebViewLoadCompletedEventArgs e) => {

				RunOnUiThread(()=>{

					AndHUD.Shared.Dismiss(this);

				});

			};


			MyWebView = FindViewById <WebView>(Resource.Id.webview);
			// NOTICE : 先換成一般的 WebViewClient
			MyWebView.SetWebViewClient(client);
			MyWebView.Settings.JavaScriptEnabled = true;
			MyWebView.Settings.UserAgentString = @"Android";

			// 負責與頁面溝通 - WebView -> Native
			MyJSInterface myJSInterface = new MyJSInterface( this );

			MyWebView.AddJavascriptInterface(myJSInterface, "TP");
			myJSInterface.CallFromPageReceived += delegate (object sender, MyJSInterface.CallFromPageReceivedEventArgs e) {
				Debug.WriteLine(e.Result);
			};

			// 負責與頁面溝通 - Native -> WebView
			JavaScriptResult callResult = new JavaScriptResult ();
			callResult.JavaScriptResultReceived += (object sender, JavaScriptResult.JavaScriptResultReceivedEventArgs e) => {

				Debug.WriteLine(e.Result);
			};


			// 載入一般網頁
			//MyWebView.LoadUrl ("http://stackoverflow.com/");
			// 載入以下程式碼進行互動


			MyWebView.LoadDataWithBaseURL (
				null
				,@"<html>
						<head>
							<title>Local String</title>
							<style type='text/css'>p{font-family : Verdana; color : purple }</style>
							<script language='JavaScript'> 
								var lookup = '中文訊息'
								function msg(){ window.location = 'callfrompage://Hi'  }
							</script>
						</head>
						<body><p>Hello World!</p><br />
							<button type='button' onclick='TP.CallFromPage(lookup)' text='Hi'>Hi</button>
						</body>
					</html>"
				, "text/html"
				, "utf-8"
				,null);



			#endregion

			#region EditText

			_InputMethodManager  = 
				(InputMethodManager)GetSystemService(Context.InputMethodService);


			//
			TxtUrl = FindViewById<EditText> (Resource.Id.txtUrl);

			TxtUrl.TextChanged += (object sender,
				Android.Text.TextChangedEventArgs e) => {

				Debug.WriteLine( TxtUrl.Text +":"+ e.Text );



			};

			#endregion


			BtnGo = FindViewById<Button> (Resource.Id.btnGo);
			BtnGo.Click += (object sender, EventArgs e) => {

				MyWebView.EvaluateJavascript( @"alert('Hi');", callResult );
				/*
				var url = TxtUrl.Text.Trim() ;

				AlertDialog.Builder alert = new AlertDialog.Builder (this);
				alert.SetTitle (url);
				alert.SetNegativeButton( "取消", (senderAlert, args) =>{


				});
				alert.SetPositiveButton( "確認", (senderAlert, args) =>{

					RunOnUiThread(
						()=>{
							AndHUD.Shared.Show(this, "Status Message", -1, MaskType.Clear);
						}

					);

					MyWebView.LoadUrl (url);

				});

				RunOnUiThread (() => {
					alert.Show();
				} );
				*/
				// 
				_InputMethodManager.HideSoftInputFromWindow( 
					TxtUrl.WindowToken, 
					HideSoftInputFlags.None );

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

		// Call from Page
		public class MyJSInterface : Java.Lang.Object
		{
			Context Context { get; set; }

			public MyJSInterface (Context context)
			{
				this.Context = context;
			}

			[Export]
			[JavascriptInterface]
			public void CallFromPage (string parameter){
				Debug.WriteLine ($"CallFromPage:{parameter}");

				EventHandler<CallFromPageReceivedEventArgs> handler = 
					CallFromPageReceived;

				if (null != handler) {
					handler (this, 
						new CallFromPageReceivedEventArgs{ 
							Result = parameter });
				}
			}

			public event EventHandler<CallFromPageReceivedEventArgs> CallFromPageReceived;

			public class CallFromPageReceivedEventArgs : EventArgs{
				public string Result { get; set; }
			} 
		}


		// Call Page
		public class JavaScriptResult : Java.Lang.Object, IValueCallback{

			public void OnReceiveValue(Java.Lang.Object result) {
				Java.Lang.String json = (Java.Lang.String)result;

				var resultString = json.ToString();

				EventHandler<JavaScriptResultReceivedEventArgs> handler = 
					JavaScriptResultReceived;

				if (null != handler) {
					handler (this, 
						new JavaScriptResultReceivedEventArgs{ 
							Result = resultString ?? "" });
				}


			}

			public event EventHandler<JavaScriptResultReceivedEventArgs> JavaScriptResultReceived;

			public class JavaScriptResultReceivedEventArgs : EventArgs{
				public string Result { get; set; }
			} 

		}

		public class ContentWebViewClient : WebViewClient
		{

			public event EventHandler<WebViewLocaitonChangedEventArgs> WebViewLocaitonChanged;

			public event EventHandler<WebViewLoadCompletedEventArgs> WebViewLoadCompleted;

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				EventHandler<WebViewLocaitonChangedEventArgs> handler = 
					WebViewLocaitonChanged;

				if (null != handler) {
					handler (this, 
						new WebViewLocaitonChangedEventArgs{ 
							CommandString = url });
				}

				return base.ShouldOverrideUrlLoading (view, url);

			}

			public override void OnPageFinished (WebView view, string url)
			{
				base.OnPageFinished (view, url);

				EventHandler<WebViewLoadCompletedEventArgs> handler = 
					WebViewLoadCompleted;

				if (null != handler) {
					handler (this, 
						new WebViewLoadCompletedEventArgs());
				}
			}

			public class WebViewLocaitonChangedEventArgs : EventArgs{

				public string CommandString {get;set;}
			}

			public class WebViewLoadCompletedEventArgs : EventArgs{

			}
		}
	}
}


