// This file has been autogenerated from a class added in the UI designer.

using System;
using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using UIKit;
using WebKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
	public partial class LoggedInUserWebViewController : UIViewController
	{
		public LoggedInUserWebViewController (IntPtr handle) : base (handle)
		{
		}

        public LoggedInUserWebViewController() : base("LoggedInUserWebViewController", null)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ImgMenuDeactiveClick();
            LblTermsAndConditionsDeactivateClick();
            LblPrivacyPolicyDeacClick();
            LblFAQDeacClick();
            LblSupportDeacClick();
            LblLogoutDClick();

            //string userName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userName);
            //string passWord = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.password);
            //string url = baseURL + "webaccount/SigninMobile?userName=" + userName + "&password=" + passWord;
            //// View.AddSubview(urlCourseLabel);
            //CGRect webRect = new CGRect(0, 126, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height - 126);
            //// CGRect webRect = new CGRect(0, 150, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height - 150);
            //// CGRect webRect = new CGRect(0, 90, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height - 60);
            //WKWebView webView = new WKWebView(webRect, new WKWebViewConfiguration());
            //View.AddSubview(webView);
            //View.SendSubviewToBack(webView);


            //var nSUrl = new NSUrl(url);
            //var request = new NSUrlRequest(nSUrl);
            //webView.LoadRequest(request);

            // set font for UILabel
            //Utility.SetFontsforHeader(new UILabel[] { lblTermsAndConditions, }, null, Constant.lstDigit[11], viewWidth);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
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

        /// <summary>
        /// Gesture implementation for menu click on deactivate dashboard.
        /// hide or show pop up
        /// </summary>
        public void ImgMenuDeactiveClick()
        {
            UITapGestureRecognizer imgMenuDeactiveClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenu.Hidden == true)
                {
                    popUpMenu.Hidden = false;
                }
                else
                {
                    popUpMenu.Hidden = true;
                }
            });
            ImgMenu.UserInteractionEnabled = true;
            ImgMenu.AddGestureRecognizer(imgMenuDeactiveClicked);
        }

        /// <summary>
        /// Gesture implementation for terms and conditions clcik on deactivate dashboard.
        /// </summary>
        public void LblTermsAndConditionsDeactivateClick()
        {
            UITapGestureRecognizer lblTermsAndConditionsDeactivateClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTermsAndConditions.UserInteractionEnabled = true;
            lblTermsAndConditions.AddGestureRecognizer(lblTermsAndConditionsDeactivateClicked);
        }
        /// <summary>
        /// Gesture implementation for privacy policy click on deactivate dashboard.
        /// </summary>
        public void LblPrivacyPolicyDeacClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacy.UserInteractionEnabled = true;
            lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// Gesture implementation for FAQ click on deactivate dashboard.
        /// </summary>
        public void LblFAQDeacClick()
        {
            UITapGestureRecognizer lblFAQDeacClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
            });
            lblFaq.UserInteractionEnabled = true;
            lblFaq.AddGestureRecognizer(lblFAQDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for Support click on deactivate dashboard.
        /// </summary>
        public void LblSupportDeacClick()
        {
            UITapGestureRecognizer lblSupportDeacClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                AbpoutAppViewController aboutAppViewController = (AbpoutAppViewController)Storyboard.InstantiateViewController("AbpoutAppViewController");
                this.NavigationController.PushViewController(aboutAppViewController, true);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for logout the deactivate dashboard.
        /// Logout user
        /// </summary>
        public void LblLogoutDClick()
        {
            UITapGestureRecognizer lblLogoutDClicked = new UITapGestureRecognizer(() =>
            {
                Utility.ClearCachedData();
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is LoginController)
                    {
                        NavigationController.PopToViewController(item, true);
                        break;
                    }
                }
            });
            lblLogout.UserInteractionEnabled = true;
            lblLogout.AddGestureRecognizer(lblLogoutDClicked);
        }

    }
}