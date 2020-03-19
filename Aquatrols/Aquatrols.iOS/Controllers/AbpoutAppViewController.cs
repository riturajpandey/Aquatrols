using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Foundation;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class AbpoutAppViewController : UIViewController
    {
        public AbpoutAppViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //ImgMenuClick();
            //LblLogoutClick();
            //LblTermsAndConditionsClick();
            //LblPrivacyPolicyClick();
            //LblFAQClick();
            //LblSupportClick();
            ImgBackClick();
        }

        ///// <summary>
        ///// Gesture implementation for Menu Click to hide or show pop up menu.
        ///// </summary>
        //public void ImgMenuClick()
        //{
        //    return;
        //    UITapGestureRecognizer imgMenuClicked = new UITapGestureRecognizer(() =>
        //    {
        //        if (popUpMenuView.Hidden == true)
        //        {
        //            popUpMenuView.Hidden = false;
        //        }
        //        else
        //        {
        //            popUpMenuView.Hidden = true;
        //        }
        //    });
        //    MenuImgView.UserInteractionEnabled = true;
        //    MenuImgView.AddGestureRecognizer(imgMenuClicked);
        //}

        ///// <summary>
        ///// Gesture implementation for logout click.
        ///// Logout user.
        ///// </summary>
        //public void LblLogoutClick()
        //{
        //    UITapGestureRecognizer lblLogoutClicked = new UITapGestureRecognizer(() =>
        //    {
        //        Utility.ClearCachedData();
        //        UIViewController[] uIViewControllers = NavigationController.ViewControllers;
        //        foreach (UIViewController item in uIViewControllers)
        //        {
        //            if (item is LoginController)
        //            {
        //                NavigationController.PopToViewController(item, true);
        //                break;
        //            }
        //        }
        //    });
        //    lblLogout.UserInteractionEnabled = true;
        //    lblLogout.AddGestureRecognizer(lblLogoutClicked);
        //}

        ///// <summary>
        /////Gesture implementation for terms and conditions click.
        ///// </summary>
        //public void LblTermsAndConditionsClick()
        //{
        //    UITapGestureRecognizer lblTermsAndConditionsClicked = new UITapGestureRecognizer(() =>
        //    {
        //        popUpMenuView.Hidden = true;
        //        TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
        //        this.NavigationController.PushViewController(termsConditionWebViewController, true);
        //        NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
        //    });
        //    lblTermsAndConditions.UserInteractionEnabled = true;
        //    lblTermsAndConditions.AddGestureRecognizer(lblTermsAndConditionsClicked);
        //}

        ///// <summary>
        ///// Gesture implementation for privacy policy click.
        ///// </summary>
        //public void LblPrivacyPolicyClick()
        //{
        //    UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
        //    {
        //        popUpMenuView.Hidden = true;
        //        TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
        //        this.NavigationController.PushViewController(termsConditionWebViewController, true);
        //        NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
        //    });
        //    lblPrivacy.UserInteractionEnabled = true;
        //    lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        //}

        ///// <summary>
        ///// Gesture implementation for FAQ click.
        ///// </summary>
        //public void LblFAQClick()
        //{
        //    UITapGestureRecognizer lblFAQClicked = new UITapGestureRecognizer(() =>
        //    {
        //        popUpMenuView.Hidden = true;
        //        TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
        //        this.NavigationController.PushViewController(termsConditionWebViewController, true);
        //        NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
        //    });
        //    lblFaq.UserInteractionEnabled = true;
        //    lblFaq.AddGestureRecognizer(lblFAQClicked);
        //}

        ///// <summary>
        ///// Gesture implementation for Support click.
        ///// </summary>
        //public void LblSupportClick()
        //{
        //    UITapGestureRecognizer lblSupportClicked = new UITapGestureRecognizer(() =>
        //    {
        //        popUpMenuView.Hidden = true;
        //        AbpoutAppViewController termsConditionWebViewController = (AbpoutAppViewController)Storyboard.InstantiateViewController("AbpoutAppViewController");
        //        this.NavigationController.PushViewController(termsConditionWebViewController, true);
        //        NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
        //    });
        //    lblSupport.UserInteractionEnabled = true;
        //    lblSupport.AddGestureRecognizer(lblSupportClicked);
        //}

        /// <summary>
        /// Back button click event handler
        /// </summary>
        public void ImgBackClick()
        {
            UITapGestureRecognizer imgBackClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
            });
            imgBack.UserInteractionEnabled = true;
            imgBack.AddGestureRecognizer(imgBackClicked);
        }

        partial void ImgBack_TouchUpInside(UIButton sender)
        {
            UITapGestureRecognizer imgBackClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
            });
            imgBack.UserInteractionEnabled = true;
            imgBack.AddGestureRecognizer(imgBackClicked);
        }
    }
}