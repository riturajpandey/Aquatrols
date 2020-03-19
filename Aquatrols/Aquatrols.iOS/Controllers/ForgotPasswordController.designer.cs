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

namespace Aquatrols.iOS.Controllers
{
    [Register ("ForgotPasswordController")]
    partial class ForgotPasswordController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCancelEmailInput { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCancelOTP { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCancelResetPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNextEmailInput { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNextOTP { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnReset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView EmailInputView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ForgotPasswordView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgResetConfirmPasswordEye { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgResetPasswordEye { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblcode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCommentPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEmail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEmailAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblReceive { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblResendPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblresetPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSendVerificationCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblShare { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblVerific { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblVerification { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView OTPView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ResetPasswordView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtConfirmPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtEmailAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtOTPCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPassword { get; set; }

        [Action ("BtnCancelEmailInput_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnCancelEmailInput_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnCancelOTP_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnCancelOTP_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnCancelResetPassword_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnCancelResetPassword_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnNextEmailInput_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnNextEmailInput_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnNextOTP_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnNextOTP_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnReset_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnReset_TouchUpInside (UIKit.UIButton sender);

        [Action ("txtResetCPassword_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtResetCPassword_EditingChanged (UIKit.UITextField sender);

        [Action ("txtResetPassword_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtResetPassword_EditingChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCancelEmailInput != null) {
                btnCancelEmailInput.Dispose ();
                btnCancelEmailInput = null;
            }

            if (btnCancelOTP != null) {
                btnCancelOTP.Dispose ();
                btnCancelOTP = null;
            }

            if (btnCancelResetPassword != null) {
                btnCancelResetPassword.Dispose ();
                btnCancelResetPassword = null;
            }

            if (btnNextEmailInput != null) {
                btnNextEmailInput.Dispose ();
                btnNextEmailInput = null;
            }

            if (btnNextOTP != null) {
                btnNextOTP.Dispose ();
                btnNextOTP = null;
            }

            if (btnReset != null) {
                btnReset.Dispose ();
                btnReset = null;
            }

            if (EmailInputView != null) {
                EmailInputView.Dispose ();
                EmailInputView = null;
            }

            if (ForgotPasswordView != null) {
                ForgotPasswordView.Dispose ();
                ForgotPasswordView = null;
            }

            if (imgResetConfirmPasswordEye != null) {
                imgResetConfirmPasswordEye.Dispose ();
                imgResetConfirmPasswordEye = null;
            }

            if (imgResetPasswordEye != null) {
                imgResetPasswordEye.Dispose ();
                imgResetPasswordEye = null;
            }

            if (lblcode != null) {
                lblcode.Dispose ();
                lblcode = null;
            }

            if (lblCommentPassword != null) {
                lblCommentPassword.Dispose ();
                lblCommentPassword = null;
            }

            if (lblEmail != null) {
                lblEmail.Dispose ();
                lblEmail = null;
            }

            if (lblEmailAddress != null) {
                lblEmailAddress.Dispose ();
                lblEmailAddress = null;
            }

            if (lblReceive != null) {
                lblReceive.Dispose ();
                lblReceive = null;
            }

            if (lblResendPassword != null) {
                lblResendPassword.Dispose ();
                lblResendPassword = null;
            }

            if (lblresetPassword != null) {
                lblresetPassword.Dispose ();
                lblresetPassword = null;
            }

            if (lblSendVerificationCode != null) {
                lblSendVerificationCode.Dispose ();
                lblSendVerificationCode = null;
            }

            if (lblShare != null) {
                lblShare.Dispose ();
                lblShare = null;
            }

            if (lblVerific != null) {
                lblVerific.Dispose ();
                lblVerific = null;
            }

            if (lblVerification != null) {
                lblVerification.Dispose ();
                lblVerification = null;
            }

            if (OTPView != null) {
                OTPView.Dispose ();
                OTPView = null;
            }

            if (ResetPasswordView != null) {
                ResetPasswordView.Dispose ();
                ResetPasswordView = null;
            }

            if (txtConfirmPassword != null) {
                txtConfirmPassword.Dispose ();
                txtConfirmPassword = null;
            }

            if (txtEmailAddress != null) {
                txtEmailAddress.Dispose ();
                txtEmailAddress = null;
            }

            if (txtOTPCode != null) {
                txtOTPCode.Dispose ();
                txtOTPCode = null;
            }

            if (txtPassword != null) {
                txtPassword.Dispose ();
                txtPassword = null;
            }
        }
    }
}