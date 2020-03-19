using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.iOS.TableVewSource;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file Redeem controller is used for reed points screen
    /// </summary>
    public partial class RedeemPointsController : UIViewController
    {
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        private string userId = String.Empty;
        private string token;
        private UserInfoEntity userInfoEntity = null;
        private List<RedeemGiftCardEntity> redeemGiftCardEntity = null;
        private string count;
        private RedeemPointResponseEntity redeemPointsResponseEntity;
        private int viewWidth;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.RedeemPointsController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public RedeemPointsController (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            // get the user id 
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
            }
            //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
            string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
            if (!String.IsNullOrEmpty(exception))
            {
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.Redeem, exception);
            }
            lblPointsPerUnit.Hidden = true;
            MenuClick();
            LblFAQClick();
            LblSupportClick();
            SetLogoSize();
            LogoutClick();
            HeaderSectionClick();
            UserProfileClick();
            HomeViewClick();
            BookViewClick();
            AboutViewClick();
            RedeemViewClick();
            TermsAndConditionsClick();
            PrivacyPolicyClick();
            RedeemMainViewClick();
            SetFonts();
            UserProfileTableViewClick();
            UserProfileUserTableViewClick();
            PointsCheckoutView.Hidden = true;
            badgeView.BackgroundColor = UIColor.Red;
            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
            badgeView.Layer.BorderWidth = 2f;
            badgeView.Layer.BorderColor = UIColor.White.CGColor;
            lblRedeemGiftTableView.Text = AppResources.LabelRedeem;
            lblRedeemPoints.Text = AppResources.LabelRedeemPoints;
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
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.Redeem);
                }
                else
                { 
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.Redeem, null); // exception handling
            }
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
                HideShowUIBasedOnUser();
                HitUserInfAPI(userId, false); // call the user information api.
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewWillAppear, AppResources.Redeem, null); // exception handling 
            }
        }
        /// <summary>
        /// Set the fonts of UILabel,UITextField,UIButton.
        /// set the padding of UITextField.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFontsforHeader(new UILabel[] { lblRedeemPoints, },null, Constant.lstDigit[11], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblHome, lblBook, lblAbout, lblTermsAndConditions, lblLogout, lblPrivacy, lblRedeem, lblRedem, lblRedeemGiftTableView ,lblRedeemGiftUserTableView}, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsItalic(new UILabel[] { lblCourseName, lblCourseNameGiftTableView,lblCourseNameGiftUserTableView,lblPointsPerUnit ,lblPointsPerUnitGiftTableView,lblPointsPerUnitGiftUserTableView}, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblFullname, lblFullnameGiftTableView ,lblFullnameGiftUserTableView}, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblBadge }, null, Constant.lstDigit[6], viewWidth); 
            } 
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.Redeem, null); // exception handling 
            }
        }
        /// <summary>
        /// Hits the wish list item count API.
        /// </summary>
        public async void HitWishListItemCountAPI()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    count = await utility.GetWishListItemCount(token);
                    if (count != null)
                    {
                        if (count == Constant.lstDigitString[5])
                        {
                            badgeView.Hidden = true;
                        }
                        else
                        {
                            badgeView.Hidden = false;
                            lblBadge.Text = count;
                        }
                        HitRewardItemListAPI(); // hit the reward item list api
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error).Show(); // show the toast message 
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.HitWishListItemCountAPI, AppResources.Redeem, null); // exceptiuon handling 
           }
        }
        /// <summary>
        /// Hide and show UI based on user profile.
        /// </summary>
        public void HideShowUIBasedOnUser()
        {
            try
            {
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                {
                    string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);

                    if (role.ToLower().Trim() == AppResources.user)
                    {
                        Utility.DebugAlert(AppResources.Message, AppResources.AccountRole);
                        lblRedeemGiftUserTableView.Text = AppResources.LabelRedeem;
                        lblRedeemPoints.Text = AppResources.LabelRedeemPoints;
                        UserInfoView.Hidden = false;
                        tblGiftCardUser.Hidden = false;
                        tblGiftCard.Hidden = true;
                        popUpMenuRedeem.Hidden = true;
                    }
                    else
                    {
                        MyCartClick();
                        PointsCheckoutView.Hidden = true;
                        lblRedeemGiftTableView.Text = AppResources.LabelRedeem;
                        lblRedeemPoints.Text = AppResources.LabelRedeemPoints;
                        UserInfoView.Hidden = false;
                        tblGiftCardUser.Hidden = true;
                        tblGiftCard.Hidden = false;
                        popUpMenuRedeem.Hidden = true;
                    }
                }   
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HideShowUIBasedOnUser, AppResources.Redeem, null);
            }
        }
        /// <summary>
        /// Set Logo size sepcified in iPhone X.
        /// set UI in iPhone X
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                string device = UIDevice.CurrentDevice.Model;
                if (device == AppResources.iPad)
                {
                    viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                    int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height;
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[14], Constant.lstDigit[14]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        LogoRedeem.Frame = new CGRect(LogoRedeem.Frame.X, LogoRedeem.Frame.Y, LogoRedeem.Frame.Width - Constant.digitNinteenPointsFive, LogoRedeem.Frame.Height + Constant.digitEighteenPointsFive);
                        headerViewUserInfo.Frame = new CGRect(headerViewUserInfo.Frame.X, headerViewUserInfo.Frame.Y, headerViewUserInfo.Frame.Width, giftCardHeaderView.Frame.Height);
                    }
                    else
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[13], Constant.lstDigit[13]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        LogoRedeem.Frame = new CGRect(LogoRedeem.Frame.X, LogoRedeem.Frame.Y, LogoRedeem.Frame.Width - Constant.digitTwelvePointsFive, LogoRedeem.Frame.Height + Constant.digitElevenPointsFive);
                        headerViewUserInfo.Frame = new CGRect(headerViewUserInfo.Frame.X, headerViewUserInfo.Frame.Y, headerViewUserInfo.Frame.Width, giftCardHeaderView.Frame.Height);
                    }
                }
                else
                {
                    viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                    int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                    if (viewWidth == (int)DeviceScreenSize.ISix)
                    {
                        if (viewHeight == Constant.lstDigit[31])
                        {
                            LogoRedeem.Frame = new CGRect(Constant.lstDigit[6], Constant.lstDigit[41], Constant.lstDigit[42], Constant.lstDigit[43]);
                            badgeView.Frame = new CGRect(Constant.lstDigit[45], Constant.lstDigit[9], Constant.lstDigit[44], Constant.lstDigit[44]);
                            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                            badgeView.Layer.BorderWidth = 2f;
                            headerViewRedeem.Frame = new CGRect(headerViewRedeem.Frame.X, headerViewRedeem.Frame.Y + Constant.lstDigit[10], headerViewRedeem.Frame.Width, headerViewRedeem.Frame.Height);
                            popUpMenuRedeem.Frame = new CGRect(popUpMenuRedeem.Frame.X, popUpMenuRedeem.Frame.Y + Constant.lstDigit[10], popUpMenuRedeem.Frame.Width, popUpMenuRedeem.Frame.Height);
                            tblGiftCard.Frame = new CGRect(tblGiftCard.Frame.X, headerViewRedeem.Frame.Y + headerViewRedeem.Frame.Height, tblGiftCard.Frame.Width, tblGiftCard.Frame.Height - Constant.lstDigit[10]);
                            tblGiftCardUser.Frame = new CGRect(tblGiftCardUser.Frame.X, headerViewRedeem.Frame.Y + headerViewRedeem.Frame.Height, tblGiftCardUser.Frame.Width, tblGiftCardUser.Frame.Height - Constant.lstDigit[10]);
                            headerViewUserInfo.Frame = new CGRect(headerViewUserInfo.Frame.X, headerViewUserInfo.Frame.Y, headerViewUserInfo.Frame.Width, giftCardHeaderView.Frame.Height);
                        }
                        else
                        {
                            headerViewUserInfo.Frame = new CGRect(headerViewUserInfo.Frame.X, headerViewUserInfo.Frame.Y, headerViewUserInfo.Frame.Width, giftCardHeaderView.Frame.Height);
                        }
                    }
                    else
                    {
                        headerViewUserInfo.Frame = new CGRect(headerViewUserInfo.Frame.X, headerViewUserInfo.Frame.Y, headerViewUserInfo.Frame.Width, giftCardHeaderView.Frame.Height);
                    }
                } 
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.setLogoSize, AppResources.Redeem, null);
            }
        }
		/// <summary>
		/// Hit the user information API.
        /// set the value of fullname,course name and balance points
		/// </summary>
		/// <param name="userId">User identifier.</param>
        protected async void HitUserInfAPI(string userId,bool isRedeem)
        {
            try
            {
                if(isRedeem)
                {
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                        userInfoEntity = await utility.GetUserInfo(userId, token);
                        if (!string.IsNullOrEmpty(userInfoEntity.userId))
                        {
                            lblFullname.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                            lblCourseName.Text = userInfoEntity.courseName;
                            int balance = userInfoEntity.balancedPoints;
                            string formatted = balance.ToString(AppResources.Comma);
                            lblPointsPerUnit.Text = "";//formatted + AppResources.PointsAvailable;
                            lblFullnameGiftTableView.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                            lblCourseNameGiftTableView.Text = userInfoEntity.courseName;
                            lblPointsPerUnitGiftTableView.Text = formatted + AppResources.PointsAvailable;
                            lblFullnameGiftUserTableView.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                            lblCourseNameGiftUserTableView.Text = userInfoEntity.courseName;
                            lblPointsPerUnitGiftUserTableView.Text = formatted + AppResources.PointsAvailable;
                            NSUserDefaults.StandardUserDefaults.SetString(Convert.ToString(userInfoEntity.balancedPoints), AppResources.BalancePoints);
                            NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isEmailPreference, AppResources.isEmailPreference);
                            NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isNotification, AppResources.isNotification);
                            NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.role, AppResources.role);
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    loadPop = new LoadingOverlay(this.View.Frame);
                    this.View.Add(loadPop);
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                        userInfoEntity = await utility.GetUserInfo(userId, token);
                        if (!string.IsNullOrEmpty(userInfoEntity.userId))
                        {
                            HitWishListItemCountAPI();
                            lblFullname.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                            lblCourseName.Text = userInfoEntity.courseName;
                            int balance = userInfoEntity.balancedPoints;
                            string formatted = balance.ToString(AppResources.Comma);
                            lblPointsPerUnit.Text = "";//formatted + AppResources.PointsAvailable;
                            lblFullnameGiftTableView.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                            lblCourseNameGiftTableView.Text = userInfoEntity.courseName;
                            lblPointsPerUnitGiftTableView.Text = formatted + AppResources.PointsAvailable;
                            lblFullnameGiftUserTableView.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                            lblCourseNameGiftUserTableView.Text = userInfoEntity.courseName;
                            lblPointsPerUnitGiftUserTableView.Text = formatted + AppResources.PointsAvailable;
                            NSUserDefaults.StandardUserDefaults.SetString(Convert.ToString(userInfoEntity.balancedPoints), AppResources.BalancePoints);
                            NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isEmailPreference, AppResources.isEmailPreference);
                            NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isNotification, AppResources.isNotification);
                            NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.role, AppResources.role);
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.HitUserInfAPI, AppResources.Redeem, null);
           } 
        }
       /// <summary>
       /// Hit the redeem API.
       /// </summary>
        public async void HitRedeemAPI(string rewardItemId,string rewardItem,int minimumBalance,int pointsPricePerUnit,int totalPointsSpent)
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
            try
            {
                RedeemPointEntity redeemPointsEntity = new RedeemPointEntity();
                redeemPointsEntity.pointsSpentBy = userId;
                redeemPointsEntity.rewardItemId = rewardItemId;
                redeemPointsEntity.rewardItem = rewardItem;
                redeemPointsEntity.minimumBalance = minimumBalance;
                redeemPointsEntity.pointsPricePerUnit = pointsPricePerUnit;
                redeemPointsEntity.totalPointsSpent = totalPointsSpent;
                token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                   redeemPointsResponseEntity = await utility.RedeemPoints(redeemPointsEntity, token);
                    if (redeemPointsResponseEntity.operationStatus.ToLower() == AppResources.success)
                    {
                        lblRedeem.Text = AppResources.RequireConfirmationRedeemLabel;
                        lblRedeemPoints.Text = AppResources.RequirePointsCheckoutLabel;
                        PointsCheckoutView.Hidden = false;
                        tblGiftCard.Hidden = true;
                        HitUserInfAPI(userId,true);
                    }
                    else
                    {
                        Toast.MakeText(redeemPointsResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                    }   
                }
                else
                {
                    Toast.MakeText(AppResources.Error,Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitRedeemAPI, AppResources.Redeem, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Hit the reward item list API.
        /// </summary>
        public async void HitRewardItemListAPI()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    redeemGiftCardEntity = await utility.GetRewardItemList(token);
                    if (redeemGiftCardEntity != null)
                    {
                        if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                        {
                            string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                            if (countryName.ToLower() == AppResources.canada)
                            {
                                redeemGiftCardEntity = redeemGiftCardEntity.Where(x => x.isAvailableCanada.Equals(true)).ToList();
                                BindDataGiftCard();
                            }
                            else
                            {
                                redeemGiftCardEntity = redeemGiftCardEntity.Where(x => x.isAvailableUs.Equals(true)).ToList();
                                BindDataGiftCard();
                            }
                        }
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitRewardItemListAPI, AppResources.Redeem, null);
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
            }
        }
        /// <summary>
        /// Bind the data.
        /// </summary>
        public void BindDataGiftCard()
        {
            try
            {
                if (redeemGiftCardEntity != null && redeemGiftCardEntity.Count > 0)
                {
                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                    {
                        string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                        if (role.ToLower().Trim() == AppResources.user)
                        {
                            Utility.DebugAlert(AppResources.Message, AppResources.AccountRole);
                            List<RedeemGiftCardEntity> lstRedeemGiftCard = new List<RedeemGiftCardEntity>();
                            lstRedeemGiftCard = redeemGiftCardEntity;
                            List<NSData> lstNSData = new List<NSData>();
                            NSData nsDataProductLogo = new NSData();
                            for (int i = 0; i < lstRedeemGiftCard.Count; i++)
                            {
                                string imgLogo = lstRedeemGiftCard[i].itemImage;
                                if (!String.IsNullOrEmpty(imgLogo))
                                {
                                    imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                    imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                    var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                    nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                }
                                lstNSData.Add(nsDataProductLogo);
                            }
                            TableViewGiftCardUser tableViewGiftCardUser = new TableViewGiftCardUser(this, lstRedeemGiftCard, lstNSData);
                            tblGiftCardUser.Source = tableViewGiftCardUser;
                            tblGiftCardUser.ReloadData();
                            tblGiftCardUser.Hidden = false;
                        }
                        else
                        {
                            List<RedeemGiftCardEntity> lstRedeemGiftCard = new List<RedeemGiftCardEntity>();
                            lstRedeemGiftCard = redeemGiftCardEntity;
                            List<NSData> lstNSData = new List<NSData>();
                            NSData nsDataProductLogo = new NSData();
                            for (int i = 0; i < lstRedeemGiftCard.Count; i++)
                            {
                                string imgLogo = lstRedeemGiftCard[i].itemImage;
                                if (!String.IsNullOrEmpty(imgLogo))
                                {
                                    imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                    imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                    var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                    nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                }
                                lstNSData.Add(nsDataProductLogo);
                            }
                            TableViewGiftCard tableViewGiftCard = new TableViewGiftCard(this, lstRedeemGiftCard, lstNSData);
                            tblGiftCard.Source = tableViewGiftCard;
                            tblGiftCard.ReloadData();
                            tblGiftCard.Hidden = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BindDataGiftCard, AppResources.Redeem, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Move the product list to open keyboard.
        /// </summary>
        public void MoveScrollViewToOpenKeyboard(nint index)
        {
            try
            {
               tblGiftCard.Frame = new CGRect(tblGiftCard.Frame.X, tblGiftCard.Frame.Y , tblGiftCard.Frame.Width, tblGiftCard.Frame.Height- Constant.lstDigit[26]);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.MoveScrollViewToOpenKeyboard, AppResources.Redeem, null);
            }
        }
        /// <summary>
        /// Move the product list to hide keyboard.
        /// </summary>
        /// <param name="index">Index.</param>
        public void MoveScrollViewToHideKeyboard(nint index)
        {
            try
            {
              tblGiftCard.Frame = new CGRect(tblGiftCard.Frame.X, tblGiftCard.Frame.Y , tblGiftCard.Frame.Width, tblGiftCard.Frame.Height + Constant.lstDigit[26]);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.MoveScrollViewToHideKeyboard, AppResources.Redeem, null);
            }
        }
        /// <summary>
        /// Gesture implementation for user profile to show user profile detail in my account screen
        /// </summary>
        public void UserProfileClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                this.PerformSegue(AppResources.SegueFromRedeemPointsToAmyAccount, this);
            });
            UserInfoView.UserInteractionEnabled = true;
            UserInfoView.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for user profile to show user profile detail in my account screen
        /// </summary>
        public void UserProfileTableViewClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                this.PerformSegue(AppResources.SegueFromRedeemPointsToAmyAccount, this);
            });
            UserInfoViewTableView.UserInteractionEnabled = true;
            UserInfoViewTableView.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for user profile to show user profile detail in my account screen
        /// </summary>
        public void UserProfileUserTableViewClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                this.PerformSegue(AppResources.SegueFromRedeemPointsToAmyAccount, this);
            });
            UserInfoViewUserTableView.UserInteractionEnabled = true;
            UserInfoViewUserTableView.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for Menu click to hide or show pop up
        /// </summary>
        public void MenuClick()
        {
            UITapGestureRecognizer menuClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenuRedeem.Hidden == true)
                {
                    popUpMenuRedeem.Hidden = false;
                }
                else
                {
                    popUpMenuRedeem.Hidden = true;
                }
            });
            ImgMenuRedeem.UserInteractionEnabled = true;
            ImgMenuRedeem.AddGestureRecognizer(menuClicked);
        }
        /// <summary>
        /// Gesture implementation for header section to hide popup
        /// </summary>
        public void HeaderSectionClick()
        {
            UITapGestureRecognizer menuClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
            });
            headerViewRedeem.UserInteractionEnabled = true;
            headerViewRedeem.AddGestureRecognizer(menuClicked);
        }
        /// <summary>
        /// Gesture implementation for mycart icon
        /// my cart click to hide or show pop up and perform segue to my cart screen.
        /// </summary>
        public void MyCartClick()
        {
            UITapGestureRecognizer imgMyCartClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                if (count == Constant.lstDigitString[5])
                {
                    Utility.DebugAlert(AppResources.Message, AppResources.MessageEmptyQueue);

                }
                else
                {
                    this.PerformSegue(AppResources.SegueFromRedeemPointsToMyCart, this); 
                }
            });
            ImgMyCartRedeemPoints.UserInteractionEnabled = true;
            ImgMyCartRedeemPoints.AddGestureRecognizer(imgMyCartClicked);
        }
        /// <summary>
        /// Redeem button on Bottom navigation
        /// hide or show view to click on Redeem and change label value on its requirement
        /// </summary>
        public void RedeemViewClick()
        {
            UITapGestureRecognizer redeemViewClicked = new UITapGestureRecognizer(() =>
            {

                try
                {
                    popUpMenuRedeem.Hidden = true;
                    PointsCheckoutView.Hidden = true;
                    HideShowUIBasedOnUser();
                    UserInfoView.Hidden = false;
                    lblRedeemGiftTableView.Text = AppResources.LabelRedeem;
                    lblRedeemPoints.Text = AppResources.LabelRedeemPoints;
                    HitUserInfAPI(userId,false);
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.RedeemViewClick, AppResources.Redeem, null);
                }
            });
            redeemView.UserInteractionEnabled = true;
            redeemView.AddGestureRecognizer(redeemViewClicked);
        }
        /// <summary>
        /// Home button on Bottom navigation
        /// </summary>
        public void HomeViewClick()
        {
            UITapGestureRecognizer homeViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is DashboardController)
                    {
                        NavigationController.PopToViewController(item, true);
                        count1++;
                        break;
                    }
                }
                if (count1 == 0)
                {
                    DashboardController dashboardController = (DashboardController)Storyboard.InstantiateViewController(AppResources.DashboardController);
                    this.NavigationController.PushViewController(dashboardController, true);
                } 
            });
            homeView.UserInteractionEnabled = true;
            homeView.AddGestureRecognizer(homeViewClicked);
        }
        /// <summary>
        /// Book button on Bottom navigation
        /// </summary>
        public void BookViewClick()
        {
            UITapGestureRecognizer bookViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is BookProductsController)
                    {
                        NavigationController.PopToViewController(item, true);
                        count1++;
                        break;
                    }
                }
                if (count1 == 0)
                {
                    BookProductsController bookProductsController = (BookProductsController)Storyboard.InstantiateViewController(AppResources.BookProductsController);
                    this.NavigationController.PushViewController(bookProductsController, true);
                }
            });
            bookView.UserInteractionEnabled = true;
            bookView.AddGestureRecognizer(bookViewClicked);
        }
        /// <summary>
        /// About button on Bottom navigation
        /// </summary>
        public void AboutViewClick()
        {
            UITapGestureRecognizer aboutViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is AboutController)
                    {
                        NavigationController.PopToViewController(item, true);
                        count1++;
                        break;
                    }
                }
                if (count1 == 0)
                {
                    this.PerformSegue(AppResources.SegueFromRedeemToAbout, this);
                }
            });
            aboutView.UserInteractionEnabled = true;
            aboutView.AddGestureRecognizer(aboutViewClicked);
        }
        /// <summary>
        /// Gesture implementation for terms and conditions click.
        /// To open web view and show privacy policy
        /// </summary>
        public void TermsAndConditionsClick()
        {
            UITapGestureRecognizer termsAndConditionsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
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
        public void PrivacyPolicyClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
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
                popUpMenuRedeem.Hidden = true;
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
                popUpMenuRedeem.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportClicked);
        }
        /// <summary>
        /// Gesture implementation for Redeem main view click to hide popup.
        /// </summary>
        public void RedeemMainViewClick()
        {
            UITapGestureRecognizer redeemMainViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuRedeem.Hidden = true;
                utility.DismissKeyboardOnTap(new UIView[] { RedeemMainView });
            });
            RedeemMainView.UserInteractionEnabled = true;
            RedeemMainView.AddGestureRecognizer(redeemMainViewClicked);
        }
        /// <summary>
        /// Gesture implementation for logout click
        /// Logout user
        /// </summary>
        public void LogoutClick()
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
        /// Did the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}