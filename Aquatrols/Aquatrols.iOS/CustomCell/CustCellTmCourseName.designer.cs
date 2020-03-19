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
    [Register ("CustCellTmCourseName")]
    partial class CustCellTmCourseName
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTmCourseName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblTmCourseName != null) {
                lblTmCourseName.Dispose ();
                lblTmCourseName = null;
            }
        }
    }
}