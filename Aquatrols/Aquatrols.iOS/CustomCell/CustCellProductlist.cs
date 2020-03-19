using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using ToastIOS;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellProductlist : UITableViewCell
    {
        private ProductListEntity productListEntity;
        private NSData nsDataProductLogo;
        private BookProductsController bookProductController;
        private string productId;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellProductlist"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellProductlist(IntPtr handle) : base(handle)
        {
           
        }
        /// <summary>
        /// Update the cell.
        /// bind the data in cell
        /// </summary>
        /// <param name="p">P.</param>
        internal void UpdateCell(ProductListEntity p,NSData data, BookProductsController controller, int index)
        {
            txtGallons.Tag = index;
            bookProductController = controller;
            productListEntity = p;
            nsDataProductLogo = data;
            txtGallons.Text = String.Empty;
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblpoint, lblText, lblEnterAmount }, new UITextField[] { txtGallons }, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(null, new UIButton[] { btnAddToCart, btnCommit }, Constant.lstDigit[11], viewWidth);
                Utility.SetPadding(new UITextField[] { txtGallons }, Constant.lstDigit[4]);
                if (!String.IsNullOrEmpty(productListEntity.productShortDescription))
                {
                    lblText.Text = productListEntity.productShortDescription;
                }
                if (!String.IsNullOrEmpty(productListEntity.productLogo))
                {
                    if(nsDataProductLogo!=null)
                    {
                        imgProduct.Image = UIImage.LoadFromData(nsDataProductLogo);  
                    }
                    productId = productListEntity.productId;
                }
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                {
                    string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                    if (countryName.ToLower() == AppResources.canada)
                    {
                        if (productListEntity.productName.Equals(AppResources.AqueductFlex))
                        {
                            lblpoint.Text = AppResources.at + productListEntity.brandPoints + AppResources.pointsPer + productListEntity.unit;
                            txtGallons.Placeholder = productListEntity.unit;
                        }
                        else
                        {
                            lblpoint.Text = AppResources.at + productListEntity.canadaBrandPoints + AppResources.pointsPer + AppResources.Litre;
                            txtGallons.Placeholder = AppResources.Litre;  
                        }
                    }
                    else
                    {
                        lblpoint.Text = AppResources.at + productListEntity.brandPoints + AppResources.pointsPer + productListEntity.unit;
                        txtGallons.Placeholder = productListEntity.unit;
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellProductlist, null);
            }
        }
        /// <summary>
        /// Add to cart button touch up inside.
        /// add the product in a queue.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void btnAddToCart_TouchUpInside(UIButton sender)
        {
            try
            {
                txtGallons.ResignFirstResponder();
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                {
                    string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                    if (countryName.ToLower() == AppResources.canada)
                    {
                        if (productListEntity.productName.Equals(AppResources.AqueductFlex))
                        {
                            if (string.IsNullOrEmpty(txtGallons.Text.Trim()))
                            {
                                Toast.MakeText(productListEntity.unit + AppResources.Require).Show();
                            }
                            else
                            {
                                if (txtGallons.Text.Length <= 8 && Convert.ToInt32(txtGallons.Text.Trim()) > Constant.lstDigit[0])
                                {
                                    bookProductController.AddToQueue(productId, txtGallons.Text.Trim(), false);
                                    txtGallons.Text = String.Empty;
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(txtGallons.Text.Trim()))
                            {
                                Toast.MakeText(AppResources.Litre + AppResources.Require).Show();
                            }
                            else
                            {
                                if (txtGallons.Text.Length <= 8 && Convert.ToInt32(txtGallons.Text.Trim()) > Constant.lstDigit[0])
                                {
                                    bookProductController.AddToQueue(productId, txtGallons.Text.Trim(), false);
                                    txtGallons.Text = String.Empty;
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                }
                            }
                        }
                    }
                    else
                    {
                        if(productListEntity!=null)
                        {
                            if (string.IsNullOrEmpty(txtGallons.Text.Trim()))
                            {
                            Toast.MakeText(productListEntity.unit+AppResources.Require).Show();
                            }
                            else
                            {
                                if (txtGallons.Text.Length <= 8 && Convert.ToInt32(txtGallons.Text.Trim()) > Constant.lstDigit[0])
                                {
                                    bookProductController.AddToQueue(productId, txtGallons.Text.Trim(),false);
                                    txtGallons.Text = String.Empty;
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.btnAddToCart_TouchUpInside, AppResources.CustCellProductlist, null);
            }
        }
        /// <summary>
        /// book now button touch up inside.
        /// book the product
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void btnCommit_TouchUpInside(UIButton sender)
        {
            try
            {
                txtGallons.ResignFirstResponder();
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                {
                    string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                    if (countryName.ToLower() == AppResources.canada)
                    {
                        if (productListEntity.productName.Equals(AppResources.AqueductFlex))
                        {
                            if (string.IsNullOrEmpty(txtGallons.Text.Trim()))
                            {
                                Toast.MakeText(AppResources.Add + productListEntity.unit).Show();
                                bookProductController.GetData();
                            }
                            else
                            {
                                if (txtGallons.Text.Length <= 8 && Convert.ToInt32(txtGallons.Text.Trim()) > Constant.lstDigit[0])
                                {
                                    bookProductController.GetDataProduct(productId, txtGallons.Text.Trim());
                                    txtGallons.Text = String.Empty;
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(txtGallons.Text.Trim()))
                            {
                                Toast.MakeText(AppResources.Add+AppResources.Litre).Show();
                                bookProductController.GetData();
                            }
                            else
                            {
                                if (txtGallons.Text.Length <= 8 && Convert.ToInt32(txtGallons.Text.Trim()) > Constant.lstDigit[0])
                                {
                                    bookProductController.GetDataProduct(productId, txtGallons.Text.Trim());
                                    txtGallons.Text = String.Empty;
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                }
                            } 
                        }
                    }
                    else
                    {
                        if (productListEntity != null)
                        {
                            if (string.IsNullOrEmpty(txtGallons.Text.Trim()))
                            {
                                Toast.MakeText(AppResources.Add + productListEntity.unit).Show();
                                bookProductController.GetData();
                            }
                            else
                            {
                                if (txtGallons.Text.Length <= 8 && Convert.ToInt32(txtGallons.Text.Trim()) > Constant.lstDigit[0])
                                {
                                    bookProductController.GetDataProduct(productId, txtGallons.Text.Trim());
                                    txtGallons.Text = String.Empty;
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.InvalidAmount).Show();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.btnCommit_TouchUpInside, AppResources.CustCellProductlist, null);
            }
        }
        /// <summary>
        /// Text gallons did begin editing events to move table view source to open keyboard.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtGallons_DidBeginEditing(UITextField sender)
        {
            UITextField[] textFields = new UITextField[] { txtGallons };
            Utility.AddDoneButtonToNumericKeyboard(textFields);
            bookProductController.MoveScrollViewToOpenKeyboard(txtGallons.Tag);
        }
        /// <summary>
        /// Text gallons did end editing events to move table view source to hide keyboard.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtGallons_DidEndEditing(UITextField sender)
        {
            bookProductController.MoveScrollViewToHideKeyboard(txtGallons.Tag);
        }
    }
}
