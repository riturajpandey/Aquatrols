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
    [Register ("CustCellDistributorResult")]
    partial class CustCellDistributorResult
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQuantity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblDistributorInner { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblDistributorName != null) {
                lblDistributorName.Dispose ();
                lblDistributorName = null;
            }

            if (lblName != null) {
                lblName.Dispose ();
                lblName = null;
            }

            if (lblQuantity != null) {
                lblQuantity.Dispose ();
                lblQuantity = null;
            }

            if (tblDistributorInner != null) {
                tblDistributorInner.Dispose ();
                tblDistributorInner = null;
            }
        }
    }
}