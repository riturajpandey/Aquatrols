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
    [Register ("CustCellSnapShotSignupCounts")]
    partial class CustCellSnapShotSignupCounts
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCountCommtments { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCountSignUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblterritoryName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblterritoryNumber { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblCountCommtments != null) {
                lblCountCommtments.Dispose ();
                lblCountCommtments = null;
            }

            if (lblCountSignUp != null) {
                lblCountSignUp.Dispose ();
                lblCountSignUp = null;
            }

            if (lblterritoryName != null) {
                lblterritoryName.Dispose ();
                lblterritoryName = null;
            }

            if (lblterritoryNumber != null) {
                lblterritoryNumber.Dispose ();
                lblterritoryNumber = null;
            }
        }
    }
}