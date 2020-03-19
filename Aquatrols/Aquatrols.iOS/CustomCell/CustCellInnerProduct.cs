using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellInnerProduct : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellInnerProduct"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellInnerProduct (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmProductDetail">Tm product detail.</param>
        internal void UpdateCell(TmProductDetail tmProductDetail)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblProductName, lblQuantity, lblDate }, null, Constant.lstDigit[8], viewWidth);
                if (tmProductDetail!=null)
                {
                    lblProductName.Text = tmProductDetail.productName;
                    lblQuantity.Text = tmProductDetail.quantity + " " + tmProductDetail.unit;
                    lblDate.Text = tmProductDetail.bookingDate;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellInnerProduct, null);
            }
        }
    }
}