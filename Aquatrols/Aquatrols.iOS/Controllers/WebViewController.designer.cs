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
    [Register ("WebViewController")]
    partial class WebViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView AffidavitView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnReject { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSignUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ChildViewAffidavit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBackClick { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAffidavit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAgree { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView WvAffadavit { get; set; }

        [Action ("BtnReject_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnReject_TouchUpInside (UIKit.UIButton sender);

        [Action ("BtnSignUp_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnSignUp_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AffidavitView != null) {
                AffidavitView.Dispose ();
                AffidavitView = null;
            }

            if (btnReject != null) {
                btnReject.Dispose ();
                btnReject = null;
            }

            if (btnSignUp != null) {
                btnSignUp.Dispose ();
                btnSignUp = null;
            }

            if (ChildViewAffidavit != null) {
                ChildViewAffidavit.Dispose ();
                ChildViewAffidavit = null;
            }

            if (imgBackClick != null) {
                imgBackClick.Dispose ();
                imgBackClick = null;
            }

            if (imgCheckBox != null) {
                imgCheckBox.Dispose ();
                imgCheckBox = null;
            }

            if (lblAffidavit != null) {
                lblAffidavit.Dispose ();
                lblAffidavit = null;
            }

            if (lblAgree != null) {
                lblAgree.Dispose ();
                lblAgree = null;
            }

            if (WvAffadavit != null) {
                WvAffadavit.Dispose ();
                WvAffadavit = null;
            }
        }
    }
}