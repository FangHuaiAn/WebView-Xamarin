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

using Java.Interop;
using static System.Console;

namespace WebViewInteraction.Droid
{
	public class MyWebViewClient : WebViewClient { }

	public class ContentWebViewClient : WebViewClient
	{

		public event EventHandler<WebViewLocaitonChangedEventArgs> WebViewLocaitonChanged;

		public event EventHandler<WebViewLoadCompletedEventArgs> WebViewLoadCompleted;

		public override bool ShouldOverrideUrlLoading (WebView view, IWebResourceRequest request)
		{
			var url = request.Url.ToString() ;

			EventHandler<WebViewLocaitonChangedEventArgs> handler =
				WebViewLocaitonChanged;

			if (null != handler) {
				handler (this,
					new WebViewLocaitonChangedEventArgs {
						CommandString = url
					});

				if (url.Contains ("callfrompage://Hi")) {
					return false;
				}

			}



			return base.ShouldOverrideUrlLoading (view, request );

		}


		public override void OnPageFinished (WebView view, string url)
		{
			base.OnPageFinished (view, url);

			EventHandler<WebViewLoadCompletedEventArgs> handler =
				WebViewLoadCompleted;

			if (null != handler) {
				handler (this,
					new WebViewLoadCompletedEventArgs ());
			}
		}

		public class WebViewLocaitonChangedEventArgs : EventArgs
		{

			public string CommandString { get; set; }
		}

		public class WebViewLoadCompletedEventArgs : EventArgs
		{

		}
	}
}
