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
    [Register ("CustCellTmCourseResultSearch")]
    partial class CustCellTmCourseResultSearch
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProductName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQuantity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTmDistributorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblInnerProduct { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (lblProductName != null) {
                lblProductName.Dispose ();
                lblProductName = null;
            }

            if (lblQuantity != null) {
                lblQuantity.Dispose ();
                lblQuantity = null;
            }

            if (lblTmDistributorName != null) {
                lblTmDistributorName.Dispose ();
                lblTmDistributorName = null;
            }

            if (tblInnerProduct != null) {
                tblInnerProduct.Dispose ();
                tblInnerProduct = null;
            }
        }
    }
}