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
    [Register ("LoginController")]
    partial class LoginController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLogin { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSignUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgVisibleEye { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAlreadyAccount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDontHaveAccount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblFogotPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblUserName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LoginChildView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView LoginScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtUsername { get; set; }

        [Action ("BtnLogin_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnLogin_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnSignUp_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnSignUp_TouchUpInside (UIKit.UIButton sender);

        [Action ("txtPassword_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtPassword_EditingChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnLogin != null) {
                btnLogin.Dispose ();
                btnLogin = null;
            }

            if (btnSignUp != null) {
                btnSignUp.Dispose ();
                btnSignUp = null;
            }

            if (imgLogo != null) {
                imgLogo.Dispose ();
                imgLogo = null;
            }

            if (imgVisibleEye != null) {
                imgVisibleEye.Dispose ();
                imgVisibleEye = null;
            }

            if (lblAlreadyAccount != null) {
                lblAlreadyAccount.Dispose ();
                lblAlreadyAccount = null;
            }

            if (lblDontHaveAccount != null) {
                lblDontHaveAccount.Dispose ();
                lblDontHaveAccount = null;
            }

            if (lblFogotPassword != null) {
                lblFogotPassword.Dispose ();
                lblFogotPassword = null;
            }

            if (lblPassword != null) {
                lblPassword.Dispose ();
                lblPassword = null;
            }

            if (lblUserName != null) {
                lblUserName.Dispose ();
                lblUserName = null;
            }

            if (LoginChildView != null) {
                LoginChildView.Dispose ();
                LoginChildView = null;
            }

            if (LoginScrollView != null) {
                LoginScrollView.Dispose ();
                LoginScrollView = null;
            }

            if (txtPassword != null) {
                txtPassword.Dispose ();
                txtPassword = null;
            }

            if (txtUsername != null) {
                txtUsername.Dispose ();
                txtUsername = null;
            }
        }
    }
}