using Aquatrols.Models;
using Foundation;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellInnerDistributorReuslt : UITableViewCell
    {
        public CustCellInnerDistributorReuslt (IntPtr handle) : base (handle)
        {
        }
        internal void UpdateCell(TmCourseProductVm tmCourseProductVm)
        {
            lblProductName.Text = tmCourseProductVm.courseName;
           // lblQuantity.Text = tmCourseProductVm.tmProductDetail + " " + tmCourseProductVm.tmProductDetai;
        }
    }
}