using System;
using System.Collections.Generic;

using Foundation;
using UIKit;
using ObjCRuntime;

using Debug = System.Diagnostics.Debug ;

namespace WebViewInteraction.iOS
{
	public partial class ViewController : UIViewController
	{
		string[] features = {@"Load HTML from Web", @"Load HTML from Local", @"Call WebPage's function", @"Call Native function"};
		string[] segues = {@"moveToLoadRemoteWebViewSegue", @"moveToLoadLocalHTMLViewSegue", @"moveToCallPageFunctonViewSegue", @"moveToCallNativeFunctionViewSegue"};


		public ViewController (IntPtr handle) : base (handle)
		{		
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			FeaturesTableSource source = new FeaturesTableSource(features);
			source.ItemSelected += (object sender, FeatureItemSelectedEventArgs e) => {

				var segueId = segues[e.SelectedRow];

				PerformSegue( segueId, this);

			};
			featuresTable.Source = source;
			featuresTable.RowHeight = 60;
		}

		public override void DidReceiveMemoryWarning ()
		{		
			base.DidReceiveMemoryWarning ();		
			// Release any cached data, images, etc that aren't in use.		
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			switch (segue.Identifier) {
				
				case @"moveToLoadRemoteWebViewSegue":
					{
						var dest = segue.DestinationViewController as LoadRemoteWebViewController;
						if (null != dest) {
							dest.Title = features [0];
						}
					}
					break;
				case @"moveToLoadLocalHTMLViewSegue":
					{
						var dest = segue.DestinationViewController as LoadLocalHTMLViewController;
						if (null != dest) {
							dest.Title = features [1];
						}
					}
					break;
				case @"moveToCallPageFunctonViewSegue":
					{
						var dest = segue.DestinationViewController as CallPageFunctonViewController;
						if (null != dest) {
							dest.Title = features [2];
						}
					}
					break;
				case @"moveToCallNativeFunctionViewSegue":
					{
						var dest = segue.DestinationViewController as CallNativeFunctionViewController;
						if (null != dest) {
							dest.Title = features [3];
						}
					}
					break;

				}

		}


		public class FeaturesTableSource : UITableViewSource{

			private List<string> Features{ get; set;}

			string cellIdentifier = @"FeatureCellView";

			public FeaturesTableSource( IEnumerable<string> dataSource)
			{
				Features = new List<string >( dataSource );
			}
				

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return Features.Count ;
			}


			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				FeatureCellView featureCellView = tableView.DequeueReusableCell (cellIdentifier) as FeatureCellView ;

				if ( null != featureCellView) {

					var name = Features [indexPath.Row] ?? @"";

					featureCellView.UpdateContentWithName (name);

					return featureCellView;
				}


				UITableViewCell defailtCell = tableView.DequeueReusableCell (cellIdentifier);

				if (defailtCell == null)
					defailtCell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);

				return defailtCell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				FeatureItemSelectedEventArgs args = new FeatureItemSelectedEventArgs ();
				args.SelectedRow = indexPath.Row;

				OnItemSelected (args);
			}

			public event EventHandler<FeatureItemSelectedEventArgs> ItemSelected;

			public virtual void OnItemSelected(FeatureItemSelectedEventArgs e){
				EventHandler<FeatureItemSelectedEventArgs> handler = ItemSelected;

				Debug.WriteLine (@"ROW:" + e.SelectedRow);

				if (null != handler) {
					handler( this, e);
				}

			}
		}

		public class FeatureItemSelectedEventArgs : EventArgs{
			public int SelectedRow{ get; set;}
		}

	}
}
