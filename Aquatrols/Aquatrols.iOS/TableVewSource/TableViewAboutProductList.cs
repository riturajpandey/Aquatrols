using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.TablevVewSource
{
    public class TableViewAboutProductList: UITableViewSource
    {
        public AboutController controller;
        public List<ProductListEntity> lstProductListEntities;
        public List<NSData> lstNSDataProductLogo;
        private string cellidentifier = AppResources.AboutProductList;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TablevVewSource.TableViewAboutProductList"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="lstProductListEntities">Product list entities.</param>
        public TableViewAboutProductList(AboutController controller, List<ProductListEntity> lstProductListEntities,List<NSData> lstNSDataProductLogo)
        {
            this.controller = controller;
            this.lstProductListEntities = lstProductListEntities;
            this.lstNSDataProductLogo = lstNSDataProductLogo;
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
                        HeightForRow = 343;
                    }
                    else
                    {
                        HeightForRow = 257;
                    }
                }
                else
                {
                    HeightForRow = Constant.lstDigit[27];
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetHeightForRow, AppResources.TableViewAboutProductList, null);
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
            CustCellAboutProductList cell = tableView.DequeueReusableCell(cellidentifier) as CustCellAboutProductList;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellAboutProductList;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.UpdateCell(lstProductListEntities[indexPath.Row],lstNSDataProductLogo[indexPath.Row], this.controller); 
            }
            else
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.UpdateCell(lstProductListEntities[indexPath.Row],lstNSDataProductLogo[indexPath.Row], this.controller); 
            }
                
            cell.ImgProductlistClick();
            return cell;
        }
    }
}
