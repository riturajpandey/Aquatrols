using Android.OS;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show dashboard view
    /// </summary>
    public class DashboardFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;

        /// <summary>
        /// This method is used to fix the state
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// To create default constructor in case of landscape mode
        /// </summary>
        public DashboardFragment()
        {

        }

        /// <summary>
        /// This method is used to get detail of main activity to show dashboard data
        /// </summary>
        /// <param name="mainActivity"></param>
        public DashboardFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// This method is used to initialize page load value and show user detail
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.DashboardFragmentLayout, container, false);
            try
            {
                LinearLayout llUserInfo = mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                TextView txtMessage = mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                llUserInfo.Visibility = ViewStates.Visible;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.DashboardFragment), null);
                }
            }
            return root;
        }
    }
}