using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.TableVewSource
{
    public class TableViewPurchaseHistory: UITableViewSource
    {
        public MyAccountController myAccountController;
        public List<PurchaseHistoryEntity> lstPurchaseHistory;
        private string cellidentifier = AppResources.PurchaseHistory;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TablevVewSource.TableViewProductList"/> class.
        /// </summary>
        /// <param name="myAccountController">Controller.</param>
        public TableViewPurchaseHistory(MyAccountController myAccountController, List<PurchaseHistoryEntity> lstPurchaseHistory)
        {
            this.myAccountController = myAccountController;
            this.lstPurchaseHistory = lstPurchaseHistory;
        }
        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return lstPurchaseHistory.Count;
        }
        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return Constant.lstDigit[14];
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellPurchaseHistory cell = tableView.DequeueReusableCell(cellidentifier) as CustCellPurchaseHistory;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellPurchaseHistory;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            cell.UpdateCell(lstPurchaseHistory[indexPath.Row], this.myAccountController);
            return cell;
        }
    }
}
