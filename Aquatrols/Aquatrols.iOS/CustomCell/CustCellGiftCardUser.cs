using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellGiftCardUser : UITableViewCell
    {
        private RedeemGiftCardEntity redeemGiftCardEntity;
        private NSData nsDataProductLogo;
        private RedeemPointsController redeemPointsController;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellGiftCardUser"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellGiftCardUser (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="p">P.</param>
        /// <param name="data">Data.</param>
        /// <param name="controller">Controller.</param>
        internal void UpdateCell(RedeemGiftCardEntity p, NSData data, RedeemPointsController controller)
        {
            redeemPointsController = controller;
            redeemGiftCardEntity = p;
            nsDataProductLogo = data;
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblProductDescUser }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblInfoTextUser,lblProductInfoUser }, null, Constant.lstDigit[8], viewWidth);
                if (!String.IsNullOrEmpty(redeemGiftCardEntity.description))
                {
                    lblProductDescUser.Text = redeemGiftCardEntity.description;
                }
                if (!String.IsNullOrEmpty(redeemGiftCardEntity.itemImage))
                {
                    if (nsDataProductLogo != null)
                    {
                        imgGiftCardItem.Image = UIImage.LoadFromData(nsDataProductLogo);
                    }
                }
                if (!String.IsNullOrEmpty(redeemGiftCardEntity.rewardItem))
                {
                    lblProductInfoUser.Text = redeemGiftCardEntity.rewardItem;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(redeemGiftCardEntity.minimumPoints)))
                {
                    int minimumPoints = redeemGiftCardEntity.minimumPoints;
                    string formatted = minimumPoints.ToString(AppResources.Comma);
                    lblInfoTextUser.Text = Convert.ToString(formatted) + AppResources.PointsRedeem + Convert.ToString(redeemGiftCardEntity.pointPricePerUnit) + " " + redeemGiftCardEntity.itemType;
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell,AppResources.CustCellGiftCardUser, null);
            }
        }
    }
}