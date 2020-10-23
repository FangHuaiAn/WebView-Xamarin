// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace WebViewInteraction.iOS
{
	[Register ("CallPageFunctonViewController")]
	partial class CallPageFunctonViewController
	{
		[Outlet]
		UIKit.UITextField txtMessage { get; set; }

		[Outlet]
		WebKit.WKWebView webView { get; set; }

		[Action ("btnMessageClicked:")]
		partial void btnMessageClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (webView != null) {
				webView.Dispose ();
				webView = null;
			}

			if (txtMessage != null) {
				txtMessage.Dispose ();
				txtMessage = null;
			}
		}
	}
}
