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

namespace Aquatrols.iOS.Controllers
{
    [Register ("CourseController")]
    partial class CourseController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAdd { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CourseChildView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Courseheaderview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView CourseScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CourseView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBackCourse { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAddCourse { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCountry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblState { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblStreet { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblZip { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCountry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtState { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtStreetAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtZipCode { get; set; }

        [Action ("BtnAdd_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BtnAdd_TouchUpInside (UIKit.UIButton sender);

        [Action ("txtCity_DidBeginEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtCity_DidBeginEditing (UIKit.UITextField sender);

        [Action ("txtCountry_EditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtCountry_EditingChanged (UIKit.UITextField sender);

        [Action ("txtStreetAddress_DidBeginEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtStreetAddress_DidBeginEditing (UIKit.UITextField sender);

        [Action ("txtZipCode_DidBeginEditing:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtZipCode_DidBeginEditing (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnAdd != null) {
                btnAdd.Dispose ();
                btnAdd = null;
            }

            if (CourseChildView != null) {
                CourseChildView.Dispose ();
                CourseChildView = null;
            }

            if (Courseheaderview != null) {
                Courseheaderview.Dispose ();
                Courseheaderview = null;
            }

            if (CourseScroll != null) {
                CourseScroll.Dispose ();
                CourseScroll = null;
            }

            if (CourseView != null) {
                CourseView.Dispose ();
                CourseView = null;
            }

            if (imgBackCourse != null) {
                imgBackCourse.Dispose ();
                imgBackCourse = null;
            }

            if (lblAddCourse != null) {
                lblAddCourse.Dispose ();
                lblAddCourse = null;
            }

            if (lblCity != null) {
                lblCity.Dispose ();
                lblCity = null;
            }

            if (lblCountry != null) {
                lblCountry.Dispose ();
                lblCountry = null;
            }

            if (lblState != null) {
                lblState.Dispose ();
                lblState = null;
            }

            if (lblStreet != null) {
                lblStreet.Dispose ();
                lblStreet = null;
            }

            if (lblZip != null) {
                lblZip.Dispose ();
                lblZip = null;
            }

            if (txtCity != null) {
                txtCity.Dispose ();
                txtCity = null;
            }

            if (txtCountry != null) {
                txtCountry.Dispose ();
                txtCountry = null;
            }

            if (txtState != null) {
                txtState.Dispose ();
                txtState = null;
            }

            if (txtStreetAddress != null) {
                txtStreetAddress.Dispose ();
                txtStreetAddress = null;
            }

            if (txtZipCode != null) {
                txtZipCode.Dispose ();
                txtZipCode = null;
            }
        }
    }
}