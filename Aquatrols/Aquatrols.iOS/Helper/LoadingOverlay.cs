using CoreGraphics;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public class LoadingOverlay : UIView
	{
		// control declarations
		private UIActivityIndicatorView activitySpinner;
		/// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.LoadingOverlay"/> class.
        /// </summary>
        /// <param name="frame">Frame.</param>
		public LoadingOverlay (CGRect frame) : base (frame)
		{
			// configurable bits
			BackgroundColor = UIColor.Black;
			Alpha = 0.75f;
			AutoresizingMask = UIViewAutoresizing.All;
            nfloat labelHeight = 50;
			nfloat labelWidth = Frame.Width - 20;
            // derive the center x and y
			nfloat centerX = Frame.Width / 2;
			nfloat centerY = Frame.Height / 2;
            // create the activity spinner, center it horizontall and put it 5 points above center x
            activitySpinner = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new CGRect (
				centerX - activitySpinner.Frame.Width/2 ,
				centerY - activitySpinner.Frame.Height/2 ,
				activitySpinner.Frame.Width,
				activitySpinner.Frame.Height);
			activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
			AddSubview (activitySpinner);
			activitySpinner.StartAnimating ();
		}
        /// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Hide ()
		{
			UIView.Animate (
				0.5, // duration
				() => {
					Alpha = 0;
				},
				() => {
					 RemoveFromSuperview ();
				}
			);
		}
        /// <summary>
        /// method to hide overlay for second
        /// </summary>
        /// <param name="second"></param>
		public void Hide (double second)
		{
			UIView.Animate (
				second, // duration
				() => {
					Alpha = 0;
				},
				() => {
					RemoveFromSuperview ();
				}
			);
		}
    }
}

