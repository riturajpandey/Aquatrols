using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using ToastIOS;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellMyCartProductList : UITableViewCell
    {
        private MyCartController myCartController;
        private WishListEntity wishListEntity;
        private string productid;
        private int wishListId;
        private NSData nsDataProductLogo;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellMyCartProductList"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellMyCartProductList (IntPtr handle) : base (handle)
        {
            
        }
        /// <summary>
        /// Update the cell.
        /// bind the data in cell
        /// </summary>
        internal void UpdateCell(WishListEntity p,NSData data, MyCartController controller,int index)
        {
            myCartController = controller;
            wishListEntity = p;
            nsDataProductLogo = data;
            txtMyCartGallons.Tag = index;
            int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
            //set fonts of UILabel and UITextField
            Utility.SetFonts(null, new UILabel[] { lblDistributorName,lblShowGallonOrLiter }, new UITextField[] { txtMyCartGallons }, Constant.lstDigit[9], viewWidth);
            Utility.SetFonts(null, new UILabel[] { lblPoints }, null, Constant.lstDigit[8], viewWidth);
            Utility.SetPadding(new UITextField[] { txtMyCartGallons }, Constant.lstDigit[4]);
            try
            {
                if (!String.IsNullOrEmpty(wishListEntity.productLogo))
                {
                    if (nsDataProductLogo != null)
                    {
                        imgMyCartProduct.Image = UIImage.LoadFromData(nsDataProductLogo);
                    }
                    productid = wishListEntity.productId;
                    wishListId = wishListEntity.wishListId;
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
                                    txtMyCartGallons.Text = Convert.ToString(wishListEntity.quantity);
                                    lblShowGallonOrLiter.Text = wishListEntity.unitName;
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(wishListEntity.quantity)))
                                {
                                    if (wishListEntity.quantity == Constant.lstDigit[0])
                                    {
                                        txtMyCartGallons.Text = string.Empty;
                                    }
                                    else
                                    {
                                        txtMyCartGallons.Text = Convert.ToString(wishListEntity.quantity);   
                                    }
                                    lblShowGallonOrLiter.Text = AppResources.Litre;
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
                                if(wishListEntity.quantity==Constant.lstDigit[0])
                                {
                                    txtMyCartGallons.Text = string.Empty;
                                }
                                else
                                {
                                    txtMyCartGallons.Text = Convert.ToString(wishListEntity.quantity); 
                                }
                                lblShowGallonOrLiter.Text = wishListEntity.unitName;
                            }
                            lblPoints.Text = wishListEntity.pointsReceived + AppResources.Points;
                        }
                    }
                }
                lblDistributorName.Text = wishListEntity.distributorName;
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell,AppResources.CustCellMyCartProductList, null);
            }
        }
        /// <summary>
       /// Text my cart gallons did begin editing events to move table view source to open keyboard.
       /// </summary>
       /// <param name="sender">Sender.</param>
       partial void txtMyCartGallons_DidBeginEditing(UITextField sender)
        {
            UITextField[] textFields = new UITextField[] { txtMyCartGallons };
            Utility.AddDoneButtonToNumericKeyboard(textFields);
            myCartController.MoveScrollViewOpenKeyboard(txtMyCartGallons.Tag);
        }
        /// <summary>
        /// Text my cart gallons did end editing events to move table view source to hide keyboard.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtMyCartGallons_DidEndEditing(UITextField sender)
        {
            myCartController.MoveScrollViewHideKeyboard(txtMyCartGallons.Tag);
            try
            {
                string txtGallons = txtMyCartGallons.Text.Trim();
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                {
                    string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                    if (countryName.ToLower() == AppResources.canada)
                    {
                        if (!string.IsNullOrEmpty(txtGallons))
                        {
                            if (txtGallons.Length <= 8)
                            {
                                if (Convert.ToInt32(txtGallons) > Constant.lstDigit[0])
                                {
                                    NSUserDefaults.StandardUserDefaults.SetString("true", AppResources.isVaildAmount);
                                    lblPoints.Text = wishListEntity.brandPoints * Convert.ToInt32(txtGallons) + AppResources.Points;
                                    myCartController.UpdateToQueue(txtGallons, wishListId);
                                }
                                else
                                {
                                    NSUserDefaults.StandardUserDefaults.SetString("false", AppResources.isVaildAmount);
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                    lblPoints.Text = Constant.lstDigit[0] + AppResources.Points;
                                }
                            }
                            else
                            {
                                txtMyCartGallons.ResignFirstResponder();
                                NSUserDefaults.StandardUserDefaults.SetString("false", AppResources.isVaildAmount);
                                Toast.MakeText(AppResources.InvalidAmount).Show();
                                lblPoints.Text = wishListEntity.brandPoints * Convert.ToInt32(txtGallons) + AppResources.Points;
                            }
                        }
                        else
                        {
                            NSUserDefaults.StandardUserDefaults.SetString("false", AppResources.isVaildAmount);
                            Toast.MakeText(AppResources.InvalidAmount).Show();
                            lblPoints.Text = Constant.lstDigit[0] + AppResources.Points; 
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtGallons))
                        {
                            if (txtGallons.Length <= 8)
                            {
                                if (Convert.ToInt32(txtGallons) > Constant.lstDigit[0])
                                {
                                    NSUserDefaults.StandardUserDefaults.SetString("true", AppResources.isVaildAmount);
                                    lblPoints.Text = wishListEntity.brandPoints * Convert.ToInt32(txtGallons) + AppResources.Points;
                                    myCartController.UpdateToQueue(txtGallons, wishListId);
                                }
                                else
                                {
                                    NSUserDefaults.StandardUserDefaults.SetString("false", AppResources.isVaildAmount);
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                    lblPoints.Text = Constant.lstDigit[0] + AppResources.Points;
                                }
                            }
                            else
                            {
                                txtMyCartGallons.ResignFirstResponder();
                                NSUserDefaults.StandardUserDefaults.SetString("false", AppResources.isVaildAmount);
                                Toast.MakeText(AppResources.InvalidAmount).Show();
                                lblPoints.Text = wishListEntity.brandPoints * Convert.ToInt32(txtGallons) + AppResources.Points;
                            }
                        }
                        else
                        {
                            NSUserDefaults.StandardUserDefaults.SetString("false", AppResources.isVaildAmount);
                            Toast.MakeText(AppResources.InvalidAmount).Show();
                            lblPoints.Text = Constant.lstDigit[0] + AppResources.Points;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.txtMyCartGallons_DidEndEditing, AppResources.CustCellMyCartProductList, null);
            }
        }
    }
}