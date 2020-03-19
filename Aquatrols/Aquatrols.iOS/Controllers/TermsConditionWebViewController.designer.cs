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
    [Register ("TermsConditionWebViewController")]
    partial class TermsConditionWebViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBackClick { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTermsAndConditions { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TermConditionsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TermsConditionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView WvTerms { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgBackClick != null) {
                imgBackClick.Dispose ();
                imgBackClick = null;
            }

            if (lblTermsAndConditions != null) {
                lblTermsAndConditions.Dispose ();
                lblTermsAndConditions = null;
            }

            if (TermConditionsView != null) {
                TermConditionsView.Dispose ();
                TermConditionsView = null;
            }

            if (TermsConditionView != null) {
                TermsConditionView.Dispose ();
                TermsConditionView = null;
            }

            if (WvTerms != null) {
                WvTerms.Dispose ();
                WvTerms = null;
            }
        }
    }
}