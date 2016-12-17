
using Android.OS;
using Android.App;
using Android.Webkit;
using Android.Widget;

namespace WebViewInteraction.Droid
{
	[Activity(Label = "顯示本地端 HTML 內容")]
	public class LoadLocalHtmlActivity : Activity
	{
		private WebView MyWebView { get; set; }
		private EditText TxtUrl { get; set; }
		private Button BtnGo { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			// View Binding
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Main);

			//

			MyWebView = FindViewById<WebView>(Resource.Id.webview);

			MyWebView.SetWebViewClient(new MyWebViewClient());

			MyWebView.Settings.JavaScriptEnabled = true;
			MyWebView.Settings.UserAgentString = @"Android";

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
	}
}
