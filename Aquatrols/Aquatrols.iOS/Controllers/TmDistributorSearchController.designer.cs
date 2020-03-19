// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Aquatrols.iOS
{
    [Register ("TmDistributorSearchController")]
    partial class TmDistributorSearchController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImgMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributorSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblFaq { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLogout { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPrivacy { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblReviewInformation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSupport { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTermsAndConditions { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView popUpMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtDistributorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtStateName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwChildContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwHeader { get; set; }

        [Action ("BtnSearch_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnSearch_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnSearch != null) {
                btnSearch.Dispose ();
                btnSearch = null;
            }

            if (imgBack != null) {
                imgBack.Dispose ();
                imgBack = null;
            }

            if (ImgMenu != null) {
                ImgMenu.Dispose ();
                ImgMenu = null;
            }

            if (lblDistributor != null) {
                lblDistributor.Dispose ();
                lblDistributor = null;
            }

            if (lblDistributorSearch != null) {
                lblDistributorSearch.Dispose ();
                lblDistributorSearch = null;
            }

            if (lblFaq != null) {
                lblFaq.Dispose ();
                lblFaq = null;
            }

            if (lblLogout != null) {
                lblLogout.Dispose ();
                lblLogout = null;
            }

            if (lblPrivacy != null) {
                lblPrivacy.Dispose ();
                lblPrivacy = null;
            }

            if (lblReviewInformation != null) {
                lblReviewInformation.Dispose ();
                lblReviewInformation = null;
            }

            if (lblSupport != null) {
                lblSupport.Dispose ();
                lblSupport = null;
            }

            if (lblTermsAndConditions != null) {
                lblTermsAndConditions.Dispose ();
                lblTermsAndConditions = null;
            }

            if (popUpMenu != null) {
                popUpMenu.Dispose ();
                popUpMenu = null;
            }

            if (txtDistributorName != null) {
                txtDistributorName.Dispose ();
                txtDistributorName = null;
            }

            if (txtStateName != null) {
                txtStateName.Dispose ();
                txtStateName = null;
            }

            if (vwChildContent != null) {
                vwChildContent.Dispose ();
                vwChildContent = null;
            }

            if (vwHeader != null) {
                vwHeader.Dispose ();
                vwHeader = null;
            }
        }
    }
}