using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellTmSuperIntendent : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellTmSuperIntendent"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellTmSuperIntendent (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmSuperIntendant">Tm super intendant.</param>
        internal void UpdateCell(TmSuperIntendantResponseEntity tmSuperIntendant)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblTmSuperIntendentName }, null, Constant.lstDigit[11], viewWidth);
                if (tmSuperIntendant != null)
                {
                    lblTmSuperIntendentName.Text = tmSuperIntendant.firstName + " " + tmSuperIntendant.lastName + "(" + tmSuperIntendant.userName + ")";
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellTmSuperIntendent, null);
            }
          
        }
    }
}