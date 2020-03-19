using Aquatrols.iOS.Helper;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellTmCourseName : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellTmCourseName"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellTmCourseName (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmCourseResponseEntity">Tm course response entity.</param>
        internal void UpdateCell(string tmCourseResponseEntity)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblTmCourseName }, null, Constant.lstDigit[11], viewWidth);
                if (tmCourseResponseEntity != null)
                {
                    lblTmCourseName.Text = tmCourseResponseEntity;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellTmCourseName, null);
            }
        }
    }
}