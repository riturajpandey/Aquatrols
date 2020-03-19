using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableTmSnapShotsignupCount : UITableViewSource
    {
        public TmSnapResultController controller;
        List<TmSnapshotSignUpCountVm> tmCommitmentsPerDistributors;
        private string cellidentifier = "CustCellSnapSignUpCount";
        private TmSnapResultController tmSnapResultController;
        private string distributorName;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableTmSnapShotsignupCount"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="tmCommitmentsPerDistributors">Tm commitments per distributors.</param>
        public TableTmSnapShotsignupCount(TmSnapResultController controller, List<TmSnapshotSignUpCountVm> tmCommitmentsPerDistributors)
        {
            this.controller = controller;
            this.tmCommitmentsPerDistributors = tmCommitmentsPerDistributors;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableTmSnapShotsignupCount"/> class.
        /// </summary>
        /// <param name="tmSnapResultController">Tm snap result controller.</param>
        /// <param name="distributorName">Distributor name.</param>
        public TableTmSnapShotsignupCount(TmSnapResultController tmSnapResultController, string distributorName)
        {
            this.tmSnapResultController = tmSnapResultController;
            this.distributorName = distributorName;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellSnapShotSignupCounts cell = tableView.DequeueReusableCell(cellidentifier) as CustCellSnapShotSignupCounts;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellSnapShotSignupCounts;
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
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 45;
        }
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
        public event Action<TmSnapshotSignUpCountVm> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.tmCommitmentsPerDistributors[indexPath.Row]);
            }
        }
    }
}
