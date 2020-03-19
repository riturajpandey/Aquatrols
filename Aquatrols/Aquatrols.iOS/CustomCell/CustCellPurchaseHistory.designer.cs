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
    [Register ("CustCellPurchaseHistory")]
    partial class CustCellPurchaseHistory
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProductName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQuantityValue { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblDistributor != null) {
                lblDistributor.Dispose ();
                lblDistributor = null;
            }

            if (lblProductName != null) {
                lblProductName.Dispose ();
                lblProductName = null;
            }

            if (lblQuantityValue != null) {
                lblQuantityValue.Dispose ();
                lblQuantityValue = null;
            }
        }
    }
}