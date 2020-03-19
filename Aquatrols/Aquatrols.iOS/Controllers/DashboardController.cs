using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using WebKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.Controllers
{
    /// <summary>
    /// Dashboard controller is used for tab bar, menu and Teritiory manager view.
    /// </summary>
    public partial class DashboardController : UIViewController, IWKNavigationDelegate
    {
        private LoadingOverlay loadPop;
        private UserInfoEntity userInfoEntity = null;
        private Utility utility = Utility.GetInstance;
        private string token, approved;
        private string isTerritory, userId = String.Empty;
        private int viewWidth;
        private string count;
        WKWebView webView;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.DashboardController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public DashboardController(IntPtr handle) : base(handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {

            //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
            string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
            if (!String.IsNullOrEmpty(exception))
            {
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.Dashboard, exception); // exception handling
            }
            base.ViewDidLoad();
            // get the user id 
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId); // get the user id 
            }
            // get the territory
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.isTerritory)))
            {
                isTerritory = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.isTerritory); // get the territory value 
                if (isTerritory.Equals(AppResources.TerritoryManager))
                {
                    imgTmViewBtn.Hidden = false; // show the territory manager view 
                }
                else
                {
                    imgTmViewBtn.Hidden = true; // hide the territory manager view 
                }
            }
            //webView.NavigationDelegate = this;
            SetLogoSize();
            ImgMyCartClick();
            ImgMenuClick();
            HomeDashboardClick();
            BookDashboardClick();
            RedeemDashboardClick();
            AboutDashboardClick();
            ChildViewDashboardClick();
            PopUpMenuClick();
            LblLogoutClick();
            UserProfileViewClick();
            ImgRefreshClick();
            ImgMenuDeactiveClick();
            DashboardDeactivatedAccountViewClick();
            PopupMenuDeactivateClick();
            LblLogoutDClick();
            LblPrivacyPolicyClick();
            LblPrivacyPolicyDeacClick();
            LblTermsAndConditionsDeactivateClick();
            LblTermsAndConditionsClick();
            SetFonts();
            LblFAQClick();
            LblFAQDeacClick();
            LblSupportClick();
            LblSupportDeacClick();
            imgTmViewBtnClick();
            badgeView.BackgroundColor = UIColor.Red; // set the background color in badge view 
            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];// set the corner radius in badge view 
            badgeView.Layer.BorderWidth = 2f;
            badgeView.Layer.BorderColor = UIColor.White.CGColor; // set the border color in badge view 
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
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.Dashboard);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
                navigateToWebView("webaccount/SigninMobile??username=", "Dashboard", true);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.Dashboard, null); // exception handling 
            }
            // Perform any additional setup after loading the view, typically from a nib.
        }

        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            bool isTerms = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isTermsConditions);
            if (isTerms)
            {
                NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isTermsConditions);
            }
            try
            {
                //if user approved then show dashboard otherwise show deactivate dashboard
                approved = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.approved);
                if (approved == AppResources.ValueApproved)
                {
                    ChildViewDashboard.Hidden = false;
                    dashboardDeactivatedAccountView.Hidden = true;
                }
                else
                {
                    Login(true);
                }
                HitUserInfoAPI(userId); // hit the user information API
                HitWishListItemCountAPI(); // hit the wish list count api 
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewWillAppear, AppResources.Dashboard, null); // exception handling
            }
        }
        /// <summary>
        /// Hit the wish list item count API.
        /// // check badge value on particular user id
        /// </summary>
        public async void HitWishListItemCountAPI()
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    count = await utility.GetWishListItemCount(token); // hit the wish list cout api
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
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error).Show(); // show the toast message
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitWishListItemCountAPI, AppResources.Dashboard, null); // exception handling 
            }
            finally
            {
                loadPop.Hide(); // hide the loader 
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
                string device = UIDevice.CurrentDevice.Model; // get the device model 
                if (device == AppResources.iPad) // check the condition in iPAD and iOS devices
                {
                    viewWidth = (int)UIScreen.MainScreen.Bounds.Width; // get the width in device 
                    int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height;// get the height the device 
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[14], Constant.lstDigit[14]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        imgTmViewBtn.Frame = new CGRect(imgTmViewBtn.Frame.X, UserProfileView.Frame.Y + UserProfileView.Frame.Height + Constant.lstDigit[12], imgTmViewBtn.Frame.Width, imgTmViewBtn.Frame.Height + Constant.lstDigit[17]);
                        LogoDashBoard.Frame = new CGRect(LogoDashBoard.Frame.X, LogoDashBoard.Frame.Y, LogoDashBoard.Frame.Width - Constant.digitNinteenPointsFive, LogoDashBoard.Frame.Height + Constant.digitEighteenPointsFive);
                    }
                    else
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[13], Constant.lstDigit[13]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        imgTmViewBtn.Frame = new CGRect(imgTmViewBtn.Frame.X, UserProfileView.Frame.Y + UserProfileView.Frame.Height + Constant.lstDigit[12], imgTmViewBtn.Frame.Width, imgTmViewBtn.Frame.Height + 45.5);
                        LogoDashBoard.Frame = new CGRect(LogoDashBoard.Frame.X, LogoDashBoard.Frame.Y, LogoDashBoard.Frame.Width - Constant.digitTwelvePointsFive, LogoDashBoard.Frame.Height + Constant.digitElevenPointsFive);
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
                            LogoDashBoardDeac.Frame = new CGRect(Constant.lstDigit[6], Constant.lstDigit[41], Constant.lstDigit[42], Constant.lstDigit[43]);
                            LogoDashBoard.Frame = new CGRect(Constant.lstDigit[6], Constant.lstDigit[41], Constant.lstDigit[42], Constant.lstDigit[43]);
                            badgeView.Frame = new CGRect(Constant.lstDigit[45], Constant.lstDigit[9], Constant.lstDigit[44], Constant.lstDigit[44]);
                            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                            badgeView.Layer.BorderWidth = 2f;
                            dashboardHeaderView.Frame = new CGRect(dashboardHeaderView.Frame.X, dashboardHeaderView.Frame.Y + Constant.lstDigit[10], dashboardHeaderView.Frame.Width, dashboardHeaderView.Frame.Height);
                            UserProfileView.Frame = new CGRect(UserProfileView.Frame.X, dashboardHeaderView.Frame.Y + dashboardHeaderView.Frame.Height + Constant.lstDigit[6], UserProfileView.Frame.Width, UserProfileView.Frame.Height);
                            imgTmViewBtn.Frame = new CGRect(imgTmViewBtn.Frame.X, UserProfileView.Frame.Y + UserProfileView.Frame.Height + Constant.lstDigit[12], imgTmViewBtn.Frame.Width, imgTmViewBtn.Frame.Height);
                            headerdashboardDeac.Frame = new CGRect(headerdashboardDeac.Frame.X, headerdashboardDeac.Frame.Y + Constant.lstDigit[10], headerdashboardDeac.Frame.Width, headerdashboardDeac.Frame.Height);
                            popUpMenuView.Frame = new CGRect(popUpMenuView.Frame.X, popUpMenuView.Frame.Y + Constant.lstDigit[10], popUpMenuView.Frame.Width, popUpMenuView.Frame.Height);
                            popupMenuDeactivate.Frame = new CGRect(popupMenuDeactivate.Frame.X, popupMenuDeactivate.Frame.Y + Constant.lstDigit[10], popupMenuDeactivate.Frame.Width, popupMenuDeactivate.Frame.Height);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.Dashboard, null);
            }
        }
        /// <summary>
        /// Set the fonts ofUILabel, UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblLogout, lblPrivacy, lblPrivacyDeac, lblTermsAndConditions, lblBook, lblRedeem, lblAbout, lblTermsAndConditionsDeactivate, lblLogoutDeactivated, lblFaq, lblFaqDeac, lblSupport, lblSupportDeac }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsItalic(new UILabel[] { lblCourseName, lblPointsPerUnit }, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblFullname }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblHome }, null, Constant.lstDigit[9], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblDashboard, lblDashboardDeac }, null, Constant.lstDigit[11], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblBadge }, null, Constant.lstDigit[6], viewWidth);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.Dashboard, null);
            }
        }
        /// <summary>
        /// Send the device token to server for push notification.
        /// </summary>
        public async void SendDeviceTokenToServer()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.DeviceToken)))
                    {
                        string deviceToken = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.DeviceToken);
                        PushDataModelEntity pushDataModelEntity = new PushDataModelEntity();
                        pushDataModelEntity.userId = userId;
                        pushDataModelEntity.deviceToken = deviceToken;
                        pushDataModelEntity.deviceType = AppResources.iOS;
                        pushDataModelEntity.region = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                        PushDataModelResponseEntity pushDataModelResponseEntity = await utility.SendServiceToken(pushDataModelEntity, token);
                        if (pushDataModelResponseEntity.operationStatus.ToLower() == AppResources.success)
                        {
                            Console.Write(AppResources.DeviceTokenSent);
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
                utility.SaveExceptionHandling(ex, AppResources.SendDeviceTokenToServer, AppResources.Dashboard, null);
            }
        }
        /// <summary>
        /// Hit the user information API.
        /// set the value of fullname,course name and balance points
        /// </summary>
        /// <param name="userId">User identifier.</param>
        protected async void HitUserInfoAPI(string userId)
        {
            try
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
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isEmailPreference, AppResources.isEmailPreference);
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.isNotification, AppResources.isNotification);
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.country, AppResources.CountryName);
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.role, AppResources.role);
                        // added to set course id to append with redeem url
                        NSUserDefaults.StandardUserDefaults.SetString(userInfoEntity.courseId, AppResources.CourseId);
                        string loyaltyPoints = await utility.GetLoyaltyPoints(userInfoEntity.MembershipId);
                        if (!string.IsNullOrWhiteSpace(loyaltyPoints))
                        {
                            lblPointsPerUnit.Text = "Points: " + loyaltyPoints;
                            lblPointsPerUnit.Hidden = false;
                        }
                        SendDeviceTokenToServer();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitUserInfAPI, AppResources.Dashboard, null);
            }
        }
        /// <summary>
        /// Hit login API
        /// login user if user login credientials is right then show dashboard screen
        /// move to dashboard.
        /// </summary>
        protected async void Login(bool isRegistration)
        {
            if (isRegistration)
            {
                try
                {
                    LoginRequestEntity loginRequestEntity = new LoginRequestEntity();
                    loginRequestEntity.username = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userName);
                    loginRequestEntity.password = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.password);
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        LoginResponseEntity loginResponseEntity = await utility.UserLogin(loginRequestEntity);
                        if (loginResponseEntity.operationStatus.ToLower() == AppResources.success && loginResponseEntity.isApproved.ToLower() == AppResources.approved)
                        {
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userId, AppResources.userId);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.token, AppResources.token);
                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.IsLogedIn);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.isApproved, AppResources.approved);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.role[0], AppResources.role);
                            ChildViewDashboard.Hidden = false;
                            dashboardDeactivatedAccountView.Hidden = true;
                            HitUserInfoAPI(loginResponseEntity.userId);
                        }
                        else if (loginResponseEntity.operationStatus.ToLower() == AppResources.MsgError)
                        {
                            // navigate to message screen when not able to login at time of new register user
                            RegistrationSuccessViewController registrationSuccess = (RegistrationSuccessViewController)Storyboard.InstantiateViewController("RegistrationSuccessViewController");
                            this.NavigationController.PushViewController(registrationSuccess, true);
                            //Toast.MakeText(loginResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                        }
                        else
                        {
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userId, AppResources.userId);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.token, AppResources.token);
                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.IsLogedIn);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.isApproved, AppResources.approved);
                            dashboardDeactivatedAccountView.Hidden = false;
                            ChildViewDashboard.Hidden = true;
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.Login, AppResources.Dashboard, null);
                }
            }
            else
            {
                try
                {
                    loadPop = new LoadingOverlay(View.Frame);
                    this.View.Add(loadPop);
                    LoginRequestEntity loginRequestEntity = new LoginRequestEntity();
                    loginRequestEntity.username = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userName);
                    loginRequestEntity.password = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.password);
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        LoginResponseEntity loginResponseEntity = await utility.UserLogin(loginRequestEntity);
                        if (loginResponseEntity.operationStatus.ToLower() == AppResources.success && loginResponseEntity.isApproved.ToLower() == AppResources.approved)
                        {
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userId, AppResources.userId);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.token, AppResources.token);
                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.IsLogedIn);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.isApproved, AppResources.approved);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.role[0], AppResources.role);
                            ChildViewDashboard.Hidden = false;
                            dashboardDeactivatedAccountView.Hidden = true;
                            HitUserInfoAPI(loginResponseEntity.userId);
                            //HitWishListItemCountAPI(false);
                        }
                        else if (loginResponseEntity.operationStatus.ToLower() == AppResources.MsgError)
                        {
                            Toast.MakeText(loginResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                        }
                        else
                        {
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userId, AppResources.userId);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.token, AppResources.token);
                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.IsLogedIn);
                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.isApproved, AppResources.approved);
                            dashboardDeactivatedAccountView.Hidden = false;
                            ChildViewDashboard.Hidden = true;
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.Login, AppResources.Dashboard, null);
                }
                finally
                {
                    loadPop.Hide();
                }
            }
        }
        /// <summary>
        /// handles the tm view button click.
        /// </summary>
        public void imgTmViewBtnClick()
        {
            UITapGestureRecognizer imgTmViewBtnClicked = new UITapGestureRecognizer(() =>
            {
                navigateToWebView("App/Dashboard/Index", "Territory Manager Dashboard");
                //PerformSegue(AppResources.SegueFromDashboardToTmViewHome, this);
            });
            imgTmViewBtn.UserInteractionEnabled = true;
            imgTmViewBtn.AddGestureRecognizer(imgTmViewBtnClicked);
        }
        /// <summary>
        /// Gesture implementation for Menu Click to hide or show pop up menu.
        /// </summary>
        public void ImgMenuClick()
        {
            UITapGestureRecognizer imgMenuClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenuView.Hidden == true)
                {
                    popUpMenuView.Hidden = false;
                }
                else
                {
                    popUpMenuView.Hidden = true;
                }
            });
            MenuImgView.UserInteractionEnabled = true;
            MenuImgView.AddGestureRecognizer(imgMenuClicked);
        }
        /// <summary>
        /// click user profile to show user profile detail in my account screen
        /// </summary>
        public void UserProfileViewClick()
        {
            UITapGestureRecognizer userProfileViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                this.PerformSegue(AppResources.SegueFromDashboardToMyAccount, this);
            });
            UserProfileView.UserInteractionEnabled = true;
            UserProfileView.AddGestureRecognizer(userProfileViewClicked);
        }
        /// <summary>
        /// Home button on bottom navigation
        /// </summary>
        public void HomeDashboardClick()
        {
            UITapGestureRecognizer homeViewDashboardClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                lblDashboard.Text = "Dashboard";
                webView.Hidden = true;
                Back_Button.Hidden = true;
                LogoDashBoard.Hidden = false;
            });
            HomeViewDashboard.UserInteractionEnabled = true;
            HomeViewDashboard.AddGestureRecognizer(homeViewDashboardClicked);
        }
        /// <summary>
        /// Book button on bottom navigation
        /// </summary>
        public void BookDashboardClick()
        {
            UITapGestureRecognizer bookViewDashboardClicked = new UITapGestureRecognizer(() =>
            {
                int count1 = 0;
                popUpMenuView.Hidden = true;
                navigateToWebView("App/Program/Index", "Programs");

                //UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                //foreach (UIViewController item in uIViewControllers)
                //{
                //    if (item is BookProductsController)
                //    {
                //        NavigationController.PopToViewController(item, true);
                //        count1++;
                //        break;
                //    }
                //}                
                //if(count1==0)
                //{
                //    this.PerformSegue(AppResources.SegueFromDashboardToBookProducts, this);
                //}
            });
            BookViewDashboard.UserInteractionEnabled = true;
            BookViewDashboard.AddGestureRecognizer(bookViewDashboardClicked);
        }
        /// <summary>
        /// Redeem button on bottom navigation
        /// </summary>
        public void RedeemDashboardClick()
        {


            UITapGestureRecognizer redeemViewDashboardClicked = new UITapGestureRecognizer(() =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                        {
                            string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                            if (role.ToLower().Trim() == "manager")
                            {
                                popUpMenuView.Hidden = true;
                                int count1 = 0;
                                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                                foreach (UIViewController item in uIViewControllers)
                                {
                                    if (item is LpRedeemWebViewController)
                                    {
                                        NavigationController.PopToViewController(item, true);
                                        count1++;
                                        break;
                                    }
                                }
                                if (count1 == 0)
                                {
                                    this.PerformSegue("SegueFromDashboardToRedeem", this);
                                }
                            }
                            else
                            {
                                Utility.DebugAlert("Only managers can redeem for points", "");
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Utility.DebugAlert(ex.Message, "");
                        Console.WriteLine(ex.Message);
                    }

                });
            /*
            UITapGestureRecognizer redeemViewDashboardClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    Utility.DebugAlert("LpRedeemWebViewController", "");
                    LpRedeemWebViewController redeemController = (LpRedeemWebViewController)Storyboard.InstantiateViewController("LpRedeemWebViewController");
                    this.NavigationController.PushViewController(redeemController, true);

                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                    {
                        string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                        if (role.ToLower().Trim() == "user")
                        {




                        }
                        else
                        {
                            Utility.DebugAlert("This feature is not supported for your role", "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                */
            /*
            popUpMenuView.Hidden = true;
            int count1 = 0;
            UIViewController[] uIViewControllers = NavigationController.ViewControllers;
            foreach (UIViewController item in uIViewControllers)
            {
                if (item is RedeemPointsController)
                {
                    NavigationController.PopToViewController(item, true);
                    count1++;
                    break;
                }
            }
            if (count1 == 0)
            {
                this.PerformSegue(AppResources.SegueFromDashBoardToRedeemPoints, this);
            }

        }); */
            RedeemViewDashboard.UserInteractionEnabled = true;
            RedeemViewDashboard.AddGestureRecognizer(redeemViewDashboardClicked);
        }
        /// <summary>
        /// About button on bottom navigation
        /// </summary>
        public void AboutDashboardClick()
        {
            UITapGestureRecognizer aboutViewDashboardClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                navigateToWebView("about", "About");
                //int count1 = 0;
                //UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                //foreach (UIViewController item in uIViewControllers)
                //{
                //    if (item is AboutController)
                //    {
                //        NavigationController.PopToViewController(item, true);
                //        count1++;
                //        break;
                //    }
                //}
                //if (count1 == 0)
                //{
                //    this.PerformSegue(AppResources.SegueFromDashboardToAbout, this);
                //}
            });
            AboutViewDashboard.UserInteractionEnabled = true;
            AboutViewDashboard.AddGestureRecognizer(aboutViewDashboardClicked);
        }
        /// <summary>
        /// Gesture implementation for Child dashboard view click to hide pop up menu.
        /// </summary>
        public void ChildViewDashboardClick()
        {
            UITapGestureRecognizer childViewDashboardClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
            });
            ChildViewDashboard.UserInteractionEnabled = true;
            ChildViewDashboard.AddGestureRecognizer(childViewDashboardClicked);
        }
        /// <summary>
        /// Gesture implementation for Pop up click.
        /// </summary>
        public void PopUpMenuClick()
        {
            UITapGestureRecognizer popUpMenuClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = false;
            });
            popUpMenuView.UserInteractionEnabled = true;
            popUpMenuView.AddGestureRecognizer(popUpMenuClicked);
        }
        /// <summary>
        /// Gesture implementation for my cart if role is user to show pop up other wise show my cart screen
        /// </summary>
        public void ImgMyCartClick()
        {
            UITapGestureRecognizer imgMyCartClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                navigateToWebView("App/Cart/Index", "My Queue");
                //try
                //{
                //    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                //    {
                //        string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                //        if (role.ToLower().Trim() == AppResources.user)
                //        {
                //            Utility.DebugAlert(AppResources.Message, AppResources.AccountRole);
                //        }
                //        else if (count == Constant.lstDigitString[5])
                //        {
                //            Utility.DebugAlert(AppResources.Message, AppResources.MessageEmptyQueue);
                //        }
                //        else
                //        {

                //            this.PerformSegue(AppResources.SegueFromDashboardToMyCart, this);
                //        }
                //    }  
                //}
                //catch(Exception ex)
                //{
                //    utility.SaveExceptionHandling(ex,AppResources.ImgMyCartClick, AppResources.Dashboard, null);
                //}
            });
            ImgMyCart.UserInteractionEnabled = true;
            ImgMyCart.AddGestureRecognizer(imgMyCartClicked);
        }
        /// <summary>
        ///Gesture implementation for terms and conditions click.
        /// </summary>
        public void LblTermsAndConditionsClick()
        {
            UITapGestureRecognizer lblTermsAndConditionsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTermsAndConditions.UserInteractionEnabled = true;
            lblTermsAndConditions.AddGestureRecognizer(lblTermsAndConditionsClicked);
        }
        /// <summary>
        /// Gesture implementation for privacy policy click.
        /// </summary>
        public void LblPrivacyPolicyClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacy.UserInteractionEnabled = true;
            lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// Gesture implementation for privacy policy click on deactivate dashboard.
        /// </summary>
        public void LblPrivacyPolicyDeacClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuDeactivate.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacyDeac.UserInteractionEnabled = true;
            lblPrivacyDeac.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// Gesture implementation for FAQ click.
        /// </summary>
        public void LblFAQClick()
        {
            UITapGestureRecognizer lblFAQClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
            });
            lblFaq.UserInteractionEnabled = true;
            lblFaq.AddGestureRecognizer(lblFAQClicked);
        }
        /// <summary>
        /// Gesture implementation for FAQ click on deactivate dashboard.
        /// </summary>
        public void LblFAQDeacClick()
        {
            UITapGestureRecognizer lblFAQDeacClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuDeactivate.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[5], AppResources.PDFValue);
            });
            lblFaqDeac.UserInteractionEnabled = true;
            lblFaqDeac.AddGestureRecognizer(lblFAQDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for Support click.
        /// </summary>
        public void LblSupportClick()
        {
            UITapGestureRecognizer lblSupportClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuView.Hidden = true;
                AbpoutAppViewController termsConditionWebViewController = (AbpoutAppViewController)Storyboard.InstantiateViewController("AbpoutAppViewController");
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportClicked);
        }
        /// <summary>
        /// Gesture implementation for Support click on deactivate dashboard.
        /// </summary>
        public void LblSupportDeacClick()
        {
            UITapGestureRecognizer lblSupportDeacClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuDeactivate.Hidden = true;
                AbpoutAppViewController termsConditionWebViewController = (AbpoutAppViewController)Storyboard.InstantiateViewController("AbpoutAppViewController");
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupportDeac.UserInteractionEnabled = true;
            lblSupportDeac.AddGestureRecognizer(lblSupportDeacClicked);
        }
        /// <summary>
        /// Gesture implementation for refresh click.
        /// if user click to refresh button then call login method, if user role is approved then change the dashboard screen 
        /// </summary>
        public void ImgRefreshClick()
        {
            UITapGestureRecognizer imgRefreshClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuDeactivate.Hidden = true;
                Login(false);
            });
            ImgRefresh.UserInteractionEnabled = true;
            ImgRefresh.AddGestureRecognizer(imgRefreshClicked);
        }
        /// <summary>
        /// Gesture implementation for menu click on deactivate dashboard.
        /// hide or show pop up
        /// </summary>
        public void ImgMenuDeactiveClick()
        {
            UITapGestureRecognizer imgMenuDeactiveClicked = new UITapGestureRecognizer(() =>
            {
                if (popupMenuDeactivate.Hidden == true)
                {
                    popupMenuDeactivate.Hidden = false;
                }
                else
                {
                    popupMenuDeactivate.Hidden = true;
                }
            });
            ImgMenuDeactive.UserInteractionEnabled = true;
            ImgMenuDeactive.AddGestureRecognizer(imgMenuDeactiveClicked);
        }
        /// <summary>
        /// Gesture implementation for terms and conditions clcik on deactivate dashboard.
        /// </summary>
        public void LblTermsAndConditionsDeactivateClick()
        {
            UITapGestureRecognizer lblTermsAndConditionsDeactivateClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuDeactivate.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTermsAndConditionsDeactivate.UserInteractionEnabled = true;
            lblTermsAndConditionsDeactivate.AddGestureRecognizer(lblTermsAndConditionsDeactivateClicked);
        }
        /// <summary>
        /// Gesture implementation for logout click.
        /// Logout user.
        /// </summary>
        public void LblLogoutClick()
        {
            UITapGestureRecognizer lblLogoutClicked = new UITapGestureRecognizer(() =>
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
            lblLogout.AddGestureRecognizer(lblLogoutClicked);
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
            lblLogoutDeactivated.UserInteractionEnabled = true;
            lblLogoutDeactivated.AddGestureRecognizer(lblLogoutDClicked);
        }
        /// <summary>
        /// Gesture implementation for Dashboard on the deactivated account to hide pop up menu.
        /// </summary>
        public void DashboardDeactivatedAccountViewClick()
        {
            UITapGestureRecognizer dashboardDeactivatedAccountViewClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuDeactivate.Hidden = true;
            });
            dashboardDeactivatedAccountView.UserInteractionEnabled = true;
            dashboardDeactivatedAccountView.AddGestureRecognizer(dashboardDeactivatedAccountViewClicked);
        }
        /// <summary>
        /// Gesture implementation for Popup menu click on deactivate dashboard to hide pop up menu.
        /// </summary>
        public void PopupMenuDeactivateClick()
        {
            UITapGestureRecognizer popupMenuDeactivateClicked = new UITapGestureRecognizer(() =>
            {
                popupMenuDeactivate.Hidden = false;
            });
            popupMenuDeactivate.UserInteractionEnabled = true;
            popupMenuDeactivate.AddGestureRecognizer(popupMenuDeactivateClicked);
        }
        /// <summary>
        /// Dids the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void navigateToWebView(string url, string header, bool isHidden = false)
        {
            if (webView == null)
            {
                if (NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token) == null)
                    return;
                CGRect webRect = new CGRect(0, 126, this.View.Bounds.Size.Width, this.View.Bounds.Size.Height - 220);
                webView = new WKWebView(webRect, new WKWebViewConfiguration());
                ChildViewDashboard.AddSubview(webView);
                popUpMenuView.Superview?.BringSubviewToFront(popUpMenuView);
                popupMenuDeactivate.Superview?.BringSubviewToFront(popupMenuDeactivate);
                // replace done to resolve + url encoding issue 
                string userName = EncryptString(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userName)).Replace("+", "%2B");
                string passWord = EncryptString(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.password)).Replace("+", "%2B");
                url = "webaccount/SigninMobile?userName=" + userName + "&password=" + passWord;
                webView.NavigationDelegate = this;
            }
            lblDashboard.Text = header;
            var request = new NSUrlRequest(new NSUrl(baseURL + url));

            if (string.Equals(header, "About"))
                request = new NSUrlRequest(new NSUrl("https://aquatrols.com/about/"));            

            webView.LoadRequest(request);
            webView.Hidden = isHidden;
            if (!isHidden)
            {
                LogoDashBoard.Hidden = true;
                Back_Button.Hidden = false;
                loadPop = new LoadingOverlay(View.Frame);
                this.View.Add(loadPop);
            }
        }
        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (webView.IsLoading)
                return;
            loadPop.Hide();
        }

        public static string EncryptString(string plainText)
        {
            string initVector = "paycnil9uzpgoa61";
            string passPhrase = "Aqua9182trol@90";
            int keysize = 256;
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
       
        partial void back_button_click(UIButton sender)
        {
            var webBackForwardList = webView.BackForwardList;
            
            if (webBackForwardList.BackList.Length <= 1) // if previous url instead of login url, then go back
            {
                webView.Hidden = true;
                Back_Button.Hidden = true;
                LogoDashBoard.Hidden = false;
                lblDashboard.Text = "Dashboard";
            }
            if (webView.CanGoBack)
                webView.GoBack();
        }
    }
}

