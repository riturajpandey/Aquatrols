using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableViewDistributorResult : UITableViewSource
    {
        public TmDistributorResultController controller;
        List<TmCourseProductVm> tmCourseProductVms;
        private string cellidentifier = "tmDistributorResult";
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableViewDistributorResult"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="tmCourseProductVms">Tm course product vms.</param>
        public TableViewDistributorResult(TmDistributorResultController controller, List<TmCourseProductVm> tmCourseProductVms)
        {
            this.controller = controller;
            this.tmCourseProductVms = tmCourseProductVms;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellDistributorResult cell = tableView.DequeueReusableCell(cellidentifier) as CustCellDistributorResult;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellDistributorResult;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmCourseProductVms[indexPath.Row]);
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
            return tmCourseProductVms.Count;
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
                var a = tmCourseProductVms[indexPath.Row].tmProductDetail;
                rowHeight = (50 * a.Count) + 100;
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetHeightForRow, AppResources.TableViewDistributorResult, null);
            }

            return rowHeight;
   
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
