using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aquatrols.iOS.Helper;
using Aquatrols.iOS.Picker;
using Aquatrols.iOS.TableVewSource;
using Aquatrols.iOS.TablevVewSource;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using Plugin.GoogleAnalytics;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.Controllers
{
    /// <summary>
    /// This class file BookProductsController is used for book products
    /// </summary>
    public partial class BookProductsController : UIViewController
    {
        private LoadingOverlay loadPop;
        private UserInfoEntity userInfoEntity = null;
        private List<DistributorInfoEntity> distributorInfoEntity = null;
        private List<ProductListEntity> productListEntities = null;
        private List<WishListEntity> wishListEntities = null;
        private Utility utility = Utility.GetInstance;
        private string token,count;
        public bool isFromMyCart = false;
        private string userId = String.Empty;
        private UIPickerView distributorPicker=new UIPickerView();
        private UIButton btnDone=new UIButton();
        private int viewWidth;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Controllers.BookProductsController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public BookProductsController(IntPtr handle) : base(handle)
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
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.Programs, exception);
            }
            // get the user id 
            if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
            {
                userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
            }
            lblPointsPerUnit.Hidden = true;
            UserProfileProductListUserClick();
            UserProfileEditGallonClick();
            UserProfileProductListClick();
            LblFAQClick();
            LblSupportClick();
            SetLogoSize();
            UserProfileClick();
            ImgHeaderClick();
            MenuClick();
            LblLogoutClick();
            ImgLblViewClick();
            ImgDistributorViewClick();
            HitDistributorsInfoAPI();
            HomeProductsClick();
            RedeemProductsClick();
            BookProductsClick();
            AboutProductsClick();
            MyCartClick();
            TermsAndConditionsClick();
            LblPrivacyPolicyClick();
            HitProductListAPI();
            DismissKeyboardClick();
            SetFonts();
            TxtDistributorNameClick();
            tblProductList.Hidden = true;
            tblProductListUser.Hidden = true;
            tblEditGallonProductList.Hidden = true;
            ChangeDistributorView.Hidden = true;
            ConfirmationView.Hidden = true;
            lblDistributorList.Text = AppResources.LabelDistributor;
            lblBookProducts.Text = AppResources.LabelEOPProducts;
            badgeView.BackgroundColor = UIColor.Red; // set back ground color in badge view 
            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];// set corner radius in badge view 
            badgeView.Layer.BorderWidth = 2f;
            badgeView.Layer.BorderColor = UIColor.White.CGColor;// set border color in badge view 
            Utility.SetPadding(new UITextField[] { txtApprovedAquatrolsDistributors}, Constant.lstDigit[4]);
            try
            {
                //implement the google analytics
                GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                GoogleAnalytics.Current.InitTracker();
                GoogleAnalytics.Current.Tracker.SendView(AppResources.Programs);
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.Programs, null); // exception handling 
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
                HitUserInfoAPI(userId); // hit the user info API 
                if (isFromMyCart)
                {
                    GetMyCartProductList(); // find the MY Cart product list 
                    isFromMyCart = false;
                }
                else
                {
                    HitWishListItemCountAPI(false, false); // call the wish list cout API
                    popUpMenuBook.Hidden = true;
                    tblProductList.Hidden = true; // hide the table product list 
                    tblEditGallonProductList.Hidden = true;
                    ChangeDistributorView.Hidden = true;
                    ConfirmationView.Hidden = true;
                    ChooseDistributorView.Hidden = false;
                    ImgHeaderView.Hidden = false;
                    lblDistributorList.Text = AppResources.LabelDistributor;
                    lblBookProducts.Text = AppResources.LabelEOPProducts;
                    txtApprovedAquatrolsDistributors.Text = String.Empty;
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewWillAppear, AppResources.Programs, null); // exception handling 
            }
        }
        /// <summary>
        /// Set the fonts of UILabel, UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetFonts(null, new UILabel[] { lblDistributor, lblBook }, new UITextField[] { txtApprovedAquatrolsDistributors }, Constant.lstDigit[9], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblBookProducts }, new UIButton[] { btnContinue, btnPlaceOrder }, Constant.lstDigit[11], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblHome, lblRedeem, lblAbout, lblTermAndConditions, lblLogout, lblPrivacy, lblDistributorList, lblDistributorListProductList,lblDistributorListUser,lblDistributorListEditGallon, lblSupport,lblFaq }, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsItalic(new UILabel[] { lblCourseName,lblCourseNameUser,lblCourseNameEditGallon,lblCourseNameProductList, lblPointsPerUnit ,lblPointsPerUnitUser,lblPointsPerUnitEditGallon,lblPointsPerUnitProductList}, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblFullName, lblFullNameProductList ,lblFullNameUser,lblFullNameEditGallon}, null, Constant.lstDigit[8], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblBadge }, null, Constant.lstDigit[6], viewWidth);

            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.Programs, null); // exception handling 
            }
        }
        /// <summary>
        /// Set Logo size sepcified in iPhone X.
        /// Set UI in iPhone X.
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                string device = UIDevice.CurrentDevice.Model; // get the device model 
                if (device == AppResources.iPad) // check the condition in iPAD and iOS devices 
                {
                    viewWidth = (int)UIScreen.MainScreen.Bounds.Width; // get the width the device 
                    int deviceHeight = (int)UIScreen.MainScreen.Bounds.Height; // get the height the device 
                    if (viewWidth == (int)DeviceScreenSize.IPadPro) // check the condition in iPAD or iPAD Pro
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[14], Constant.lstDigit[14]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        LogoBookProduct.Frame = new CGRect(LogoBookProduct.Frame.X, LogoBookProduct.Frame.Y, LogoBookProduct.Frame.Width - Constant.digitNinteenPointsFive, LogoBookProduct.Frame.Height + Constant.digitEighteenPointsFive);
                        tblViewUserInfo.Frame = new CGRect(tblViewUserInfo.Frame.X, tblViewUserInfo.Frame.Y, tblViewUserInfo.Frame.Width, ImgHeaderView.Frame.Height);
                    }
                    else
                    {
                        badgeView.Frame = new CGRect(badgeView.Frame.X, badgeView.Frame.Y, Constant.lstDigit[13], Constant.lstDigit[13]);
                        badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                        badgeView.Layer.BorderWidth = 2f;
                        LogoBookProduct.Frame = new CGRect(LogoBookProduct.Frame.X, LogoBookProduct.Frame.Y, LogoBookProduct.Frame.Width - Constant.digitTwelvePointsFive, LogoBookProduct.Frame.Height + Constant.digitElevenPointsFive);
                        tblViewUserInfo.Frame = new CGRect(tblViewUserInfo.Frame.X, tblViewUserInfo.Frame.Y, tblViewUserInfo.Frame.Width, ImgHeaderView.Frame.Height);
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
                            LogoBookProduct.Frame = new CGRect(Constant.lstDigit[6], Constant.lstDigit[41], Constant.lstDigit[42], Constant.lstDigit[43]);
                            badgeView.Frame = new CGRect(Constant.lstDigit[45], Constant.lstDigit[9], Constant.lstDigit[44], Constant.lstDigit[44]);
                            badgeView.Layer.CornerRadius = badgeView.Frame.Size.Width / Constant.lstDigit[2];
                            badgeView.Layer.BorderWidth = 2f;
                            headerView.Frame = new CGRect(headerView.Frame.X, headerView.Frame.Y + Constant.lstDigit[10], headerView.Frame.Width, headerView.Frame.Height);
                            ImgHeaderView.Frame = new CGRect(ImgHeaderView.Frame.X, headerView.Frame.Y + headerView.Frame.Height, ImgHeaderView.Frame.Width, ImgHeaderView.Frame.Height);
                            popUpMenuBook.Frame = new CGRect(popUpMenuBook.Frame.X, popUpMenuBook.Frame.Y + Constant.lstDigit[10], popUpMenuBook.Frame.Width, popUpMenuBook.Frame.Height);
                            ChooseDistributorView.Frame = new CGRect(ChooseDistributorView.Frame.X, ImgHeaderView.Frame.Y + ImgHeaderView.Frame.Height, ChooseDistributorView.Frame.Width, ChooseDistributorView.Frame.Height - Constant.lstDigit[10]);
                            tblProductListUser.Frame = new CGRect(tblProductListUser.Frame.X, headerView.Frame.Y + headerView.Frame.Height, tblProductListUser.Frame.Width, tblProductListUser.Frame.Height - Constant.lstDigit[10]);
                            ConfirmationView.Frame = new CGRect(ConfirmationView.Frame.X, ImgHeaderView.Frame.Y + ImgHeaderView.Frame.Height, ConfirmationView.Frame.Width, ConfirmationView.Frame.Height - Constant.lstDigit[10]);
                            tblEditGallonProductList.Frame = new CGRect(tblEditGallonProductList.Frame.X, headerView.Frame.Y + headerView.Frame.Height, tblEditGallonProductList.Frame.Width, tblEditGallonProductList.Frame.Height - Constant.lstDigit[10]);
                            tblProductList.Frame = new CGRect(tblProductList.Frame.X, headerView.Frame.Y + headerView.Frame.Height, tblProductList.Frame.Width, tblProductList.Frame.Height - Constant.lstDigit[10]);
                            tblViewUserInfo.Frame = new CGRect(tblViewUserInfo.Frame.X, tblViewUserInfo.Frame.Y, tblViewUserInfo.Frame.Width, ImgHeaderView.Frame.Height);
                        }
                        else
                        {
                            tblViewUserInfo.Frame = new CGRect(tblViewUserInfo.Frame.X, tblViewUserInfo.Frame.Y, tblViewUserInfo.Frame.Width, ImgHeaderView.Frame.Height);
                        }
                    }
                    else
                    {
                        tblViewUserInfo.Frame = new CGRect(tblViewUserInfo.Frame.X, tblViewUserInfo.Frame.Y, tblViewUserInfo.Frame.Width, ImgHeaderView.Frame.Height);
                    }

                } 
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.Programs, null); // exception handling 
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
                bool isConnected = utility.CheckInternetConnection(); // check the internet connection 
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    userInfoEntity = await utility.GetUserInfo(userId,token); // hit the user information API and get the user information 
                    if (!string.IsNullOrEmpty(userInfoEntity.userId))
                    {
                        lblFullName.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseName.Text = userInfoEntity.courseName;
                        int balance = userInfoEntity.balancedPoints;
                        string formatted = balance.ToString(AppResources.Comma);
                        lblPointsPerUnit.Text = "";//formatted + AppResources.PointsAvailable;
                        lblFullNameProductList.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseNameProductList.Text = userInfoEntity.courseName;
                        lblPointsPerUnitProductList.Text = "";//formatted + AppResources.PointsAvailable;
                        lblFullNameEditGallon.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseNameEditGallon.Text = userInfoEntity.courseName;
                        lblPointsPerUnitEditGallon.Text = "";//formatted + AppResources.PointsAvailable;
                        lblFullNameUser.Text = userInfoEntity.firstName + " " + userInfoEntity.lastName;
                        lblCourseNameUser.Text = userInfoEntity.courseName;
                        lblPointsPerUnitUser.Text = "";//formatted + AppResources.PointsAvailable;
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
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitUserInfAPI, AppResources.Programs, null);
            }
        }
        /// <summary>
        /// Hit distributors information API.
        /// To get distributor list according to state 
        /// </summary>
        protected async void HitDistributorsInfoAPI()
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop); // show the loader 
            try
            {
                token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token); // get the token 
                bool isConnected = utility.CheckInternetConnection(); // check the internet connection 
                if (isConnected)
                {
                    distributorInfoEntity = await utility.GetDistributorInfo(userId,token); // hit the distributor info API and get the distributor information based on user id and token 
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide(); // hide the loader 
                }
                utility.SaveExceptionHandling(ex, AppResources.HitDistributorsInfoAPI, AppResources.Programs, null); // exception handling 
            }
        }
        /// <summary>
        /// Hit product list API.
        /// to get all product list
        /// </summary>
        protected async void HitProductListAPI()
        {
            try
            { 
                bool isConnected = utility.CheckInternetConnection(); // check the internet connection 
                token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token); // get the token 
                if (isConnected)
                {
                    productListEntities = await utility.GetProductList(token); // get the product list detail hit the product list API
                    if(productListEntities!=null)
                    {
                        if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                        {
                            string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName); // get the country name
                            if (countryName.ToLower() == AppResources.canada) // check the condition based on country  
                            {
                                productListEntities = productListEntities.Where(x => x.canadaOnly.Equals(AppResources.Yes)).ToList();
                            }
                            else
                            {
                                productListEntities = productListEntities.ToList();
                            }
                            HitWishListItemCountAPI(false, false); // get the wishlist count hit the WishlistItem Count API.
                        }  
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.HitProductListAPI, AppResources.Programs, null); // exception handling.
            }
        }
        /// <summary>
        /// Hit the product checkout API
        /// manage badge value 
        /// delete item from the my cart screen if user checkout finally
        /// </summary>
        protected async void HitProductCheckoutAPI()
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop); // show the loader 
            try
            {
                bool isConnected = utility.CheckInternetConnection(); // check the in ternet connection 
                token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token); // get the token 
                if (isConnected)
                {
                    ProductCheckoutResponseEntity productCheckoutResponse = await utility.ProductCheckout(GetProductCheckoutData(),token); // hit the prodcut check out API
                    if(productCheckoutResponse.operationStatus.ToLower()==AppResources.success)
                    {
                        HitClearQueueAPI(); // hit the Clear Queue API 
                    }
                    else
                    {
                        Toast.MakeText(productCheckoutResponse.operationMessage, Constant.durationOfToastMessage).Show(); // show the toast message  
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.HitProductCheckoutAPI, AppResources.Programs, null); // exception handling 
            }
        }
        /// <summary>
        /// Hit the clear queue API.
        /// </summary>
        public async void HitClearQueueAPI()
        {
            try
            {
                token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    MyQueueResponseEntity myQueueResponseEntity = await utility.ClearQueue(userId, token); // hit the clear queue APi 
                    if(myQueueResponseEntity.operationStatus.ToLower() == AppResources.success)
                    {
                        HitWishListItemCountAPI(false,true); // hit the Wishlist count api 
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch (Exception ex)
            { 
                utility.SaveExceptionHandling(ex, AppResources.HitClearQueueAPI, AppResources.Programs, null); // exception handling 
            }  
        }
        /// <summary>
        /// Get the product checkout data.
        /// </summary>
        /// <returns>The product checkout data.</returns>
        public List<ProductCheckoutEntity> GetProductCheckoutData()
        {
            List<ProductCheckoutEntity> lstProductCheckoutEntities = new List<ProductCheckoutEntity>();
            try
            {
                if (wishListEntities != null)
                {
                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                    {
                        string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                        if (countryName.ToLower() == AppResources.canada)
                        {
                            foreach (var item in wishListEntities)
                            {
                                lstProductCheckoutEntities.Add(new ProductCheckoutEntity() { productId = item.productId,productName=item.productName, bookedBy = userId, distributorId = item.dId,distributorName=item.distributorName, quantity = Convert.ToInt32(item.quantity),brandPoints=item.brandPoints, pointsReceived = Convert.ToInt32(item.pointsReceived),userName=item.userName});
                            }
                        }
                        else
                        {
                            foreach (var item in wishListEntities)
                            {
                                lstProductCheckoutEntities.Add(new ProductCheckoutEntity() { productId = item.productId,productName = item.productName, bookedBy = userId, distributorId = item.dId, distributorName = item.distributorName, quantity = Convert.ToInt32(item.quantity),brandPoints = item.brandPoints, pointsReceived = Convert.ToInt32(item.pointsReceived), userName = item.userName});
                            }
                        }
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.ErrorRecord, Constant.durationOfToastMessage).Show(); // show the toast message 
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetProductCheckoutData, AppResources.Programs, null); // exception handling 
            }
            return lstProductCheckoutEntities; 
        }
        /// <summary>
        /// Button to continue touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnContinue_TouchUpInside(UIButton sender)
        {
            distributorPicker.Hidden = true;
            btnDone.Hidden = true;
            Continue();
        }
        /// <summary>
        /// get the Bookable product list.
        /// To show product list according to user role.
        /// </summary>
        public async void Continue()
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop); // show the laoder 
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                if (txtApprovedAquatrolsDistributors.Text.Trim().Equals(String.Empty))
                {
                    Toast.MakeText(AppResources.RequireDistributor, Constant.durationOfToastMessage).Show();
                    ChooseDistributorView.Hidden = false;
                    ImgHeaderView.Hidden = false;
                }
                else
                {
                    if (productListEntities != null  && productListEntities.Count>0)
                    {
                        if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                        {
                            string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                            if (role.ToLower().Trim() == AppResources.user)
                            {
                                Utility.DebugAlert(AppResources.Message, AppResources.AccountRole);
                                List<ProductListEntity> lstProductLists = new List<ProductListEntity>();
                                lstProductLists = productListEntities.Where(x => x.isBookable.Equals(true)).OrderBy(x => x.productName).ToList();
                                if (lstProductLists.Count>0)
                                {
                                    List<NSData> lstNSData = new List<NSData>();
                                    NSData nsDataProductLogo = new NSData();
                                    for (int i = 0; i < lstProductLists.Count; i++)
                                    {
                                        string imgLogo = lstProductLists[i].productLogo;
                                        if (!String.IsNullOrEmpty(imgLogo))
                                        {
                                            imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                            imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                            var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                            nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                        }
                                        lstNSData.Add(nsDataProductLogo);
                                    }
                                    TableViewProductListUser tableViewProductListUser = new TableViewProductListUser(this, lstProductLists, lstNSData);
                                    tblProductListUser.Source = tableViewProductListUser;
                                    tblProductListUser.ReloadData();
                                    tblProductListUser.Hidden = false;
                                    lblDistributorListUser.Text = AppResources.RequireBookingLabel;
                                    ChooseDistributorView.Hidden = true;
                                    ImgHeaderView.Hidden = true;
                                }
                                else
                                {
                                    lblDistributorListUser.Text = AppResources.RequireBookingLabelNoProducts;
                                    tblProductList.Hidden = false;
                                    ChooseDistributorView.Hidden = true;
                                    ImgHeaderView.Hidden = true;
                                    tblProductList.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                                }
                            }
                            else
                            {
                                List<ProductListEntity> lstProductLists = new List<ProductListEntity>();
                                lstProductLists = productListEntities.Where(x => x.isBookable.Equals(true)).OrderBy(x => x.productName).ToList();
                                if (lstProductLists.Count>0)
                                {
                                    List<NSData> lstNSData = new List<NSData>();
                                    NSData nsDataProductLogo = new NSData();
                                    for (int i = 0; i < lstProductLists.Count; i++)
                                    {
                                        string imgLogo = lstProductLists[i].productLogo;
                                        if (!String.IsNullOrEmpty(imgLogo))
                                        {
                                            imgLogo = imgLogo.Replace(Constant.lstDigitString[7], Constant.lstDigitString[8]);
                                            imgLogo = imgLogo.Replace(" ", AppResources.SpaceManage);
                                            var urlProductLogo = new NSUrl(ConfigEntity.baseURL + Constant.lstDigitString[9] + imgLogo);
                                            nsDataProductLogo = NSData.FromUrl(urlProductLogo);
                                        }
                                        lstNSData.Add(nsDataProductLogo);
                                    }
                                    TableViewProductList tableViewProductList = new TableViewProductList(this, lstProductLists, lstNSData);
                                    tblProductList.Source = tableViewProductList;
                                    tblProductList.ReloadData();
                                    tblProductList.Hidden = false;
                                    lblDistributorListProductList.Text = AppResources.RequireBookingLabel;
                                    ChooseDistributorView.Hidden = true;
                                    ImgHeaderView.Hidden = true;
                                }
                                else
                                {
                                    lblDistributorListProductList.Text = AppResources.RequireBookingLabelNoProducts;
                                    tblProductList.Hidden = false;
                                    ChooseDistributorView.Hidden = true;
                                    ImgHeaderView.Hidden = true;
                                    tblProductList.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                                }
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.RequireProductList, Constant.durationOfToastMessage).Show();
                        ChooseDistributorView.Hidden = false;
                        ImgHeaderView.Hidden = false;
                        tblProductList.Hidden = true;
                        tblProductList.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.Continue, AppResources.Programs, null); // exception handling 
            }
            finally
            { 
                loadPop.Hide(); // hide the loader 
            }  
        }
        /// <summary>
        /// Get product data to the specified productid.
        /// move the segue from book products to my cart screen
        /// </summary>
        /// <returns>The getdata.</returns>
        /// <param name="productid">Productid.</param>
        public void GetDataProduct(string productid,string gallons)
        {
            try
            {
                AddToQueue(productid, gallons,true); // add the value in the queue 
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetDataProduct, AppResources.Programs, null); // exception handling 
            }
        }
        /// <summary>
        /// Get the data.
        /// </summary>
        public async void GetData()
        {
            loadPop = new LoadingOverlay(this.View.Frame);
            this.View.Add(loadPop); // show the loader 
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                this.PerformSegue(AppResources.SegueFromBookProductsToMyCart, this); // perform segue to book page to my queue page 
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetData, AppResources.Programs, null); // exception handling 
            }
            finally
            {
                loadPop.Hide(); // hide the loader
            }
        }
        /// <summary>
        /// Prepares for segue.
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            try
            {
                if (segue.Identifier == AppResources.SegueFromBookProductsToMyCart)
                {
                    var navctlr = segue.DestinationViewController as MyCartController;
                    if (navctlr != null)
                    {
                        navctlr.isFromEOP = true;
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.PrepareForSegue, AppResources.Programs, null);
            }
        }
        /// <summary>
        /// Get product list on my cart screen.
        /// </summary>
        public async void GetMyCartProductList()
        {
            try
            {
                tblEditGallonProductList.Hidden = false;
                ChangeDistributorView.Hidden = false;
                tblProductList.Hidden = true;
                ChooseDistributorView.Hidden = true;
                ImgHeaderView.Hidden = true;
                lblDistributorListEditGallon.Text = AppResources.RequireReviewLabel;
                lblBookProducts.Text = AppResources.RequireCheckoutLabel;
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    wishListEntities = await utility.GetWishListItem(userId, token);
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
                        TableViewEditGallonProductList tableViewEditGallonProductList = new TableViewEditGallonProductList(this, tempWishList, lstNSData);
                        tblEditGallonProductList.Source = tableViewEditGallonProductList;
                        tblEditGallonProductList.ReloadData();
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
                HitWishListItemCountAPI(false, false);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.GetMyCartProductList, AppResources.Programs, null);
            }
        }
        /// <summary>
        /// Create table if table is not created.
        /// Insert and update Product data on sqlite database.
        /// </summary>
        /// <param name="productid">Productid.</param>
        public void AddToQueue(string productid,string quantity,bool isMyQueue)
        {
            try
            {
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName)))
                {
                    string countryName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CountryName);
                    if (countryName.ToLower() == AppResources.canada)
                    {
                        List<ProductListEntity> tempProductListEntity = productListEntities;
                        tempProductListEntity = tempProductListEntity.Where(x => x.productId == productid).ToList();
                        MyQueueEntity myQueueEntity = new MyQueueEntity();
                        foreach (var item in tempProductListEntity)
                        {
                            myQueueEntity.productId = item.productId;
                            myQueueEntity.quantity = Convert.ToInt32(quantity);
                            myQueueEntity.brandPoints = item.canadaBrandPoints;
                            myQueueEntity.distributorId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.DistributorId);
                            myQueueEntity.userId = userId;
                        }
                        AddMyQueue(myQueueEntity, isMyQueue);
                    }
                    else
                    {
                        List<ProductListEntity> tempProductListEntity = productListEntities;
                        tempProductListEntity = tempProductListEntity.Where(x => x.productId == productid).ToList();
                        MyQueueEntity myQueueEntity = new MyQueueEntity();
                        foreach (var item in tempProductListEntity)
                        {
                            myQueueEntity.productId = item.productId;
                            myQueueEntity.quantity = Convert.ToInt32(quantity);
                            myQueueEntity.brandPoints = item.brandPoints;
                            myQueueEntity.distributorId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.DistributorId);
                            myQueueEntity.userId = userId;
                        }
                        AddMyQueue(myQueueEntity, isMyQueue);
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.AddToQueue, AppResources.Programs, null);
            }
        }
        /// <summary>
        /// Add the my Queue.
        /// </summary>
        protected async void AddMyQueue(MyQueueEntity myQueueEntity,bool isMyQueue)
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                    MyQueueResponseEntity myQueueResponseEntity = await utility.AddMyQueue(myQueueEntity, token);
                    if (myQueueResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                    {
                        Toast.MakeText(myQueueResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                        HitWishListItemCountAPI(isMyQueue,false);
                    }
                    else
                    {
                        Toast.MakeText(myQueueResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
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
                utility.SaveExceptionHandling(ex, AppResources.AddMyQueue, AppResources.Programs, null);
           }
        }
        /// <summary>
        /// Hit the wish list item count API.
        /// // check badge value on particular user id
        /// </summary>
        public async void HitWishListItemCountAPI(bool isMyQueue,bool isFinalCheckout)
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
                            if (isMyQueue)
                            {
                                this.PerformSegue(AppResources.SegueFromBookProductsToMyCart, this);
                            }
                            if (isFinalCheckout)
                            {
                                ConfirmationView.Hidden = false;
                                ImgHeaderView.Hidden = false;
                                lblDistributorList.Text = AppResources.RequireConfirmationLabel;
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
                  utility.SaveExceptionHandling(ex, AppResources.HitWishListItemCountAPI, AppResources.Programs, null);
                }
                finally
                {
                    if (loadPop != null)
                    {
                        loadPop.Hide();
                    }
                }       
        }
        /// <summary>
        /// place order button touch up inside.
        /// call the productcheckout API.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void btnPlaceOrder_TouchUpInside(UIButton sender)
        {
            HitProductCheckoutAPI();
        }
        /// <summary>
        /// Gesture implementation to click on UITextField on Distributor Name
        /// Call the distributor picker method
        /// </summary>
        public void TxtDistributorNameClick()
        {
            UITapGestureRecognizer txtDistributorNameClicked = new UITapGestureRecognizer(() =>
            {
                DistributorPicker();
            });
            txtApprovedAquatrolsDistributors.UserInteractionEnabled = true;
            txtApprovedAquatrolsDistributors.AddGestureRecognizer(txtDistributorNameClicked);
        }
        /// <summary>
        /// Distributor picker.
        /// show the UIPicker
        /// bind data on distributor picker
        /// put the value of distributor id on NSUser defaults
        /// </summary>
        public void DistributorPicker()
        {
            try
            {
                if (distributorInfoEntity != null)
                {
                    distributorPicker = new UIPickerView(new CGRect(Constant.lstDigit[0],UIScreen.MainScreen.Bounds.Height - Constant.lstDigit[23], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[23]));
                    distributorPicker.BackgroundColor = UIColor.White;
                    distributorPicker.ShowSelectionIndicator = true;
                    btnDone = new UIButton(new CGRect(distributorPicker.Frame.X, distributorPicker.Frame.Y - Constant.lstDigit[15], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[15]));
                    btnDone.BackgroundColor = UIColor.Gray;
                    btnDone.SetTitle(AppResources.Done, UIControlState.Normal);
                    var picker = new DistributorModel(distributorInfoEntity);
                    distributorPicker.Model = picker;
                    View.AddSubviews(distributorPicker, btnDone);
                    picker.ValueChanged += (s, e) =>
                    {
                        txtApprovedAquatrolsDistributors.Text = picker.SelectedValue;
                        if (!string.IsNullOrEmpty(txtApprovedAquatrolsDistributors.Text))
                        {
                            string DistributorId = distributorInfoEntity.Where(x => x.distributorName == txtApprovedAquatrolsDistributors.Text).Select(x => x.distributorId).FirstOrDefault().ToString();
                            NSUserDefaults.StandardUserDefaults.SetString(DistributorId, AppResources.DistributorId);
                            NSUserDefaults.StandardUserDefaults.SetString(txtApprovedAquatrolsDistributors.Text.Trim(), AppResources.DistributorName);
                        }
                    };
                    btnDone.TouchUpInside += (s, e) =>
                    {
                        distributorPicker.Hidden = true;
                        btnDone.Hidden = true;
                    };
                    View.Add(distributorPicker);
                }
                else
                {
                    Toast.MakeText(AppResources.NoDistributors, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.DistributorPicker, AppResources.Programs, null);
            }
        }
        /// <summary>
        /// Move the product list to open keyboard.
        /// </summary>
        public void MoveScrollViewToOpenKeyboard(nint index)
        {
            try
            {
                tblProductList.Frame = new CGRect(tblProductList.Frame.X, tblProductList.Frame.Y , tblProductList.Frame.Width, tblProductList.Frame.Height - Constant.lstDigit[26]);
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.MoveScrollViewToOpenKeyboard, AppResources.Programs, null);
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
               tblProductList.Frame = new CGRect(tblProductList.Frame.X, tblProductList.Frame.Y , tblProductList.Frame.Width, tblProductList.Frame.Height + Constant.lstDigit[26]);
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.MoveScrollViewToHideKeyboard, AppResources.Programs, null);
            }
        }
        /// <summary>
        /// Gesture implementation for click on Header
        /// Header click to hide pop up menu
        /// </summary>
        public void ImgHeaderClick()
        {
            UITapGestureRecognizer imgHeaderClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
            });
            ImgHeaderView.UserInteractionEnabled = true;
            ImgHeaderView.AddGestureRecognizer(imgHeaderClicked);
        }
        /// <summary>
        /// Gesture implementation for click on User profile
        /// click user profile to show user profile detail in my account screen
        /// perform segue from book products to My Account screen
        /// </summary>
        public void UserProfileClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
                this.PerformSegue(AppResources.SegueFromBookProductsToMyAccount, this);
            });
            ImgUserProfileView.UserInteractionEnabled = true;
            ImgUserProfileView.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for click on User profile
        /// click user profile to show user profile detail in my account screen
        /// perform segue from book products to My Account screen
        /// </summary>
        public void UserProfileProductListClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
                this.PerformSegue(AppResources.SegueFromBookProductsToMyAccount, this);
            });
            UserInfoViewProductList.UserInteractionEnabled = true;
            UserInfoViewProductList.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for click on User profile
        /// click user profile to show user profile detail in my account screen
        /// perform segue from book products to My Account screen
        /// </summary>
        public void UserProfileEditGallonClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
                this.PerformSegue(AppResources.SegueFromBookProductsToMyAccount, this);
            });
            UserInfoViewEditGallon.UserInteractionEnabled = true;
            UserInfoViewEditGallon.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for click on User profile
        /// click user profile to show user profile detail in my account screen
        /// perform segue from book products to My Account screen
        /// </summary>
        public void UserProfileProductListUserClick()
        {
            UITapGestureRecognizer userProfileClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
                this.PerformSegue(AppResources.SegueFromBookProductsToMyAccount, this);
            });
            UserInfoViewUser.UserInteractionEnabled = true;
            UserInfoViewUser.AddGestureRecognizer(userProfileClicked);
        }
        /// <summary>
        /// Gesture implementation for click on Menu
        /// Menu click to hide or show popup
        /// </summary>
        public void MenuClick()
        {
            UITapGestureRecognizer menuClicked = new UITapGestureRecognizer(() =>
            {
                if (popUpMenuBook.Hidden == true)
                {
                    popUpMenuBook.Hidden = false;
                }
                else
                {
                    popUpMenuBook.Hidden = true;
                }
            });
            ImgMenuBook.UserInteractionEnabled = true;
            ImgMenuBook.AddGestureRecognizer(menuClicked);
        }
        /// <summary>
        /// Gesture implementation for click on Book Label
        /// Book label click to hide pop up menu
        /// </summary>
        public void ImgLblViewClick()
        {
            UITapGestureRecognizer imglblViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
            });
            ImgLblView.UserInteractionEnabled = true;
            ImgLblView.AddGestureRecognizer(imglblViewClicked);
        }
        /// <summary>
        /// Gesture implementation for click on Home
        /// Home button on bottom navigation
        /// </summary>
        public void HomeProductsClick()
        {
            UITapGestureRecognizer homeViewProductsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
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
            HomeViewProducts.UserInteractionEnabled = true;
            HomeViewProducts.AddGestureRecognizer(homeViewProductsClicked);
        }
        /// <summary>
        /// Gesture implementation for click on Book
        /// Book button on bottom navigation
        /// </summary>
        public void BookProductsClick()
        {
            UITapGestureRecognizer bookViewProductsClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    HitWishListItemCountAPI(false, false);
                    popUpMenuBook.Hidden = true;
                    tblProductList.Hidden = true;
                    tblEditGallonProductList.Hidden = true;
                    ChangeDistributorView.Hidden = true;
                    ConfirmationView.Hidden = true;
                    ChooseDistributorView.Hidden = false;
                    ImgHeaderView.Hidden = false;
                    lblDistributorList.Text = AppResources.LabelDistributor;
                    lblBookProducts.Text = AppResources.LabelEOPProducts;
                    txtApprovedAquatrolsDistributors.Text = String.Empty; 
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.BookProductsClick, AppResources.Programs, null);
                }
            });
            BookViewProducts.UserInteractionEnabled = true;
            BookViewProducts.AddGestureRecognizer(bookViewProductsClicked);
        }
        /// <summary>
        /// Gesture implementation for click on Redeem
        /// Redeem button on bottom navigation
        /// </summary>
        public void RedeemProductsClick()
        {
            UITapGestureRecognizer redeemViewProductsClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                    {
                        string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                        if (role.ToLower().Trim() == "manager")
                        {
                            popUpMenuBook.Hidden = true;
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
                                LpRedeemWebViewController termsConditionWebViewController = (LpRedeemWebViewController)Storyboard.InstantiateViewController("LpRedeemWebViewController");
                                this.NavigationController.PushViewController(termsConditionWebViewController, true);
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
                    Console.WriteLine(ex.Message);
                }
                                

                /*
                popUpMenuBook.Hidden = true;
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
                                    this.PerformSegue(AppResources.SegueFromBookProductsToRedeemPoints, this);
                                }
                */                
                
            });
            RedeemViewProducts.UserInteractionEnabled = true;
            RedeemViewProducts.AddGestureRecognizer(redeemViewProductsClicked);
        }
        /// <summary>
        /// Gesture implementation for click on About
        /// About button on bottom navigation
        /// </summary>
        public void AboutProductsClick()
        {
            UITapGestureRecognizer aboutViewProductsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
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
                    this.PerformSegue(AppResources.SegueFromBookProductsToAbout, this);
                }
            });
            AboutViewProducts.UserInteractionEnabled = true;
            AboutViewProducts.AddGestureRecognizer(aboutViewProductsClicked);
        }
        /// <summary>
        /// Gesture implementation for click on distributor
        /// to hide pop up menu click on distributor
        /// </summary>
        public void ImgDistributorViewClick()
        {
            UITapGestureRecognizer imgDistributorViewClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
            });
            ChooseDistributorView.UserInteractionEnabled = true;
            ChooseDistributorView.AddGestureRecognizer(imgDistributorViewClicked);
        }
        /// <summary>
        /// Gesture implementation for click on MyCart
        /// click my cart if role is user to show pop up other wise show my cart screen
        /// </summary>
        public void MyCartClick()
        {
            UITapGestureRecognizer imgMyCartClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
                try
                {
                    if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role)))
                    {
                        string role = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.role);
                        if (role.ToLower().Trim() == AppResources.user)
                        {
                            Utility.DebugAlert(AppResources.Message, AppResources.AccountRole);
                        }
                        else if (count == Constant.lstDigitString[5])
                        {
                            Utility.DebugAlert(AppResources.Message, AppResources.MessageEmptyQueue);
                        }
                        else
                        {
                            this.PerformSegue(AppResources.SegueFromBookProductsToMyCart, this);
                        }
                    }  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.MyCartClick, AppResources.Programs, null);
                }
            });
            ImgMyCart.UserInteractionEnabled = true;
            ImgMyCart.AddGestureRecognizer(imgMyCartClicked);
        }
        /// <summary>
        /// Terms and conditions click.
        /// To open web view and show terms and conditions 
        /// </summary>
        public void TermsAndConditionsClick()
        {
            UITapGestureRecognizer termsAndConditionsClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[1], AppResources.PDFValue);
            });
            lblTermAndConditions.UserInteractionEnabled = true;
            lblTermAndConditions.AddGestureRecognizer(termsAndConditionsClicked);
        }
        /// <summary>
        /// privacy policy click.
        /// To open web view and show privacy policy 
        /// </summary>
        public void LblPrivacyPolicyClick()
        {
            UITapGestureRecognizer lblPrivacyClicked = new UITapGestureRecognizer(() =>
            {
                popUpMenuBook.Hidden = true;
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
                popUpMenuBook.Hidden = true;
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
                popUpMenuBook.Hidden = true;
                TermsConditionWebViewController termsConditionWebViewController = (TermsConditionWebViewController)Storyboard.InstantiateViewController(AppResources.TermsConditionWebViewController);
                this.NavigationController.PushViewController(termsConditionWebViewController, true);
                NSUserDefaults.StandardUserDefaults.SetString(Constant.lstDigitString[6], AppResources.PDFValue);
            });
            lblSupport.UserInteractionEnabled = true;
            lblSupport.AddGestureRecognizer(lblSupportClicked);
        }
        /// <summary>
        /// Dismiss the keyboard click on Book product View.
        /// </summary>
        public void DismissKeyboardClick()
        {
            UITapGestureRecognizer dismissKeyboardClicked = new UITapGestureRecognizer(() =>
            {
                utility.DismissKeyboardOnTap(new UIView[] { BookProductsView });
            });
            BookProductsView.UserInteractionEnabled = true;
            BookProductsView.AddGestureRecognizer(dismissKeyboardClicked);
        }
        /// <summary>
        /// Logout the user
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
        /// Dids the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

