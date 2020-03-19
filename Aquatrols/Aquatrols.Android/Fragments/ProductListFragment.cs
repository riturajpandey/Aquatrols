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
    /// This class is used to show Product detail in case of multipile record
    /// </summary>
    public class ProductListFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView recyclerProductlist;
        private LinearLayoutManager layoutmngr;
        private ProductListAdapter adapter;
        private MainActivity mainActivity;
        private RelativeLayout parentLayout;
        private string country = string.Empty;
        private LinearLayout llHeader;
        private TextView txtMessage;
        private List<ProductListEntity> productListEntities = null;

        /// <summary>
        /// This method is used to initialize page load value
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HitProductlistAPI();
        }

        /// <summary>
        /// This method is used to get detail of main activity to product detail
        /// </summary>
        /// <param name="mainActivity"></param>
        public ProductListFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// This method is used to initialize page load value and set lay out of recycler Product list
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.ProductLayoutFragment, container, false);
            try
            {
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.country), null)))
                {
                    country = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.country), null);
                }
                TextView txtHeading = this.mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                txtMessage = this.mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                parentLayout = this.mainActivity.FindViewById<RelativeLayout>(Resource.Id.parentLayout);
                llHeader = this.mainActivity.FindViewById<LinearLayout>(Resource.Id.llHeader);
                txtHeading.Text = Resources.GetString(Resource.String.book);
                recyclerProductlist = root.FindViewById<RecyclerView>(Resource.Id.lvProductList);
                layoutmngr = new LinearLayoutManager(this.mainActivity);
                recyclerProductlist.SetLayoutManager(layoutmngr);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.ProductListFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// Method to call product List API
        /// </summary>
        public async void HitProductlistAPI()
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
                            productListEntities = await util.GetAllProducts(token);
                            if (productListEntities == null || productListEntities.Count == 0)
                            {
                                txtMessage.Text = Resources.GetString(Resource.String.NoProductBookMessage);
                                Toast.MakeText(this.mainActivity, Resource.String.NoRecord, ToastLength.Long).Show();
                            }
                            else
                            {
                                txtMessage.Text = Resources.GetString(Resource.String.BookProductMessage);
                                if (!string.IsNullOrEmpty(country))
                                {
                                    if (country.ToLower().Equals(Resources.GetString(Resource.String.canada)))
                                    {
                                        productListEntities = productListEntities.Where(x => x.isBookable == true && x.canadaOnly.ToLower() == Resources.GetString(Resource.String.yes)).OrderBy(x => x.productName).ToList();
                                        adapter = new ProductListAdapter(this.mainActivity, productListEntities);
                                        recyclerProductlist.SetAdapter(adapter);
                                    }
                                    else
                                    {
                                        productListEntities = productListEntities.Where(x => x.isBookable == true).OrderBy(x => x.productName).ToList();
                                        adapter = new ProductListAdapter(this.mainActivity, productListEntities);
                                        recyclerProductlist.SetAdapter(adapter);
                                    }
                                    if (productListEntities.Count > 0)
                                    {
                                        txtMessage.Text = Resources.GetString(Resource.String.BookProductMessage);
                                    }
                                    else
                                    {
                                        txtMessage.Text = Resources.GetString(Resource.String.NoProductBookMessage);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.ProductListFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Showing Overlay screen
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.ProductListFragment), null);
                }
            }
        }
    }
}