using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellDistributorResult : UITableViewCell
    {
        private int viewWidth;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellDistributorResult"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellDistributorResult (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmCourseProductVm">Tm course product vm.</param>
        internal void UpdateCell(TmCourseProductVm tmCourseProductVm)
        {
            try
            {   viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblDistributorName }, null, Constant.lstDigit[7], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] {lblName,lblQuantity }, null, Constant.lstDigit[6], viewWidth);
                if (tmCourseProductVm!=null)
                {
                    lblDistributorName.Text = tmCourseProductVm.courseName;
                    TableViewTmDistributorResultInner tableViewTmInner = new TableViewTmDistributorResultInner(tmCourseProductVm.tmProductDetail);
                    tblDistributorInner.Source = tableViewTmInner;
                    tblDistributorInner.ReloadData();
                    tblDistributorInner.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell,AppResources.CustCellDistributorResult, null);
            }
        } 
    }
}