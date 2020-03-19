using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.TableVewSource
{
    public class TableViewMyCartProductList: UITableViewSource
    {
        public MyCartController myCartController;
        public List<WishListEntity> lstWishListEntity;
        public List<NSData> lstNSData;
        private string cellidentifier = AppResources.MyCartProductList;
        private string userId = String.Empty;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TableVewSource.TableViewMyCartProductList"/> class.
        /// </summary>
        /// <param name="myCartController">My cart controller.</param>
        public TableViewMyCartProductList(MyCartController myCartController,List<WishListEntity> lstWishListEntity,List<NSData> lstNSData)
        {
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
            }
            this.myCartController = myCartController;
            this.lstWishListEntity = lstWishListEntity;
            this.lstNSData=lstNSData;
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
                utility.SaveExceptionHandling(ex, AppResources.GetHeightForRow,AppResources.TableViewMyCartProductList, null);
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
            CustCellMyCartProductList cell = tableView.DequeueReusableCell(cellidentifier) as CustCellMyCartProductList;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellidentifier) as CustCellMyCartProductList;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                cell.UpdateCell(lstWishListEntity[indexPath.Row],lstNSData[indexPath.Row], this.myCartController, indexPath.Row); 
            }
            else
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                cell.UpdateCell(lstWishListEntity[indexPath.Row],lstNSData[indexPath.Row], this.myCartController, indexPath.Row); 
            }
            return cell;
        }
        /// <summary>
        /// Gets the trailing swipe actions configuration.
        /// </summary>
        /// <returns>The trailing swipe actions configuration.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UISwipeActionsConfiguration GetTrailingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            var flagAction = ContextualFlagAction(indexPath.Row);
            //UISwipeActionsConfiguration
            var trailingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { flagAction });
            trailingSwipe.PerformsFirstActionWithFullSwipe = false;
            return trailingSwipe;
        }
        /// <summary>
        /// override method for edit action Row
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            //UIContextualActions
            var flagAction = ContextualFlagAction(indexPath.Row);
            //UISwipeActionsConfiguration
            var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { flagAction });
            leadingSwipe.PerformsFirstActionWithFullSwipe = false;
            return leadingSwipe;
        }
        /// <summary>
        /// Contextuals the flag action.
        /// </summary>
        /// <returns>The flag action.</returns>
        /// <param name="row">Row.</param>
        public UIContextualAction ContextualFlagAction(int row)
        {
            var action = UIContextualAction.FromContextualActionStyle(UIContextualActionStyle.Normal,
            AppResources.Delete,
            (FlagAction, view, success) => {
                CallDeleteProductList(lstWishListEntity[row].wishListId);
                success(true);
            });
            action.BackgroundColor = UIColor.Red;
            return action;
        }
        /// <summary>
        /// delete product list based on userid and product list.
        /// </summary>
        /// <param name="productid">Productid.</param>
        public void CallDeleteProductList(int wishListId)
        {
            try
            {
                myCartController.HitWishListDeleteAPI(wishListId);
            } 
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
