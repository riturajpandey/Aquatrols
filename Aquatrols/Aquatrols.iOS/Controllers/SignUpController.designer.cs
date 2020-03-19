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
    [Register ("SignUpController")]
    partial class SignUpController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSelectImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView headerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBackArrowSignup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgCPasswordSignUpEye { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgPasswordSignUpEye { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCommentPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCourseAff { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCourseDesc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDSRName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblfirstname { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLastname { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPhoneNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblReferralCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblReferredBy { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSignUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblUsername { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SignUpChildView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView SignupScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SignUpView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblCoursename { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblDistributorList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCourseDesc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCourseName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtDistributorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtDSRName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtFirstname { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtLastName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPhoneNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtReferralCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtUsername { get; set; }

        [Action ("Btn_Img_Select:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Btn_Img_Select (UIKit.UIButton sender);

        [Action ("BtnSubmit_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnSubmit_TouchUpInside (UIKit.UIButton sender);

        [Action ("txtCourseName_Changed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtCourseName_Changed (UIKit.UITextField sender);

        [Action ("txtCPasswordSignUp_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtCPasswordSignUp_EditingChanged (UIKit.UITextField sender);

        [Action ("txtDistributorName_Changed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtDistributorName_Changed (UIKit.UITextField sender);

        [Action ("txtFirstname_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtFirstname_EditingChanged (UIKit.UITextField sender);

        [Action ("txtLastName_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtLastName_EditingChanged (UIKit.UITextField sender);

        [Action ("txtPasswordSignUp_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtPasswordSignUp_EditingChanged (UIKit.UITextField sender);

        [Action ("txtPhoneNumber_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtPhoneNumber_EditingChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnSelectImage != null) {
                btnSelectImage.Dispose ();
                btnSelectImage = null;
            }

            if (btnSubmit != null) {
                btnSubmit.Dispose ();
                btnSubmit = null;
            }

            if (headerView != null) {
                headerView.Dispose ();
                headerView = null;
            }

            if (imgBackArrowSignup != null) {
                imgBackArrowSignup.Dispose ();
                imgBackArrowSignup = null;
            }

            if (imgCPasswordSignUpEye != null) {
                imgCPasswordSignUpEye.Dispose ();
                imgCPasswordSignUpEye = null;
            }

            if (imgPasswordSignUpEye != null) {
                imgPasswordSignUpEye.Dispose ();
                imgPasswordSignUpEye = null;
            }

            if (lblAddress != null) {
                lblAddress.Dispose ();
                lblAddress = null;
            }

            if (lblCName != null) {
                lblCName.Dispose ();
                lblCName = null;
            }

            if (lblCommentPassword != null) {
                lblCommentPassword.Dispose ();
                lblCommentPassword = null;
            }

            if (lblCourseAff != null) {
                lblCourseAff.Dispose ();
                lblCourseAff = null;
            }

            if (lblCourseDesc != null) {
                lblCourseDesc.Dispose ();
                lblCourseDesc = null;
            }

            if (lblCPassword != null) {
                lblCPassword.Dispose ();
                lblCPassword = null;
            }

            if (lblDistributorName != null) {
                lblDistributorName.Dispose ();
                lblDistributorName = null;
            }

            if (lblDSRName != null) {
                lblDSRName.Dispose ();
                lblDSRName = null;
            }

            if (lblfirstname != null) {
                lblfirstname.Dispose ();
                lblfirstname = null;
            }

            if (lblLastname != null) {
                lblLastname.Dispose ();
                lblLastname = null;
            }

            if (lblPassword != null) {
                lblPassword.Dispose ();
                lblPassword = null;
            }

            if (lblPhoneNumber != null) {
                lblPhoneNumber.Dispose ();
                lblPhoneNumber = null;
            }

            if (lblReferralCode != null) {
                lblReferralCode.Dispose ();
                lblReferralCode = null;
            }

            if (lblReferredBy != null) {
                lblReferredBy.Dispose ();
                lblReferredBy = null;
            }

            if (lblSignUp != null) {
                lblSignUp.Dispose ();
                lblSignUp = null;
            }

            if (lblUsername != null) {
                lblUsername.Dispose ();
                lblUsername = null;
            }

            if (SignUpChildView != null) {
                SignUpChildView.Dispose ();
                SignUpChildView = null;
            }

            if (SignupScroll != null) {
                SignupScroll.Dispose ();
                SignupScroll = null;
            }

            if (SignUpView != null) {
                SignUpView.Dispose ();
                SignUpView = null;
            }

            if (tblCoursename != null) {
                tblCoursename.Dispose ();
                tblCoursename = null;
            }

            if (tblDistributorList != null) {
                tblDistributorList.Dispose ();
                tblDistributorList = null;
            }

            if (txtCourseDesc != null) {
                txtCourseDesc.Dispose ();
                txtCourseDesc = null;
            }

            if (txtCourseName != null) {
                txtCourseName.Dispose ();
                txtCourseName = null;
            }

            if (txtCPassword != null) {
                txtCPassword.Dispose ();
                txtCPassword = null;
            }

            if (txtDistributorName != null) {
                txtDistributorName.Dispose ();
                txtDistributorName = null;
            }

            if (txtDSRName != null) {
                txtDSRName.Dispose ();
                txtDSRName = null;
            }

            if (txtFirstname != null) {
                txtFirstname.Dispose ();
                txtFirstname = null;
            }

            if (txtLastName != null) {
                txtLastName.Dispose ();
                txtLastName = null;
            }

            if (txtPassword != null) {
                txtPassword.Dispose ();
                txtPassword = null;
            }

            if (txtPhoneNumber != null) {
                txtPhoneNumber.Dispose ();
                txtPhoneNumber = null;
            }

            if (txtReferralCode != null) {
                txtReferralCode.Dispose ();
                txtReferralCode = null;
            }

            if (txtUsername != null) {
                txtUsername.Dispose ();
                txtUsername = null;
            }
        }
    }
}