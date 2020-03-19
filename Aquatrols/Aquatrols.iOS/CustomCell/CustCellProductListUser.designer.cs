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
    [Register ("CustCellProductListUser")]
    partial class CustCellProductListUser
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgProductUser { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTextUser { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgProductUser != null) {
                imgProductUser.Dispose ();
                imgProductUser = null;
            }

            if (lblTextUser != null) {
                lblTextUser.Dispose ();
                lblTextUser = null;
            }
        }
    }
}