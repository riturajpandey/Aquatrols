using System;
using System.Collections.Generic;
using Aquatrols.Models;
using Foundation;
using UIKit;

namespace Aquatrols.iOS
{
    public class TableViewCourse:UITableViewSource
    {
        SignUpController controller;
        List<CourseEntity> lstCourseEntity;
        public string cellidentifier = "CellCourse";
        public TableViewCourse(SignUpController controller,List<CourseEntity> lstCourseEntity)
        {
            this.controller = controller;
            this.lstCourseEntity = lstCourseEntity;
        }
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
            return 30;
		}
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return lstCourseEntity.Count;
        }
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellCoursename cell = tableView.DequeueReusableCell(cellidentifier) as CustCellCoursename;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellCoursename;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            cell.UpdateCell(lstCourseEntity[indexPath.Row]);

            return cell;
        }
        public event Action<string> OnRowSelect;
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (OnRowSelect != null)
            {
                OnRowSelect(lstCourseEntity[indexPath.Row].courseName);
            }
        }


    }
}
