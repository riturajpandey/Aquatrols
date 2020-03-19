// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Aquatrols.iOS
{
    [Register ("LoggedInUserWebViewController")]
    partial class LoggedInUserWebViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImgMenu { get; set; }

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
        UIKit.UILabel lblSupport { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTermsAndConditions { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView popUpMenu { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ImgMenu != null) {
                ImgMenu.Dispose ();
                ImgMenu = null;
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
        }
    }
}