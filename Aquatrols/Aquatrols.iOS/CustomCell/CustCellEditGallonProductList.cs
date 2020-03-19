using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellEditGallonProductList : UITableViewCell
    {
        private WishListEntity wishListEntity;
        private BookProductsController Bookcontroller;
        private NSData nsDataProductLogo;
        private string productid;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellEditGallonProductList"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellEditGallonProductList (IntPtr handle) : base (handle)
        {
            
        }
        /// <summary>
        /// Update the cell.
        /// bind the data in cell
        /// </summary>
        /// <param name="p">P.</param>
        /// <param name="controller">Controller.</param>
        internal void UpdateCell(WishListEntity p,NSData data, BookProductsController controller)
        {
            Bookcontroller = controller;
            wishListEntity = p;
            nsDataProductLogo = data;
            int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
            //set fonts for UILabel and UITextField
            Utility.SetFonts(null, new UILabel[] { lblDistributorName }, new UITextField[] { txtGallon }, Constant.lstDigit[9],viewWidth);
            Utility.SetFonts(null, new UILabel[] { lblPoints }, null, Constant.lstDigit[8],viewWidth);
            Utility.SetPadding(new UITextField[] { txtGallon }, Constant.lstDigit[4]);
            try
            {
                if (!String.IsNullOrEmpty(wishListEntity.productLogo))
                {
                    if(nsDataProductLogo!=null)
                    {
                        imgProductGa.Image = UIImage.LoadFromData(nsDataProductLogo);  
                    }
                    productid = wishListEntity.productId;
                }
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                {
                    string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                    if (countryName.ToLower() == AppResources.canada)
                    {
                        if (wishListEntity != null)
                        {
                            if (wishListEntity.productName.Equals(AppResources.AqueductFlex))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(wishListEntity.quantity)))
                                {
                                    txtGallon.Text = wishListEntity.quantity +""+ wishListEntity.unitName;
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(wishListEntity.quantity)))
                                {
                                    txtGallon.Text = wishListEntity.quantity + " " + AppResources.Litre;
                                    lblPoints.Text = wishListEntity.pointsReceived + AppResources.Points;
                                }
                            }
                        }
                    }
                    else
                    {
                        if(wishListEntity!=null)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(wishListEntity.quantity)))
                            {
                                txtGallon.Text = wishListEntity.quantity +" "+ wishListEntity.unitName;
                            }
                            lblPoints.Text =  wishListEntity.pointsReceived + AppResources.Points;
                        }
                    }
                }
                lblDistributorName.Text = wishListEntity.distributorName;
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell,AppResources.CustCellEditGallonProductList, null);
            }
        }
    }
}