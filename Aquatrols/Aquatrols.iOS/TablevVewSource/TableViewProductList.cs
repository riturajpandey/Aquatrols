using System;
using System.Collections.Generic;
using Aquatrols.iOS.Controllers;
using Aquatrols.Models;
using Foundation;
using UIKit;

namespace Aquatrols.iOS.TablevVewSource
{
    public class TableViewProductList:UITableViewSource
    {
        BookProductsController controller;
        List<ProductListEntity> productListEntities;
        public string cellidentifier = "ProductList";
        public TableViewProductList(BookProductsController controller,List<ProductListEntity> productListEntities)
        {
            this.controller = controller;
            this.productListEntities = productListEntities;
        }
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return productListEntities.Count;
        }
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
            return 240;
		}
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellProductlist cell = tableView.DequeueReusableCell(cellidentifier) as CustCellProductlist;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellProductlist;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            cell.UpdateCell(productListEntities[indexPath.Row],this.controller);
            return cell;
        }


    }
}
