using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellProdCommitmentPerDist : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellProdCommitmentPerDist"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellProdCommitmentPerDist (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmCommitmentsPerDistributor">Tm commitments per distributor.</param>
        internal void UpdateCell(TmCommitmentsPerDistributor tmCommitmentsPerDistributor)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblDistributorName, lblCourseCount }, null, Constant.lstDigit[8], viewWidth);
                if (tmCommitmentsPerDistributor != null)
                {
                    lblDistributorName.Text = tmCommitmentsPerDistributor.distributorName;
                    lblCourseCount.Text = tmCommitmentsPerDistributor.courseCount.ToString();
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellProdCommitmentPerDist, null);
            }
        }
    }
}