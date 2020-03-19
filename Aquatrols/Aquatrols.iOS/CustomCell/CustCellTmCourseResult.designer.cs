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
    [Register ("CustCellTmCourseResult")]
    partial class CustCellTmCourseResult
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTmDistributorResult { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblInnerDistributorResult { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblTmDistributorResult != null) {
                lblTmDistributorResult.Dispose ();
                lblTmDistributorResult = null;
            }

            if (tblInnerDistributorResult != null) {
                tblInnerDistributorResult.Dispose ();
                tblInnerDistributorResult = null;
            }
        }
    }
}