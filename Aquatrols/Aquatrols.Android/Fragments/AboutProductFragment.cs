using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show product layout in about screen
    /// </summary>
    public class AboutProductFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;
        private RecyclerView lvProductsIcon;
        private RecyclerView.LayoutManager layoutmngr;
        private AboutProductListAdapter Aboutadapter;
        private List<ProductListEntity> productListEntities = null;

        /// <summary>
        /// This method is used to get detail of main activity to add about detail
        /// </summary>
        /// <param name="mainActivity"></param>
        public AboutProductFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// This method is used to fix the state
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HitProductListAPI();
            // Create your fragment here
        }

        /// <summary>
        /// This method is used to initialize page load value and set drop down value data
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.AboutProductLayoutFragment, container, false);
            try
            {
                LinearLayout llTextMessage = this.mainActivity.FindViewById<LinearLayout>(Resource.Id.llTextMessage);
                TextView txtHeading = this.mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                txtHeading.Text = Resources.GetString(Resource.String.allproduct);
                llTextMessage.Visibility = ViewStates.Gone;
                Spinner spinnerMarket = root.FindViewById<Spinner>(Resource.Id.spinnerMarket);
                lvProductsIcon = root.FindViewById<RecyclerView>(Resource.Id.lvProductsIcon);
                layoutmngr = new LinearLayoutManager(this.mainActivity);
                lvProductsIcon.SetLayoutManager(layoutmngr);
                spinnerMarket.ItemSelected += SpinnerMarket_ItemSelected;
                ArrayAdapter adapter = new ArrayAdapter<String>(this.mainActivity, Resource.Layout.Spinner_item_selectedLight, Constants.liMarket);
                adapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                spinnerMarket.Adapter = adapter;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.AboutProductFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// On market dropdown value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinnerMarket_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                Show_Overlay();
                Spinner spinner = (Spinner)sender;
                string marketValue = spinner.GetItemAtPosition(e.Position).ToString();
                if (productListEntities != null)
                {
                    if (marketValue.Equals(Resources.GetString(Resource.String.turf)))
                    {
                        List<ProductListEntity> tempProductListEntities = productListEntities;
                        tempProductListEntities = tempProductListEntities.Where(x => x.categoryName.Trim() == Resources.GetString(Resource.String.turf)).ToList();
                        Aboutadapter = new AboutProductListAdapter(this.mainActivity, tempProductListEntities.OrderBy(x => x.productName).ToList());
                        lvProductsIcon.SetAdapter(Aboutadapter);
                    }
                    else if (marketValue.Equals(Resources.GetString(Resource.String.horticulture)))
                    {
                        List<ProductListEntity> tempProductListEntities = productListEntities;
                        tempProductListEntities = tempProductListEntities.Where(x => x.categoryName.Trim() == Resources.GetString(Resource.String.horticulture)).ToList();
                        Aboutadapter = new AboutProductListAdapter(this.mainActivity, tempProductListEntities.OrderBy(x => x.productName).ToList());
                        lvProductsIcon.SetAdapter(Aboutadapter);
                    }
                    else if (marketValue.Equals(Resources.GetString(Resource.String.agriculture)))
                    {
                        List<ProductListEntity> tempProductListEntities = productListEntities;
                        tempProductListEntities = tempProductListEntities.Where(x => x.categoryName.Trim() == Resources.GetString(Resource.String.agriculture)).ToList();
                        Aboutadapter = new AboutProductListAdapter(this.mainActivity, tempProductListEntities.OrderBy(x => x.productName).ToList());
                        lvProductsIcon.SetAdapter(Aboutadapter);
                    }
                    else
                    {
                        Aboutadapter = new AboutProductListAdapter(this.mainActivity, productListEntities.OrderBy(x => x.productName).ToList());
                        lvProductsIcon.SetAdapter(Aboutadapter);
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SpinnerMarket_ItemSelected), Resources.GetString(Resource.String.AboutProductFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Showing overlay screen
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.AboutProductFragment), null);
                }
            }
        }

        /// <summary>
        /// Calling product list API
        /// </summary>
        public async void HitProductListAPI()
        {
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
                            productListEntities = await util.GetAllProducts(token);
                            if (productListEntities == null && productListEntities.Count == 0)
                            {
                                Toast.MakeText(this.mainActivity, Resource.String.NoRecord, ToastLength.Long).Show();
                            }
                            else
                            {
                                Aboutadapter = new AboutProductListAdapter(this.mainActivity, productListEntities.OrderBy(x => x.productName).ToList());
                                lvProductsIcon.SetAdapter(Aboutadapter);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitProductListAPI ), Resources.GetString(Resource.String.AboutProductFragment), null);
                }
            }
        }
    }
}