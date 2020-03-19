using Aquatrols.iOS.Helper;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableViewCourse:UITableViewSource
    {
        public SignUpController controller;
        public List<string> lstCourseEntity;
        private string cellidentifier = AppResources.CellCourse;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableViewCourse"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="lstCourseEntity">Lst course entity.</param>
        public TableViewCourse(SignUpController controller,List<string> lstCourseEntity)
        {
            this.controller = controller;
            this.lstCourseEntity = lstCourseEntity;
        }
        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
            return Constant.lstDigit[15];
		}
        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return lstCourseEntity.Count;
        }
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellCoursename cell = tableView.DequeueReusableCell(cellidentifier) as CustCellCoursename;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellCoursename;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            cell.UpdateCell(lstCourseEntity[indexPath.Row]);
            return cell;
        }
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
        public event Action<string> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(lstCourseEntity[indexPath.Row]);
            }
        }
    }
}
