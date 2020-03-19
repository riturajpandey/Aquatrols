using Aquatrols.iOS.Helper;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.TableVewSource
{
    public class TableViewTmCourseList:UITableViewSource
    {
        public TmCourseSearchController controller;
        public List<string> lstTmCourselist;
        private string cellidentifier = AppResources.TmCourseCell;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableVewSource.TableViewTmCourseList"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="lstTmCourselist">Lst tm courselist.</param>
        public TableViewTmCourseList(TmCourseSearchController controller, List<string> lstTmCourselist)
        {
            this.controller = controller;
            this.lstTmCourselist = lstTmCourselist;
        }
        /// <summary>
        /// Get the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellTmCourseName cell = tableView.DequeueReusableCell(cellidentifier) as CustCellTmCourseName;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellTmCourseName;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(lstTmCourselist[indexPath.Row]);
            return cell;
        }
        /// <summary>
        /// Get the height for row.
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
            return lstTmCourselist.Count;
        }
        /// <summary>
        /// Occurs when on row select.
        /// </summary>
        public event Action<string> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.lstTmCourselist[indexPath.Row]);
            }
        }
    }
}
