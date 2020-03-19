using Android;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Webkit;
using Android.Widget;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Plugin.GoogleAnalytics;
using ShowcaseView.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class file is used to show user detail and latest version of android.
    /// </summary>
    [Activity]
    public class MainActivity : AppCompatActivity, OnViewInflateListener
    {
        const string NOTIFICATION_ACTION = Constants.receiverValue;
        NotificationBroadcastReceiver notificationBroadcastReceiver = new NotificationBroadcastReceiver();
        public TextView txtFullname, txtPoints, txtCoursename, txtuserCategory, txtHeading, txtBadgeCount;
        public ImageView imgMenu, imgRefresh, imgCart, btnTmView;
        private string token, isApproved, role, cartItem, userId, linkdata, IsTerritory;
        private List<string> liUserInfo;
        public LinearLayout rllUserInfo, llHeader, llTextMessage, llUserInfo, llTMview;
        public RelativeLayout rlHeader, parentLayout;
        private BottomNavigationView bottomBar;
        public static MainActivity Instance;
        public WebView webView;
        public MainLayoutWebViewClient mainLayoutWebViewClient;

        /// <summary>
        /// This method is used to initialize page load value and get latest version  of android and hit user info api
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Instance = this;
                // Handle exception handling 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                // Create your application here
                GetLatestVersion();
                SetContentView(Resource.Layout.MainLayout);           //setting Layout View=SignInLayout
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode.  
                CheckAppPermissions();
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                {
                    token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                }
                if (string.IsNullOrEmpty(token))
                {
                    Intent intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.isApproved), null)))
                    {
                        isApproved = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.isApproved), null);
                    }
                    if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.Role), null)))
                    {
                        role = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.Role), null);
                    }
                    if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null)))
                    {
                        userId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null);
                    }
                    if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsTerritory), null)))
                    {
                        IsTerritory = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsTerritory), null);
                    }
                    FindControlsById();
                    Utility.SettingFonts(null, new[] { txtFullname, txtPoints, txtCoursename, txtuserCategory, txtHeading }, null);
                    GetCartItemData_ByLoggedInUser();
                    txtuserCategory.Visibility = ViewStates.Gone;
                    if (!string.IsNullOrEmpty(isApproved))
                    {
                        if (!isApproved.ToLower().Equals(Resources.GetString(Resource.String.Approved)))
                        {
                            imgCart.Visibility = ViewStates.Gone;
                            bottomBar.Visibility = ViewStates.Gone;
                            imgRefresh.Visibility = ViewStates.Visible;
                            llUserInfo.Visibility = ViewStates.Gone;
                            BindInactiveFragment();
                        }
                        else
                        {
                            imgCart.Visibility = ViewStates.Visible;
                            bottomBar.Visibility = ViewStates.Visible;
                            llUserInfo.Visibility = ViewStates.Visible;
                            imgRefresh.Visibility = ViewStates.Gone;
                            BindDashboardFragment();
                        }
                    }
                    if (!string.IsNullOrEmpty(IsTerritory))
                    {
                        if (IsTerritory.Equals(Resources.GetString(Resource.String.TerritoryManager)))
                        {
                            btnTmView.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            btnTmView.Visibility = ViewStates.Gone;
                        }
                    }

                    await HitGetUserInfoAPI();
                    #region start animation for badge
                    Animator animator = ObjectAnimator.OfFloat(txtBadgeCount, "scaleX", -1f, 1f);
                    animator.SetDuration(1000);
                    animator.Start();
                    #endregion

                    //handle deep link data to redirect to a page
                    if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.LinkData), null)))
                    {
                        linkdata = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.LinkData), null);
                    }
                    HandleDeeplink();
                    #region //Google Analytics
                    GoogleAnalytics.Current.Config.TrackingId = Resources.GetString(Resource.String.TrackingId);
                    GoogleAnalytics.Current.Config.AppId = ApplicationContext.PackageName;
                    GoogleAnalytics.Current.Config.AppName = this.ApplicationInfo.LoadLabel(PackageManager).ToString();
                    GoogleAnalytics.Current.Config.AppVersion = this.PackageManager.GetPackageInfo(this.PackageName, 0).VersionName;
                    GoogleAnalytics.Current.InitTracker();
                    GoogleAnalytics.Current.Tracker.SendView("Dashboard");
                    #endregion

                    navigateToWebView("webaccount/SigninMobile?userName=" + token, "Dashboard");
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// This Method is used to get latest version of android
        /// </summary>
        /// <returns></returns>
        public async Task GetLatestVersion()
        {
            try
            {
                var url = $"https://play.google.com/store/apps/details?id=com.approach.android";
                var PlayStoreVersion = "";
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    using (var handler = new HttpClientHandler())
                    {
                        using (var client = new HttpClient(handler))
                        {
                            using (var responseMsg = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                            {
                                try
                                {
                                    var content = responseMsg.Content == null ? null : await responseMsg.Content.ReadAsStringAsync();
                                    var versionMatch = Regex.Match(content, Constants.matchContent).Groups[1];
                                    if (versionMatch.Success)
                                    {
                                        PlayStoreVersion = versionMatch.Value.Trim();
                                    }
                                }
                                catch (Exception e)
                                {
                                    using (Utility utility = new Utility(this))
                                    {
                                        utility.SaveExceptionHandling(e, Resources.GetString(Resource.String.GetLatestVersion), Resources.GetString(Resource.String.MainActivity), null);
                                    }
                                }
                            }
                        }
                    }
                }
                var CurrentVersion = this.PackageManager.GetPackageInfo(this.PackageName, 0).VersionName;
                if (Convert.ToDouble(PlayStoreVersion) > Convert.ToDouble(CurrentVersion))
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alert.SetTitle(Resources.GetString(Resource.String.newVersionmessage));
                    alert.SetMessage(Resources.GetString(Resource.String.newVersion));
                    alert.SetPositiveButton(Resources.GetString(Resource.String.Ok), (senderAlert, args) =>
                    {
                        var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse($"https://play.google.com/store/apps/details?id=com.approach.android"));
                        Application.Context.StartActivity(intent);
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    dialog.SetCancelable(false);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetLatestVersion), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// This Method is used to Check App Permissions
        /// </summary>
        private void CheckAppPermissions()
        {
            try
            {
                if ((int)Build.VERSION.SdkInt < 23)
                {
                    return;
                }
                else
                {
                    if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                        && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                    {
                        var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                        RequestPermissions(permissions, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckAppPermissions), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Handler for TM view button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTmView_Click(object sender, EventArgs e)
        {
            try
            {
                navigateToWebView("App/Dashboard/Index", "Territory Manager Dashboard");
                return;
                Intent intent = new Intent(this, typeof(TMViewHomeActivity));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BtnTmView_Click), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to handle deeplink Url to send respective screens
        /// </summary>
        /// <param name="linkdata"></param>
        public void HandleDeeplink()
        {
            try
            {
                if (linkdata != null && linkdata.ToUpper().Contains(Resources.GetString(Resource.String.book)))
                {
                    Android.Support.V4.App.Fragment fragment = new BookFragment(this);
                    llHeader.SetBackgroundResource(Resource.Drawable.BookingImage);
                    parentLayout.SetBackgroundResource(0);
                    llTextMessage.Visibility = ViewStates.Visible;
                    rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                    txtHeading.Text = Resources.GetString(Resource.String.Bookproducts);
                    SupportFragmentManager.BeginTransaction()
                    .AddToBackStack(txtHeading.Text)
                    .Replace(Resource.Id.content_frame, fragment)
                    .Commit();
                    bottomBar.Menu.GetItem(1).SetChecked(true);
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.LinkData), null).Commit();
                }
                else if (linkdata != null && linkdata.Contains(Resources.GetString(Resource.String.redeem)))
                {
                    Android.Support.V4.App.Fragment fragment = new RedeemFragment(this);
                    llHeader.SetBackgroundResource(Resource.Drawable.redeemBackground);
                    parentLayout.SetBackgroundResource(0);
                    rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                    llTextMessage.Visibility = ViewStates.Visible;
                    txtHeading.Text = Resources.GetString(Resource.String.RedeemPoints);
                    SupportFragmentManager.BeginTransaction()
                    .AddToBackStack(txtHeading.Text)
                    .Replace(Resource.Id.content_frame, fragment)
                    .Commit();
                    bottomBar.Menu.GetItem(2).SetChecked(true);
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.LinkData), null).Commit();
                }
                else if (linkdata != null && linkdata.Contains(Resources.GetString(Resource.String.Profile)))
                {
                    Intent intent = new Intent(this, typeof(MyAccountActivity));
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                    intent.PutStringArrayListExtra(Resources.GetString(Resource.String.userinfo), liUserInfo);
                    this.StartActivityForResult(intent, 10);
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.LinkData), null).Commit();
                }
                else if (linkdata != null && linkdata.Contains(Resources.GetString(Resource.String.Home)))
                {
                    BindDashboardFragment();
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.LinkData), null).Commit();
                }
                else if (linkdata != null && linkdata.Contains(Resources.GetString(Resource.String.Product)))
                {
                    Android.Support.V4.App.Fragment fragment = new AboutProductFragment(this);
                    llHeader.SetBackgroundResource(Resource.Drawable.AboutusBackground);
                    parentLayout.SetBackgroundResource(0);
                    rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                    txtHeading.Text = Resources.GetString(Resource.String.about);
                    SupportFragmentManager.BeginTransaction()
                       .Replace(Resource.Id.content_frame, fragment)
                       .Commit();
                    bottomBar.Menu.GetItem(3).SetChecked(true);
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.LinkData), null).Commit();
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HandleDeeplink), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to get CartItem count based on logged in user from database
        /// </summary>
        public async Task GetCartItemData_ByLoggedInUser()
        {
            try
            {
                using (Utility util = new Utility(this))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                    }
                    else
                    {
                        cartItem = await util.GetWishListItemCount(token);
                        if (string.IsNullOrEmpty(cartItem) || cartItem.Equals("0"))
                        {
                            txtBadgeCount.Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            txtBadgeCount.Visibility = ViewStates.Visible;
                            txtBadgeCount.Text = cartItem;
                        }
                        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.cartItem), cartItem).Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetCartItemData_ByLoggedInUser), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Loading dashboard frgment
        /// </summary>
        private void BindDashboardFragment()
        {
            try
            {
                Android.Support.V4.App.Fragment fragment = null;
                fragment = new DashboardFragment(this);
                txtHeading.Text = Resources.GetString(Resource.String.Dashboard);
                parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, fragment)
                       .Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BindDashboardFragment), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Loading Inactive user frgment
        /// </summary>
        private void BindInactiveFragment()
        {
            try
            {
                Android.Support.V4.App.Fragment fragment = null;
                fragment = new InactiveFragment(this);
                txtHeading.Text = Resources.GetString(Resource.String.Dashboard);
                parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, fragment)
                       .Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BindInactiveFragment), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Getting the reference of controls
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                txtFullname = FindViewById<TextView>(Resource.Id.txtFullname);
                txtBadgeCount = FindViewById<TextView>(Resource.Id.txtBadgeCount);
                txtCoursename = FindViewById<TextView>(Resource.Id.txtcoursename);
                txtPoints = FindViewById<TextView>(Resource.Id.txtPoints);
                txtuserCategory = FindViewById<TextView>(Resource.Id.txtusercategory);
                txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
                imgMenu = FindViewById<ImageView>(Resource.Id.imgMenu);
                imgRefresh = FindViewById<ImageView>(Resource.Id.imgRefresh);
                imgCart = FindViewById<ImageView>(Resource.Id.imgCart);
                bottomBar = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
                rllUserInfo = FindViewById<LinearLayout>(Resource.Id.rllUserInfo);
                rlHeader = FindViewById<RelativeLayout>(Resource.Id.rlHeader);
                parentLayout = FindViewById<RelativeLayout>(Resource.Id.parentLayout);
                llHeader = FindViewById<LinearLayout>(Resource.Id.llHeader);
                btnTmView = FindViewById<ImageView>(Resource.Id.btnTmView);
                llTextMessage = FindViewById<LinearLayout>(Resource.Id.llTextMessage);
                llUserInfo = FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                webView = FindViewById<WebView>(Resource.Id.webViewMainLayout);
                imgMenu.Click += ImgMenu_Click;
                imgCart.Click += ImgCart_Click;
                imgRefresh.Click += ImgRefresh_Click;
                llUserInfo.Click += llUserInfo_Click;
                btnTmView.Click += BtnTmView_Click;
                bottomBar.NavigationItemSelected += BottomBar_NavigationItemSelected;
                RemoveShiftMode(bottomBar);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised on Refresh Icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgRefresh_Click(object sender, EventArgs e)
        {
            HitSignInAPI();
        }

        /// <summary>
        ///method to Call signIn API
        /// </summary>
        public async void HitSignInAPI()
        {
            try
            {
                Show_Overlay(); //SHOWS AN OVERLAY ON SCREEN TO PREVENT THE USER INTERACTION.
                using (Utility util = new Utility(this))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        LoginRequestEntity loginEntity = new LoginRequestEntity();
                        loginEntity.username = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.Smusername), null);
                        loginEntity.password = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.password), null);
                        LoginResponseEntity loginResponseEntity = await util.UserLogin(loginEntity);
                        if (loginResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                        {
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), loginResponseEntity.userId).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), loginResponseEntity.token).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.isApproved), Convert.ToString(loginResponseEntity.isApproved)).Commit();
                            if (!loginResponseEntity.isApproved.ToLower().Equals(Resources.GetString(Resource.String.Approved)))
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.accountNotActivated), ToastLength.Long).Show();
                            }
                            else
                            {
                                overlay.Dismiss();
                                HitGetUserInfoAPI();
                                imgCart.Visibility = ViewStates.Visible;
                                bottomBar.Visibility = ViewStates.Visible;
                                llUserInfo.Visibility = ViewStates.Visible;
                                imgRefresh.Visibility = ViewStates.Gone;
                                BindDashboardFragment();
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, loginResponseEntity.operationMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitSignInAPI), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
            finally
            {
                if (overlay != null)
                    overlay.Dismiss();
            }
        }

        /// <summary>
        /// Raised on cart icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ImgCart_Click(object sender, EventArgs e)
        {
            try
            {
                navigateToWebView("App/Cart/Index", "My Queue");
                return;
                await GetCartItemData_ByLoggedInUser();
                if (string.IsNullOrEmpty(cartItem) || cartItem.Equals("0"))
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this, Android.Resource.Style.ThemeDeviceDefaultDialogAlert);
                    alert.SetTitle(Resources.GetString(Resource.String.message));
                    alert.SetMessage(Resources.GetString(Resource.String.mycartWarning));
                    alert.SetPositiveButton(Resources.GetString(Resource.String.Ok), (senderAlert, args) =>
                    {
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    Android.Support.V4.App.Fragment fragment = new MyQueueFragment(this);
                    llHeader.SetBackgroundResource(0);
                    parentLayout.SetBackgroundResource(0);
                    llTextMessage.Visibility = ViewStates.Visible;
                    rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                    txtHeading.Text = Resources.GetString(Resource.String.myqueue);
                    SupportFragmentManager.BeginTransaction()
                    .AddToBackStack(txtHeading.Text)
                   .Replace(Resource.Id.content_frame, fragment)
                   .Commit();
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ImgCart_Click), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// On Activity Result Method
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if (resultCode == Result.Ok)
                {
                    txtBadgeCount.Text = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.cartItem), null);
                    if (!string.IsNullOrEmpty(txtBadgeCount.Text))
                    {
                        txtBadgeCount.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtBadgeCount.Visibility = ViewStates.Gone;
                    }
                    if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.from), null)))
                    {
                        string res = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.from), null);
                        if (res == Resources.GetString(Resource.String.OneItem))
                        {
                            Android.Support.V4.App.Fragment fragment = new ProductCheckOutFragment(this);
                            llHeader.SetBackgroundResource(Resource.Drawable.BookingImage);
                            parentLayout.SetBackgroundResource(0);
                            llTextMessage.Visibility = ViewStates.Visible;
                            rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                            txtHeading.Text = Resources.GetString(Resource.String.Bookingcheckout);
                            SupportFragmentManager.BeginTransaction()
                            .AddToBackStack(txtHeading.Text)
                           .Replace(Resource.Id.content_frame, fragment)
                           .Commit();
                        }
                        else
                        {
                            int id = this.SupportFragmentManager.BackStackEntryCount;
                            string name = this.SupportFragmentManager.GetBackStackEntryAt(id - 1).Name;
                            if (name.Equals(Resources.GetString(Resource.String.Bookproducts)))
                            {
                                Android.Support.V4.App.Fragment fragment = new ProductListFragment(this);
                                llHeader.SetBackgroundResource(Resource.Drawable.BookingImage);
                                parentLayout.SetBackgroundResource(0);
                                llTextMessage.Visibility = ViewStates.Visible;
                                rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                                txtHeading.Text = Resources.GetString(Resource.String.Bookproducts);
                                SupportFragmentManager.BeginTransaction()
                                .AddToBackStack(txtHeading.Text)
                               .Replace(Resource.Id.content_frame, fragment)
                               .Commit();
                                bottomBar.Menu.GetItem(1).SetChecked(true);
                            }
                            else if (name.Equals(Resources.GetString(Resource.String.RedeemPoints)))
                            {
                                Android.Support.V4.App.Fragment fragment = new RedeemFragment(this);
                                llHeader.SetBackgroundResource(Resource.Drawable.redeemBackground);
                                parentLayout.SetBackgroundResource(0);
                                rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                                llTextMessage.Visibility = ViewStates.Visible;
                                txtHeading.Text = Resources.GetString(Resource.String.RedeemPoints);
                                SupportFragmentManager.BeginTransaction()
                                .AddToBackStack(txtHeading.Text)
                               .Replace(Resource.Id.content_frame, fragment)
                               .Commit();
                                bottomBar.Menu.GetItem(2).SetChecked(true);
                            }
                            else if (name.Equals(Resources.GetString(Resource.String.about)))
                            {
                                Android.Support.V4.App.Fragment fragment = new AboutFragment(this);
                                llHeader.SetBackgroundResource(Resource.Drawable.AboutusBackground);
                                parentLayout.SetBackgroundResource(0);
                                rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                                txtHeading.Text = Resources.GetString(Resource.String.about);
                                SupportFragmentManager.BeginTransaction()
                                    .Replace(Resource.Id.content_frame, fragment)
                                    .AddToBackStack(txtHeading.Text)
                                    .Commit();
                                bottomBar.Menu.GetItem(3).SetChecked(true);
                            }
                            else
                            {
                                Android.Support.V4.App.Fragment fragment = new DashboardFragment(this);
                                parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                                llTextMessage.Visibility = ViewStates.Gone;
                                llHeader.SetBackgroundResource(0);
                                rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                                txtHeading.Text = Resources.GetString(Resource.String.Dashboard);
                                parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                                SupportFragmentManager.BeginTransaction()
                                        .Replace(Resource.Id.content_frame, fragment)
                                        .AddToBackStack(txtHeading.Text)
                                       .Commit();
                                bottomBar.Menu.GetItem(0).SetChecked(true);
                            }
                        }
                        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.from), null).Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnActivityResult), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Removing the shifting style of bottom navigation bar
        /// </summary>
        /// <param name="view"></param>
        static void RemoveShiftMode(BottomNavigationView view)
        {
            BottomNavigationMenuView menuView = (BottomNavigationMenuView)view.GetChildAt(0);
            try
            {
                var shiftingMode = menuView.Class.GetDeclaredField("mShiftingMode");
                shiftingMode.Accessible = true;
                shiftingMode.SetBoolean(menuView, false);
                shiftingMode.Accessible = false;
                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    BottomNavigationItemView item = (BottomNavigationItemView)menuView.GetChildAt(i);
                    item.SetShiftingMode(false);
                    // set once again checked value, so view will be updated
                    item.SetChecked(item.ItemData.IsChecked);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Bottombar click event to show different screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BottomBar_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            try
            {
                BottomNavigationView b = sender as BottomNavigationView;
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(b.WindowToken, 0);
                Android.Support.V4.App.Fragment fragment = null;
                switch (e.Item.ToString())
                {
                    case Constants.Home:
                        navigateToWebView("App/Dashboard/Index", Resources.GetString(Resource.String.Dashboard));
                        return;
                        fragment = new DashboardFragment(this);
                        parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                        llHeader.SetBackgroundResource(0);
                        rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                        llTextMessage.Visibility = ViewStates.Gone;
                        txtHeading.Text = Resources.GetString(Resource.String.Dashboard);
                        if (IsTerritory.Equals(Resources.GetString(Resource.String.TerritoryManager)))
                        {
                            btnTmView.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            btnTmView.Visibility = ViewStates.Gone;
                        }
                        break;
                    case Constants.Programs:
                        navigateToWebView("App/Program/Index", Resources.GetString(Resource.String.book));
                        return;
                        fragment = new BookFragment(this);
                        llHeader.SetBackgroundResource(Resource.Drawable.BookingImage);
                        parentLayout.SetBackgroundResource(0);
                        llTextMessage.Visibility = ViewStates.Visible;
                        rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                        txtHeading.Text = Resources.GetString(Resource.String.book);
                        break;
                    case Constants.Redeem:
                        {
                            bool equal = String.Equals(role, "Manager", StringComparison.InvariantCulture);
                            if (equal)
                            {
                                Intent intent = new Intent(this, typeof(LpRedeemWebViewActivity));
                                StartActivity(intent);
                            }
                            else
                            {
                                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                                //alertDiag.SetTitle("Title");
                                alertDiag.SetMessage("Only managers can redeem for points");
                                alertDiag.SetPositiveButton("OK", (c, ev) =>
                                {
                                    // Ok button click task  
                                });
                                alertDiag.Show();
                            }
                            break;

                        }
                        break;
                    case Constants.About:
                        navigateToWebView("about", Resources.GetString(Resource.String.about));
                        return;
                        fragment = new AboutFragment(this);
                        llHeader.SetBackgroundResource(Resource.Drawable.AboutusBackground);
                        parentLayout.SetBackgroundResource(0);
                        rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                        txtHeading.Text = Resources.GetString(Resource.String.about);
                        break;
                    default:
                        break;
                }
                if (fragment != null)
                {
                    SupportFragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, fragment)
                        .AddToBackStack(txtHeading.Text)
                        .Commit();
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BottomBar_NavigationItemSelected), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised on Menu icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Android.Support.V7.Widget.PopupMenu menu = new Android.Support.V7.Widget.PopupMenu(this, imgMenu);
                menu.Inflate(Resource.Menu.OptionMenu);
                menu.Show();
                Android.Support.V4.App.Fragment fragment = null;
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
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerritory), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.Role), null).Commit();
                            StartActivity(intent);
                            Finish();
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ImgMenu_Click), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Calling userinfo API to get user detail
        /// </summary>
        public async Task HitGetUserInfoAPI()
        {
            try
            {
                Show_Overlay(); //SHOWS AN OVERLAY ON SCREEN TO PREVENT THE USER INTERACTION.
                using (Utility util = new Utility(this))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null) != null)
                        {
                            string userId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null);
                            UserInfoEntity userInfoEntity = await util.GetUserInfo(userId, token);
                            liUserInfo = new List<string>();

                            if (userInfoEntity != null)
                            {
                                txtFullname.Text = Convert.ToString(userInfoEntity.firstName + " " + userInfoEntity.lastName);
                                txtCoursename.Text = userInfoEntity.courseName;
                                //txtPoints.Text = userInfoEntity.balancedPoints.ToString(Resources.GetString(Resource.String.CommaFormat)) + " " + Resources.GetString(Resource.String.pointsavailable);
                                txtPoints.Text = string.Empty;
                                liUserInfo.Add(txtFullname.Text);
                                liUserInfo.Add(txtCoursename.Text);
                                liUserInfo.Add(userInfoEntity.statusName);
                                liUserInfo.Add(userInfoEntity.role);
                                liUserInfo.Add(userInfoEntity.earnedPoints.ToString(Resources.GetString(Resource.String.CommaFormat)));
                                liUserInfo.Add(userInfoEntity.pointsSpent.ToString(Resources.GetString(Resource.String.CommaFormat)));
                                liUserInfo.Add(userInfoEntity.balancedPoints.ToString(Resources.GetString(Resource.String.CommaFormat)));
                                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.country), userInfoEntity.country).Commit();
                                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.BalancePoint), userInfoEntity.balancedPoints.ToString()).Commit();
                                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsNotificationChecked), userInfoEntity.isNotification.ToString()).Commit();
                                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsEmailChecked), userInfoEntity.isEmailPreference.ToString()).Commit();
                                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.tmCourseId), userInfoEntity.courseId).Commit();
                                string lpPoints = await util.GetLoyaltyPoints(userInfoEntity.MembershipId);
                                if (!string.IsNullOrWhiteSpace(lpPoints))
                                    txtPoints.Text = "Points: " + lpPoints;
                                SendDeviceTokenToServer();//sending device token to server for push notification
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, Resource.String.NoRecord, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitGetUserInfoAPI), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Sends the Device token to server for push notification
        /// </summary>
        public async void SendDeviceTokenToServer()
        {
            try
            {
                using (Utility util = new Utility(this))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.deviceToken), null) != null)
                        {
                            string deviceToken = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.deviceToken), null);
                            PushDataModelEntity pushDataModelEntity = new PushDataModelEntity()
                            {
                                deviceToken = deviceToken,
                                region = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.country), null),
                                deviceType = Resources.GetString(Resource.String.Devicetype),
                                userId = userId
                            };
                            PushDataModelResponseEntity responseEntity = await util.SendDeviceToken(pushDataModelEntity, token);
                            if (responseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                            {
                                Console.Write(responseEntity.operationMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SendDeviceTokenToServer), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// Userinfo box click event to go myaccount screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void llUserInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(this, typeof(MyAccountActivity));
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                intent.PutStringArrayListExtra(Resources.GetString(Resource.String.userinfo), liUserInfo);
                this.StartActivityForResult(intent, 10);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.llUserInfo_Click), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// showing a waiting cursor
        /// </summary>
        public OverlayActivity overlay;
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// On device back button click
        /// </summary> 
        public override void OnBackPressed()
        {
            try
            {
                var webBackForwardList = webView.CopyBackForwardList();
                if (webBackForwardList.CurrentIndex > 1 && txtHeading.Text.ToString() != "Dashboard") // if previous url instead of login url, then go back
                    webView.GoBack();
                else if (webBackForwardList.CurrentIndex == 1 && txtHeading.Text.ToString() != "Dashboard") // if no previous url and not on dashboard page, go to dashboard page
                {
                    webView.Visibility = ViewStates.Invisible;
                    txtHeading.Text = "Dashboard";
                }
                else // show exit app popup if already on dashboard page 
                {
                    Android.Support.V4.App.Fragment fragment = this.SupportFragmentManager.FindFragmentById(Resource.Id.content_frame);
                    if (fragment is DashboardFragment || fragment is InactiveFragment)
                    {
                        Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this, Android.Resource.Style.ThemeDeviceDefaultDialogAlert);
                        alert.SetTitle(Resources.GetString(Resource.String.confirm));
                        alert.SetMessage(Resources.GetString(Resource.String.confirmationMessage));
                        alert.SetPositiveButton(Resources.GetString(Resource.String.Ok), (senderAlert, args) =>
                        {
                            Finish();
                        });
                        alert.SetNegativeButton(Resources.GetString(Resource.String.cancel), (senderAlert, args) =>
                        {

                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                    else
                    {
                        fragment = new DashboardFragment(this);
                        parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                        if (IsTerritory.Equals(Resources.GetString(Resource.String.TerritoryManager)))
                        {
                            btnTmView.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            btnTmView.Visibility = ViewStates.Gone;
                        }
                        llTextMessage.Visibility = ViewStates.Gone;
                        llHeader.SetBackgroundResource(0);
                        rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                        txtHeading.Text = Resources.GetString(Resource.String.Dashboard);
                        parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                        SupportFragmentManager.BeginTransaction()
                                .Replace(Resource.Id.content_frame, fragment)
                                .AddToBackStack(txtHeading.Text)
                               .Commit();
                        bottomBar.Menu.GetItem(0).SetChecked(true);
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnBackPressed), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// System On Start method
        /// </summary>
        protected override void OnStart()
        {
            try
            {
                base.OnStart();
                IntentFilter intentFilter = new IntentFilter(NOTIFICATION_ACTION);
                RegisterReceiver(notificationBroadcastReceiver, intentFilter);
                // Set the application state
                //ApplicationStateVerification.ActivityResumed();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnStart), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        /// <summary>
        /// System On Stop method
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                base.OnStop();
                UnregisterReceiver(notificationBroadcastReceiver);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnStop), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        public void OnViewInflated(View view)
        {

        }

        private void navigateToWebView(string url, string header)
        {
            try
            {

                if (mainLayoutWebViewClient == null)
                {
                    mainLayoutWebViewClient = new MainLayoutWebViewClient(this);

                    string userName = EncryptString(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.Smusername), null)).Replace("+", "%2B");
                    string passWord = EncryptString(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.password), null)).Replace("+", "%2B");
                    url = ConfigEntity.baseURL + "webaccount/SigninMobile?userName=" + userName + "&password=" + passWord;

                    webView.SetWebViewClient(mainLayoutWebViewClient);
                    webView.Settings.JavaScriptEnabled = true;
                    webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                    webView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;
                    webView.Settings.DomStorageEnabled = true;
                    webView.Visibility = ViewStates.Invisible;
                    webView.LoadUrl(url);
                    return;
                }

                txtHeading.Text = header;
                if (header == "Dashboard")
                {
                    webView.Visibility = ViewStates.Invisible;
                    return;
                }

                url = ConfigEntity.baseURL + url;

                if (string.Equals(header, Resources.GetString(Resource.String.about)))
                    url = "https://aquatrols.com/about/";

                webView.SetWebViewClient(mainLayoutWebViewClient);
                webView.Visibility = ViewStates.Visible;
                webView.Settings.JavaScriptEnabled = true;
                webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                webView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;
                webView.Settings.DomStorageEnabled = true;
                Show_Overlay();
                webView.LoadUrl(url);
            }
            catch (Exception e)
            {

            }
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
    }

    public class MainLayoutWebViewClient : WebViewClient
    {
        OverlayActivity overlay;
        MainActivity activity;

        /// <summary>
        /// This method is to prevent user to move in side the app with out click on i agree or not
        /// </summary>
        /// <param name="activity"></param>
        public MainLayoutWebViewClient(MainActivity activity)
        {
            this.activity = activity;
            try
            {
                overlay = this.activity.overlay;
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(activity))
                {
                    utility.SaveExceptionHandling(ex, activity.Resources.GetString(Resource.String.HelloWebViewClient), "LoggedInUserWebView", null);
                }
            }
        }

        /// <summary>
        /// This Method is for loading when data change
        /// </summary>
        /// <param name="view"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl(request.Url.ToString());
            return false;
        }

        /// <summary>
        /// Invokes On load finished of webview
        /// </summary>
        /// <param name="view"></param>
        /// <param name="url"></param>
        public override void OnPageFinished(WebView view, string url)
        {
            try
            {
                overlay = this.activity.overlay;
                base.OnPageFinished(view, url);
                overlay.Dismiss();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(activity))
                {
                    utility.SaveExceptionHandling(ex, activity.Resources.GetString(Resource.String.OnPageFinished), activity.Resources.GetString(Resource.String.TermsWebviewActivity), null);
                }
            }
        }
    }
}