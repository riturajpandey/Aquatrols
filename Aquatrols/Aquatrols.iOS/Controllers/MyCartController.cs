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
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file is used for the my cart screen
    /// </summary>
    public partial class MyCartController : UIViewController
    {
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        private UserInfoEntity userInfoEntity = null;
        public bool isFromEOP = false;
        private string userId = String.Empty;
        private string token,count;
        private int viewWidth;
        public MyCartController (IntPtr handle) : base (handle)
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
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.MyQueue, exception); // exception handling 
            }
            // get the user id 
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
            }
            lblPointsPerUnit.Hidden = true;
            LblFAQClick();
            LblSupportClick();
            SetLogoSize();
            MenuClick();
            PopUpMenuClick();
            LogoutClick();
            TopNavigationClick();
            TermsAndConditionsClick();
            PrivacyPolicyClick();
            ShowCaseClick();
            HomeViewClick();
            EOPViewClick();
            RedeemViewClick();
            AboutViewClick();
            HitUserInfoAPI(userId);
            UserProfileClick();
            SetFonts();
            HideOrShowCaseView();
            UserProfileMyCartClick();
            lblMessageMyQueue.Text = AppResources.MsgMyQueue;
            lblMyMessageMyQueueTableView.Text= AppResources.MsgMyQueue;
            badgeView.BackgroundColor = UIColor.Red;
            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
            badgeView.Layer.BorderWidth = 2f;
            badgeView.Layer.BorderColor = UIColor.White.CGColor;
            try
            {
                bool isConnected = utility.CheckInternetConnection(); // check the internet connection 
                if (isConnected)
                {
                    //implement the google analytics
                    GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                    GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                    GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                    GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                    GoogleAnalytics.Current.InitTracker();
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.MyQueue);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.MyQueue, null); // exception handling 
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
                // get the terms and conditions value and remove the object of terms and conditions if value exist.
                bool isTerms = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isTermsConditions);
                if (isTerms)
                {
                    NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isTermsConditions);
                }
                MyCartViewClick();
                HitWishListItemCountAPI(); // call the Wish List Item Cout Api
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewWillAppear, AppResources.MyQueue, null); // exception handling 
            }
        }
        /// <summary>
        /// Hides the or show case view.
        /// </summary>
        public void HideOrShowCaseView()
        {
            try
            {
                var showcase = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.ShowCaseMyCart);
                ShowCaseView.Hidden = true;
                if (!String.IsNullOrEmpty(count))
                {
                    if (String.IsNullOrEmpty(showcase))
                    {
                        ShowCaseView.Hidden = false;
                        NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[0], AppResources.ShowCaseMyCart);
                    }
                    else
                    {
                        ShowCaseView.Hidden = true;
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.HideOrShowCaseView, AppResources.MyQueue, null); // exception handling 
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
                        ImgLogo.Frame = new CGRect(ImgLogo.Frame.X, ImgLogo.Frame.Y, ImgLogo.Frame.Width - Constant.digitNinteenPointsFive, ImgLogo.Frame.Height + Constant.digitEighteenPointsFive);
                        tblMyCartHeaderView.Frame = new CGRect(tblMyCartHeaderView.Frame.X, tblMyCartHeaderView.Frame.Y, tblMyCartHeaderView.Frame.Width, MyQueueHeaderView.Frame.Height+ShowMessageView.Frame.Height);
                    }
                    else
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[13], Constant.lstDigit[13]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        ImgLogo.Frame = new CGRect(ImgLogo.Frame.X, ImgLogo.Frame.Y, ImgLogo.Frame.Width - Constant.digitTwelvePointsFive, ImgLogo.Frame.Height + Constant.digitElevenPointsFive);
                        tblMyCartHeaderView.Frame = new CGRect(tblMyCartHeaderView.Frame.X, tblMyCartHeaderView.Frame.Y, tblMyCartHeaderView.Frame.Width, MyQueueHeaderView.Frame.Height + ShowMessageView.Frame.Height);
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
                            ImgLogo.Frame = new CGRect(Constant.lstDigit[6], Constant.lstDigit[41], Constant.lstDigit[42], Constant.lstDigit[43]);
                            badgeView.Frame = new CGRect(Constant.lstDigit[45], Constant.lstDigit[9], Constant.lstDigit[44], Constant.lstDigit[44]);
                            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                            badgeView.Layer.BorderWidth = 2f;
                            MyCartView.Frame = new CGRect(MyCartView.Frame.X, MyCartView.Frame.Y + Constant.lstDigit[10], MyCartView.Frame.Width, MyCartView.Frame.Height - Constant.lstDigit[10]);
                            popUpMenuMyCart.Frame = new CGRect(popUpMenuMyCart.Frame.X, popUpMenuMyCart.Frame.Y + Constant.lstDigit[10], popUpMenuMyCart.Frame.Width, popUpMenuMyCart.Frame.Height);
                            tblMyCartHeaderView.Frame = new CGRect(tblMyCartHeaderView.Frame.X, tblMyCartHeaderView.Frame.Y, tblMyCartHeaderView.Frame.Width, MyQueueHeaderView.Frame.Height + ShowMessageView.Frame.Height);
                        }
                        else
                        {
                            tblMyCartHeaderView.Frame = new CGRect(tblMyCartHeaderView.Frame.X, tblMyCartHeaderView.Frame.Y, tblMyCartHeaderView.Frame.Width, MyQueueHeaderView.Frame.Height + ShowMessageView.Frame.Height);
                        }
                    }
                    else
                    {
                        tblMyCartHeaderView.Frame = new CGRect(tblMyCartHeaderView.Frame.X, tblMyCartHeaderView.Frame.Y, tblMyCartHeaderView.Frame.Width, MyQueueHeaderView.Frame.Height + ShowMessageView.Frame.Height);
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.MyQueue, null);
            }
        }
        /// <summary>
        /// Set the fonts of UILabel, UiTextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFontsforHeader(new UILabel[] { lblMyCart, lblShopping }, new UIButton[] { btnConfirm }, Constant.lstDigit[11], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblTermsAndConditions, lblLogout, lblPrivacy, lblHome, lblEOP, lblAbout, lblRedeem, lblMessageMyQueue ,lblFaq,lblSupport, lblMyMessageMyQueueTableView }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblBadge }, null, Constant.lstDigit[6], viewWidth);
                Utility.SetFontsItalic(new UILabel[] { lblCourseName, lblCourseNameTableViewMyCart,lblPointsPerUnit,lblPointsPerUnitTableViewMyCart }, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblFullName, lblFullNameTableViewMyCart }, null, Constant.lstDigit[8], viewWidth); 
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.MyQueue, null);
            }
        }
        /// <summary>
        /// Hit the user information API.
        /// set the value of fullname,course name and balance points
        /// </summary>
        /// <param name="userId">User identifier.</param>
        protected async void HitUserInfoAPI(string userId)
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
                    userInfoEntity = await utility.GetUserInfo(userId, token);
                    if (!string.IsNullOrEmpty(userInfoEntity.userId))
                    {
                        lblFullName.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseName.Text = userInfoEntity.courseName;
                        int balance = userInfoEntity.balancedPoints;
                        string formatted = balance.ToString(AppResources.Comma);
                        lblPointsPerUnit.Text = "";//formatted + AppResources.PointsAvailable;
                        lblFullNameTableViewMyCart.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseNameTableViewMyCart.Text = userInfoEntity.courseName;
                        lblPointsPerUnitTableViewMyCart.Text = "";//formatted + AppResources.PointsAvailable;
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
                utility.SaveExceptionHandling(ex, AppResources.HitUserInfAPI, AppResources.MyQueue, null);
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
            }
        }
        /// <summary>
        /// Hit the wish list delete API.
        /// </summary>
        public async void HitWishListDeleteAPI(int wishListId)
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    MyQueueResponseEntity myQueueResponseEntity = await utility.DeleteSelectedItem(wishListId, token);
                    if(myQueueResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                    {
                        loadPop.Hide();
                        await RefreshProductList();  
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitWishListDeleteAPI, AppResources.MyQueue, null);
            }  
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Gesture implementation for My cart View to hide popup
        /// </summary>
        public void MyCartViewClick()
        {
            UITapGestureRecognizer myCartClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
            });
            MyCartView.UserInteractionEnabled = true;
            MyCartView.AddGestureRecognizer(myCartClicked);
        }
        /// <summary>
        /// Updates to queue.
        /// </summary>
        /// <param name="quantity">Quantity.</param>
        /// <param name="wishListId">Wish list identifier.</param>
        public async void UpdateToQueue(string quantity, int wishListId)
        {
            try
            {
                loadPop = new LoadingOverlay(View.Frame);
                this.View.Add(loadPop);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    List<WishListEntity> wishListEntities = await utility.GetWishListItem(userId, token);
                    if (wishListEntities!=null)
                    {
                        List<WishListEntity> tempWishListEntity = wishListEntities;
                        tempWishListEntity = tempWishListEntity.Where(x => x.wishListId == wishListId).ToList();
                        MyQueueEntity myQueueEntity = new MyQueueEntity();
                        foreach (var item in tempWishListEntity)
                        {
                            myQueueEntity.wishListId = item.wishListId;
                            myQueueEntity.quantity = Convert.ToInt32(quantity);
                            myQueueEntity.brandPoints = item.brandPoints;
                            myQueueEntity.userId = userId;
                        }
                        AddMyQueue(myQueueEntity);
                    }
                    else
                    {
                        Toast.MakeText(AppResources.ErrorRecord, Constant.durationOfToastMessage).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.UpdateToQueue, AppResources.MyQueue, null);
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
            }
        }
        /// <summary>
        /// Add to my queue.
        /// </summary>
        /// <param name="myQueueEntity">My queue entity.</param>
        protected async void AddMyQueue(MyQueueEntity myQueueEntity)
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    MyQueueResponseEntity myQueueResponseEntity = await utility.AddMyQueue(myQueueEntity, token);
                    if (myQueueResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                    {
                        HitWishListItemCountAPI();
                    }
                    else
                    {
                        Toast.MakeText(myQueueResponseEntity.operationMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.AddMyQueue, AppResources.MyQueue, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Hit the wish list item count API.
        /// // check badge value on particular user id
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
                    if(isFromEOP)
                    {
                        if (count != null)
                        {
                            if (count == Constant.lstDigitString[5])
                            {
                                badgeView.Hidden = true;
                                EmptyShoppingView.Hidden = false;
                                ShowMessageView.Hidden = false;
                                MyQueueHeaderView.Hidden = false;
                                tblMyCartProductList.Hidden = true;
                                btnConfirm.Hidden = true;
                                loadPop.Hide();
                            }
                            else
                            {
                                badgeView.Hidden = false;
                                lblBadge.Text = count;
                                EmptyShoppingView.Hidden = true;
                                ShowMessageView.Hidden = true;
                                MyQueueHeaderView.Hidden = true;
                                tblMyCartProductList.Hidden = false;
                                btnConfirm.Hidden = false;
                                MyCart();
                            }
                        }  
                    }
                    else
                    {
                        if (count != null)
                        {
                            if (count == Constant.lstDigitString[5])
                            {
                                badgeView.Hidden = true;
                                EmptyShoppingView.Hidden = false;
                                ShowMessageView.Hidden = false;
                                MyQueueHeaderView.Hidden = false;
                                Utility.DebugAlert(AppResources.Message, AppResources.MessageEmptyQueue);
                                tblMyCartProductList.Hidden = true;
                                btnConfirm.Hidden = true;
                                loadPop.Hide();
                            }
                            else
                            {
                                badgeView.Hidden = false;
                                lblBadge.Text = count;
                                EmptyShoppingView.Hidden = true;
                                ShowMessageView.Hidden = true;
                                MyQueueHeaderView.Hidden = true;
                                tblMyCartProductList.Hidden = false;
                                btnConfirm.Hidden = false;
                                MyCart();
                            }
                        }   
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.HitWishListItemCountAPI, AppResources.MyQueue, null);
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
            }
        }
        /// <summary>
		/// get the product list on local data base on my cart screen.
		/// </summary>
		public async void MyCart()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    List<WishListEntity> wishListEntities = await utility.GetWishListItem(userId, token);
                    if (wishListEntities != null)
                    {
                        List<WishListEntity> tempWishList = wishListEntities;
                        List<NSData> lstNSData = new List<NSData>();
                        NSData nsDataProductLogo = new NSData();
                        for (int i = 0; i < tempWishList.Count; i++)
                        {
                            string imgLogo = tempWishList[i].productLogo;
                            if (!String.IsNullOrEmpty(imgLogo))
                            {
                                imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                            }
                            lstNSData.Add(nsDataProductLogo);
                        }
                        TableViewMyCartProductList tableViewMyCartProductList = new TableViewMyCartProductList(this, tempWishList, lstNSData);
                        tblMyCartProductList.Source = tableViewMyCartProductList;
                        tblMyCartProductList.ReloadData();
                        tblMyCartProductList.Hidden = false;
                        btnConfirm.Hidden = false;
                        ShowMessageView.Hidden = true;
                        MyQueueHeaderView.Hidden = true;
                    }
                    else
                    {
                        Toast.MakeText(AppResources.ErrorRecord, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.MyCart, AppResources.MyQueue, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Refresh product list on my cart
        /// </summary>
        public async Task RefreshProductList()
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    List<WishListEntity> wishListEntities = await utility.GetWishListItem(userId, token);
                    if (wishListEntities != null)
                    {
                        List<WishListEntity> tempWishList = wishListEntities;
                        List<NSData> lstNSData = new List<NSData>();
                        NSData nsDataProductLogo = new NSData();
                        for (int i = 0; i < tempWishList.Count; i++)
                        {
                            string imgLogo = tempWishList[i].productLogo;
                            if (!String.IsNullOrEmpty(imgLogo))
                            {
                                imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                            }
                            lstNSData.Add(nsDataProductLogo);
                        }
                        TableViewMyCartProductList tableViewMyCartProductList = new TableViewMyCartProductList(this, tempWishList, lstNSData);
                        tblMyCartProductList.Source = tableViewMyCartProductList;
                        tblMyCartProductList.ReloadData();
                        tblMyCartProductList.Hidden = false;
                        ShowMessageView.Hidden = true;
                        MyQueueHeaderView.Hidden = true;
                    }
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    count = await utility.GetWishListItemCount(token);
                    if (count == Constant.lstDigitString[5])
                    {
                        badgeView.Hidden = true;
                        EmptyShoppingView.Hidden = false;
                        ShowMessageView.Hidden = false;
                        MyQueueHeaderView.Hidden = false;
                        tblMyCartProductList.Hidden = true;
                        btnConfirm.Hidden = true;
                    }
                    else
                    {
                        badgeView.Hidden = false;
                        lblBadge.Text = count;
                        EmptyShoppingView.Hidden = true;
                        ShowMessageView.Hidden = true;
                        MyQueueHeaderView.Hidden = true;
                        tblMyCartProductList.Hidden = false;
                        btnConfirm.Hidden = false;
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.RefreshProductList, AppResources.MyQueue, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// confirm button touch up inside.
        /// perform segue from mycart to book products screen
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnConfirm_TouchUpInside(UIButton sender)
        {
            Confirm();
        }
        /// <summary>
        /// Confirm this instance.
        /// </summary>
        public async void Confirm()
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop);
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                string isValid = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.isVaildAmount);
                if ((!Utility.isBlank) && ((isValid == "true") || (isValid == null) || (isValid == "null")))
                {
                    NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isVaildAmount);
                    this.PerformSegue(AppResources.SegueFromMyCartToBookProducts, null);
                }
                else
                {
                    Toast.MakeText(AppResources.InvalidAmount, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.Confirm, AppResources.MyQueue, null);
            } 
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Prepares for segue for my cart to book products.
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            try
            {
                if (segue.Identifier == AppResources.SegueFromMyCartToBookProducts)
                {
                    var navctlr = segue.DestinationViewController as BookProductsController;
                    if (navctlr != null)
                    {
                        navctlr.isFromMyCart = true;
                    }
                } 
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.PrepareForSegue, AppResources.MyQueue, null);
            }
        }
        /// <summary>
        /// Gesture implementation for menu click to hide or show pop up menu 
        /// </summary>
        public void MenuClick()
        {
            UITapGestureRecognizer imgMenuClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenuMyCart.Hidden == true)
                {
                    popUpMenuMyCart.Hidden = false;
                }
                else
                {
                    popUpMenuMyCart.Hidden = true;
                }
            });
            ImgMenuMyCart.UserInteractionEnabled = true;
            ImgMenuMyCart.AddGestureRecognizer(imgMenuClicked);
        }
        /// <summary>
        /// Gesture implementation for user profile to show user profile detail in my account screen
        /// </summary>
        public void UserProfileMyCartClick()
        {
            UITapGestureRecognizer userProfileMyCartClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
                this.PerformSegue(AppResources.SegueFromMyQueueToMyAccount, this);
            });
            UserInfoView.UserInteractionEnabled = true;
            UserInfoView.AddGestureRecognizer(userProfileMyCartClicked);
        }
        /// <summary>
        /// Gesture implementation for user profile to show user profile detail in my account screen
        /// </summary>
        public void UserProfileClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
                this.PerformSegue(AppResources.SegueFromMyQueueToMyAccount, this);
            });
            UserProfileView.UserInteractionEnabled = true;
            UserProfileView.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for menu click to show popup.
        /// </summary>
        public void PopUpMenuClick()
        {
            UITapGestureRecognizer popUpMenuClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = false;
            });
            popUpMenuMyCart.UserInteractionEnabled = true;
            popUpMenuMyCart.AddGestureRecognizer(popUpMenuClicked);
        }
        /// <summary>
        /// Gesture implementation for top Navigation click to hide pop up.
        /// </summary>
        public void TopNavigationClick()
        {
            UITapGestureRecognizer topNavigationClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
            });
            MyQueueHeaderView.UserInteractionEnabled = true;
            MyQueueHeaderView.AddGestureRecognizer(topNavigationClicked);
        }
        /// <summary>
        /// Gesture implementation for show case click to show instruction of the user.
        /// </summary>
        public void ShowCaseClick()
        {
            UITapGestureRecognizer showCaseClicked = new UITapGestureRecognizer(() =>
            {
                ShowCaseView.Hidden = true;
            });
            ImgShowCase.UserInteractionEnabled = true;
            ImgShowCase.AddGestureRecognizer(showCaseClicked);
        }
        /// <summary>
        /// Gesture implementation for terms and conditions click.
        /// To open web view and show privacy policy
        /// </summary>
        public void TermsAndConditionsClick()
        {
            UITapGestureRecognizer termsAndConditionsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
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
                popUpMenuMyCart.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[4], AppResources.PDFValue);
            });
            lblPrivacy.UserInteractionEnabled = true;
            lblPrivacy.AddGestureRecognizer(lblPrivacyClicked);
        }
        /// <summary>
        /// Home button on Bottom navigation
        /// </summary>
        public void HomeViewClick()
        {
            UITapGestureRecognizer homeViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
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
            HomeView.UserInteractionEnabled = true;
            HomeView.AddGestureRecognizer(homeViewClicked);
        }
        /// <summary>
        /// EOP button on Bottom navigation
        /// </summary>
        public void EOPViewClick()
        {
            UITapGestureRecognizer eopViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is BookProductsController)
                    {
                        ((BookProductsController)item).BookProductsClick();
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
            EOPView.UserInteractionEnabled = true;
            EOPView.AddGestureRecognizer(eopViewClicked);
        }
        /// <summary>
        /// About button on Bottom navigation
        /// </summary>
        public void AboutViewClick()
        {
            UITapGestureRecognizer aboutViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is AboutController)
                    {
                        ((AboutController)item).AboutClick();
                        NavigationController.PopToViewController(item, true);
                        count1++;
                        break;
                    }
                }
                if (count1 == 0)
                {
                    AboutController aboutController = (AboutController)Storyboard.InstantiateViewController(AppResources.AboutController);
                    this.NavigationController.PushViewController(aboutController, true);
                }
            });
            AboutView.UserInteractionEnabled = true;
            AboutView.AddGestureRecognizer(aboutViewClicked);
        }
        /// <summary>
        /// Redeem button on Bottom navigation
        /// </summary>
        public void RedeemViewClick()
        {
            UITapGestureRecognizer redeemViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
                int count1 = 0;
                UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                foreach (UIViewController item in uIViewControllers)
                {
                    if (item is RedeemPointsController)
                    {
                        ((RedeemPointsController)item).RedeemViewClick();
                        NavigationController.PopToViewController(item, true);
                        count1++;
                        break;
                    }
                }
                if (count1 == 0)
                {
                    RedeemPointsController redeemPointsController = (RedeemPointsController)Storyboard.InstantiateViewController(AppResources.RedeemPointsController);
                    this.NavigationController.PushViewController(redeemPointsController, true);

                }
            });
            RedeemView.UserInteractionEnabled = true;
            RedeemView.AddGestureRecognizer(redeemViewClicked);
        }
        /// <summary>
        /// Move the table view source to open keyboard.
        /// </summary>
        public void MoveScrollViewOpenKeyboard(nint index)
        {
            try
            {
               tblMyCartProductList.Frame = new CGRect(tblMyCartProductList.Frame.X, tblMyCartProductList.Frame.Y , tblMyCartProductList.Frame.Width, tblMyCartProductList.Frame.Height - Constant.lstDigit[26]);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.MoveScrollViewToOpenKeyboard, AppResources.MyQueue, null);
            }
        }
        /// <summary>
        /// Move the table view source to hide keyboard.
        /// </summary>
        /// <param name="index">Index.</param>
        public void MoveScrollViewHideKeyboard(nint index)
        {
            try
            {
                tblMyCartProductList.Frame = new CGRect(tblMyCartProductList.Frame.X, tblMyCartProductList.Frame.Y , tblMyCartProductList.Frame.Width, tblMyCartProductList.Frame.Height + Constant.lstDigit[26]);

            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.MoveScrollViewToHideKeyboard, AppResources.MyQueue, null);
            }
        }
        /// <summary>
        /// Gesture implementation for FAQ click.
        /// </summary>
        public void LblFAQClick()
        {
            UITapGestureRecognizer lblFAQClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuMyCart.Hidden = true;
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
                popUpMenuMyCart.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportClicked);
        }
        /// <summary>
        /// Gesture implementation for logout click
        ///Logout the user
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
        /// Dids the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}