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
    [Register ("TmCourseSearchController")]
    partial class TmCourseSearchController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCourseSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSuperIntendantSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImgMenu { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCourse { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCourseSearch { get; set; }

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
        UIKit.UILabel lblSuperintendant { get; set; }

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
        UIKit.UITableView tblSuperIntendent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblTmCourse { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCourseName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtSuperIntendant { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwChild { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwHeader { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwMain { get; set; }

        [Action ("BtnCourseSearch_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnCourseSearch_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnSuperIntendantSearch_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnSuperIntendantSearch_TouchUpInside (UIKit.UIButton sender);

        [Action ("TxtTmCourse_textChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TxtTmCourse_textChanged (UIKit.UITextField sender);

        [Action ("TxtTmSuperIntendent_TextChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TxtTmSuperIntendent_TextChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCourseSearch != null) {
                btnCourseSearch.Dispose ();
                btnCourseSearch = null;
            }

            if (btnSuperIntendantSearch != null) {
                btnSuperIntendantSearch.Dispose ();
                btnSuperIntendantSearch = null;
            }

            if (imgBack != null) {
                imgBack.Dispose ();
                imgBack = null;
            }

            if (ImgMenu != null) {
                ImgMenu.Dispose ();
                ImgMenu = null;
            }

            if (lblCourse != null) {
                lblCourse.Dispose ();
                lblCourse = null;
            }

            if (lblCourseSearch != null) {
                lblCourseSearch.Dispose ();
                lblCourseSearch = null;
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

            if (lblSuperintendant != null) {
                lblSuperintendant.Dispose ();
                lblSuperintendant = null;
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

            if (tblSuperIntendent != null) {
                tblSuperIntendent.Dispose ();
                tblSuperIntendent = null;
            }

            if (tblTmCourse != null) {
                tblTmCourse.Dispose ();
                tblTmCourse = null;
            }

            if (txtCourseName != null) {
                txtCourseName.Dispose ();
                txtCourseName = null;
            }

            if (txtSuperIntendant != null) {
                txtSuperIntendant.Dispose ();
                txtSuperIntendant = null;
            }

            if (vwChild != null) {
                vwChild.Dispose ();
                vwChild = null;
            }

            if (vwHeader != null) {
                vwHeader.Dispose ();
                vwHeader = null;
            }

            if (vwMain != null) {
                vwMain.Dispose ();
                vwMain = null;
            }
        }
    }
}