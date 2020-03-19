using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Aquatrols.iOS.TableVewSource
{
    public class TableViewTmSuperIntendentList:UITableViewSource
    {
        public TmCourseSearchController controller;
        List<TmSuperIntendantResponseEntity> tmSuperIntendantResponseEntities;
        private string cellidentifier = AppResources.TmSuperIntendentCell;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableVewSource.TableViewTmSuperIntendentList"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="tmSuperIntendantResponseEntities">Tm super intendant response entities.</param>
        public TableViewTmSuperIntendentList(TmCourseSearchController controller,List<TmSuperIntendantResponseEntity> tmSuperIntendantResponseEntities)
        {
            this.controller = controller;
            this.tmSuperIntendantResponseEntities = tmSuperIntendantResponseEntities;
        }
        /// <summary>
        /// Get the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellTmSuperIntendent cell = tableView.DequeueReusableCell(cellidentifier) as CustCellTmSuperIntendent;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellTmSuperIntendent;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            cell.UpdateCell(tmSuperIntendantResponseEntities[indexPath.Row]);
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
            return tmSuperIntendantResponseEntities.Count;
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
        /// Occurs when on row select.
        /// </summary>
        public event Action<TmSuperIntendantResponseEntity> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(this.tmSuperIntendantResponseEntities[indexPath.Row]);
            }
        }
    }
}
