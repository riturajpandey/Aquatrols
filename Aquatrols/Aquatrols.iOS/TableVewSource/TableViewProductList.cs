using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.TablevVewSource
{
    public class TableViewProductList:UITableViewSource
    {
        public BookProductsController controller;
        public List<ProductListEntity> lstProductListEntities;
        public List<NSData> lstNSData;
        private string cellidentifier = AppResources.ProductList;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TablevVewSource.TableViewProductList"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="lstProductListEntities">Product list entities.</param>
        public TableViewProductList(BookProductsController controller,List<ProductListEntity> lstProductListEntities,List<NSData> lstNSData)
        {
            this.controller = controller;
            this.lstProductListEntities = lstProductListEntities;
            this.lstNSData = lstNSData;
        }
        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return lstProductListEntities.Count;
        }
        /// <summary>
		/// Gets the height for row.
		/// </summary>
		/// <returns>The height for row.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
            nfloat HeightForRow=0;
            try
            {
                string device = UIDevice.CurrentDevice.Model;
                if (device == AppResources.iPad)
                {
                    int viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                    int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height;
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        HeightForRow = 400;
                    }
                    else
                    {
                        HeightForRow = Constant.lstDigit[32];
                    }
                }
                else
                {
                    HeightForRow = Constant.lstDigit[28];
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetHeightForRow, AppResources.TableViewProductList, null);
            }
            return HeightForRow;
        }
		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CustCellProductlist cell = tableView.DequeueReusableCell(cellidentifier) as CustCellProductlist;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellProductlist;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.UpdateCell(lstProductListEntities[indexPath.Row],lstNSData[indexPath.Row], this.controller, indexPath.Row); 
            }
            else
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.UpdateCell(lstProductListEntities[indexPath.Row], lstNSData[indexPath.Row], this.controller, indexPath.Row); 
            }
            return cell;
        }
    }
}
