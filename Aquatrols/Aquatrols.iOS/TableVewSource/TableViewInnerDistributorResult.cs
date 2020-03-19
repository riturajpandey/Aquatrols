using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
namespace Aquatrols.iOS
{
    public class TableViewInnerDistributorResult : UITableViewSource
    {
        public TmDistributorResultController controller;
        TmDistributorResultResponseEntity tmDistributorResultResponseEntities;
        private string cellidentifier = AppResources.TmSuperIntendentCell;
        public TableViewInnerDistributorResult(TmDistributorResultController controller, TmDistributorResultResponseEntity tmDistributorResultResponseEntities)
        {
           //this.controller = controller;
           //this.tmDistributorResultResponseEntities = tmDistributorResultResponseEntities;
        }

        //public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        //{
        //    CustCellTmCourseResult cell = tableView.DequeueReusableCell(cellidentifier) as CustCellTmCourseResult;
        //    if (cell == null)
        //    {
        //        cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellTmCourseResult;
        //        cell.SelectionStyle = UITableViewCellSelectionStyle.None;
        //    }
        //    cell.UpdateCell(tmDistributorResultResponseEntities[indexPath.Row]);
        //    return cell;
        //}

        //public override nint RowsInSection(UITableView tableview, nint section)
        //{
        //    return tmDistributorResultResponseEntities.Count;
        //}
        //public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        //{
        //    return Constant.lstDigit[15];
        //}
        ///// <summary>
        ///// Occurs when on row select.
        ///// </summary>
        //public event Action<TmDistributorResultResponseEntity> OnRowSelect;
        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    if (OnRowSelect != null)
        //    {
        //        OnRowSelect(this.tmDistributorResultResponseEntities[indexPath.Row]);
        //    }
        //}
    }
}
