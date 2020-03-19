using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableTmCourseSearchResult : UITableViewSource
    {
        public TmCourseResultController controller;
        List<TmDistributorProductVm> tmDistributorProducts;
        private string cellidentifier = AppResources.tmCourseSearchResultCell;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableTmCourseSearchResult"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="tmDistributorProducts">Tm distributor products.</param>
        public TableTmCourseSearchResult(TmCourseResultController controller , List<TmDistributorProductVm> tmDistributorProducts)
        {
            this.controller = controller;
            this.tmDistributorProducts = tmDistributorProducts;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellTmCourseResultSearch cell = tableView.DequeueReusableCell(cellidentifier) as CustCellTmCourseResultSearch;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellTmCourseResultSearch;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmDistributorProducts[indexPath.Row],indexPath.Row);
            return cell;
        }
        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tmDistributorProducts.Count;
        }
        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            nfloat rowHeight = 0;
            try
            {
                var a = tmDistributorProducts[indexPath.Row].tmProductDetail;
                rowHeight = (50 * a.Count) + 95;
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetHeightForRow, AppResources.TableTmCourseSearchResult, null);
            }
            return rowHeight;   
        }
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
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
