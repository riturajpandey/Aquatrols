using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Com.Tomergoldst.Tooltips;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used to show user account detail
    /// </summary>
    [Activity]
    public class MyAccountActivity : AppCompatActivity, View.IOnClickListener, ToolTipsManager.ITipListener, View.IOnTouchListener, View.IOnFocusChangeListener
    {
        private IList<string> liuserInfo = null;
        private Button btnSubmit, btnSend;
        private TextView txtNameValue, txtStatusvalue, txtCourse, txtRole, txtApproachPointHeader, txtpointseEarned, txtPointsRedeem, txtPointsBalance, txtNoRecordMessage;
        private TextInputLayout txtInputLayoutOPassword, txtInputLayoutCPassword, txtInputLayoutNPassword;
        private RelativeLayout rlSummary, rlNotification, rlInfoandInfo, rlPurchaseHistory;
        private LinearLayout llRole, llPointsHeader, llEarnedHeader, llRedeemHeader, llBalanceHeader, llEmail, llNotification, llContact, llPurchaseHistoryHeader;
        private EditText editOPassword, editCPassword, editNPassword;
        private ImageView imgMenu, imgBack;
        private CheckBox chkEmail, chkNotification;
        private string token, userId;
        public static bool canseeOldpassword, canseeNewpassword, canseeConpassword;
        public bool isEmailPreference = false, isNotification = false, isPurchaseHistoryDataEmpty;
        private ListView lstpHistory;
        private ImageView imgPointsEarned, imgPointsRedeem, imgPointsBalance;

        #region
        //tooltip
        private string Tag = nameof(MyAccountActivity);
        private ToolTipsManager _toolTipsManager;
        private RelativeLayout _rootLayout;
        int _align = ToolTip.AlignLeft;
        #endregion

        /// <summary>
        /// This method is used to initialize page load value and Hit Purchase Historty List api
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.MyAccountLayout);
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode. 
                // Handle exception handling 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                FindControlsById();
                canseeOldpassword = false;
                canseeNewpassword = false;
                canseeConpassword = false;
                llPointsHeader.Visibility = ViewStates.Gone;
                _toolTipsManager = new ToolTipsManager(this);
                editCPassword.ClearFocus();
                editNPassword.ClearFocus();
                editOPassword.ClearFocus();
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                {
                    token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null)))
                {
                    userId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null);
                }
                #region //Google Analytics
                GoogleAnalytics.Current.Config.TrackingId = Resources.GetString(Resource.String.TrackingId);
                GoogleAnalytics.Current.Config.AppId = ApplicationContext.PackageName;
                GoogleAnalytics.Current.Config.AppName = this.ApplicationInfo.LoadLabel(PackageManager).ToString();
                GoogleAnalytics.Current.Config.AppVersion = this.PackageManager.GetPackageInfo(this.PackageName, 0).VersionName;
                GoogleAnalytics.Current.InitTracker();
                GoogleAnalytics.Current.Tracker.SendView("MyAccount");
                #endregion
                HitPurchaseHistortList();
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsNotificationChecked), null)))
                {
                    string flag = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsNotificationChecked), null);
                    if (flag.ToLower().Equals(Resources.GetString(Resource.String.yes)))
                    {
                        chkNotification.Checked = true;
                    }
                    else
                    {
                        chkNotification.Checked = false;
                    }
                }
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsEmailChecked), null) != null)
                {
                    string flag = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsEmailChecked), null);
                    if (flag.ToLower().Equals(Resources.GetString(Resource.String.yes)))
                    {
                        chkEmail.Checked = true;
                    }
                    else
                    {
                        chkEmail.Checked = false;
                    }
                }
                if (Intent.GetStringArrayListExtra(Resources.GetString(Resource.String.userinfo)) != null)
                {
                    liuserInfo = Intent.GetStringArrayListExtra(Resources.GetString(Resource.String.userinfo));
                    if (liuserInfo.Count > 0)
                    {
                        txtNameValue.Text = liuserInfo[0];
                        txtCourse.Text = liuserInfo[1];
                        txtStatusvalue.Text = liuserInfo[2];
                        txtRole.Text = liuserInfo[3];
                        txtpointseEarned.Text = liuserInfo[4] + " " + Resources.GetString(Resource.String.Points);
                        txtPointsRedeem.Text = liuserInfo[5] + " " + Resources.GetString(Resource.String.Points);
                        txtPointsBalance.Text = liuserInfo[6] + " " + Resources.GetString(Resource.String.Points);
                    }
                }
                chkEmail.CheckedChange += ChkEmail_CheckedChange;
                chkNotification.CheckedChange += ChkNotification_CheckedChange;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to call purchase history API
        /// </summary>
        public async void HitPurchaseHistortList()
        {
            List<PurchaseHistoryEntity> purchaseHistoryEntity;
            try
            {
                using (Utility utility = new Utility(this))
                {
                    bool internetStatus = utility.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        purchaseHistoryEntity = await utility.GetPurchaseHistory(token, userId);
                        if (purchaseHistoryEntity != null && purchaseHistoryEntity.Count > 0)
                        {
                            isPurchaseHistoryDataEmpty = false;
                            lstpHistory.Adapter = new PurchaseHistoryAdapter(this, purchaseHistoryEntity);
                        }
                        else
                        {
                            isPurchaseHistoryDataEmpty = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitPurchaseHistortList), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }

        }

        /// <summary>
        /// Show/Hide summary section
        /// </summary>
        public void ToggleAccountSummary()
        {
            try
            {
                if (llEarnedHeader.Visibility == ViewStates.Visible)
                {
                    llEarnedHeader.Visibility = ViewStates.Gone;
                }
                else
                {
                    llEarnedHeader.Visibility = ViewStates.Visible;
                }

                if (llRedeemHeader.Visibility == ViewStates.Visible)
                {
                    llRedeemHeader.Visibility = ViewStates.Gone;
                }
                else
                {
                    llRedeemHeader.Visibility = ViewStates.Visible;
                }

                if (llBalanceHeader.Visibility == ViewStates.Visible)
                {
                    llBalanceHeader.Visibility = ViewStates.Gone;
                }
                else
                {
                    llBalanceHeader.Visibility = ViewStates.Visible;
                }
                llRedeemHeader.Visibility = ViewStates.Gone;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ToggleAccountSummary), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Show/Hide ino and password section.
        /// </summary>
        public void ToggleInfoAndPass()
        {
            try
            {
                if (txtInputLayoutCPassword.Visibility == ViewStates.Visible && txtInputLayoutOPassword.Visibility == ViewStates.Visible && txtInputLayoutNPassword.Visibility == ViewStates.Visible && btnSubmit.Visibility == ViewStates.Visible)
                {
                    txtInputLayoutCPassword.Visibility = ViewStates.Gone;
                    txtInputLayoutOPassword.Visibility = ViewStates.Gone;
                    txtInputLayoutNPassword.Visibility = ViewStates.Gone;
                    btnSubmit.Visibility = ViewStates.Gone;
                }
                else
                {
                    txtInputLayoutCPassword.Visibility = ViewStates.Visible;
                    txtInputLayoutOPassword.Visibility = ViewStates.Visible;
                    txtInputLayoutNPassword.Visibility = ViewStates.Visible;
                    btnSubmit.Visibility = ViewStates.Visible;
                }
                editCPassword.ClearFocus();
                editNPassword.ClearFocus();
                editOPassword.ClearFocus();
                txtInputLayoutCPassword.ErrorEnabled = false;
                txtInputLayoutNPassword.ErrorEnabled = false;
                txtInputLayoutOPassword.ErrorEnabled = false;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Show/Hide notificaton section
        /// </summary>
        public void ToggleNotification()
        {
            try
            {
                if (llEmail.Visibility == ViewStates.Visible)
                {
                    llEmail.Visibility = ViewStates.Gone;
                }
                else
                {
                    llEmail.Visibility = ViewStates.Visible;
                }

                if (llNotification.Visibility == ViewStates.Visible)
                {
                    llNotification.Visibility = ViewStates.Gone;
                }
                else
                {
                    llNotification.Visibility = ViewStates.Visible;
                }

                if (llContact.Visibility == ViewStates.Visible)
                {
                    llContact.Visibility = ViewStates.Gone;
                }
                else
                {
                    llContact.Visibility = ViewStates.Visible;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ToggleNotification), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Show/Hide purchase History section
        /// </summary>
        public void TogglePurchasehistory()
        {
            try
            {
                if (lstpHistory.Visibility == ViewStates.Visible || txtNoRecordMessage.Visibility == ViewStates.Visible)
                {
                    lstpHistory.Visibility = ViewStates.Gone;
                    llPurchaseHistoryHeader.Visibility = ViewStates.Gone;
                    txtNoRecordMessage.Visibility = ViewStates.Gone;
                }
                else
                {
                    if (isPurchaseHistoryDataEmpty == false)
                    {
                        lstpHistory.Visibility = ViewStates.Visible;
                        llPurchaseHistoryHeader.Visibility = ViewStates.Visible;
                        txtNoRecordMessage.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        lstpHistory.Visibility = ViewStates.Gone;
                        llPurchaseHistoryHeader.Visibility = ViewStates.Gone;
                        txtNoRecordMessage.Visibility = ViewStates.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.TogglePurchasehistory), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when notification checkbox value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkNotification_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                isNotification = e.IsChecked;
                if (e.IsChecked)
                {
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsNotificationChecked), Resources.GetString(Resource.String.yes)).Commit();
                }
                else
                {
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsNotificationChecked), Resources.GetString(Resource.String.no)).Commit();
                }
                HitUpdateNotificationAPI();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ChkNotification_CheckedChange), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when email checkbox value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkEmail_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                isEmailPreference = e.IsChecked;
                if (e.IsChecked)
                {
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsEmailChecked), Resources.GetString(Resource.String.yes)).Commit();
                }
                else
                {
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsEmailChecked), Resources.GetString(Resource.String.no)).Commit();
                }
                HitUpdateNotificationAPI();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ChkEmail_CheckedChange), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Showing wait cursor
        /// </summary>
        OverlayActivity overlay;
        public void Show_Overlay()
        {
            try
            {
                overlay = new OverlayActivity(this);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// API call to update notification setting
        /// </summary>
        public async void HitUpdateNotificationAPI()
        {
            Show_Overlay();
            NotificationResponseEntity notificationResponseEntity;
            NotificationEntity notificationEntity = new NotificationEntity();
            try
            {
                notificationEntity.isEmailPreference = isEmailPreference;
                notificationEntity.isNotification = isNotification;
                if (userId != null)
                {
                    notificationEntity.UserId = userId;
                }
                using (Utility utility = new Utility(this))
                {
                    bool internetStatus = utility.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        notificationResponseEntity = await utility.UpdateNotificationSetting(notificationEntity, token);
                        if (notificationResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                        {
                            Toast.MakeText(this, notificationResponseEntity.operationMessage, ToastLength.Long).Show();
                        }
                        else
                        {
                            Toast.MakeText(this, notificationResponseEntity.operationMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitUpdateNotificationAPI), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Getting the reference of Controls
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                txtNameValue = FindViewById<TextView>(Resource.Id.txtNamevalue);
                txtApproachPointHeader = FindViewById<TextView>(Resource.Id.txtApproachPointHeader);
                txtStatusvalue = FindViewById<TextView>(Resource.Id.txtStatusvalue);
                txtCourse = FindViewById<TextView>(Resource.Id.txtCoursevalue);
                txtRole = FindViewById<TextView>(Resource.Id.txtRolevalue);
                txtpointseEarned = FindViewById<TextView>(Resource.Id.txtPointsEarned);
                txtPointsRedeem = FindViewById<TextView>(Resource.Id.txtPointsRedeem);
                txtPointsBalance = FindViewById<TextView>(Resource.Id.txtPointsBalance);
                txtNoRecordMessage = FindViewById<TextView>(Resource.Id.txtNoRecordMessage);
                txtInputLayoutOPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutOPassword);
                txtInputLayoutCPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutCPassword);
                txtInputLayoutNPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutNPassword);
                llRole = FindViewById<LinearLayout>(Resource.Id.llRole);
                llPointsHeader = FindViewById<LinearLayout>(Resource.Id.llpointsHeader);
                llEarnedHeader = FindViewById<LinearLayout>(Resource.Id.llEarnedHeader);
                llRedeemHeader = FindViewById<LinearLayout>(Resource.Id.llRedeemHeader);
                llBalanceHeader = FindViewById<LinearLayout>(Resource.Id.llBalanceHeader);
                llPurchaseHistoryHeader = FindViewById<LinearLayout>(Resource.Id.llPurchaseHistoryHeader);
                llEmail = FindViewById<LinearLayout>(Resource.Id.llEmail);
                llContact = FindViewById<LinearLayout>(Resource.Id.llContact);
                llNotification = FindViewById<LinearLayout>(Resource.Id.llNotification);
                chkEmail = FindViewById<CheckBox>(Resource.Id.chkEmail);
                chkNotification = FindViewById<CheckBox>(Resource.Id.chkNotification);
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                rlSummary = FindViewById<RelativeLayout>(Resource.Id.rlSummary);
                rlNotification = FindViewById<RelativeLayout>(Resource.Id.rlNotification);
                rlInfoandInfo = FindViewById<RelativeLayout>(Resource.Id.rlInfoAndPass);
                rlPurchaseHistory = FindViewById<RelativeLayout>(Resource.Id.rlPurchaseHistory);
                imgMenu = FindViewById<ImageView>(Resource.Id.imgMenu);
                editOPassword = FindViewById<EditText>(Resource.Id.editOldPassword);
                editCPassword = FindViewById<EditText>(Resource.Id.editConPassword);
                editNPassword = FindViewById<EditText>(Resource.Id.editNewPassword);
                txtInputLayoutOPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutOPassword);
                txtInputLayoutNPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutNPassword);
                txtInputLayoutCPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutCPassword);
                btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
                btnSend = FindViewById<Button>(Resource.Id.btnSend);
                imgPointsEarned = FindViewById<ImageView>(Resource.Id.imgPointsEarned);
                imgPointsRedeem = FindViewById<ImageView>(Resource.Id.imgPointsRedeem);
                imgPointsBalance = FindViewById<ImageView>(Resource.Id.imgPointsBalance);
                lstpHistory = FindViewById<ListView>(Resource.Id.lstPurchaseHistory);
                #region
                //setting event listeners
                editOPassword.AddTextChangedListener(new CustomTextWatcher(editOPassword, this));
                editNPassword.AddTextChangedListener(new CustomTextWatcher(editNPassword, this));
                editCPassword.AddTextChangedListener(new CustomTextWatcher(editCPassword, this));
                editOPassword.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                editNPassword.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                editCPassword.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                editOPassword.SetOnTouchListener(this);
                editNPassword.SetOnTouchListener(this);
                editCPassword.SetOnTouchListener(this);
                imgMenu.Click += ImgMenu_Click;
                rlSummary.SetOnClickListener(this);
                rlInfoandInfo.SetOnClickListener(this);
                rlNotification.SetOnClickListener(this);
                rlPurchaseHistory.SetOnClickListener(this);
                btnSubmit.SetOnClickListener(this);
                btnSend.SetOnClickListener(this);
                imgBack.SetOnClickListener(this);
                imgPointsEarned.SetOnClickListener(this);
                imgPointsRedeem.SetOnClickListener(this);
                imgPointsBalance.SetOnClickListener(this);
                #endregion
                TextInputLayout[] textInputs = { txtInputLayoutOPassword, txtInputLayoutNPassword, txtInputLayoutCPassword };
                foreach (TextInputLayout txtInput in textInputs)
                {
                    txtInput.ErrorEnabled = false;
                    txtInput.Error = null;
                }
                EditText[] editTxts = { editOPassword, editNPassword, editCPassword };
                foreach (EditText edit in editTxts)
                {
                    edit.OnFocusChangeListener = this;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when Menu item click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgMenu_Click(object sender, EventArgs e)
        {
            try
            {
                PopupMenu menu = new PopupMenu(this, imgMenu);
                menu.Inflate(Resource.Menu.OptionMenu);
                menu.Show();
                menu.MenuItemClick += (s1, arg1) =>
                {
                    Intent intent;
                    switch (arg1.Item.ItemId)
                    {
                        case Resource.Id.logoutID:
                            intent = new Intent(this, typeof(LoginActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.isApproved), null).Commit();
                            Finish();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.termsID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.yes)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.privacyID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.no)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.FAQ:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.FAQ)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.SupportID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.support)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.About:
                            intent = new Intent(this, typeof(AboutAppActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.about), Resources.GetString(Resource.String.about)).Commit();
                            this.StartActivity(intent);
                            break;
                        default:
                            break;
                    }
                };
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ImgMenu_Click), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when device back button click
        /// </summary>
        public override void OnBackPressed()
        {
            Finish();
        }

        /// <summary>
        /// Calling API to change password
        /// </summary>
        public async void HitChangePasswordAPI()
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    Show_Overlay();
                    bool internetStatus = utility.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        ChangePasswordResponseEntity response = await utility.ChangePassword(GetPasswordEntity(), token);
                        if (response.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                        {
                            editCPassword.Text = string.Empty;
                            editNPassword.Text = string.Empty;
                            editOPassword.Text = string.Empty;
                            Intent intent = new Intent(this, typeof(LoginActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.isApproved), null).Commit();
                            StartActivity(intent);
                            Finish();
                        }
                        Toast.MakeText(this, response.operationMessage, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitChangePasswordAPI), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// This method gets the password entity data
        /// </summary>
        /// <returns></returns>
        public ChangePasswordEntity GetPasswordEntity()
        {
            ChangePasswordEntity changePasswordEntity = new ChangePasswordEntity();
            try
            {
                changePasswordEntity.userId = userId;
                changePasswordEntity.currentPassword = editOPassword.Text;
                changePasswordEntity.newPassword = editNPassword.Text;
                changePasswordEntity.confirmPassword = editCPassword.Text;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetPasswordEntity), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
            return changePasswordEntity;
        }

        /// <summary>
        /// After All Validation Done calling Change Password API
        /// </summary>
        public void SubmitForm()
        {
            try
            {
                Utility utility = new Utility(this);
                if (string.IsNullOrEmpty(editOPassword.Text))
                {
                    txtInputLayoutOPassword.Error = Resources.GetString(Resource.String.currentpasswordrequired);
                    editOPassword.RequestFocus();
                }
                else
                {
                    if (string.IsNullOrEmpty(editNPassword.Text))
                    {
                        txtInputLayoutNPassword.Error = Resources.GetString(Resource.String.newpasswordrequired);
                        editNPassword.RequestFocus();
                    }
                    else
                    {
                        if (!utility.IsValidPassword(editNPassword.Text))
                        {
                            txtInputLayoutNPassword.Error = Resources.GetString(Resource.String.InvalidPassword);
                            editNPassword.RequestFocus();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(editCPassword.Text))
                            {
                                txtInputLayoutCPassword.Error = Resources.GetString(Resource.String.confirmpassswordrequired);
                                editCPassword.RequestFocus();
                            }
                            else
                            {
                                if (!utility.IsValidPassword(editCPassword.Text))
                                {
                                    txtInputLayoutCPassword.Error = Resources.GetString(Resource.String.InvalidPassword);
                                    editCPassword.RequestFocus();
                                }
                                else if (!editCPassword.Text.Equals(editNPassword.Text))
                                {
                                    txtInputLayoutCPassword.Error = Resources.GetString(Resource.String.PasswordNotMatch);
                                    editCPassword.RequestFocus();
                                }
                                else
                                {
                                    txtInputLayoutCPassword.ErrorEnabled = false;
                                    editCPassword.ClearFocus();
                                    editNPassword.ClearFocus();
                                    editOPassword.ClearFocus();
                                    HitChangePasswordAPI();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SubmitForm), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Onclick event
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                ToolTip.Builder builder;
                _rootLayout = FindViewById<RelativeLayout>(Resource.Id.RootLayout);
                switch (v.Id)
                {
                    case Resource.Id.imgBack:
                        Finish();
                        break;
                    case Resource.Id.rlSummary:
                        ToggleAccountSummary();
                        break;
                    case Resource.Id.rlInfoAndPass:
                        ToggleInfoAndPass();
                        break;
                    case Resource.Id.rlNotification:
                        ToggleNotification();
                        break;
                    case Resource.Id.rlPurchaseHistory:
                        TogglePurchasehistory();
                        break;
                    case Resource.Id.btnSubmit:
                        SubmitForm();
                        break;
                    case Resource.Id.imgPointsEarned:
                        _toolTipsManager.FindAndDismiss(imgPointsEarned);
                        builder = new ToolTip.Builder(this, imgPointsEarned, _rootLayout, Resources.GetString(Resource.String.pointsEarnedText), ToolTip.PositionAbove);
                        builder.SetAlign(_align);
                        builder.SetBackgroundColor(ContextCompat.GetColor(this, Resource.Color.grey));
                        _toolTipsManager.Show(builder.Build());
                        break;
                    case Resource.Id.imgPointsBalance:
                        _toolTipsManager.FindAndDismiss(imgPointsBalance);
                        builder = new ToolTip.Builder(this, imgPointsBalance, _rootLayout, Resources.GetString(Resource.String.pointsBalanceText), ToolTip.PositionAbove);
                        builder.SetAlign(_align);
                        builder.SetBackgroundColor(ContextCompat.GetColor(this, Resource.Color.grey));
                        _toolTipsManager.Show(builder.Build());
                        break;
                    case Resource.Id.imgPointsRedeem:
                        _toolTipsManager.FindAndDismiss(imgPointsRedeem);
                        builder = new ToolTip.Builder(this, imgPointsRedeem, _rootLayout, Resources.GetString(Resource.String.pointsRedeemText), ToolTip.PositionAbove);
                        builder.SetAlign(_align);
                        builder.SetBackgroundColor(ContextCompat.GetColor(this, Resource.Color.grey));
                        _toolTipsManager.Show(builder.Build());
                        break;
                    case Resource.Id.btnSend:
                        try
                        {
                            Intent intent = new Intent(Intent.ActionSend);
                            intent.SetType("text/html");
                            intent.PutExtra(Intent.ExtraEmail, new String[] { Resources.GetText(Resource.String.ApproachEmail) });
                            intent.PutExtra(Intent.ExtraSubject, string.Empty);
                            StartActivity(Intent.CreateChooser(intent, "Send Email"));
                        }
                        catch
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.noAppFound), ToastLength.Long).Show();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to Show/hide password when click on eye icon
        /// </summary>
        /// <param name="edittext"></param>
        public void ShowHidePassword(EditText edittext)
        {
            try
            {
                EditText edit = edittext;
                switch (edit.Id)
                {
                    case Resource.Id.editOldPassword:
                        {
                            if (canseeOldpassword)
                            {
                                canseeOldpassword = false;
                                edit.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisibleBlue, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            else
                            {
                                canseeOldpassword = true;
                                edit.InputType = InputTypes.TextVariationPassword;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisibleBlue, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            break;
                        }
                    case Resource.Id.editConPassword:
                        {
                            if (canseeConpassword)
                            {
                                canseeConpassword = false;
                                edit.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisibleBlue, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            else
                            {
                                canseeConpassword = true;
                                edit.InputType = InputTypes.TextVariationPassword;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisibleBlue, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            break;
                        }
                    case Resource.Id.editNewPassword:
                        {
                            if (canseeNewpassword)
                            {
                                canseeNewpassword = false;
                                edit.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisibleBlue, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            else
                            {
                                canseeNewpassword = true;
                                edit.InputType = InputTypes.TextVariationPassword;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisibleBlue, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ShowHidePassword), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// This method is checks validation when focus changes of edittext
        /// </summary>
        /// <param name="v"></param>
        /// <param name="hasFocus"></param>
        public void OnFocusChange(View v, bool hasFocus)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.editOldPassword:
                        CheckValidation(editOPassword, txtInputLayoutOPassword, hasFocus);
                        break;
                    case Resource.Id.editNewPassword:
                        CheckValidation(editNPassword, txtInputLayoutNPassword, hasFocus);
                        break;
                    case Resource.Id.editConPassword:
                        CheckValidation(editCPassword, txtInputLayoutCPassword, hasFocus);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnFocusChange), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// On touch listener to show/hide password characters
        /// </summary>
        /// <param name="v"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnTouch(View v, MotionEvent e)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.editOldPassword:
                        {
                            if (e.Action == MotionEventActions.Down)
                            {
                                if (editOPassword.GetCompoundDrawables()[2] != null)
                                {
                                    if (e.RawX >= (editOPassword.Right - editOPassword.GetCompoundDrawables()[2].Bounds.Width()))
                                    {
                                        ShowHidePassword(editOPassword);
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case Resource.Id.editConPassword:
                        {
                            if (e.Action == MotionEventActions.Down)
                            {
                                if (editCPassword.GetCompoundDrawables()[2] != null)
                                {
                                    if (e.RawX >= (editCPassword.Right - editCPassword.GetCompoundDrawables()[2].Bounds.Width()))
                                    {
                                        ShowHidePassword(editCPassword);
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case Resource.Id.editNewPassword:
                        {
                            if (e.Action == MotionEventActions.Down)
                            {
                                if (editNPassword.GetCompoundDrawables()[2] != null)
                                {
                                    if (e.RawX >= (editNPassword.Right - editNPassword.GetCompoundDrawables()[2].Bounds.Width()))
                                    {
                                        ShowHidePassword(editNPassword);
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnTouch), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
            return false;
        }

        /// <summary>
        /// Performing validation
        /// </summary>
        /// <param name="edittxt"></param>
        /// <param name="txtInputLayout"></param>
        /// <param name="hasFocus"></param>
        private void CheckValidation(EditText edittxt, TextInputLayout txtInputLayout, bool hasFocus)
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    if (hasFocus == false)
                    {
                        if (string.IsNullOrEmpty(edittxt.Text.Trim())) //Check if any edittext is empty except the redemption code
                        {
                            switch (edittxt.Id)
                            {
                                case Resource.Id.editOldPassword:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.currentpasswordrequired);
                                    break;
                                case Resource.Id.editNewPassword:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.newpasswordrequired);
                                    break;
                                case Resource.Id.editConPassword:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.confirmpassswordrequired);
                                    break;
                            }
                            return;
                        }
                        else if (edittxt.Id == Resource.Id.editNewPassword) //Check the valid Email
                        {
                            if (!utility.IsValidPassword(editNPassword.Text.Trim()))
                            {
                                txtInputLayout.Error = Resources.GetString(Resource.String.InvalidPassword);
                            }
                            else
                            {
                                txtInputLayout.ErrorEnabled = false;
                            }
                            return;
                        }
                        else if (edittxt.Id == Resource.Id.editConPassword) //Check the valid mobile
                        {
                            if (!utility.IsValidPassword(editCPassword.Text.Trim()))
                            {
                                txtInputLayout.Error = Resources.GetString(Resource.String.InvalidPassword);
                            }
                            else if (!editCPassword.Text.Equals(editNPassword.Text))
                            {
                                txtInputLayout.Error = Resources.GetString(Resource.String.PasswordNotMatch);
                            }
                            else
                            {
                                txtInputLayout.ErrorEnabled = false;
                            }
                            return;
                        }
                    }
                    else //when focus comes again
                    {
                        txtInputLayout.ErrorEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckValidation), Resources.GetString(Resource.String.MyAccountActivity), null);
                }
            }
        }

        /// <summary>
        /// Dissmiss the tooltip
        /// </summary>
        /// <param name="view"></param>
        /// <param name="anchorViewId"></param>
        /// <param name="byUser"></param>
        public void OnTipDismissed(View view, int anchorViewId, bool byUser)
        {
            if (anchorViewId == Resource.Id.imgPointsEarned)
            {
                // Do something when a tip near view with id "Resource.Id.text_view" has been dismissed
            }
        }
    }
}