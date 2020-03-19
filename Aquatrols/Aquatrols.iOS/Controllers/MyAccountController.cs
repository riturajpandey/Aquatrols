using System;
using System.Collections.Generic;
using Aquatrols.iOS.Helper;
using Aquatrols.iOS.TableVewSource;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using MessageUI;
using Plugin.GoogleAnalytics;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.Controllers
{
    /// <summary>
    /// This class file My Account controller is used for user account screen
    /// </summary>
    public partial class MyAccountController : UIViewController
    {
        private LoadingOverlay loadPop;
        private UserInfoEntity userInfoEntity = null;
        private List<PurchaseHistoryEntity> lstPurchaseHistoryEntity = null;
        private string token;
        private bool flag, flag1;
        private Utility utility = Utility.GetInstance;
        private int viewWidth, viewHeight;
        private string userId = String.Empty;
        private string device;
        MFMailComposeViewController mailController;
        public MyAccountController(IntPtr handle) : base(handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
            string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
            if (!String.IsNullOrEmpty(exception))
            {
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.MyAccount, exception);
            }
            // get the user id 
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
            }

            lblPointsRedeemed.Hidden = true;
            lblPointsBalance.Hidden = true;
            ImgPointsBalancInfo.Hidden = true;
            lblAccountSummary.Hidden = true;
            LblFAQClick();
            LblSupportClick();
            ImgBackArrowClick();
            ImgMenuClick();
            LblLogoutClick();
            HitUserInfAPI(userId);
            UserInfoClick();
            HeaderClick();
            MainMyAccountViewClick();
            PopUpClick();
            CheckBoxEmailClick();
            CheckBoxNotificationClick();
            LblAccountSummaryClick();
            LblInformationPasswordClick();
            LblNotificationsClick();
            PointsEarnedInfoClick();
            PointsRedeemedInfoClick();
            PointsBalancInfoClick();
            ConfirmPasswordEyeClick();
            ChangePasswordEyeClick();
            CurrentPasswordEyeClick();
            TermsAndConditionsClick();
            LblPrivacyPolicyClick();
            AddDoneButton();
            DismissKeyboardClick();
            SetUISize();
            SetFonts();
            tblPurchaseHistory.Hidden = true;
            SubViewPurchaseHistory.Hidden = true;
            try
            {
                bool isConnected = utility.CheckInternetConnection(); // check the internet connection
                if (isConnected)
                {
                    // implement the google analytics
                    GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                    GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                    GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                    GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                    GoogleAnalytics.Current.InitTracker();
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.MyAccount);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.MyAccount, null);// exception handling 
            }
            // Perform any additional setup after loading the view, typically from a nib.
        }
        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            try
            {
                bool isTerms = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isTermsConditions);
                if (isTerms)
                {
                    NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isTermsConditions);
                }
                //check setting to the user account, to check email and notification setting
                flag = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isEmailPreference);
                if (flag == true)
                {
                    ImgCheckboxEmail.Image = UIImage.FromBundle(AppResources.ImgCheckbox);
                }
                else
                {
                    ImgCheckboxEmail.Image = UIImage.FromBundle(AppResources.ImgUnCheckbox);
                }
                flag1 = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isNotification);
                if (flag1 == true)
                {
                    ImgCheckboxNotification.Image = UIImage.FromBundle(AppResources.ImgCheckbox);
                }
                else
                {
                    ImgCheckboxNotification.Image = UIImage.FromBundle(AppResources.ImgUnCheckbox);
                }
                HitPurchaseHistoryAPI(); // hit the purchase history API
                LblPurchaseHistoryClick();
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewWillAppear, AppResources.MyAccount, null); // exception handling 
            }
        }
        /// <summary>
        /// Set the fonts of UILabel, UITextField and UIButton.
        /// Padding to UITextField.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetPadding(new UITextField[] { txtCurrentPassword, txtConfirmPassword, txtUpdatePassword }, Constant.lstDigit[4]);
                Utility.SetFontsforHeader(new UILabel[] { lblMyAccount, }, new UIButton[] { btnSubmit, btnSendEmail }, Constant.lstDigit[11], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblName, lblCourse, lblStatus, lblRole, lblTermsAndConditions, lblLogout, lblPrivacy, lblApproachValue, lblStatusValue, lblCoursevalue, lblFullNameValue, lblPointsEarned, lblPointsRedeemed, lblPointsBalance, lblPointsEarnedValue, lblPointsRedemeedValue, lblPointsBalanceValue, lblCurrentPassword, lblUpdatePassword, lblConfirmPassword, lblEmail, lblNotification, lblSend, lblUpdate, lblContact,lblFaq,lblSupport }, new UITextField[] { txtCurrentPassword, txtConfirmPassword, txtUpdatePassword }, Constant.lstDigit[8], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblAccountSummary, lblInformationPassword, lblNotifications ,lblPurchaseHistory}, null, Constant.lstDigit[9], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblCommentPassword }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblProduct,lblQuantity,lblDistributorName,lblNoRecord }, null, Constant.lstDigit[8], viewWidth);
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.MyAccount, null); // exception handling 
            }
        }
        /// <summary>
        /// Set UI size sepcified in iPhone X.
        /// </summary>
        public void SetUISize()
        {
            try
            {
                device = UIDevice.CurrentDevice.Model; // get the device type 
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;// get the device width
                viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;// get the device height 
                if (viewWidth == (int)DeviceScreenSize.ISix)
                {
                    if (viewHeight == Constant.lstDigit[31])
                    {
                        headerView.Frame = new CGRect(headerView.Frame.X, headerView.Frame.Y + Constant.lstDigit[10], headerView.Frame.Width, headerView.Frame.Height);
                        MyAccountScroll.Frame = new CGRect(MyAccountScroll.Frame.X, headerView.Frame.Y + headerView.Frame.Height, MyAccountScroll.Frame.Width, MyAccountScroll.Frame.Height);
                        MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, viewHeight - headerView.Bounds.Height);
                        popUpMenuMyAccount.Frame = new CGRect(popUpMenuMyAccount.Frame.X, popUpMenuMyAccount.Frame.Y + Constant.lstDigit[10], popUpMenuMyAccount.Frame.Width, popUpMenuMyAccount.Frame.Height);
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetUISize, AppResources.MyAccount, null); // exception handling
            }
        }
        /// <summary>
        /// Hit the user information API.
        /// get the earned,spent and balance points value and put the text
        /// set the value of fullname,course name and balance points
        /// </summary>
        /// <param name="userId">User identifier.</param>
        protected async void HitUserInfAPI(string userId)
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    userInfoEntity = await utility.GetUserInfo(userId,token);
                    if (!string.IsNullOrEmpty(userInfoEntity.userId))
                    {
                        lblFullNameValue.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCoursevalue.Text = userInfoEntity.courseName;
                        lblStatusValue.Text = userInfoEntity.statusName;
                        lblApproachValue.Text = userInfoEntity.role;
                        int earned = userInfoEntity.earnedPoints;
                        string earnerPoint = earned.ToString(AppResources.Comma);
                        lblPointsEarnedValue.Text = earnerPoint + AppResources.Points;
                        int spent = userInfoEntity.pointsSpent;
                        string spentPoint = spent.ToString(AppResources.Comma);
                        lblPointsRedemeedValue.Text = spentPoint + AppResources.Points;
                        int balance = userInfoEntity.balancedPoints;
                        string balancePoint = balance.ToString(AppResources.Comma);
                        lblPointsBalanceValue.Text = "";//balancePoint + AppResources.Points;
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isEmailPreference, AppResources.isEmailPreference);
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isNotification, AppResources.isNotification);
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitUserInfAPI, AppResources.MyAccount, null); // exception handling 
            }
            finally
            {
                loadPop.Hide(); // hide the loader 
            }
        }
        /// <summary>
        /// Hit the notification API.
        /// Update the value of Email and notifications to click on check box
        /// </summary>
        protected async void HitNotificationAPI()
        {
            try
            {
                NotificationEntity notificationEntity = new NotificationEntity();
                notificationEntity.UserId = userId;
                notificationEntity.isEmailPreference =NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isEmailPreference);
                notificationEntity.isNotification =NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isNotification);
                token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    NotificationResponseEntity notificationResponseEntity = await utility.UpdateNotificationSetting(notificationEntity,token);
                    if (notificationResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                    {
                        Toast.MakeText(notificationResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                    }
                    else
                    {
                        Toast.MakeText(notificationResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitNotificationAPI, AppResources.MyAccount, null);
            }
        }
        /// <summary>
        /// Hit the purchase history API.
        /// </summary>
        protected async void HitPurchaseHistoryAPI()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                if (isConnected)
                {
                    lstPurchaseHistoryEntity = await utility.GetPurchaseHistoryInformation(userId,token);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                } 
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitPurchaseHistoryAPI, AppResources.MyAccount, null);
            }
        }
        /// <summary>
        /// Purchase the history.
        /// To bind data to purchase history table view source
        /// if purchase history is null then show the view of no record found otherwise show the data of table view source
        /// </summary>
        public void PurchaseHistory()
        {
            try
            {
                if (lstPurchaseHistoryEntity != null)
                {
                    List<PurchaseHistoryEntity> lstPurchaseHistory = new List<PurchaseHistoryEntity>();
                    lstPurchaseHistory = lstPurchaseHistoryEntity;
                    TableViewPurchaseHistory tableViewPurchaseHistory = new TableViewPurchaseHistory(this, lstPurchaseHistory);
                    tblPurchaseHistory.Source = tableViewPurchaseHistory;
                    tblPurchaseHistory.ReloadData();
                    tblPurchaseHistory.Hidden = false;
                    lblNoRecord.Hidden = true;
                }
                else
                {
                    lblNoRecord.Hidden = false;
                    tblPurchaseHistory.Hidden = true;
                    lblProduct.Hidden = true;
                    lblQuantity.Hidden = true;
                    lblDistributorName.Hidden = true;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.PurchaseHistory, AppResources.MyAccount, null);
            }
        }
        /// <summary>
        /// Change password of the user.
        /// Hit Change password API
        /// </summary>
        protected async void ChangePasswordMyAccount()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                ChangePasswordEntity changePasswordEntity = new ChangePasswordEntity();
                changePasswordEntity.userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
                changePasswordEntity.currentPassword = txtCurrentPassword.Text.Trim();
                changePasswordEntity.newPassword = txtUpdatePassword.Text.Trim();
                changePasswordEntity.confirmPassword = txtConfirmPassword.Text.Trim();
                token= NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    ChangePasswordResponseEntity changePasswordResponseEntity = await utility.ChangePassword(changePasswordEntity,token);
                    if (changePasswordResponseEntity.operationStatus.ToLower() == AppResources.success)
                    {
                        Toast.MakeText(changePasswordResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                        txtCurrentPassword.Text = string.Empty;
                        txtConfirmPassword.Text = string.Empty;
                        txtUpdatePassword.Text = string.Empty;
                        ImgCurrentPasswordEye.Hidden = true;
                        ImgChangePasswordEye.Hidden = true;
                        ImgConfirmPasswordEye.Hidden = true;
                        Utility.ClearCachedData();
                        this.NavigationController.PopToRootViewController(true);
                    }
                    else
                    {
                        Toast.MakeText(changePasswordResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ChangePasswordMyAccount, AppResources.MyAccount, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// submit button touch up inside.
        /// check validation of UITextField and call method to change password
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnSubmit_TouchUpInside(UIButton sender)
        {
            try
            {
                bool flag2 = true;
                if (String.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireCurrentPassword, Constant.durationOfToastMessage).Show();
                    flag2 = false;
                }
                else if (String.IsNullOrEmpty(txtUpdatePassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireNewPassword, Constant.durationOfToastMessage).Show();
                    flag2 = false;
                }
                else if (utility.IsValidPassword(txtUpdatePassword.Text.Trim()) == false)
                {
                    Toast.MakeText(AppResources.InvalidPassword, Constant.durationOfToastMessage).Show();
                    flag2 = false;
                }
                else if (String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireCPassword, Constant.durationOfToastMessage).Show();
                    flag2 = false;
                }
                else if (!txtUpdatePassword.Text.Equals(txtConfirmPassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.InvalidConfirmPassword, Constant.durationOfToastMessage).Show();
                    flag2 = false;
                }
                else if(flag2)
                {
                    ChangePasswordMyAccount();  
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BtnSubmit_TouchUpInside, AppResources.MyAccount, null);
            }
        }
        /// <summary>
        /// send email button touch up inside.
        /// To open email in your app
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnSendEmail_TouchUpInside(UIButton sender)
        {
            try
            {
                if (MFMailComposeViewController.CanSendMail)
                {
                    var to = new string[] { AppResources.ToMail };
                    if (MFMailComposeViewController.CanSendMail)
                    {
                        mailController = new MFMailComposeViewController();
                        mailController.SetToRecipients(to);
                        mailController.SetSubject("");
                        mailController.SetMessageBody("", false);
                        mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                            Console.WriteLine(AppResources.result + args.Result.ToString()); // sent or cancelled
                            BeginInvokeOnMainThread(() => {
                                args.Controller.DismissViewController(true, null);
                            });
                        };
                    }
                    this.PresentViewController(mailController, true, null);
                }
                else
                {
                    Toast.MakeText(AppResources.NoAppFound, Constant.durationOfToastMessage).Show();
                }   
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BtnSendEmail_TouchUpInside, AppResources.MyAccount, null);
            }
        }
        /// <summary>
        /// Gesture implementation for email check box click to check email preferences.
        /// </summary>
        public void CheckBoxEmailClick()
        {
            bool isEmailPreference = false;
            UITapGestureRecognizer imgCheckBoxEmailClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    flag = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isEmailPreference);
                    popUpMenuMyAccount.Hidden = true;
                    if (flag == false)
                    {
                        isEmailPreference = true;
                        ImgCheckboxEmail.Image = UIImage.FromBundle(AppResources.ImgCheckbox);
                        NSUserDefaults.StandardUserDefaults.SetBool(isEmailPreference, AppResources.isEmailPreference);
                    }
                    else
                    {
                        isEmailPreference = false;
                        ImgCheckboxEmail.Image = UIImage.FromBundle(AppResources.ImgUnCheckbox);
                        NSUserDefaults.StandardUserDefaults.SetBool(isEmailPreference, AppResources.isEmailPreference);
                    }
                    HitNotificationAPI(); 
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.CheckBoxEmailClick, AppResources.MyAccount, null);
                }
            });
            ImgCheckboxEmail.UserInteractionEnabled = true;
            ImgCheckboxEmail.AddGestureRecognizer(imgCheckBoxEmailClicked);
        }
        /// <summary>
        /// Gesture implementation for notification check box click.
        /// </summary>
        public void CheckBoxNotificationClick()
        {
            bool isNotification = false;
            UITapGestureRecognizer imgCheckBoxNotificationClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    flag1 = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isNotification);
                    popUpMenuMyAccount.Hidden = true;
                    if (flag1 == false)
                    {
                        isNotification = true;
                        ImgCheckboxNotification.Image = UIImage.FromBundle(AppResources.ImgCheckbox);
                        NSUserDefaults.StandardUserDefaults.SetBool(isNotification, AppResources.isNotification);
                    }
                    else
                    {
                        isNotification = false;
                        ImgCheckboxNotification.Image = UIImage.FromBundle(AppResources.ImgUnCheckbox);
                        NSUserDefaults.StandardUserDefaults.SetBool(isNotification, AppResources.isNotification);
                    }
                    HitNotificationAPI();  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.CheckBoxNotificationClick, AppResources.MyAccount, null);
                }
            });
            ImgCheckboxNotification.UserInteractionEnabled = true;
            ImgCheckboxNotification.AddGestureRecognizer(imgCheckBoxNotificationClicked);
        }
        /// <summary>
        /// Gesture implementation for account summary click.
        /// Manage UI to hide or show view.
        /// </summary>
        public void LblAccountSummaryClick()
        {
            UITapGestureRecognizer lblAccountSummaryClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    popUpMenuMyAccount.Hidden = true;
                    if (AccountSummarySubView.Hidden == true)
                    {
                        AccountSummarySubView.Hidden = false;
                        InformationPasswordView.Frame = new CGRect(
                            InformationPasswordView.Frame.X, InformationPasswordView.Frame.Y + AccountSummarySubView.Frame.Height, InformationPasswordView.Frame.Width, InformationPasswordView.Frame.Height
                        );
                        InformationPasswordSubView.Frame = new CGRect(
                            InformationPasswordSubView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, InformationPasswordSubView.Frame.Width, InformationPasswordSubView.Frame.Height
                        );
                        if (InformationPasswordSubView.Hidden == true)
                        {
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                           );
                            if(NotificationsSubView.Hidden == true)
                            {
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               ); 
                            }
                            else
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + 100);
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               ); 
                            }
                        }
                        else
                        {
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordSubView.Frame.Y + InformationPasswordSubView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                           );
                            if (NotificationsSubView.Hidden == true)
                            {
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                            else
                            {
                              
                                if (device == AppResources.iPad)
                                {
                                    //MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + 100);
                                }
                                else
                                {
                                    var aa = MyAccountScroll.ContentSize;
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, aa.Height + 100);
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                        }
                    }
                    else
                    {
                        AccountSummarySubView.Hidden = true;
                        InformationPasswordView.Frame = new CGRect(
                            InformationPasswordView.Frame.X, AccountSummaryView.Frame.Y + AccountSummaryView.Frame.Height, InformationPasswordView.Frame.Width, InformationPasswordView.Frame.Height
                        );
                        InformationPasswordSubView.Frame = new CGRect(
                            InformationPasswordSubView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, InformationPasswordSubView.Frame.Width, InformationPasswordSubView.Frame.Height
                        );
                        if (InformationPasswordSubView.Hidden == true)
                        {
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                           );
                            if (NotificationsSubView.Hidden == true)
                            {
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                            else
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + 100);
                                }
                                    
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                        }
                        else
                        {
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordSubView.Frame.Y + InformationPasswordSubView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                           );
                            if(NotificationsSubView.Hidden == true)
                            {
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               ); 
                            }
                            else
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height + InformationPasswordSubView.Bounds.Height + NotificationsView.Bounds.Height - AccountSummarySubView.Bounds.Height);
                                }
                                    
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               ); 
                            }
                        }
                    }   
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.LblAccountSummaryClick, AppResources.MyAccount, null);
                }
            });
            lblAccountSummary.UserInteractionEnabled = true;
            lblAccountSummary.AddGestureRecognizer(lblAccountSummaryClicked);
        }
        /// <summary>
        /// Gesture implementation for information password click.
        /// Manage UI to hide or show view
        /// </summary>
        public void LblInformationPasswordClick()
        {
            UITapGestureRecognizer lblInformationPasswordClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    popUpMenuMyAccount.Hidden = true;
                    if (AccountSummarySubView.Hidden == true)
                    {
                        InformationPasswordView.Frame = new CGRect(
                            InformationPasswordView.Frame.X, AccountSummaryView.Frame.Y + AccountSummaryView.Frame.Height, InformationPasswordView.Frame.Width, InformationPasswordView.Frame.Height
                        );
                        InformationPasswordSubView.Frame = new CGRect(
                            InformationPasswordSubView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, InformationPasswordSubView.Frame.Width, InformationPasswordSubView.Frame.Height
                        );
                        if (InformationPasswordSubView.Hidden == true)
                        { 
                            InformationPasswordSubView.Hidden = false;
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordSubView.Frame.Y + InformationPasswordSubView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                            );
                            if (NotificationsSubView.Hidden == true)
                            {
                                if (SubViewPurchaseHistory.Hidden == true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                    }
                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height);
                                    }
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                            else
                            {
                                if (SubViewPurchaseHistory.Hidden == true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height-200);
                                    }
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                        }
                        else
                        {
                            InformationPasswordSubView.Hidden = true;
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                            );
                            if (NotificationsSubView.Hidden == true)
                            {
                                if (SubViewPurchaseHistory.Hidden == true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }

                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                            else
                            {
                                if (SubViewPurchaseHistory.Hidden == true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummaryView.Bounds.Height + InformationPasswordView.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummaryView.Bounds.Height + InformationPasswordView.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummaryView.Bounds.Height + InformationPasswordView.Bounds.Height + NotificationsSubView.Bounds.Height + NotificationsView.Bounds.Height + 150 - InformationPasswordSubView.Bounds.Height);

                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummaryView.Bounds.Height + InformationPasswordView.Bounds.Height - InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                        }
                    }
                    else
                    {
                       
                        InformationPasswordView.Frame = new CGRect(
                            InformationPasswordView.Frame.X, AccountSummarySubView.Frame.Y + AccountSummarySubView.Frame.Height, InformationPasswordView.Frame.Width, InformationPasswordView.Frame.Height
                        );
                        InformationPasswordSubView.Frame = new CGRect(
                            InformationPasswordSubView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, InformationPasswordSubView.Frame.Width, InformationPasswordSubView.Frame.Height
                        );
                        if (InformationPasswordSubView.Hidden == true)
                        {
                            InformationPasswordSubView.Hidden = false;
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordSubView.Frame.Y + InformationPasswordSubView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                            );
                            if (NotificationsSubView.Hidden == true)
                            {
                                if(SubViewPurchaseHistory.Hidden==true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                    }  
                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height);
                                    }
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                            else
                            {
                                if (SubViewPurchaseHistory.Hidden == true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }   
                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                        }
                        else
                        {
                            InformationPasswordSubView.Hidden = true;
                            NotificationsView.Frame = new CGRect(
                                NotificationsView.Frame.X, InformationPasswordView.Frame.Y + InformationPasswordView.Frame.Height, NotificationsView.Frame.Width, NotificationsView.Frame.Height
                            );
                            NotificationsSubView.Frame = new CGRect(
                                NotificationsSubView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, NotificationsSubView.Frame.Width, NotificationsSubView.Frame.Height
                            );
                            if (NotificationsSubView.Hidden == true)
                            {
                                if (SubViewPurchaseHistory.Hidden == true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }   
                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height - InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                            else
                            {
                                if (SubViewPurchaseHistory.Hidden == true)
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height + InformationPasswordView.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height + InformationPasswordView.Bounds.Height - InformationPasswordSubView.Bounds.Height + NotificationsSubView.Bounds.Height);
                                    }
                                }
                                else
                                {
                                    if (device == AppResources.iPad)
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height + InformationPasswordView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height - InformationPasswordSubView.Bounds.Height - 150);
                                    }
                                    else
                                    {
                                        MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummarySubView.Bounds.Height + AccountSummaryView.Bounds.Height + +InformationPasswordView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height - InformationPasswordSubView.Bounds.Height);
                                    }

                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                    PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                    SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                               );
                            }
                        }
                    } 
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.LblInformationPasswordClick, AppResources.MyAccount, null);
                }
            });
            lblInformationPassword.UserInteractionEnabled = true;
            lblInformationPassword.AddGestureRecognizer(lblInformationPasswordClicked);
        }
        /// <summary>
        /// Gesture implementation for notifications click.
        /// hide or show view
        /// </summary>
        public void LblNotificationsClick()
        {
            UITapGestureRecognizer lblNotificationsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                try
                {
                    if(InformationPasswordSubView.Hidden == true)
                    {
                        if (NotificationsSubView.Hidden == true)
                        {
                            if(SubViewPurchaseHistory.Hidden==true)
                            {
                                NotificationsSubView.Hidden = false;
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height);
                                }
                                else
                                {
                                     MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height);
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                                );  
                            }
                            else
                            {
                                NotificationsSubView.Hidden = false;
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height+SubViewPurchaseHistory.Bounds.Height-150);
                                }

                               
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                                );  
                            }    
                        }
                        else
                        {
                            if (SubViewPurchaseHistory.Hidden == true)
                            {
                                NotificationsSubView.Hidden = true;
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordView.Bounds.Height + NotificationsView.Bounds.Height - NotificationsSubView.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height+ InformationPasswordView.Bounds.Height + NotificationsView.Bounds.Height - NotificationsSubView.Bounds.Height);
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                              );
                            }
                            else
                            {
                                NotificationsSubView.Hidden = true;
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordView.Bounds.Height + NotificationsView.Bounds.Height - NotificationsSubView.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height+InformationPasswordView.Bounds.Height+NotificationsView.Bounds.Height - NotificationsSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height);
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                              );  
                            }
                        }
                    }
                    else
                    {
                        if (NotificationsSubView.Hidden == true)
                        {
                            if(SubViewPurchaseHistory.Hidden==true)
                            {
                                NotificationsSubView.Hidden = false;
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                  );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                                ); 
                            }
                            else
                            {
                                NotificationsSubView.Hidden = false;
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height + InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height);
                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsSubView.Frame.Y + NotificationsSubView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                  );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                                );
                                
                            }
                        }
                        else
                        {
                            if(SubViewPurchaseHistory.Hidden==true)
                            {
                                NotificationsSubView.Hidden = true;
                                MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordView.Bounds.Height + InformationPasswordSubView.Bounds.Height + PurchaseHistoryView.Bounds.Height - NotificationsSubView.Bounds.Height);
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                              ); 
                            }
                            else
                            {
                                NotificationsSubView.Hidden = true;
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - NotificationsSubView.Bounds.Height + InformationPasswordSubView.Bounds.Height + PurchaseHistoryView.Bounds.Height + 150);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - NotificationsSubView.Bounds.Height + InformationPasswordSubView.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + PurchaseHistoryView.Bounds.Height);

                                }
                                PurchaseHistoryView.Frame = new CGRect(
                                PurchaseHistoryView.Frame.X, NotificationsView.Frame.Y + NotificationsView.Frame.Height, PurchaseHistoryView.Frame.Width, PurchaseHistoryView.Frame.Height
                                );
                                SubViewPurchaseHistory.Frame = new CGRect(
                                SubViewPurchaseHistory.Frame.X, PurchaseHistoryView.Frame.Y + PurchaseHistoryView.Frame.Height, SubViewPurchaseHistory.Frame.Width, SubViewPurchaseHistory.Frame.Height
                              ); 
                            }
                        } 
                    }
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.LblNotificationsClick, AppResources.MyAccount, null);
                }
            });
            lblNotifications.UserInteractionEnabled = true;
            lblNotifications.AddGestureRecognizer(lblNotificationsClicked);
        }
        /// <summary>
        /// Gesture implementation for purchase history click.
        /// </summary>
        public void LblPurchaseHistoryClick()
        {
            UITapGestureRecognizer lblPurchaseHistoryClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                try
                {
                    if (InformationPasswordSubView.Hidden == true)
                    {
                        if(NotificationsSubView.Hidden == true)
                        {
                            if (SubViewPurchaseHistory.Hidden == true)
                            {
                                if (device == AppResources.iPad)
                                {
                                   
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + SubViewPurchaseHistory.Bounds.Height-150);
                                }
                                SubViewPurchaseHistory.Hidden = false;
                                PurchaseHistory();
                            }
                            else
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - SubViewPurchaseHistory.Bounds.Height);
                                }
                               
                                SubViewPurchaseHistory.Hidden = true;
                            }   
                        }
                        else
                        {
                            if (SubViewPurchaseHistory.Hidden == true)
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + SubViewPurchaseHistory.Bounds.Height+NotificationsSubView.Bounds.Height);
                                }
                                SubViewPurchaseHistory.Hidden = false;
                                PurchaseHistory();
                            }
                            else
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height - SubViewPurchaseHistory.Bounds.Height+NotificationsSubView.Bounds.Height+ AccountSummarySubView.Bounds.Height+AccountSummaryView.Bounds.Height+ NotificationsView.Bounds.Height);
                                } SubViewPurchaseHistory.Hidden = true;
                            }    
                        }
                    }
                    else
                    {
                        if (NotificationsSubView.Hidden == true)
                        {
                            if (SubViewPurchaseHistory.Hidden == true)
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + InformationPasswordSubView.Bounds.Height);
                                }
                                SubViewPurchaseHistory.Hidden = false;
                                PurchaseHistory();
                            }
                            else
                            {
                             MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummaryView.Bounds.Height + AccountSummarySubView.Bounds.Height+ NotificationsView.Bounds.Height + InformationPasswordView.Bounds.Height + InformationPasswordSubView.Bounds.Height - SubViewPurchaseHistory.Bounds.Height);
                             SubViewPurchaseHistory.Hidden = true;
                            }
                        }
                        else
                        {
                            if (SubViewPurchaseHistory.Hidden == true)
                            {
                                if (device == AppResources.iPad)
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + NotificationsSubView.Bounds.Height + InformationPasswordSubView.Bounds.Height + NotificationsView.Frame.Height);
                                }
                                else
                                {
                                    MyAccountScroll.ContentSize = new CGSize(MyAccountScrollChildview.Bounds.Width, MyAccountScroll.Bounds.Height + SubViewPurchaseHistory.Bounds.Height + NotificationsSubView.Bounds.Height+InformationPasswordSubView.Bounds.Height-150);
                                }
                                SubViewPurchaseHistory.Hidden = false;
                                PurchaseHistory();
                            }
                            else
                            {
                                MyAccountScroll.ContentSize = new CGSize(MyAccountScroll.Bounds.Width, MyAccountScroll.Bounds.Height + AccountSummaryView.Bounds.Height + AccountSummarySubView.Bounds.Height + NotificationsSubView.Bounds.Height + NotificationsView.Bounds.Height + InformationPasswordView.Bounds.Height + InformationPasswordSubView.Bounds.Height - SubViewPurchaseHistory.Bounds.Height);
                                SubViewPurchaseHistory.Hidden = true;
                            }
                        } 
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.LblPurchaseHistoryClick, AppResources.MyAccount, null);
                }
            });
            lblPurchaseHistory.UserInteractionEnabled = true;
            lblPurchaseHistory.AddGestureRecognizer(lblPurchaseHistoryClicked);
        }
        /// <summary>
        /// Gesture implementation for points earned click.
        /// To show data in easy tip view
        /// </summary>
        public void PointsEarnedInfoClick()
        {
            var etv = new EasyTipView.EasyTipView();
            UITapGestureRecognizer imgPointsEarnedInfoClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                try
                {
                    etv.Text = new Foundation.NSString(AppResources.EarnedlblTooltip);
                    etv.ArrowPosition = EasyTipView.ArrowPosition.Top;
                    etv.ForegroundColor = UIColor.White;
                    etv.BubbleColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                    etv.Show(this.ImgPointsEarnedInfo, this.View, true);
                    etv.DismissOnTap = true;
                    etv.ClearsContextBeforeDrawing = true;  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.PointsEarnedInfoClick, AppResources.MyAccount, null);
                }
            });
            ImgPointsEarnedInfo.UserInteractionEnabled = true;
            ImgPointsEarnedInfo.AddGestureRecognizer(imgPointsEarnedInfoClicked);
        }
        /// <summary>
        /// Gesture implementation for points redeemed click.
        /// To show data in easy tip view
        /// </summary>
        public void PointsRedeemedInfoClick()
        {
            var etv = new EasyTipView.EasyTipView();
            UITapGestureRecognizer imgPointsRedeemedInfoClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                try
                {
                    etv.Text = new Foundation.NSString(AppResources.RedeemlblTooltip);
                    etv.ArrowPosition = EasyTipView.ArrowPosition.Top;
                    etv.ForegroundColor = UIColor.White;
                    etv.BubbleColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                    etv.Show(this.ImgPointsRedeemedInfo, this.View, true);
                    etv.DismissOnTap = true;
                    etv.ClearsContextBeforeDrawing = true;  
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.PointsRedeemedInfoClick, AppResources.MyAccount, null);
                }
            });
            ImgPointsRedeemedInfo.UserInteractionEnabled = true;
            ImgPointsRedeemedInfo.AddGestureRecognizer(imgPointsRedeemedInfoClicked);
        }
        /// <summary>
        /// Gesture implementation for points balance click.
        /// To show data in easy tip view
        /// </summary>
        public void PointsBalancInfoClick()
        {
            var etv = new EasyTipView.EasyTipView();
            UITapGestureRecognizer imgPointsBalancInfoClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                try
                {
                    etv.Text = new Foundation.NSString(AppResources.BalancelblTooltip);
                    etv.ArrowPosition = EasyTipView.ArrowPosition.Top;
                    etv.ForegroundColor = UIColor.White;
                    etv.BubbleColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
                    etv.Show(this.ImgPointsBalancInfo, this.View, true);
                    etv.DismissOnTap = true;
                    etv.ClearsContextBeforeDrawing = true;  
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.PointsBalancInfoClick, AppResources.MyAccount, null);
                }
            });
            ImgPointsBalancInfo.UserInteractionEnabled = true;
            ImgPointsBalancInfo.AddGestureRecognizer(imgPointsBalancInfoClicked);
        }
        /// <summary>
        /// Gesture implementation for User information click.
        /// hide the pop up in menu account
        /// </summary>
        public void UserInfoClick()
        {
            UITapGestureRecognizer userinfoClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
            });
            userInfoView.UserInteractionEnabled = true;
            userInfoView.AddGestureRecognizer(userinfoClicked);
        }
        /// <summary>
        /// Gesture implementation for Header click.
        /// hide the pop up in menu account
        /// </summary>
        public void HeaderClick()
        {
            UITapGestureRecognizer headerClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
            });
            headerView.UserInteractionEnabled = true;
            headerView.AddGestureRecognizer(headerClicked);
        }
        /// <summary>
        /// Gesture implementation for Main view click on my account.
        /// hide the pop up in menu account
        /// </summary>
        public void MainMyAccountViewClick()
        {
            UITapGestureRecognizer mainMyAccountViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
            });
            MainMyAccountView.UserInteractionEnabled = true;
            MainMyAccountView.AddGestureRecognizer(mainMyAccountViewClicked);
        }
        /// <summary>
        /// Gestue implementation for Pop up click.
        /// show the pop up in menu account
        /// </summary>
        public void PopUpClick()
        {
            UITapGestureRecognizer popUpClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = false;
            });
            popUpMenuMyAccount.UserInteractionEnabled = true;
            popUpMenuMyAccount.AddGestureRecognizer(popUpClicked);
        }
        /// <summary>
        /// Gesture implementation for back arrow click.
        /// to navigate previous view
        /// </summary>
        public void ImgBackArrowClick()
        {
            UITapGestureRecognizer imgBackArrowClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                this.NavigationController.PopViewController(true);
            });
            imgBackArrowMyAccount.UserInteractionEnabled = true;
            imgBackArrowMyAccount.AddGestureRecognizer(imgBackArrowClicked);
        }
        /// <summary>
        /// Gesture implementation for menu click.
        /// hide or show the pop up in menu account
        /// </summary>
        public void ImgMenuClick()
        {
            UITapGestureRecognizer imgMenuClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenuMyAccount.Hidden == true)
                {
                    popUpMenuMyAccount.Hidden = false;
                }
                else
                {
                    popUpMenuMyAccount.Hidden = true;
                }
            });
            ImgMenuMyAccount.UserInteractionEnabled = true;
            ImgMenuMyAccount.AddGestureRecognizer(imgMenuClicked);
        }
        /// <summary>
        /// Gesture implementation for logout click.
        /// Logout user
        /// </summary>
        public void LblLogoutClick()
        {
            UITapGestureRecognizer lbllogoutClicked = new UITapGestureRecognizer(() =>
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
            lblLogout.AddGestureRecognizer(lbllogoutClicked);
        }
        /// <summary>
        /// Changed event on confirm password UITextField
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtConfirmPassword_Changed(UITextField sender)
        {
            if (!String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                ImgConfirmPasswordEye.Hidden = false;
            }
            else
            {
                ImgConfirmPasswordEye.Hidden = true;
            }
        }
        /// <summary>
        /// Change event on Change Password UITextField
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtChangePassword_Changed(UITextField sender)
        {
            if (!String.IsNullOrEmpty(txtUpdatePassword.Text.Trim()))
            {
                ImgChangePasswordEye.Hidden = false;
            }
            else
            {
                ImgChangePasswordEye.Hidden = true;
            }
        }
        /// <summary>
        /// Changed event on the current password UITextField
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtCurrentPassword_Changed(UITextField sender)
        {
            if (!String.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                ImgCurrentPasswordEye.Hidden = false;
            }
            else
            {
                ImgCurrentPasswordEye.Hidden = true;
            }
        }
        /// <summary>
        /// Gesture implementation for eye click on current password UITextField.
        /// </summary>
        public void CurrentPasswordEyeClick()
        {
            bool imageshow = false;
            UITapGestureRecognizer currentPasswordEyeClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (imageshow == false)
                    {
                        imageshow = true;
                        ImgCurrentPasswordEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                        txtCurrentPassword.SecureTextEntry = false;
                    }
                    else
                    {
                        imageshow = false;
                        ImgCurrentPasswordEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                        txtCurrentPassword.SecureTextEntry = true;
                    }  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.CurrentPasswordEyeClick, AppResources.MyAccount, null);
                }
            });
            ImgCurrentPasswordEye.UserInteractionEnabled = true;
            ImgCurrentPasswordEye.AddGestureRecognizer(currentPasswordEyeClicked);
        }
       /// <summary>
        /// Gesture implementation for eye click on change password UITextField.
        /// show visible or invisible eye icon
       /// </summary>
        public void ChangePasswordEyeClick()
        {
            bool imageshow = false;
            UITapGestureRecognizer changePasswordEyeClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (imageshow == false)
                    {
                        imageshow = true;
                        ImgChangePasswordEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                        txtUpdatePassword.SecureTextEntry = false;
                    }
                    else
                    {
                        imageshow = false;
                        ImgChangePasswordEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                        txtUpdatePassword.SecureTextEntry = true;
                    }  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.CurrentPasswordEyeClick, AppResources.MyAccount, null);
                }
            });
            ImgChangePasswordEye.UserInteractionEnabled = true;
            ImgChangePasswordEye.AddGestureRecognizer(changePasswordEyeClicked);
        }
       /// <summary>
        /// Gesture implementation for eye on confirm password text box
        /// show visible or invisible eye icon
       /// </summary>
        public void ConfirmPasswordEyeClick()
        {
            bool imageshow = false;
            UITapGestureRecognizer confirmPasswordEyeClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (imageshow == false)
                    {
                        imageshow = true;
                        ImgConfirmPasswordEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                        txtConfirmPassword.SecureTextEntry = false;
                    }
                    else
                    {
                        imageshow = false;
                        ImgConfirmPasswordEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                        txtConfirmPassword.SecureTextEntry = true;
                    }   
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.ConfirmPasswordEyeClick, AppResources.MyAccount, null);
                }
            });
            ImgConfirmPasswordEye.UserInteractionEnabled = true;
            ImgConfirmPasswordEye.AddGestureRecognizer(confirmPasswordEyeClicked);
        }
        /// <summary>
        /// Gesture implementation for terms and conditions click.
        /// To open web view and show privacy policy 
        /// </summary>
        public void TermsAndConditionsClick()
        {
            UITapGestureRecognizer termsAndConditionsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTermsAndConditions.UserInteractionEnabled = true;
            lblTermsAndConditions.AddGestureRecognizer(termsAndConditionsClicked);
        }
        /// <summary>
        /// Gesture implementation for privacy policy click.
        /// To open web view and show privacy policy
        /// </summary>
        public void LblPrivacyPolicyClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacy.UserInteractionEnabled = true;
            lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// Gesture implementation for FAQ click.
        /// </summary>
        public void LblFAQClick()
        {
            UITapGestureRecognizer lblFAQClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
            });
            lblFaq.UserInteractionEnabled = true;
            lblFaq.AddGestureRecognizer(lblFAQClicked);
        }
        /// <summary>
        /// Gesture implementation for Support click.
        /// </summary>
        public void LblSupportClick()
        {
            UITapGestureRecognizer lblSupportClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyAccount.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportClicked);
        }
        /// <summary>
        /// Dismiss the keyboard click on forgot password.
        /// </summary>
        public void DismissKeyboardClick()
        {
            UITapGestureRecognizer dismissKeyboardClicked = new UITapGestureRecognizer(() =>
            {
                utility.DismissKeyboardOnTap(new UIView[] { MainMyAccountView });
            });
            MainMyAccountView.UserInteractionEnabled = true;
            MainMyAccountView.AddGestureRecognizer(dismissKeyboardClicked);
        }
        /// <summary>
        /// add Done BUtton on keyboard
        /// </summary>
        public void AddDoneButton()
        {
            UITextField[] textFields = new UITextField[] { txtCurrentPassword, txtConfirmPassword, txtUpdatePassword };
            Utility.AddDoneButtonToKeyboard(textFields);
        }
        /// <summary>
        /// Did the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
	}
}

