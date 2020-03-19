using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show confirmation of redeem value
    /// </summary>
    public class RedeemConfirmFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;

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
        public RedeemConfirmFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
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
            View root = inflater.Inflate(Resource.Layout.RedeemConfirmLayoutFragment, container, false);
            try
            {
                TextView txtHeading = this.mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                TextView txtMessage = this.mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                RelativeLayout parentLayout = this.mainActivity.FindViewById<RelativeLayout>(Resource.Id.parentLayout);
                BottomNavigationView bottomBar = this.mainActivity.FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
                LinearLayout llUserInfo = this.mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                txtMessage.Visibility = ViewStates.Visible;
                llUserInfo.Visibility = ViewStates.Visible;
                txtHeading.Text = Resources.GetString(Resource.String.RedeemPointsCheckout);
                txtMessage.Text = Resources.GetString(Resource.String.PointsCheckoutConfirmation);
                LinearLayout RedeemCheckoutConfirmation = root.FindViewById<LinearLayout>(Resource.Id.RedeemCheckoutConfirmation);
                parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
                HitUserInfo();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.RedeemConfirmFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// This method is used to hit User info api
        /// </summary>
        public async void HitUserInfo()
        {
            await this.mainActivity.HitGetUserInfoAPI();
        }
    }
}