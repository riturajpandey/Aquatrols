using System;
using System.Collections.Generic;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using UIKit;
namespace Aquatrols.iOS
{
    public class TableViewTmDistributorResult : UITableViewSource
    {
        public TmDistributorResultController controller;
        List<TmCourseProductVm> tmCourseProductVms;
        private string cellidentifier = AppResources.TmSuperIntendentCell;
        public TableViewTmDistributorResult(TmDistributorResultController controller, List<TmCourseProductVm> tmCourseProductVms)
        {
            this.controller = controller;
            this.tmCourseProductVms = tmCourseProductVms;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellTmCourseResult cell = tableView.DequeueReusableCell(cellidentifier) as CustCellTmCourseResult;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellTmCourseResult;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmCourseProductVms[indexPath.Row]);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tmCourseProductVms.Count;
        }
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return Constant.lstDigit[15];
        }
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
        public event Action<TmCourseProductVm> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.tmCourseProductVms[indexPath.Row]);
            }
        }
    }
}
