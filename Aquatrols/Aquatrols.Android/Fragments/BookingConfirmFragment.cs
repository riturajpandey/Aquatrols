using Android.OS;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show confirmation commit after queue
    /// </summary>
    public class BookingConfirmFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;

        /// <summary>
        /// This method is used to fix the state
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        /// <summary>
        /// This method is used to get detail of main activity to confirm queue process
        /// </summary>
        /// <param name="mainActivity"></param>
        public BookingConfirmFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// This method is used to initialize page load value and set back ground process
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.BookingConfirmLayoutFragment, container, false);
            try
            {
                TextView txtHeading = mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                TextView txtMessage = mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                RelativeLayout parentLayout = this.mainActivity.FindViewById<RelativeLayout>(Resource.Id.parentLayout);
                LinearLayout llUserInfo = mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                txtMessage.Visibility = ViewStates.Visible;
                llUserInfo.Visibility = ViewStates.Gone;
                txtHeading.Text = Resources.GetString(Resource.String.Bookingcheckout);
                txtMessage.Text = Resources.GetString(Resource.String.BookingcheckoutConfirmation);
                LinearLayout llBookConfirmation = root.FindViewById<LinearLayout>(Resource.Id.llBookConfirmation);
                parentLayout.SetBackgroundResource(Resource.Drawable.DashboardBackground);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.BookingConfirmFragment), null);
                }
            }
            return root;
        }
    }
}