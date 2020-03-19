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
    [Register ("CustCellProdCommitmentPerDist")]
    partial class CustCellProdCommitmentPerDist
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCourseCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributorName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblCourseCount != null) {
                lblCourseCount.Dispose ();
                lblCourseCount = null;
            }

            if (lblDistributorName != null) {
                lblDistributorName.Dispose ();
                lblDistributorName = null;
            }
        }
    }
}