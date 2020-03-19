using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellProductListUser : UITableViewCell
    {
        private ProductListEntity productListEntity;
        private BookProductsController Bookcontroller;
        private NSData nsDataProductLogo;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellProductListUser"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellProductListUser (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Update the cell.
        /// bind the data in cell.
        /// </summary>
        /// <param name="p">P.</param>
        internal void UpdateCell(ProductListEntity p, NSData data, BookProductsController controller)
        {
            Bookcontroller = controller;
            productListEntity = p;
            nsDataProductLogo = data;
            int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
            Utility.SetFonts(null, new UILabel[] { lblTextUser }, null, Constant.lstDigit[8], viewWidth);
            try
            {
                if (!String.IsNullOrEmpty(productListEntity.productShortDescription))
                {
                    lblTextUser.Text = productListEntity.productShortDescription;

                }
                if (!String.IsNullOrEmpty(productListEntity.productLogo))
                {
                    if (nsDataProductLogo != null)
                    {
                        imgProductUser.Image = UIImage.LoadFromData(nsDataProductLogo);
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell,AppResources.CustCellProductListUser, null);
            }
        }
    }
}