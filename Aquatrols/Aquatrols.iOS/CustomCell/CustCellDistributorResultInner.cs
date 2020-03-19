using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellDistributorResultInner : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellDistributorResultInner"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellDistributorResultInner (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmDistributorProductDetail">Tm distributor product detail.</param>
        internal void UpdateCell(TmDistributorProductDetail tmDistributorProductDetail)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblName, lblQty }, null, Constant.lstDigit[8], viewWidth);
                if (tmDistributorProductDetail!=null)
                {
                    lblName.Text = tmDistributorProductDetail.productName;
                    lblQty.Text = tmDistributorProductDetail.quantity + " " + tmDistributorProductDetail.unit;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellDistributorResultInner, null);

            }
        }
    }
}