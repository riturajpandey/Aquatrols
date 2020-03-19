using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.TableVewSource
{
    public class TableViewTmInnerProduct:UITableViewSource
    {
        public TmCourseResultController controller;
        List<TmProductDetail> tmProducts;
        private string cellidentifier = AppResources.InnerProductCell;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableVewSource.TableViewTmInnerProduct"/> class.
        /// </summary>
        /// <param name="tmProducts">Tm products.</param>
        public TableViewTmInnerProduct(List<TmProductDetail> tmProducts)
        {
            this.tmProducts = tmProducts;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellInnerProduct cell = tableView.DequeueReusableCell(cellidentifier) as CustCellInnerProduct;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellInnerProduct;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmProducts[indexPath.Row]);
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
            return tmProducts.Count;
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
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
        public event Action<TmProductDetail> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.tmProducts[indexPath.Row]);
            }
        } 
    }
}
