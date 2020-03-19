using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using CoreGraphics;
using Foundation;
using System;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file is used for Home Screen of Territory Manager view
    /// </summary>
    public partial class TmViewHomeController : UIViewController
    {
        private int viewWidth;
        public string countryName ,token;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TmViewHomeController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public TmViewHomeController (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
                string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
                if (!String.IsNullOrEmpty(exception))
                {
                    utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.TmViewHome, exception); // exception handling
                }
                ImgHomeClick();
                TmImgCourseClick();
                TmImgDistributorClick();
                tmImgSnapshotbuttonClick();
                SetLogoSize();
                ImgMenuDeactiveClick();
                LblTermsAndConditionsDeactivateClick();
                LblPrivacyPolicyDeacClick();
                LblFAQDeacClick();
                LblSupportDeacClick();
                LblLogoutDClick();
                TmViewHomeClick();
                SetFonts();
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.TmViewHome, null); // exceptin handling 
            }
        }
        /// <summary>
        /// Gesture implementation for back button Click to go back.
        /// </summary>
        public void ImgHomeClick()
        {
            UITapGestureRecognizer imgHomeClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
            });
            tmDashboardHome.UserInteractionEnabled = true;
            tmDashboardHome.AddGestureRecognizer(imgHomeClicked);
        }
        /// <summary>
        /// Event handler for course Buttons touch up inside.
        /// </summary>
        public void TmImgCourseClick()
        {
            UITapGestureRecognizer TmImgCourseClicked = new UITapGestureRecognizer(() =>
            {
                this.PerformSegue(AppResources.SeguefromTmhomeToCourseSearch, this);
            });
            tmImgCourse.UserInteractionEnabled = true;
            tmImgCourse.AddGestureRecognizer(TmImgCourseClicked);
        }
        /// <summary>
        /// Event handler for course Buttons touch up inside.
        /// </summary>
        public void TmImgDistributorClick()
        {
            UITapGestureRecognizer TmImgCourseClicked = new UITapGestureRecognizer(() =>
            {
                this.PerformSegue(AppResources.TmSeguefromDistributorToDistributorSearch, this);
            });
            tmImgDistributor.UserInteractionEnabled = true;
            tmImgDistributor.AddGestureRecognizer(TmImgCourseClicked);
        }
        /// <summary>
        /// Event handler for course Buttons touch up inside.
        /// </summary>
        public void tmImgSnapshotbuttonClick()
        {
            UITapGestureRecognizer TmImgCourseClicked = new UITapGestureRecognizer(() =>
            {
                this.PerformSegue(AppResources.SeguefromTmSnapShoptoSnapShotScreen, this);
            });
            tmImgSnapshot.UserInteractionEnabled = true;
            tmImgSnapshot.AddGestureRecognizer(TmImgCourseClicked);
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
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
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
        /// <summary>
        /// Prepares for segue.
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            try
            {
                if (segue.Identifier == AppResources.SeguefromTmSnapShoptoSnapShotScreen)
                {
                    var navctlr = segue.DestinationViewController as TmSnapResultController;
                    if (navctlr != null)
                    {
                        if (!string.IsNullOrEmpty(countryName))
                        {
                            navctlr.countryName = this.countryName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.PrepareForSegue, AppResources.TmViewHome, null);
            }
        }

        /// <summary>
        /// Set Logo size sepcified in iPhone X.
        /// set UI in iPhone X
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                string device = UIDevice.CurrentDevice.Model;
                viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                if (device == AppResources.iPad)
                {
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        LogoDashBoard.Frame = new CGRect(LogoDashBoard.Frame.X, LogoDashBoard.Frame.Y, LogoDashBoard.Frame.Width - Constant.digitNinteenPointsFive, LogoDashBoard.Frame.Height + Constant.digitEighteenPointsFive);
                    }
                    else
                    {

                        LogoDashBoard.Frame = new CGRect(LogoDashBoard.Frame.X, LogoDashBoard.Frame.Y, LogoDashBoard.Frame.Width - Constant.digitTwelvePointsFive, LogoDashBoard.Frame.Height + Constant.digitElevenPointsFive);
                        lblReviewInformation.Frame = new CGRect(lblReviewInformation.Frame.X, lblReviewInformation.Frame.Y-Constant.lstDigit[4], lblReviewInformation.Frame.Width, lblReviewInformation.Frame.Height- Constant.lstDigit[12]);
                        tmImgCourse.Frame = new CGRect(tmImgCourse.Frame.X, lblReviewInformation.Frame.Y+ lblReviewInformation.Frame.Height, tmImgCourse.Frame.Width-Constant.lstDigit[13], tmImgCourse.Frame.Height+ 56);
                        tmImgDistributor.Frame = new CGRect(tmImgDistributor.Frame.X+ Constant.lstDigit[13], lblReviewInformation.Frame.Y + lblReviewInformation.Frame.Height, tmImgDistributor.Frame.Width- Constant.lstDigit[13], tmImgDistributor.Frame.Height + Constant.digitFiftySixPointFive);
                        tmImgSnapshot.Frame = new CGRect(tmImgSnapshot.Frame.X, tmImgCourse.Frame.Y+ tmImgCourse.Frame.Height+Constant.digitTwentyFive, tmImgSnapshot.Frame.Width, tmImgSnapshot.Frame.Height+ Constant.lstDigit[15]);
                        tmDashboardHome.Frame = new CGRect(tmDashboardHome.Frame.X, tmImgSnapshot.Frame.Y+ tmImgSnapshot.Frame.Height+ Constant.digitTwentyFive, tmDashboardHome.Frame.Width, tmDashboardHome.Frame.Height+ Constant.lstDigit[15]);
             
                    }
                }
                else
                {
                    int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                    if (viewWidth == (int)DeviceScreenSize.ISix)
                    {
                        if (viewHeight == Constant.lstDigit[31])
                        {
                            LogoDashBoard.Frame = new CGRect(Constant.lstDigit[6], Constant.lstDigit[41], Constant.lstDigit[42], Constant.lstDigit[43]);
                            vwHeader.Frame = new CGRect(vwHeader.Frame.X, vwHeader.Frame.Y + Constant.lstDigit[10], vwHeader.Frame.Width, vwHeader.Frame.Height);
                            vwChild.Frame = new CGRect(vwChild.Frame.X, vwChild.Frame.Y + Constant.lstDigit[10], vwChild.Frame.Width, vwChild.Frame.Height);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.setLogoSize, AppResources.TmViewHome, null);
            }
        }
        /// <summary>
        /// Set the fonts of UILabel, UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblReviewInformation}, null, Constant.lstDigit[9], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblTMView }, null, Constant.lstDigit[11], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblLogout, lblPrivacy, lblSupport,lblTermsAndConditions,lblFaq }, null, Constant.lstDigit[8], viewWidth);
     
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.TmViewHome, null);
            }
        }
        /// <summary>
        /// Tms the view home click.
        /// </summary>
        public void TmViewHomeClick()
        {
            UITapGestureRecognizer imgDistributorViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
            });
            TmViewHome.UserInteractionEnabled = true;
            TmViewHome.AddGestureRecognizer(imgDistributorViewClicked);
        }
    }
}