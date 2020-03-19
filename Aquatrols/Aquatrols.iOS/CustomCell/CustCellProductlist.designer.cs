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
    [Register ("CustCellProductlist")]
    partial class CustCellProductlist
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAddToCart { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCommit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgProduct { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEnterAmount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblpoint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtGallons { get; set; }

        [Action ("btnAddToCart_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnAddToCart_TouchUpInside (UIKit.UIButton sender);

        [Action ("btnCommit_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnCommit_TouchUpInside (UIKit.UIButton sender);

        [Action ("txtGallons_DidBeginEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtGallons_DidBeginEditing (UIKit.UITextField sender);

        [Action ("txtGallons_DidEndEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtGallons_DidEndEditing (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnAddToCart != null) {
                btnAddToCart.Dispose ();
                btnAddToCart = null;
            }

            if (btnCommit != null) {
                btnCommit.Dispose ();
                btnCommit = null;
            }

            if (imgProduct != null) {
                imgProduct.Dispose ();
                imgProduct = null;
            }

            if (lblEnterAmount != null) {
                lblEnterAmount.Dispose ();
                lblEnterAmount = null;
            }

            if (lblpoint != null) {
                lblpoint.Dispose ();
                lblpoint = null;
            }

            if (lblText != null) {
                lblText.Dispose ();
                lblText = null;
            }

            if (txtGallons != null) {
                txtGallons.Dispose ();
                txtGallons = null;
            }
        }
    }
}