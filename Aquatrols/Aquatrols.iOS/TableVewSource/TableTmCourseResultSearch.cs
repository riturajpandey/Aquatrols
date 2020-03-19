using System;
using System.Collections.Generic;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableTmCourseResultSearch : UITableViewSource
    {
        public TmCourseResultController controller;
        List<TmDistributorProductVm> tmDistributorProducts;
        private string cellidentifier = AppResources.IdnTmCourseSearchResult;
        public TableTmCourseResultSearch(TmCourseResultController controller , List<TmDistributorProductVm> tmDistributorProducts)
        {
            this.controller = controller;
            this.tmDistributorProducts = tmDistributorProducts;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellTmCourseResultSearch cell = tableView.DequeueReusableCell(cellidentifier) as CustCellTmCourseResultSearch;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellTmCourseResultSearch;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmDistributorProducts[indexPath.Row]);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tmDistributorProducts.Count;
        }
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return Constant.lstDigit[15];
        }

        public event Action<TmDistributorProductVm> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.tmDistributorProducts[indexPath.Row]);
            }
        }      
    }
}
