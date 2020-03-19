using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellDistributorList : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellDistributorList"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellDistributorList (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Update the cell Course.
        /// bind the data in cell
        /// </summary>
        /// <param name="distributorInfoEntity">Distributor entity.</param>
        internal void UpdateCell(DistinctDistributorEntity distributorInfoEntity)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblDistributorName }, null, Constant.lstDigit[11], viewWidth);
                if (distributorInfoEntity != null)
                {
                    lblDistributorName.Text = distributorInfoEntity.distributorName;
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellDistributorList, null);
            }
        }
    }
}