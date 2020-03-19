using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableViewProductCommPerDist : UITableViewSource
    {
        public TmSnapResultController controller;
        List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributors;
        private string cellidentifier =AppResources.IdnPrdCommimentPerDistributor;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableViewProductCommPerDist"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="tmCommitmentsPerDistributors">Tm commitments per distributors.</param>
        public TableViewProductCommPerDist(TmSnapResultController controller, List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributors)
        {
            this.controller = controller;
            this.tmCommitmentsPerDistributors = tmCommitmentsPerDistributors;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellProdCommitmentPerDist cell = tableView.DequeueReusableCell(cellidentifier) as CustCellProdCommitmentPerDist;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellProdCommitmentPerDist;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmCommitmentsPerDistributors[indexPath.Row]);
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
            return tmCommitmentsPerDistributors.Count;
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
        public event Action<TmCommitmentsPerDistributor> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.tmCommitmentsPerDistributors[indexPath.Row]);
            }
        }
    }
}
