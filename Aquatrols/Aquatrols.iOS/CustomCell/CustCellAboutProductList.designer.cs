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
    [Register ("CustCellAboutProductList")]
    partial class CustCellAboutProductList
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgProductlist { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgProductlist != null) {
                imgProductlist.Dispose ();
                imgProductlist = null;
            }
        }
    }
}