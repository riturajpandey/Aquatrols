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
    [Register ("TmDistributorResultController")]
    partial class TmDistributorResultController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDownLoadExcelDistributor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnShareExcel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDistributorSearchResult { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblErrormsg { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProgramCommitments { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblDistributorResult { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwChildContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vwHeader { get; set; }

        [Action ("btnDownLoadExcelDistributor_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnDownLoadExcelDistributor_TouchUpInside (UIKit.UIButton sender);

        [Action ("btnShareExcel_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnShareExcel_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnDownLoadExcelDistributor != null) {
                btnDownLoadExcelDistributor.Dispose ();
                btnDownLoadExcelDistributor = null;
            }

            if (btnShareExcel != null) {
                btnShareExcel.Dispose ();
                btnShareExcel = null;
            }

            if (imgBack != null) {
                imgBack.Dispose ();
                imgBack = null;
            }

            if (lblDistributor != null) {
                lblDistributor.Dispose ();
                lblDistributor = null;
            }

            if (lblDistributorSearchResult != null) {
                lblDistributorSearchResult.Dispose ();
                lblDistributorSearchResult = null;
            }

            if (lblErrormsg != null) {
                lblErrormsg.Dispose ();
                lblErrormsg = null;
            }

            if (lblProgramCommitments != null) {
                lblProgramCommitments.Dispose ();
                lblProgramCommitments = null;
            }

            if (tblDistributorResult != null) {
                tblDistributorResult.Dispose ();
                tblDistributorResult = null;
            }

            if (vwChildContent != null) {
                vwChildContent.Dispose ();
                vwChildContent = null;
            }

            if (vwHeader != null) {
                vwHeader.Dispose ();
                vwHeader = null;
            }
        }
    }
}