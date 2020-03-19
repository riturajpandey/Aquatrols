using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.iOS.Picker;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This Class file is used for distributor search screen
    /// </summary>
    public partial class TmDistributorSearchController : UIViewController
    {
        private int viewWidth;
        private UIPickerView tmDistributorPicker = new UIPickerView();
        private UIPickerView tmStatePicker = new UIPickerView();
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        private string token,distributorId;
        private List<TmStateList> tmStateLists;
        private List<TmDistributorList> tmDistributorLists;
        private UIButton btnDone = new UIButton();
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TmDistributorSearchController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public TmDistributorSearchController (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                // get the token
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token)))
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                }
                //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
                string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
                if (!String.IsNullOrEmpty(exception))
                {
                    utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.TmDistributorSearch, exception);
                }
                SetFonts();
                SetLogoSize();
                ImgMenuDeactiveClick();
                TxtStateNameClick();
                TxtDistributorNameClick();
                ImgBackClick();
                LblTermsAndConditionsDeactivateClick();
                LblPrivacyPolicyDeacClick();
                LblFAQDeacClick();
                LblSupportDeacClick();
                LblLogoutDClick();
                ChildViewClick();
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.TmDistributorSearch, null);
            }
        }
        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public async override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            await GetTmStateList(token); // get the territory manager state list
         }
        /// <summary>
        /// Gesture implementation for menu click on deactivate dashboard.
        /// hide or show pop up
        /// </summary>
        public void ImgMenuDeactiveClick()
        {
            UITapGestureRecognizer imgMenuDeactiveClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenu.Hidden == true)
                {
                    popUpMenu.Hidden = false;
                }
                else
                {
                    popUpMenu.Hidden = true;
                }
            });
            ImgMenu.UserInteractionEnabled = true;
            ImgMenu.AddGestureRecognizer(imgMenuDeactiveClicked);
        }
        /// <summary>
        /// Set Logo size sepcified in iPhone X.
        /// set UI in iPhone X
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                if (viewWidth == (int)DeviceScreenSize.ISix)
                {
                    if (viewHeight == Constant.lstDigit[31])
                    {
                        vwHeader.Frame = new CGRect(vwHeader.Frame.X, vwHeader.Frame.Y + Constant.lstDigit[10], vwHeader.Frame.Width, vwHeader.Frame.Height);
                        vwChildContent.Frame = new CGRect(vwChildContent.Frame.X, vwChildContent.Frame.Y + Constant.lstDigit[10], vwChildContent.Frame.Width, vwChildContent.Frame.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.TmDistributorSearch, null);
            }
        }
        /// <summary>
        /// Gesture implementation for terms and conditions clcik on deactivate dashboard.
        /// </summary>
        public void LblTermsAndConditionsDeactivateClick()
        {
            UITapGestureRecognizer lblTermsAndConditionsDeactivateClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTermsAndConditions.UserInteractionEnabled = true;
            lblTermsAndConditions.AddGestureRecognizer(lblTermsAndConditionsDeactivateClicked);
        }
        /// <summary>
        /// Gesture implementation for privacy policy click on deactivate dashboard.
        /// </summary>
        public void LblPrivacyPolicyDeacClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacy.UserInteractionEnabled = true;
            lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// Gesture implementation for FAQ click on deactivate dashboard.
        /// </summary>
        public void LblFAQDeacClick()
        {
            UITapGestureRecognizer lblFAQDeacClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
            });
            lblFaq.UserInteractionEnabled = true;
            lblFaq.AddGestureRecognizer(lblFAQDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for Support click on deactivate dashboard.
        /// </summary>
        public void LblSupportDeacClick()
        {
            UITapGestureRecognizer lblSupportDeacClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenu.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for logout the deactivate dashboard.
        /// Logout user
        /// </summary>
        public void LblLogoutDClick()
        {
            UITapGestureRecognizer lblLogoutDClicked = new UITapGestureRecognizer(() =>
            {
                Utility.ClearCachedData();
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is LoginController)
                    {
                        NavigationController.PopToViewController(item, true);
                        break;
                    }
                }
            });
            lblLogout.UserInteractionEnabled = true;
            lblLogout.AddGestureRecognizer(lblLogoutDClicked);
        }
        /// <summary>
        /// Gets the territory Manager state list.
        /// </summary>
        /// <returns>The tm state list.</returns>
        /// <param name="token">Token.</param>
        public async Task GetTmStateList(string token)
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    tmStateLists = await utility.GetTmStateList(token);
                    if(tmStateLists!=null)
                    {
                       TmStateList obj = new TmStateList();
                        obj.regionState = AppResources.SelectState;
                        tmStateLists.Insert(0, obj);
                        List<string> state = tmStateLists.Select(x => x.regionState).ToList();
                        txtStateName.Text = state[1];
                        GetTmDistributorListDetail(txtStateName.Text, token,false);
                        NSUserDefaults.StandardUserDefaults.SetString(txtStateName.Text, AppResources.TmStateName);
                    }
                    else
                    {
                        Toast.MakeText(AppResources.TmState).Show();
                    }
               }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.GetTmStateList, AppResources.TmDistributorSearch, null);
            }
        }
        /// <summary>
        /// Gets the tm distributor list.
        /// </summary>
        /// <returns>The tm distributor list.</returns>
        /// <param name="stateId">State identifier.</param>
        /// <param name="token">Token.</param>
        public async void GetTmDistributorListDetail(string stateId,string token,bool pickerValue)
        {
            if(pickerValue)
            {
                loadPop = new LoadingOverlay(this.View.Frame);
                this.View.Add(loadPop);
                try
                {
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        tmDistributorLists = await utility.GetTmDistributorList(stateId, token);
                        if (tmDistributorLists != null)
                        {
                            TmDistributorList obj = new TmDistributorList();
                            obj.distributorName = AppResources.SelectDistributor;
                            tmDistributorLists.Insert(0, obj);
                            List<string> distributorName = tmDistributorLists.Select(x => x.distributorName).ToList();
                            txtDistributorName.Text = distributorName[0];
                        }
                        else
                        {
                            Toast.MakeText(AppResources.NoDistributors).Show();
                           
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex,AppResources.GetTmDistributorListDetail, AppResources.TmDistributorSearch, null);
                }
                finally
                {
                    loadPop.Hide();
                }
            }
            else
            {
                try
                {
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        tmDistributorLists = await utility.GetTmDistributorList(stateId, token);
                        if (tmDistributorLists != null)
                        {
                            TmDistributorList obj = new TmDistributorList();
                            obj.distributorName = AppResources.SelectDistributor;
                            tmDistributorLists.Insert(0, obj);
                            List<string> distributorName = tmDistributorLists.Select(x => x.distributorName).ToList();
                            txtDistributorName.Text = distributorName[0];
                        }
                        else
                        {
                            Toast.MakeText(AppResources.NoDistributors).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex,AppResources.GetTmDistributorListDetail, AppResources.TmDistributorSearch, null);
                }
                finally
                {
                    loadPop.Hide();
                }
            }
          
        }
        /// <summary>
        /// Gesture implementation to click on UITextField on State Name
        /// Call the distributor picker method
        /// </summary>
        public void TxtStateNameClick()
        {
            UITapGestureRecognizer txtStateSearchClicked = new UITapGestureRecognizer(() =>
            {
                if(tmDistributorPicker!=null)
                {
                    tmDistributorPicker.Hidden = true;
                    btnDone.Hidden = true;
                }
                BindTmStatePicker();
            });
            txtStateName.UserInteractionEnabled = true;
            txtStateName.AddGestureRecognizer(txtStateSearchClicked);
        }
        /// <summary>
        /// Gesture implementation to click on UITextField on Distributor Name
        /// Call the distributor picker method
        /// </summary>
        public void TxtDistributorNameClick()
        {
            UITapGestureRecognizer txtDistributorSearchClicked = new UITapGestureRecognizer(() =>
            {
                if (tmStatePicker != null)
                {
                    tmStatePicker.Hidden = true;
                    btnDone.Hidden = true;
                }
                if (!(string.IsNullOrEmpty(txtStateName.Text) || txtStateName.Text==AppResources.SelectState))
                {
                    BindTmDistributorPicker();
                }
            });
            txtDistributorName.UserInteractionEnabled = true;
            txtDistributorName.AddGestureRecognizer(txtDistributorSearchClicked);
        }
        public void ChildViewClick()
        {
            UITapGestureRecognizer childViewClicked = new UITapGestureRecognizer(() =>
            {
                utility.DismissKeyboardOnTap(new UIView[] { vwChildContent });
            });
            vwChildContent.UserInteractionEnabled = true;
            vwChildContent.AddGestureRecognizer(childViewClicked);
        }
        /// <summary>
        /// Distributor picker.
        /// show the UIPicker
        /// bind data on distributor picker
        /// put the value of distributor id on NSUser defaults
        /// </summary>
        public void BindTmStatePicker()
        {
            try
            {
                if (tmStateLists != null)
                {
                    tmStatePicker = new UIPickerView(new CGRect(Constant.lstDigit[0], UIScreen.MainScreen.Bounds.Height - Constant.lstDigit[23], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[23]));
                    tmStatePicker.BackgroundColor = UIColor.White;
                    tmStatePicker.ShowSelectionIndicator = true;
                    btnDone = new UIButton(new CGRect(tmStatePicker.Frame.X, tmStatePicker.Frame.Y - Constant.lstDigit[15], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[15]));
                    btnDone.BackgroundColor = UIColor.Gray;
                    btnDone.SetTitle(AppResources.Done, UIControlState.Normal);
                    var picker = new TmStateModel(tmStateLists);
                    tmStatePicker.Model = picker;
                    View.AddSubviews(tmStatePicker, btnDone);
                    picker.ValueChanged += (s, e) =>
                    {
                        txtStateName.Text = picker.SelectedValue;
                        if (!(string.IsNullOrEmpty(txtStateName.Text) || txtStateName.Text == AppResources.SelectState))
                        {
                            txtDistributorName.Text = AppResources.SelectDistributor;
                            GetTmDistributorListDetail(txtStateName.Text, token, true);
                            NSUserDefaults.StandardUserDefaults.SetString(txtStateName.Text, AppResources.TmStateName);
                        }
                        else
                        {
                            txtDistributorName.Text = AppResources.SelectDistributor;
                        }
                    };
                    btnDone.TouchUpInside += (s, e) =>
                    {
                        btnDone.Hidden = true;
                        tmStatePicker.Hidden = true;
                        //if (!(string.IsNullOrEmpty(txtStateName.Text) || txtStateName.Text == AppResources.SelectState))
                        //{
                        //    txtDistributorName.Text = AppResources.SelectDistributor;
                        //    GetTmDistributorListDetail(txtStateName.Text, token,true);
                        //    NSUserDefaults.StandardUserDefaults.SetString(txtStateName.Text, AppResources.TmStateName);
                        //}
                    };
                    View.Add(tmStatePicker);
                }
                else
                {
                    Toast.MakeText(AppResources.TmState, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BindTmStatePicker, AppResources.TmDistributorSearch, null);
            }
        }
        /// <summary>
        /// Distributor picker.
        /// show the UIPicker
        /// bind data on distributor picker
        /// put the value of distributor id on NSUser defaults
        /// </summary>
        public void BindTmDistributorPicker()
        {
            try
            {
                if (tmDistributorLists != null)
                {
                    tmDistributorPicker = new UIPickerView(new CGRect(Constant.lstDigit[0], UIScreen.MainScreen.Bounds.Height - Constant.lstDigit[23], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[23]));
                    tmDistributorPicker.BackgroundColor = UIColor.White;
                    tmDistributorPicker.ShowSelectionIndicator = true;
                    btnDone = new UIButton(new CGRect(tmDistributorPicker.Frame.X, tmDistributorPicker.Frame.Y - Constant.lstDigit[15], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[15]));
                    btnDone.BackgroundColor = UIColor.Gray;
                    btnDone.SetTitle(AppResources.Done, UIControlState.Normal);
                    var picker = new TmDistributorModel(tmDistributorLists);
                    tmDistributorPicker.Model = picker;
                    View.AddSubviews(tmDistributorPicker, btnDone);
                    picker.ValueChanged += (s, e) =>
                    {
                        txtDistributorName.Text = picker.SelectedValue;
                        if (!(string.IsNullOrEmpty(txtDistributorName.Text) || txtDistributorName.Text == AppResources.SelectDistributor))
                        {
                            distributorId = tmDistributorLists.Where(x => x.distributorName == txtDistributorName.Text).Select(x => x.dId).FirstOrDefault();
                            NSUserDefaults.StandardUserDefaults.SetString(distributorId,AppResources.TmDistributorID);
                            NSUserDefaults.StandardUserDefaults.SetString(txtDistributorName.Text, AppResources.TmDistributorName);
                        }
                    };
                    btnDone.TouchUpInside += (s, e) =>
                    {
                        tmDistributorPicker.Hidden = true;
                        btnDone.Hidden = true;
                    };
                    View.Add(tmDistributorPicker);
                }
                else
                {
                    Toast.MakeText(AppResources.NoDistributors, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BindTmDistributorPicker, AppResources.TmDistributorSearch, null);
            }
        }

        /// <summary>
        /// Buttonsearch touch up inside event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnSearch_TouchUpInside(UIButton sender)
        {
            try
            {
                if (tmDistributorPicker != null)
                {
                    tmDistributorPicker.Hidden = true;
                }
                if (tmStatePicker != null)
                {
                    tmStatePicker.Hidden = true;
                }
                if(btnDone!=null)
                {
                    btnDone.Hidden = true;
                }
                if (string.IsNullOrEmpty(txtStateName.Text) || txtStateName.Text == AppResources.SelectState)
                {
                    Toast.MakeText(AppResources.RequireState, Constant.durationOfToastMessage).Show();
                }
                else if (string.IsNullOrEmpty(txtDistributorName.Text) || txtDistributorName.Text == AppResources.SelectDistributor)
                {
                    Toast.MakeText(AppResources.RequireDistributor, Constant.durationOfToastMessage).Show();
                }
                else
                {
                    PerformSegue(AppResources.SegueFromTmDistributorToDistributorResult, this);
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BtnSearch_TouchUpInside, AppResources.TmDistributorSearch, null);
            }
        }

        /// <summary>
        /// Back button click event handler
        /// </summary>
        public void ImgBackClick()
        {
            UITapGestureRecognizer imgBackClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
            });
            imgBack.UserInteractionEnabled = true;
            imgBack.AddGestureRecognizer(imgBackClicked);
        }

        /// <summary>
        /// Set the fonts of UILabel, UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblReviewInformation }, new UITextField[] { txtStateName, txtDistributorName }, Constant.lstDigit[9], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblLogout, lblPrivacy, lblSupport, lblTermsAndConditions, lblFaq }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblDistributor,lblDistributorSearch }, new UIButton[] { btnSearch}, Constant.lstDigit[11], viewWidth);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.TmDistributorSearch, null);
            }
        }
    }
}