using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableViewTotalGallonPerLiter : UITableViewSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableViewTotalGallonPerLiter"/> class.
        /// </summary>
        public TableViewTotalGallonPerLiter()
        {
        }
        public TmSnapResultController controller;
        List<TmTotalCommitments> tmTotalCommitments;
        private string cellidentifier = AppResources.IdnCellGallonPerLiter;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableViewTotalGallonPerLiter"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="tmTotalCommitments">Tm total commitments.</param>
        public TableViewTotalGallonPerLiter(TmSnapResultController controller, List<TmTotalCommitments> tmTotalCommitments)
        {
            this.controller = controller;
            this.tmTotalCommitments = tmTotalCommitments;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellTotGallon cell = tableView.DequeueReusableCell(cellidentifier) as CustCellTotGallon;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellTotGallon;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmTotalCommitments[indexPath.Row]);
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
            return tmTotalCommitments.Count;
        }
        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 45;
        }
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
        public event Action<TmTotalCommitments> OnRowSelect;
        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.tmTotalCommitments[indexPath.Row]);
            }
        }
    }
}
