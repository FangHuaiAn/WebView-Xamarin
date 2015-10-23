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
	[Register ("FeatureCellView")]
	partial class FeatureCellView
	{
		[Outlet]
		UIKit.UIImageView imageNext { get; set; }

		[Outlet]
		UIKit.UILabel lbName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imageNext != null) {
				imageNext.Dispose ();
				imageNext = null;
			}

			if (lbName != null) {
				lbName.Dispose ();
				lbName = null;
			}
		}
	}
}
