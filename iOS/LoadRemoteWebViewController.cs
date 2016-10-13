// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace WebViewInteraction.iOS
{
	public partial class LoadRemoteWebViewController : UIViewController
	{
		public LoadRemoteWebViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			webView.LoadRequest (new NSUrlRequest (
				new NSUrl (@"https://www.google.com")));
			// Note : 
			//webView.ScalesPageToFit = true;

		}

		public override void DidReceiveMemoryWarning ()
		{		
			base.DidReceiveMemoryWarning ();		
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
