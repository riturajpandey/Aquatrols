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
    public class TableViewEditGallonProductList: UITableViewSource
    {
        public BookProductsController controller;
        public List<WishListEntity> lstWishListEntity;
        public List<NSData> lstNSData;
        private string cellidentifier = AppResources.EditGallonProductList;
        private string userId = String.Empty;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Aquatrols.iOS.TablevVewSource.TableViewEditGallonProductList"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        public TableViewEditGallonProductList(BookProductsController controller,List<WishListEntity> lstWishListEntity,List<NSData> lstNSData)
        {
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
            }
            this.controller = controller;
            this.lstWishListEntity = lstWishListEntity;
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
            return lstWishListEntity.Count;
        }
        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            nfloat HeightForRow = 0;
            try
            {
                string device = UIDevice.CurrentDevice.Model;
                if (device == AppResources.iPad)
                {
                    int viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                    int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height;
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        HeightForRow = Constant.lstDigit[26];
                    }
                    else
                    {
                        HeightForRow = Constant.lstDigit[23];
                    }
                }
                else
                {
                    HeightForRow = Constant.lstDigit[18];
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetHeightForRow, AppResources.TableViewEditGallonProductList, null);
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
            CustCellEditGallonProductList cell = tableView.DequeueReusableCell(cellidentifier) as CustCellEditGallonProductList;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellEditGallonProductList;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                cell.UpdateCell(lstWishListEntity[indexPath.Row],lstNSData[indexPath.Row], this.controller);  
            }
            else
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                cell.UpdateCell(lstWishListEntity[indexPath.Row],lstNSData[indexPath.Row], this.controller);  
            }
            return cell;
        }
    }
}
