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
    [Register ("CustCellEditGallonProductList")]
    partial class CustCellEditGallonProductList
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgProductGa { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPoints { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtGallon { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgProductGa != null) {
                imgProductGa.Dispose ();
                imgProductGa = null;
            }

            if (lblDistributorName != null) {
                lblDistributorName.Dispose ();
                lblDistributorName = null;
            }

            if (lblPoints != null) {
                lblPoints.Dispose ();
                lblPoints = null;
            }

            if (txtGallon != null) {
                txtGallon.Dispose ();
                txtGallon = null;
            }
        }
    }
}