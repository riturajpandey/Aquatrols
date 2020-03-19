using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aquatrols.Droid.Fragments
{

    /// <summary>
    /// This class is used to show redeem detail
    /// </summary>
    public class RedeemFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView lvGiftCardList;
        private LinearLayoutManager layoutmngr;
        private GiftCardAdapter adapter;
        private MainActivity mainActivity;
        private string point, country;

        /// <summary>
        /// This method is used to initialize page load value
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        /// <summary>
        /// This method is used to get detail of main activity to redeem detail
        /// </summary>
        /// <param name="mainActivity"></param>
        public RedeemFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;

            #region //Google Analytics
            GoogleAnalytics.Current.Config.TrackingId = this.mainActivity.Resources.GetString(Resource.String.TrackingId);
            GoogleAnalytics.Current.Config.AppId = this.mainActivity.ApplicationContext.PackageName;
            GoogleAnalytics.Current.Config.AppName = this.mainActivity.ApplicationInfo.LoadLabel(this.mainActivity.PackageManager).ToString();
            GoogleAnalytics.Current.Config.AppVersion = this.mainActivity.PackageManager.GetPackageInfo(this.mainActivity.PackageName, 0).VersionName;
            GoogleAnalytics.Current.InitTracker();
            GoogleAnalytics.Current.Tracker.SendView("Redeem");
            #endregion
        }

        /// <summary>
        /// This method is used to initialize page load value and confirm redeem value
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.RedeemLayoutFragment, container, false);
            try
            {
                point = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.BalancePoint), null);
                country = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.country), null);
                TextView txtHeading = this.mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                TextView txtMessage = this.mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                ImageView imgTmView = mainActivity.FindViewById<ImageView>(Resource.Id.btnTmView);
                imgTmView.Visibility = ViewStates.Gone;
                txtMessage.Visibility = ViewStates.Visible;
                LinearLayout llUserInfo = this.mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                LinearLayout llHeader = this.mainActivity.FindViewById<LinearLayout>(Resource.Id.llHeader);
                if (llHeader.Visibility == ViewStates.Gone)
                {
                    llHeader.Visibility = ViewStates.Visible;
                }
                llUserInfo.Visibility = ViewStates.Visible;
                txtHeading.Text = Resources.GetString(Resource.String.RedeemPoints);
                txtMessage.Text = Resources.GetString(Resource.String.RedeemPointsMessage);
                lvGiftCardList = root.FindViewById<RecyclerView>(Resource.Id.lvGiftCardList);
                layoutmngr = new LinearLayoutManager(this.mainActivity);
                lvGiftCardList.SetLayoutManager(layoutmngr);
                HitRewardItemListAPI();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.RedeemFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// Method to get All available Gift Items
        /// </summary>
        public async void HitRewardItemListAPI()
        {
            Show_Overlay();
            try
            {
                using (Utility util = new Utility(this.mainActivity))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this.mainActivity, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                        {
                            string token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                            List<RedeemGiftCardEntity> redeemGiftCardEntities = await util.GetRewardItem(token);
                            if (country.ToLower().Trim().Equals(Resources.GetString(Resource.String.canada)))
                            {
                                redeemGiftCardEntities = redeemGiftCardEntities.Where(x => x.isAvailableCanada).ToList();
                                adapter = new GiftCardAdapter(this.mainActivity, redeemGiftCardEntities);
                                lvGiftCardList.SetAdapter(adapter);
                            }
                            else
                            {
                                redeemGiftCardEntities = redeemGiftCardEntities.Where(x => x.isAvailableUs).ToList();
                                adapter = new GiftCardAdapter(this.mainActivity, redeemGiftCardEntities);
                                lvGiftCardList.SetAdapter(adapter);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitRewardItemListAPI), Resources.GetString(Resource.String.RedeemFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        OverlayActivity overlay;
        /// <summary>
        /// Showing overlay screen
        /// </summary>
        public void Show_Overlay()
        {
            try
            {
                overlay = new OverlayActivity(this.mainActivity);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.RedeemFragment), null);
                }
            }
        }

    }
}