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
    [Register ("CustCellGiftCardUser")]
    partial class CustCellGiftCardUser
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgGiftCardItem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInfoTextUser { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProductDescUser { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProductInfoUser { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgGiftCardItem != null) {
                imgGiftCardItem.Dispose ();
                imgGiftCardItem = null;
            }

            if (lblInfoTextUser != null) {
                lblInfoTextUser.Dispose ();
                lblInfoTextUser = null;
            }

            if (lblProductDescUser != null) {
                lblProductDescUser.Dispose ();
                lblProductDescUser = null;
            }

            if (lblProductInfoUser != null) {
                lblProductInfoUser.Dispose ();
                lblProductInfoUser = null;
            }
        }
    }
}