using Aquatrols.Models;
using Foundation;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellTmCourseResult : UITableViewCell
    {
        public CustCellTmCourseResult(IntPtr handle) : base(handle)
        {
        }
        internal void UpdateCell(TmDistributors tmDistributors)
        {
            lblTmDistributorResult.Text = tmDistributors.distributorName;
            //TableTmInnerDistributorResult tableViewTmInner = new TableTmInnerDistributorResult(tmDistributorProduct.tmProductDetail);
            //tblInnerDistributorResult.Source = tableViewTmInner;
            //tblInnerDistributorResult.ReloadData();
            //tblInnerDistributorResult.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }
    }
}