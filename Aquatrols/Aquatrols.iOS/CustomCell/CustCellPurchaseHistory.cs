using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellPurchaseHistory : UITableViewCell
    {
        private PurchaseHistoryEntity purchaseHistoryEntity;
        private MyAccountController myAccountController;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellPurchaseHistory"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellPurchaseHistory (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Update the cell.
        /// bind the data in cell
        /// </summary>
        /// <param name="purchaseHistory">Purchase history.</param>
        /// <param name="controller">Controller.</param>
        internal void UpdateCell(PurchaseHistoryEntity purchaseHistory, MyAccountController controller)
        {
            myAccountController = controller;
            purchaseHistoryEntity = purchaseHistory; 
            int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
            Utility.SetFonts(null, new UILabel[] { lblProductName, lblQuantityValue, lblDistributor },null, Constant.lstDigit[8], viewWidth);
            try
            {
                if (purchaseHistoryEntity!=null)
                {
                    lblProductName.Text = purchaseHistoryEntity.productName;
                    lblQuantityValue.Text = Convert.ToString(purchaseHistoryEntity.quantity);
                    lblDistributor.Text = purchaseHistoryEntity.distributorName;
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellPurchaseHistory, null);
            }
        }
    }
}