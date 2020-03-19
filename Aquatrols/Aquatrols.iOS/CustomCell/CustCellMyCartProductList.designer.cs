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
    [Register ("CustCellMyCartProductList")]
    partial class CustCellMyCartProductList
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgMyCartProduct { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPoints { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblShowGallonOrLiter { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtMyCartGallons { get; set; }

        [Action ("txtMyCartGallons_DidBeginEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtMyCartGallons_DidBeginEditing (UIKit.UITextField sender);

        [Action ("txtMyCartGallons_DidEndEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtMyCartGallons_DidEndEditing (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (imgMyCartProduct != null) {
                imgMyCartProduct.Dispose ();
                imgMyCartProduct = null;
            }

            if (lblDistributorName != null) {
                lblDistributorName.Dispose ();
                lblDistributorName = null;
            }

            if (lblPoints != null) {
                lblPoints.Dispose ();
                lblPoints = null;
            }

            if (lblShowGallonOrLiter != null) {
                lblShowGallonOrLiter.Dispose ();
                lblShowGallonOrLiter = null;
            }

            if (txtMyCartGallons != null) {
                txtMyCartGallons.Dispose ();
                txtMyCartGallons = null;
            }
        }
    }
}