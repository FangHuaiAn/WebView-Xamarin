using System;

using Android.OS;
using Android.App;
using Android.Webkit;
using Android.Widget;
using Android.Content;

using Java.Interop;
using static System.Console;

namespace WebViewInteraction.Droid
{
	[Activity(Label = "CallPlatformFeaturesActivity")]
	public class CallPlatformFeaturesActivity : Activity
	{
		private WebView MyWebView { get; set; }
		private EditText TxtUrl { get; set; }
		private Button BtnGo { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			//
			SetContentView(Resource.Layout.Main);

			//
			var client = new ContentWebViewClient();

			// 
			MyWebView = FindViewById<WebView>(Resource.Id.webview);
			MyWebView.SetWebViewClient(client);
			MyWebView.Settings.JavaScriptEnabled = true;



			// 負責與頁面溝通 - WebView -> Native
			MyJSInterface myJSInterface = new MyJSInterface(this);

			MyWebView.AddJavascriptInterface(myJSInterface, "TP");
			myJSInterface.CallFromPageReceived += delegate (object sender, MyJSInterface.CallFromPageReceivedEventArgs e)
			{

				WriteLine(e.Result);

				AlertDialog.Builder alert = new AlertDialog.Builder(this);
				alert.SetTitle("Info");
				alert.SetMessage($"回傳內容 { e.Result }");

				alert.SetPositiveButton("確認", (senderAlert, args) => { });

				RunOnUiThread(() =>
				{
					alert.Show();
				});
			};


			MyWebView.LoadDataWithBaseURL(
				null
				, @"<html>
						<head>
							<title>Local String</title>
							<style type='text/css'>p{font-family : Verdana; color : purple }</style>
							<script language='JavaScript'> 
								var lookup = '中文訊息'
								function msg(){ window.location = 'callfrompage://Hi'  }
							</script>
						</head>
						<body><p>Hello World!</p><br />
							<button type='button' onclick='TP.CallFromPage(lookup)' text='Hi From Page'>Hi From Page</button>
						</body>
					</html>"
				, "text/html"
				, "utf-8"
				, null);


		}


		// Call from Page
		public class MyJSInterface : Java.Lang.Object
		{
			Context Context { get; set; }

			public MyJSInterface(Context context)
			{
				Context = context;
			}

			[Export]
			[JavascriptInterface]
			public void CallFromPage(string parameter)
			{
				WriteLine($"CallFromPage:{parameter}");

				EventHandler<CallFromPageReceivedEventArgs> handler =
					CallFromPageReceived;

				if (null != handler)
				{
					handler(this,
						new CallFromPageReceivedEventArgs
						{
							Result = parameter
						});
				}
			}

			public event EventHandler<CallFromPageReceivedEventArgs> CallFromPageReceived;

			public class CallFromPageReceivedEventArgs : EventArgs
			{
				public string Result { get; set; }
			}
		}
	}
}
