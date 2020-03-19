using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.TableVewSource
{
    public class TableViewGiftCardUser: UITableViewSource
    {
        public RedeemPointsController controller;
        public List<RedeemGiftCardEntity> lstGiftCard;
        public List<NSData> lstNSData;
        private string cellidentifier = AppResources.GiftCardUser;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TablevVewSource.TableViewGiftCard"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="lstGiftCard">Product list entities.</param>
        public TableViewGiftCardUser(RedeemPointsController controller, List<RedeemGiftCardEntity> lstGiftCard, List<NSData> lstNSData)
        {
            this.controller = controller;
            this.lstGiftCard = lstGiftCard;
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
            return lstGiftCard.Count;
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
                        HeightForRow = Constant.sizeOfGiftCardUserContentiPadPro;
                    }
                    else
                    {
                        HeightForRow = Constant.sizeOfGiftCardUserContentiPad;
                    }
                }
                else
                {
                    HeightForRow = Constant.sizeOfGiftCardUserContent;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetHeightForRow, AppResources.TableViewGiftCardUser, null);
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
            CustCellGiftCardUser cell = tableView.DequeueReusableCell(cellidentifier) as CustCellGiftCardUser;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellGiftCardUser;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.UpdateCell(lstGiftCard[indexPath.Row], lstNSData[indexPath.Row], this.controller);
            }
            else
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.UpdateCell(lstGiftCard[indexPath.Row], lstNSData[indexPath.Row], this.controller);
            }
            return cell;
        }
    }
}
