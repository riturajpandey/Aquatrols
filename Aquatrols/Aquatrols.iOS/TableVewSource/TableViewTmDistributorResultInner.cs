using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
namespace Aquatrols.iOS
{
    public class TableViewTmDistributorResultInner : UITableViewSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableViewTmDistributorResultInner"/> class.
        /// </summary>
        public TableViewTmDistributorResultInner()
        {
        }
        /// <summary>
        /// The controller.
        /// </summary>
        public TmDistributorResultController controller;
        List<TmDistributorProductDetail> tmDistributorProductDetails;
        private string cellidentifier = AppResources.tmDistributorResultInner;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableViewTmDistributorResultInner"/> class.
        /// </summary>
        /// <param name="tmDistributorProductDetails">Tm distributor product details.</param>
        public TableViewTmDistributorResultInner(List<TmDistributorProductDetail> tmDistributorProductDetails)
        {
            this.tmDistributorProductDetails = tmDistributorProductDetails;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellDistributorResultInner cell = tableView.DequeueReusableCell(cellidentifier) as CustCellDistributorResultInner;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellDistributorResultInner;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmDistributorProductDetails[indexPath.Row]);
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
            return tmDistributorProductDetails.Count;
        }
        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
           return 50;
        }
    }
}
