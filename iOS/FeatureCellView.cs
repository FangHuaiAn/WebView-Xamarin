// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace WebViewInteraction.iOS
{
	public partial class FeatureCellView : UITableViewCell
	{
		public FeatureCellView (IntPtr handle) : base (handle)
		{
		}

		public void UpdateContentWithName(string name){
			lbName.Text = name;
		}
	}
}
