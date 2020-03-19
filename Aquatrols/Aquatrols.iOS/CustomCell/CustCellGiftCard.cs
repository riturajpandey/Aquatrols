using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using Foundation;
using System;
using ToastIOS;
using UIKit;

namespace Aquatrols.iOS
{
    public partial class CustCellGiftCard : UITableViewCell
    {
        private RedeemGiftCardEntity redeemGiftCardEntity;
        private NSData nsDataProductLogo;
        private RedeemPointsController redeemPointsController;
        private string formatted,rewardItem,rewardId;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.CustCellGiftCard"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustCellGiftCard (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="p">P.</param>
        /// <param name="data">Data.</param>
        /// <param name="controller">Controller.</param>
        /// <param name="index">Index.</param>
        internal void UpdateCell(RedeemGiftCardEntity p, NSData data, RedeemPointsController controller, int index)
        {
            txtAmount.Tag = index;
            redeemPointsController = controller;
            redeemGiftCardEntity = p;
            nsDataProductLogo = data;
            txtAmount.Text = String.Empty;
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblProductDesc }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(null, new UIButton[] { btnRedeem }, Constant.lstDigit[11], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblInfoText,lblProductInfo }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblAmount }, new UITextField[] {txtAmount},Constant.lstDigit[9], viewWidth);
                Utility.SetPadding(new UITextField[] { txtAmount }, Constant.lstDigit[4]);
                if (!String.IsNullOrEmpty(redeemGiftCardEntity.description))
                {
                    lblProductDesc.Text = redeemGiftCardEntity.description;
                }
                if (!String.IsNullOrEmpty(redeemGiftCardEntity.itemImage))
                {
                    if (nsDataProductLogo != null)
                    {
                        imgGiftCardLogo.Image = UIImage.LoadFromData(nsDataProductLogo);
                    }
                    rewardId = redeemGiftCardEntity.rewardId;
                }
                if (!String.IsNullOrEmpty(redeemGiftCardEntity.rewardItem))
                {
                    lblProductInfo.Text = redeemGiftCardEntity.rewardItem;
                }
                if (!String.IsNullOrEmpty(redeemGiftCardEntity.labelForAmount))
                {
                    lblAmount.Text = redeemGiftCardEntity.labelForAmount;
                }
                if(!String.IsNullOrEmpty(Convert.ToString(redeemGiftCardEntity.minimumPoints)))
                {
                    int minimumPoints = redeemGiftCardEntity.minimumPoints;
                    formatted = minimumPoints.ToString(AppResources.Comma);
                    lblInfoText.Text = formatted + AppResources.PointsRedeem + Convert.ToString(redeemGiftCardEntity.pointPricePerUnit) + " " + redeemGiftCardEntity.itemType;  
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateCell, AppResources.CustCellGiftCard, null);
            }
        }
        /// <summary>
        /// Button the redeem touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnRedeem_TouchUpInside(UIButton sender)
        {
            try
            {
                txtAmount.ResignFirstResponder();
                if (!String.IsNullOrEmpty(txtAmount.Text.Trim()))
                {
                    int enteredPoint = Constant.lstDigit[0];
                    string balancedpoints= NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.BalancePoints);
                    if (!String.IsNullOrEmpty(balancedpoints))
                    {
                        string amt= txtAmount.Text.Trim().Replace(",", string.Empty);
                        bool flag = int.TryParse(amt, out int amount);
                        if (flag == true)
                        {
                            enteredPoint = amount;
                            if (enteredPoint>0)
                            {
                                int availablePoint = Convert.ToInt32(balancedpoints);
                                if (enteredPoint <= availablePoint)
                                {
                                    if (enteredPoint % (redeemGiftCardEntity.minimumPoints) == Constant.lstDigit[0])
                                    {
                                        rewardItem = redeemGiftCardEntity.rewardItem;
                                        redeemPointsController.HitRedeemAPI(rewardId, rewardItem, redeemGiftCardEntity.minimumPoints, redeemGiftCardEntity.pointPricePerUnit, enteredPoint);
                                        txtAmount.Text = String.Empty;
                                    }
                                    else
                                    {
                                        Toast.MakeText(AppResources.enterMultiplierPoints + formatted, Constant.durationOfToastMessage).Show();
                                    }
                                }
                                else
                                {
                                    Toast.MakeText(AppResources.notEnoughPoints, Constant.durationOfToastMessage).Show();
                                }
                            }
                            else
                            {
                                Toast.MakeText(AppResources.enterMultiplierPoints + formatted, Constant.durationOfToastMessage).Show();
                            }
                        }
                        else
                        {
                            Toast.MakeText(AppResources.enterValidAmount, Constant.durationOfToastMessage).Show();
                        }
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Required, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BtnRedeem_TouchUpInside, AppResources.CustCellGiftCard, null);
            }  
        }
        /// <summary>
        /// Text amount did begin editing events to move table view source to open keyboard.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtAmount_DidBeginEditing(UITextField sender)
        {
            UITextField[] textFields = new UITextField[] { txtAmount };
            Utility.AddDoneButtonToNumericKeyboard(textFields);
            redeemPointsController.MoveScrollViewToOpenKeyboard(txtAmount.Tag);
        }
        /// <summary>
        /// Text amount did end editing events to move table view source to hide keyboard.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtAmount_DidEndEditing(UITextField sender)
        {
            redeemPointsController.MoveScrollViewToHideKeyboard(txtAmount.Tag);
        }
        /// <summary>
        /// To add commas in text box value
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtAmount_EditingChanged(UITextField sender)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtAmount.Text.Trim()))
                {
                    if (txtAmount.Text.Length <= Constant.lstDigit[8])
                    {
                        float amountFairWays;
                        string txtAmt = txtAmount.Text.Trim().Replace(",", string.Empty);
                        bool bPArsed = float.TryParse(txtAmt as string, out amountFairWays);
                        if (bPArsed)
                        {
                            long txtAmnt = Convert.ToInt64(txtAmt);
                            string formatted = txtAmnt.ToString(AppResources.Comma);
                            sender.Text = formatted;
                            txtAmount.Text = sender.Text;
                        }
                    }
                    else
                    {
                        txtAmount.ResignFirstResponder();
                        Toast.MakeText(AppResources.enterValidAmount, Constant.durationOfToastMessage).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.txtAmount_EditingChanged, AppResources.CustCellGiftCard, null);
            } 
        }
    }
}