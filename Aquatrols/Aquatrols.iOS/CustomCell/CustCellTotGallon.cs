using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellTotGallon : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellTotGallon"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellTotGallon (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmTotalCommitments">Tm total commitments.</param>
        internal void UpdateCell(TmTotalCommitments tmTotalCommitments)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblProductName, lblQty , lblcountry }, null, Constant.lstDigit[8], viewWidth);
                if (tmTotalCommitments != null)
                {
                    lblProductName.Text = tmTotalCommitments.productName;
                    lblQty.Text = tmTotalCommitments.quantity + " " + tmTotalCommitments.unit;
                    lblcountry.Text = tmTotalCommitments.country;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellTmSuperIntendent, null); // exception handling
            }
        }
    }
}