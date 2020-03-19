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
    [Register ("CustCellTotGallon")]
    partial class CustCellTotGallon
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblcountry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProductName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblQty { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblcountry != null) {
                lblcountry.Dispose ();
                lblcountry = null;
            }

            if (lblProductName != null) {
                lblProductName.Dispose ();
                lblProductName = null;
            }

            if (lblQty != null) {
                lblQty.Dispose ();
                lblQty = null;
            }
        }
    }
}