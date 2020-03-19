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
    [Register ("RegistrationSuccessViewController")]
    partial class RegistrationSuccessViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnOk { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RegistrationSuccessView { get; set; }

        [Action ("BtnOk_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnOk_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BtnOk != null) {
                BtnOk.Dispose ();
                BtnOk = null;
            }

            if (imgLogo != null) {
                imgLogo.Dispose ();
                imgLogo = null;
            }

            if (RegistrationSuccessView != null) {
                RegistrationSuccessView.Dispose ();
                RegistrationSuccessView = null;
            }
        }
    }
}