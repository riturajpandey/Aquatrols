using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using Plugin.GoogleAnalytics;
using System;
using System.Linq;
using ToastIOS;
using UIKit;
using Xamarin.Essentials;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.Controllers
{
    /// <summary>
    /// This class file login controller is used for login screen
    /// </summary>
    public partial class LoginController : UIViewController
    {
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.LoginController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public LoginController(IntPtr handle) : base(handle)
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
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.Login, exception); // exception handling
            }
            base.ViewDidLoad();
            NavigationController.SetNavigationBarHidden(true, true);
            LoginScrollView.ContentSize = new CGSize(LoginChildView.Bounds.Width, LoginChildView.Bounds.Height);
            LoginChildView.BackgroundColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
            imgLogo.TintColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[18], Constant.lstDigit[21]);
            btnLogin.BackgroundColor = UIColor.FromRGB(Constant.lstDigit[0], Constant.lstDigit[16], Constant.lstDigit[19]);
            // Perform any additional setup after loading the view, typically from a nib.
            DismissKeyboardClick();
            ForgetPasswordClick();
            PasswordImgClick();
            AddDoneBUtton();
            MoveScrollView();
            SetFonts();
            SetImagesSize();
            try
            {
                bool isConnected = utility.CheckInternetConnection(); // check the internet connection
                if (isConnected)
                {
                    //Implement google analytics
                    GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                    GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                    GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                    GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                    GoogleAnalytics.Current.InitTracker();
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.Login);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.Login, null); // exception handling 

            }
        }
        /// <summary>
        /// Sets the size of the images.
        /// </summary>
        public void SetImagesSize()
        {
            try
            {
                string device = UIDevice.CurrentDevice.Model;// find the device type
                if (device == AppResources.iPad)
                {
                    int viewWidth = (int)UIScreen.MainScreen.Bounds.Width;
                    int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height;
                    if (viewWidth == (int)DeviceScreenSize.IPadPro)
                    {
                        imgLogo.Frame = new CGRect(imgLogo.Frame.X + Constant.lstDigit[15], imgLogo.Frame.Y - Constant.digitTwentyFive, imgLogo.Frame.Width - Constant.lstDigit[16], imgLogo.Frame.Height + Constant.lstDigit[16]);
                    }
                    else
                    {
                        imgLogo.Frame = new CGRect(imgLogo.Frame.X + Constant.lstDigit[14], imgLogo.Frame.Y - Constant.digitTwentyFive, imgLogo.Frame.Width - Constant.digitEighty, imgLogo.Frame.Height + Constant.digitNintyNinePointsFive);
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetImagesSize, AppResources.Login, null); // exception handling
            }
        }
        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override async void ViewWillAppear(bool animated)
        {
            //check user already login or not if already login then move to dashboard screen
            bool isConnected = utility.CheckInternetConnection();
            if (isConnected)
            {
                bool flag = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.IsLogedIn);
                if (flag)
                {
                    string token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);

                    //bool isValidToken = await utility.CheckTokenValidity(token);
                    //if(isValidToken)
                    //{
                    //    this.PerformSegue("SegueFromLoginToLoggedInWebView", null);
                    //    return;
                    //}
                    //else
                    //{
                    //    imgVisibleEye.Hidden = true;
                    //    return;
                    //}
                    string linkData = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.LinkData);
                    if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.EOP))
                    {
                        BookProductsController bookProductsController = (BookProductsController)Storyboard.InstantiateViewController(AppResources.BookProductsController);
                        this.NavigationController.PushViewController(bookProductsController, true);
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                    }
                    else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Redeem))
                    {
                        RedeemPointsController redeemPointsController = (RedeemPointsController)Storyboard.InstantiateViewController(AppResources.RedeemPointsController);
                        this.NavigationController.PushViewController(redeemPointsController, true);
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                    }
                    else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Home))
                    {
                        DashboardController dashboardController = (DashboardController)Storyboard.InstantiateViewController(AppResources.DashboardController);
                        this.NavigationController.PushViewController(dashboardController, true);
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                    }
                    else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Profile))
                    {
                        MyAccountController myAccountController = (MyAccountController)Storyboard.InstantiateViewController(AppResources.MyAccountController);
                        this.NavigationController.PushViewController(myAccountController, true);
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                    }
                    else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Product))
                    {
                        NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.deepLink);
                        AboutController aboutController = (AboutController)Storyboard.InstantiateViewController(AppResources.AboutController);
                        this.NavigationController.PushViewController(aboutController, true);
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                    }
                    else if (!string.IsNullOrEmpty(linkData) && linkData.Equals("Signup"))
                    {
                        var uri = new Uri("https://approachv3.aquatrols.com/WebAccount/Signup");
                        await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                        //NSUserDefaults.StandardUserDefaults.SetBool(true,AppResources.deepLink);
                        //this.PerformSegue(AppResources.SignUpSeque,null);
                        //NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                    }
                    else
                    {
                        this.PerformSegue(AppResources.SegueFromLoginToDashboard, null);
                    }
                }
                else
                {
                    string linkData = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.LinkData);
                    if (!string.IsNullOrEmpty(linkData) && linkData.Equals("Signup"))
                    {
                        this.PerformSegue(AppResources.SignUpSeque, null);
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                    }
                }
            }
            else
            {
                Toast.MakeText(AppResources.Error).Show(); // show the toast message 
            }
            // hide or show eye on specify condition
            if (!String.IsNullOrEmpty(txtPassword.Text))
            {
                imgVisibleEye.Hidden = false;
            }
            else
            {
                imgVisibleEye.Hidden = true;
            }
        }
        /// <summary>
        /// Set the fonts UILabel, UITextField and UIButton.
        /// Padding the UITextField
        /// </summary>
        public void SetFonts()
        {
            try
            {
                Utility.SetPadding(new UITextField[] { txtUsername, txtPassword }, Constant.lstDigit[4]);
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblUserName, lblPassword }, new UITextField[] { txtUsername, txtPassword }, Constant.lstDigit[11], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblAlreadyAccount, lblDontHaveAccount, lblFogotPassword }, null, Constant.lstDigit[10], viewWidth);
                Utility.SetFontsforHeader(null, new UIButton[] { btnLogin, btnSignUp }, Constant.lstDigit[11], viewWidth);

            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.Login, null); // exception handling
            }
        }
        /// <summary>
		/// User login.
        /// Hit user login API
        /// check validation on fields
		/// </summary>
		protected async void Login()
        {
            try
            {
                loadPop = new LoadingOverlay(View.Frame);
                this.View.Add(loadPop);
                if (String.IsNullOrEmpty(txtUsername.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireUserName, Constant.durationOfToastMessage).Show();
                }
                else
                {
                    if (utility.IsValidEmail(txtUsername.Text.Trim()))
                    {
                        if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
                        {
                            Toast.MakeText(AppResources.RequirePassword, Constant.durationOfToastMessage).Show();
                        }
                        else
                        {
                            LoginRequestEntity loginRequestEntity = new LoginRequestEntity();
                            loginRequestEntity.username = txtUsername.Text.Trim();
                            NSUserDefaults.StandardUserDefaults.SetString(loginRequestEntity.username, AppResources.userName);
                            loginRequestEntity.password = txtPassword.Text.Trim();
                            NSUserDefaults.StandardUserDefaults.SetString(loginRequestEntity.password, AppResources.password);
                            bool isConnected = utility.CheckInternetConnection();
                            if (isConnected)
                            {
                                LoginResponseEntity loginResponseEntity = await utility.UserLogin(loginRequestEntity); // hit the login API 
                                if (loginResponseEntity.operationStatus.ToLower() == AppResources.success && loginResponseEntity.isApproved.ToLower() == AppResources.approved)
                                {
                                    var usertype = loginResponseEntity.role.Where(a => a.Equals("Manager")).FirstOrDefault();
                                    if (usertype != null)
                                    {  
                                        if (loginResponseEntity.role[0].ToLower().Equals(AppResources.admin))
                                        {
                                            Utility.DebugAlert("Alert", "There is already an Approach account registered with this golf course or business. Please contact approach@aquatrols.com for more information about this issue. Thank you.");
                                            //Toast.MakeText(AppResources.Unauthorized, Constant.durationOfToastMessage).Show();
                                        }
                                        else
                                        {
                                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.IsLogedIn);
                                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.token, AppResources.token);
                                            //this.PerformSegue("SegueFromLoginToLoggedInWebView", null);                                        
                                            //return;
                                            // removing all functionality and loggin in web view
                                            LoggedInUserWebViewController logInSuccess = (LoggedInUserWebViewController)Storyboard.InstantiateViewController("LoggedInUserWebViewController");
                                            this.NavigationController.PushViewController(logInSuccess, true);
                                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userId, AppResources.userId);
                                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userName, AppResources.userName);
                                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.role[0], AppResources.role);
                                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.IsLogedIn);
                                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.role[1], AppResources.isTerritory);
                                            NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.isApproved, AppResources.approved);
                                            txtUsername.Text = string.Empty;
                                            txtPassword.Text = string.Empty;

                                            string linkData = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.LinkData);
                                            if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.EOP))
                                            {
                                                BookProductsController bookProductsController = (BookProductsController)Storyboard.InstantiateViewController(AppResources.BookProductsController);
                                                this.NavigationController.PushViewController(bookProductsController, true);
                                                NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                            }
                                            else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Redeem))
                                            {
                                                RedeemPointsController redeemPointsController = (RedeemPointsController)Storyboard.InstantiateViewController(AppResources.RedeemPointsController);
                                                this.NavigationController.PushViewController(redeemPointsController, true);
                                                NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                            }
                                            else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Home))
                                            {
                                                DashboardController dashboardController = (DashboardController)Storyboard.InstantiateViewController(AppResources.DashboardController);
                                                this.NavigationController.PushViewController(dashboardController, true);
                                                NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                            }
                                            else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Profile))
                                            {
                                                MyAccountController myAccountController = (MyAccountController)Storyboard.InstantiateViewController(AppResources.MyAccountController);
                                                this.NavigationController.PushViewController(myAccountController, true);
                                                NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                            }
                                            else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Product))
                                            {
                                                NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.deepLink);
                                                AboutController aboutController = (AboutController)Storyboard.InstantiateViewController(AppResources.AboutController);
                                                this.NavigationController.PushViewController(aboutController, true);
                                                NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                            }
                                            else
                                            {
                                                this.PerformSegue(AppResources.SegueFromLoginToDashboard, null);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Utility.DebugAlert("Alert", "There is already an Approach account registered with this golf course or business. Please contact approach@aquatrols.com for more information about this issue. Thank you.");
                                    }
                                }
                                else if (loginResponseEntity.operationStatus.ToLower() == AppResources.MsgError)
                                {
                                    Toast.MakeText(loginResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                                    txtPassword.Text = string.Empty;
                                    imgVisibleEye.Hidden = true;
                                }
                                else
                                {
                                    if (loginResponseEntity.role[0].ToLower().Equals(AppResources.admin))
                                    {
                                        Toast.MakeText(AppResources.Unauthorized, Constant.durationOfToastMessage).Show();
                                    }
                                    else
                                    {
                                        NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userId, AppResources.userId);
                                        NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.userName, AppResources.userName);
                                        NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.token, AppResources.token);
                                        NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.role[0], AppResources.role);
                                        NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.IsLogedIn);
                                        NSUserDefaults.StandardUserDefaults.SetString(loginResponseEntity.isApproved, AppResources.approved);
                                        txtUsername.Text = string.Empty;
                                        txtPassword.Text = string.Empty;
                                        string linkData = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.LinkData);
                                        if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.EOP))
                                        {
                                            BookProductsController bookProductsController = (BookProductsController)Storyboard.InstantiateViewController(AppResources.BookProductsController);
                                            this.NavigationController.PushViewController(bookProductsController, true);
                                            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                        }
                                        else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Redeem))
                                        {
                                            RedeemPointsController redeemPointsController = (RedeemPointsController)Storyboard.InstantiateViewController(AppResources.RedeemPointsController);
                                            this.NavigationController.PushViewController(redeemPointsController, true);
                                            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                        }
                                        else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Home))
                                        {
                                            DashboardController dashboardController = (DashboardController)Storyboard.InstantiateViewController(AppResources.DashboardController);
                                            this.NavigationController.PushViewController(dashboardController, true);
                                            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                        }
                                        else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Profile))
                                        {
                                            MyAccountController myAccountController = (MyAccountController)Storyboard.InstantiateViewController(AppResources.MyAccountController);
                                            this.NavigationController.PushViewController(myAccountController, true);
                                            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                        }
                                        else if (!string.IsNullOrEmpty(linkData) && linkData.Equals(AppResources.Product))
                                        {
                                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.deepLink);
                                            AboutController aboutController = (AboutController)Storyboard.InstantiateViewController(AppResources.AboutController);
                                            this.NavigationController.PushViewController(aboutController, true);
                                            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.LinkData);
                                        }
                                        else
                                        {
                                            this.PerformSegue(AppResources.SegueFromLoginToDashboard, null);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.InvalidEmail, Constant.durationOfToastMessage).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.Login, AppResources.Login, null); // exception handling 
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Move the scroll view to open keyboard.
        /// </summary>
        public void MoveScrollView()
        {
            bool isScrollViewUp = false;
            UIKeyboard.Notifications.ObserveWillShow((s, e) =>
            {
                if (!isScrollViewUp)
                {
                    isScrollViewUp = true;
                    var keyboardFrame = UIKeyboard.FrameBeginFromNotification(e.Notification);
                    LoginChildView.Frame = new CGRect(LoginChildView.Frame.X, LoginChildView.Frame.Y - keyboardFrame.Height, LoginChildView.Frame.Width, LoginChildView.Frame.Height + Constant.lstDigit[20]);
                }
            });
            UIKeyboard.Notifications.ObserveWillHide((s, e) =>
            {
                LoginChildView.Frame = new CGRect(LoginChildView.Frame.X, Constant.lstDigit[0], LoginChildView.Frame.Width, LoginChildView.Frame.Height - Constant.lstDigit[20]);
                isScrollViewUp = false;
            });
        }
        /// <summary>
        /// Add done button on keyboard
        /// </summary>
        public void AddDoneBUtton()
        {
            UITextField[] textFields = new UITextField[] { txtUsername, txtPassword };
            Utility.AddDoneButtonToKeyboard(textFields);
        }
        /// <summary>
        /// Login button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnLogin_TouchUpInside(UIButton sender)
        {
            txtPassword.ResignFirstResponder();
            Login();
        }
        /// <summary>
        /// Buttons the sign up touch up inside.
        /// perform segue to sign up screen
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnSignUp_TouchUpInside(UIButton sender)
        {
            this.PerformSegue(AppResources.SignUpSeque, null);
        }
        /// <summary>
        /// Dismiss the keyboard click on Login child view.
        /// </summary>
        public void DismissKeyboardClick()
        {
            UITapGestureRecognizer dismissKeyboardClicked = new UITapGestureRecognizer(() =>
            {
                utility.DismissKeyboardOnTap(new UIView[] { LoginChildView });
            });
            LoginChildView.UserInteractionEnabled = true;
            LoginChildView.AddGestureRecognizer(dismissKeyboardClicked);
        }
        /// <summary>
        /// Gesture implementation for Forget password.
        /// Perform segue from login to forgot password screen
        /// </summary>
        public void ForgetPasswordClick()
        {
            UITapGestureRecognizer forgetPasswordClicked = new UITapGestureRecognizer(() =>
           {
               this.PerformSegue(AppResources.SegueFromLoginToForgotPassword, null);
           });
            lblFogotPassword.UserInteractionEnabled = true;
            lblFogotPassword.AddGestureRecognizer(forgetPasswordClicked);
        }
        /// <summary>
        /// Changed events of UITextField of password to hide or shoe show eye icon
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtPassword_EditingChanged(UITextField sender)
        {
            if (!String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                imgVisibleEye.Hidden = false;
            }
            else
            {
                imgVisibleEye.Hidden = true;
            }
        }
        /// <summary>
        /// Pasword Eye image visible or invisible click.
        /// </summary>
        public void PasswordImgClick()
        {
            bool imageshow = false;
            UITapGestureRecognizer passwordImgClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (imageshow == false)
                    {
                        imageshow = true;
                        imgVisibleEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                        txtPassword.SecureTextEntry = false;
                    }
                    else
                    {
                        imageshow = false;
                        imgVisibleEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                        txtPassword.SecureTextEntry = true;
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.PasswordImgClick, AppResources.Login, null);
                }
            });
            imgVisibleEye.UserInteractionEnabled = true;
            imgVisibleEye.AddGestureRecognizer(passwordImgClicked);
        }
        /// <summary>
        /// Dids the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

