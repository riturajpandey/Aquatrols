using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    ///  This class is used for distributor screen
    /// </summary>
    [Activity]
    public class TMDistributorHomeActivity : Activity, View.IOnClickListener
    {
        private Spinner spinnerTmDistributor, spinnerTmState;
        private ImageView imgMenu, imgBack;
        private List<TmStateList> lstTmStateList;
        private List<TmDistributorList> lstTmDistributorList;
        private string token, tmState, tmDistriutor, tmDistriutorID;
        private Button btnTmDistributorEnter;

        /// <summary>
        /// This method is used to initialize page load value and call state List
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.TMDistributorHomeLayout);
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode. 
                // Handle exception handling 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }

                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                {
                    token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                }
                FindControlsById();
                await GetAllTmStateList(token);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to reset controls when back from another page
        /// </summary>
        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                spinnerTmState.SetSelection(1);
                spinnerTmDistributor.SetSelection(0);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnResume), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
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
                imgMenu = FindViewById<ImageView>(Resource.Id.imgMenu);
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                btnTmDistributorEnter = FindViewById<Button>(Resource.Id.btnTmDistributorEnter);
                spinnerTmDistributor = FindViewById<Spinner>(Resource.Id.spinnerTmDistributor);
                spinnerTmState = FindViewById<Spinner>(Resource.Id.spinnerTmState);
                spinnerTmState.ItemSelected += SpinnerTmState_ItemSelected;
                spinnerTmDistributor.ItemSelected += SpinnerTmDistributor_ItemSelected;
                btnTmDistributorEnter.SetOnClickListener(this);
                imgMenu.SetOnClickListener(this);
                imgBack.SetOnClickListener(this);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Distributor item value selected event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinnerTmDistributor_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                tmDistriutor = spinnerTmDistributor.SelectedItem.ToString();
                if (!tmDistriutor.Equals(Resources.GetString(Resource.String.selectDistributor)))
                {
                    tmDistriutorID = lstTmDistributorList.Where(x => x.distributorName.Equals(tmDistriutor)).Select(x => x.dId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SpinnerTmDistributor_ItemSelected), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// State value item selected event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinnerTmState_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                tmState = spinnerTmState.SelectedItem.ToString();
                if (!tmState.Equals(Resources.GetString(Resource.String.selectState)))
                {
                    GetAllTmDistributorList(token, tmState);
                }
                else
                {
                    List<string> tmDistributor = new List<string>();
                    tmDistributor.Insert(0, Resources.GetString(Resource.String.selectDistributor));
                    var tmDistributorAdapter = new ArrayAdapter<String>(this, Resource.Layout.Spinner_item_selectedLight, tmDistributor);
                    tmDistributorAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                    spinnerTmDistributor.Adapter = tmDistributorAdapter;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SpinnerTmState_ItemSelected), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Method implementation to bind Territory state list
        /// </summary>
        /// <param name="token"></param>
        public async Task GetAllTmStateList(string token)
        {
            Show_Overlay();
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
                        using (Utility utility = new Utility(this))
                        {
                            lstTmStateList = await utility.GetTmStateList(token);
                            if (lstTmStateList != null)
                            {
                                List<string> state = lstTmStateList.Select(x => x.regionState).ToList();
                                state.Insert(0, Resources.GetString(Resource.String.selectState));
                                var tmStateAdapter = new ArrayAdapter<String>(this, Resource.Layout.Spinner_item_selectedLight, state);
                                tmStateAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                                spinnerTmState.Adapter = tmStateAdapter;
                                spinnerTmState.SetSelection(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetAllTmStateList), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method implementation to bind Territory distributor list
        /// </summary>
        /// <param name="token"></param>
        public async void GetAllTmDistributorList(string token, string tmState)
        {
            Show_Overlay();
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
                        using (Utility utility = new Utility(this))
                        {
                            lstTmDistributorList = await utility.GetTmDistributorList(token, tmState);
                            if (lstTmDistributorList != null)
                            {
                                List<string> tmDistributor = lstTmDistributorList.Select(x => x.distributorName).ToList();
                                tmDistributor.Insert(0, Resources.GetString(Resource.String.selectDistributor));
                                var tmDistributorAdapter = new ArrayAdapter<String>(this, Resource.Layout.Spinner_item_selectedLight, tmDistributor);
                                tmDistributorAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                                spinnerTmDistributor.Adapter = tmDistributorAdapter;
                            }
                            else
                            {
                                List<string> tmDistributor = new List<string>();
                                tmDistributor.Insert(0, Resources.GetString(Resource.String.selectDistributor));
                                var tmDistributorAdapter = new ArrayAdapter<String>(this, Resource.Layout.Spinner_item_selectedLight, tmDistributor);
                                tmDistributorAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                                spinnerTmDistributor.Adapter = tmDistributorAdapter;
                                Toast.MakeText(this, Resources.GetString(Resource.String.DistNotfound), ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetAllTmDistributorList), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// This method shows an overlay dialog.
        /// </summary>
        OverlayActivity overlay;
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// On click events for controls
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.imgBack:
                        Finish();
                        break;
                    case Resource.Id.btnTmDistributorEnter:
                        if (tmState.Equals(Resources.GetString(Resource.String.selectState)))
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.selectState), ToastLength.Long).Show();
                        }
                        else if (tmDistriutor.Equals(Resources.GetString(Resource.String.selectDistributor)))
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.selectDistributor), ToastLength.Long).Show();
                        }
                        else
                        {
                            Intent intent = new Intent(this, typeof(TmCourseResultActivity));
                            intent.PutExtra(Resources.GetString(Resource.String.tmDistributorId), tmDistriutor);
                            intent.PutExtra(Resources.GetString(Resource.String.tmState), tmState);
                            StartActivity(intent);
                        }
                        break;
                    case Resource.Id.imgMenu:
                        Android.Support.V7.Widget.PopupMenu menu = new Android.Support.V7.Widget.PopupMenu(this, imgMenu);
                        menu.Inflate(Resource.Menu.OptionMenu);
                        menu.Show();
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
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.TMDistributorHomeActivity), null);
                }
            }
        }
    }
}