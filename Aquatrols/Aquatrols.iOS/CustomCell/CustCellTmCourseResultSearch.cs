using Aquatrols.iOS.Helper;
using Aquatrols.iOS.TableVewSource;
using Aquatrols.Models;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellTmCourseResultSearch : UITableViewCell
    {
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellTmCourseResultSearch"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellTmCourseResultSearch (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="tmDistributorProduct">Tm distributor product.</param>
        /// <param name="index">Index.</param>
        internal void UpdateCell(TmDistributorProductVm tmDistributorProduct,int index)
        {
            try
            {
              
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                //set fonts of UILabel
                Utility.SetFonts(null, new UILabel[] { lblTmDistributorName }, null, Constant.lstDigit[7], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblProductName,lblDate, lblQuantity }, null, Constant.lstDigit[6], viewWidth);
                if (tmDistributorProduct != null)
                {
                    lblTmDistributorName.Text = tmDistributorProduct.distributorName;
                    TableViewTmInnerProduct tableViewTmInner = new TableViewTmInnerProduct(tmDistributorProduct.tmProductDetail);
                    tblInnerProduct.Source = tableViewTmInner;
                    tblInnerProduct.ReloadData();
                    tblInnerProduct.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellTmCourseResultSearch, null);
            }
        }
    }
}