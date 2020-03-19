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
    [Register ("CustCellGiftCard")]
    partial class CustCellGiftCard
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnRedeem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgGiftCardLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAmount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInfoText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProductDesc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProductInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtAmount { get; set; }

        [Action ("BtnRedeem_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnRedeem_TouchUpInside (UIKit.UIButton sender);

        [Action ("txtAmount_DidBeginEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtAmount_DidBeginEditing (UIKit.UITextField sender);

        [Action ("txtAmount_DidEndEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtAmount_DidEndEditing (UIKit.UITextField sender);

        [Action ("txtAmount_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtAmount_EditingChanged (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnRedeem != null) {
                btnRedeem.Dispose ();
                btnRedeem = null;
            }

            if (imgGiftCardLogo != null) {
                imgGiftCardLogo.Dispose ();
                imgGiftCardLogo = null;
            }

            if (lblAmount != null) {
                lblAmount.Dispose ();
                lblAmount = null;
            }

            if (lblInfoText != null) {
                lblInfoText.Dispose ();
                lblInfoText = null;
            }

            if (lblProductDesc != null) {
                lblProductDesc.Dispose ();
                lblProductDesc = null;
            }

            if (lblProductInfo != null) {
                lblProductInfo.Dispose ();
                lblProductInfo = null;
            }

            if (txtAmount != null) {
                txtAmount.Dispose ();
                txtAmount = null;
            }
        }
    }
}