using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show distributor deatil in program screen
    /// </summary>
    public class BookFragment : Android.Support.V4.App.Fragment, View.IOnClickListener
    {
        private Spinner spinnerDistributor;
        private Button btnContinue;
        private string Distributor;
        private MainActivity mainActivity;
        private List<DistributorInfoEntity> lstdistributorInfoEntities;

        /// <summary>
        /// this method is used to get detail of main activity and implement google analytics
        /// </summary>
        /// <param name="context"></param>
        public BookFragment(MainActivity context)
        {
            try
            {
                this.mainActivity = context;
                #region //Google Analytics
                GoogleAnalytics.Current.Config.TrackingId = this.mainActivity.Resources.GetString(Resource.String.TrackingId);
                GoogleAnalytics.Current.Config.AppId = this.mainActivity.ApplicationContext.PackageName;
                GoogleAnalytics.Current.Config.AppName = this.mainActivity.ApplicationInfo.LoadLabel(this.mainActivity.PackageManager).ToString();
                GoogleAnalytics.Current.Config.AppVersion = this.mainActivity.PackageManager.GetPackageInfo(this.mainActivity.PackageName, 0).VersionName;
                GoogleAnalytics.Current.InitTracker();
                GoogleAnalytics.Current.Tracker.SendView("EOP Products");
                #endregion
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BookFragment), Resources.GetString(Resource.String.BookFragment), null);
                }
            }
        }

        /// <summary>
        /// This method is used to fix the state
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// This method is used to initialize page load value and Hit Distributor API
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.BookLayoutFragment, container, false);
            try
            {
                TextView txtHeading = mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                TextView txtMessage = mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                LinearLayout lluserInfo = mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                ImageView imgTmView = mainActivity.FindViewById<ImageView>(Resource.Id.btnTmView);
                imgTmView.Visibility = ViewStates.Gone;
                txtMessage.Visibility = ViewStates.Visible;
                lluserInfo.Visibility = ViewStates.Visible;
                btnContinue = root.FindViewById<Button>(Resource.Id.btnContinue);
                spinnerDistributor = root.FindViewById<Spinner>(Resource.Id.spinnerDistributor);
                HitDistributorAPI();
                txtMessage.Text = Resources.GetString(Resource.String.BookingDistributormessage);

                #region Event handlers
                btnContinue.SetOnClickListener(this);
                spinnerDistributor.ItemSelected += SpinnerDistributor_ItemSelected;
                #endregion
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.BookFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// Showing wait cursor 
        /// </summary>
        OverlayActivity overlay;
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.BookFragment), null);
                }
            }
        }

        /// <summary>
        /// Raised when Spinner distributor selected index changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinnerDistributor_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                Spinner spinner = (Spinner)sender;
                string distributorName = spinner.GetItemAtPosition(e.Position).ToString();
                if (!distributorName.Equals(Resources.GetString(Resource.String.selectDistributor)))
                {
                    Distributor = lstdistributorInfoEntities.Where(x => x.distributorName == distributorName).Select(x => x.distributorId).FirstOrDefault();
                }
                else
                {
                    Distributor = string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SpinnerDistributor_ItemSelected), Resources.GetString(Resource.String.BookFragment), null);
                }
            }
        }

        /// <summary>
        /// Calling distributor API to bind distributor list
        /// </summary>
        public async void HitDistributorAPI()
        {
            try
            {
                using (Utility util = new Utility(this.mainActivity))
                {
                    Show_Overlay();
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this.mainActivity, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        using (Utility utility = new Utility(this.mainActivity))
                        {
                            string token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                            string userId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null);
                            lstdistributorInfoEntities = await utility.GetDistributorList(userId, token);
                            List<string> lstdistributors = null;
                            if (lstdistributorInfoEntities != null && lstdistributorInfoEntities.Count > 0)
                            {
                                lstdistributors = lstdistributorInfoEntities.Select(x => x.distributorName).ToList();
                                lstdistributors.Insert(0, Resources.GetString(Resource.String.selectDistributor));
                                var stateAdapter = new ArrayAdapter<String>(this.mainActivity, Resource.Layout.Spinner_item_selectedLight, lstdistributors);
                                stateAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                                spinnerDistributor.Adapter = stateAdapter;
                            }
                            else
                            {
                                lstdistributors = new List<string>();
                                lstdistributors.Add(GetString(Resource.String.selectDistributor));
                                var stateAdapter = new ArrayAdapter<String>(this.mainActivity, Resource.Layout.Spinner_item_selectedLight, lstdistributors);
                                stateAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                                spinnerDistributor.Adapter = stateAdapter;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitDistributorAPI), Resources.GetString(Resource.String.BookFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Continue button click event
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                Android.Support.V4.App.Fragment fragment = null;
                switch (v.Id)
                {
                    case Resource.Id.btnContinue:
                        if (!string.IsNullOrEmpty(Distributor))
                        {
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.distributorId), Distributor).Commit();
                            string distributorName = lstdistributorInfoEntities.Where(x => x.distributorId == Distributor).Select(x => x.distributorName).FirstOrDefault();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.distributorName), distributorName).Commit();
                            fragment = new ProductListFragment(this.mainActivity);
                            FragmentManager.BeginTransaction()
                           .Replace(Resource.Id.content_frame, fragment)
                           .Commit();
                        }
                        else
                        {
                            Toast.MakeText(this.mainActivity, Resources.GetString(Resource.String.selectDistributor), ToastLength.Long).Show();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.BookFragment), null);
                }
            }
        }
    }
}