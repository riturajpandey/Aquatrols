using System;

using UIKit;

using Foundation;
using SafariServices;
using WebKit;
using Aquatrols.iOS.Helper;
using static Aquatrols.Models.ConfigEntity;
using CoreGraphics;

namespace Aquatrols.iOS.Controllers
{

    public partial class LpRedeemWebViewController : UIViewController
    {


        public LpRedeemWebViewController(IntPtr handle) : base(handle)
        {

        }

        public LpRedeemWebViewController() : base("LpRedeemWebViewController", null)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            BackArrowClick();

            string courseId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CourseId);
            var urlcourse = "https://aquatrols.mp.prime-cloud.com/?lpidentifier=" + courseId;
            //var urlCourseLabel = new UILabel()
            //{
            //    Text = urlcourse,
            //    Font = UIFont.FromName("Papyrus", 14f),
            //    TextColor = UIColor.Black,
            //    TextAlignment = UITextAlignment.Center
            //};
            //urlCourseLabel.Frame = new CGRect(0, 90, this.View.Bounds.Size.Width, 60);
            //urlCourseLabel.LineBreakMode = UILineBreakMode.WordWrap;
            //urlCourseLabel.Lines = 2;


           // View.AddSubview(urlCourseLabel);
            CGRect webRect = new CGRect(0, 60, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height - 60);
            // CGRect webRect = new CGRect(0, 150, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height - 150);
            // CGRect webRect = new CGRect(0, 90, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height - 60);
            WKWebView webView = new WKWebView(webRect, new WKWebViewConfiguration());
            View.AddSubview(webView);






            var url = new NSUrl(urlcourse);
            var request = new NSUrlRequest(url);
            webView.LoadRequest(request);


            int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
            // set font for UILabel
            //Utility.SetFontsforHeader(new UILabel[] { lblTermsAndConditions, }, null, Constant.lstDigit[11], viewWidth);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Gesture implementation for back arrow click
        /// back arrow click to navigate previous page.
        /// </summary>
        public void BackArrowClick()
        {
            UITapGestureRecognizer imgBackArrowClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
            });
            imgBack.UserInteractionEnabled = true;
            imgBack.AddGestureRecognizer(imgBackArrowClicked);
        }

        partial void ImgBack_TouchUpInside(UIButton sender)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set UI size sepcified in iPhone X.
        /// </summary>
        public void SetUISize()
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                if (viewWidth == (int)DeviceScreenSize.ISix)
                {
                    if (viewHeight == Constant.lstDigit[31])
                    {
                        this.View.Frame = new CGRect(View.Frame.X, View.Frame.Y + Constant.lstDigit[10], View.Frame.Width, View.Frame.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                //utility.SaveExceptionHandling(ex, AppResources.SetUISize, AppResources.TermsConditionWebView, null);
            }
        }

        /*
        public override void ViewWillAppear(bool animated)
        {
            //View.BackgroundColor = UIColor.White;
            ParentViewController.TabBarController.TabBar.Hidden = false;
            base.ViewWillAppear(animated);
        }
        */




    }
}

