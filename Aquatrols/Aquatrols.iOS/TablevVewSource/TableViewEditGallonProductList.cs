using System;
using System.Collections.Generic;
using Aquatrols.iOS.Controllers;
using Aquatrols.Models;
using Foundation;
using UIKit;

namespace Aquatrols.iOS.TablevVewSource
{
    public class TableViewEditGallonProductList: UITableViewSource
    {
        BookProductsController controller;
        List<ProductCheckoutEntity> productCheckoutEntities;
        public string cellidentifier = "EditGallonProductList";
        public TableViewEditGallonProductList(BookProductsController controller, List<ProductCheckoutEntity> productCheckoutEntities)
        {
            this.controller = controller;
            this.productCheckoutEntities = productCheckoutEntities;
        }
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return productCheckoutEntities.Count;
        }
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 136;
        }
       public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellEditGallonProductList cell = tableView.DequeueReusableCell(cellidentifier) as CustCellEditGallonProductList;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellEditGallonProductList;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            cell.UpdateCell1(productCheckoutEntities[indexPath.Row], this.controller);

            return cell;

        }
    }
}
