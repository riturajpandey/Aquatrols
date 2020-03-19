using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.TableVewSource
{
    public class TableViewDistributorList: UITableViewSource
    {
        public SignUpController signUpController;
        public List<DistinctDistributorEntity> lstDistributorInfoEntity;
        private string cellidentifier = AppResources.CellDistributor;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableVewSource.TableViewDistributorList"/> class.
        /// </summary>
        /// <param name="signUpController">Book products controller.</param>
        /// <param name="lstDistributorInfoEntity">Lst distributor info entity.</param>
        public TableViewDistributorList(SignUpController signUpController,List<DistinctDistributorEntity> lstDistributorInfoEntity)
        {
            this.signUpController = signUpController;
            this.lstDistributorInfoEntity = lstDistributorInfoEntity;
        }
        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return Constant.lstDigit[13];
        }
        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
		public override nint RowsInSection(UITableView tableview, nint section)
		{
            return lstDistributorInfoEntity.Count;
		}
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            CustCellDistributorList cellDistributorList = tableView.DequeueReusableCell(cellidentifier) as CustCellDistributorList;
            if (cellDistributorList == null)
                cellDistributorList = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellDistributorList;
            cellDistributorList.SelectionStyle = UITableViewCellSelectionStyle.None;
            cellDistributorList.UpdateCell(lstDistributorInfoEntity[indexPath.Row]);
            return cellDistributorList;
		}
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
        public event Action<string> OnRowSelect;
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            if(OnRowSelect!=null)
            {
                OnRowSelect(lstDistributorInfoEntity[indexPath.Row].distributorName);
            }
		}
	}
}
