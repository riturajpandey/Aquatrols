using Aquatrols.iOS.Helper;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellCoursename : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellCoursename"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellCoursename (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Update the cell Course.
        /// bind the data in cell
        /// </summary>
        /// <param name="courseEntity">Course entity.</param>
        internal void UpdateCell(string courseEntity)
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblCourseName }, null, Constant.lstDigit[11], viewWidth);
                if (courseEntity != null)
                {
                    lblCourseName.Text = courseEntity;
                }   
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellCoursename, null);
            }
        }
    }
}