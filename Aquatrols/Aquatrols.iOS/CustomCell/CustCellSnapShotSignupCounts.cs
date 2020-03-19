using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellSnapShotSignupCounts : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellSnapShotSignupCounts"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellSnapShotSignupCounts (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmCommitmentsPerDistributor">Tm commitments per distributor.</param>
        internal void UpdateCell(TmSnapshotSignUpCountVm tmCommitmentsPerDistributor)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblterritoryNumber, lblterritoryName, lblCountSignUp, lblCountCommtments }, null, Constant.lstDigit[8], viewWidth);
                if (tmCommitmentsPerDistributor!=null)
                {
                    lblterritoryNumber.Text = tmCommitmentsPerDistributor.territoryNumber;
                    lblterritoryName.Text = tmCommitmentsPerDistributor.territoryName;
                    lblCountSignUp.Text = tmCommitmentsPerDistributor.countSignUps.ToString();
                    lblCountCommtments.Text = tmCommitmentsPerDistributor.countCommitments.ToString();
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellSnapShotSignupCounts, null);
            }
        }
    }
}