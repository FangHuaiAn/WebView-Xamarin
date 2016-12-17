using System;

using Android.OS;
using Android.App;
using Android.Webkit;
using Android.Widget;
using Android.Content;
using Android.Views.InputMethods;
using static System.Console;


namespace WebViewInteraction.Droid
{
	[Activity(Label = "CallContentFunctionsActivity")]
	public class CallContentFunctionsActivity : Activity
	{
		private WebView MyWebView { get; set; }
		private EditText TxtUrl { get; set; }
		private Button BtnGo { get; set; }
		private InputMethodManager _InputMethodManager;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			//
			SetContentView(Resource.Layout.Main);


			//
			TxtUrl = FindViewById<EditText>(Resource.Id.txtUrl);

			_InputMethodManager =
				(InputMethodManager)GetSystemService(Context.InputMethodService);
			_InputMethodManager.HideSoftInputFromWindow(
					TxtUrl.WindowToken,
					HideSoftInputFlags.None);

			//
			var client = new ContentWebViewClient();

			client.WebViewLocaitonChanged += (sender, e) =>
			{

				WriteLine($"{ e.CommandString }");
			};

			// 
			MyWebView = FindViewById<WebView>(Resource.Id.webview);
			MyWebView.SetWebViewClient(client);
			MyWebView.Settings.JavaScriptEnabled = true;

			// 負責與頁面溝通 - Native -> WebView
			JavaScriptResult callResult = new JavaScriptResult();
			callResult.JavaScriptResultReceived += (object sender, JavaScriptResult.JavaScriptResultReceivedEventArgs e) =>
			{

				WriteLine(e.Result);

				RunOnUiThread(() =>
				{
					TxtUrl.Text = e.Result;
				});

			};

			//
			BtnGo = FindViewById<Button>(Resource.Id.btnGo);
			BtnGo.Click += (sender, e) =>
			{
				RunOnUiThread(() =>
				{
					MyWebView.EvaluateJavascript(@"msg( 1234  );", callResult);
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
								function msg( text ){ return text + ' received';   }
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

		// Call Page
		public class JavaScriptResult : Java.Lang.Object, IValueCallback
		{

			public void OnReceiveValue(Java.Lang.Object result)
			{
				Java.Lang.String json = (Java.Lang.String)result;

				var resultString = json.ToString();

				EventHandler<JavaScriptResultReceivedEventArgs> handler =
					JavaScriptResultReceived;

				if (null != handler)
				{
					handler(this,
						new JavaScriptResultReceivedEventArgs
						{
							Result = resultString ?? ""
						});
				}


			}

			public event EventHandler<JavaScriptResultReceivedEventArgs> JavaScriptResultReceived;

			public class JavaScriptResultReceivedEventArgs : EventArgs
			{
				public string Result { get; set; }
			}

		}

	}
}
