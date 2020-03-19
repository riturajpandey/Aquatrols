using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show Product detail in case of single record
    /// </summary>
    public class ProductCheckOutFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView lvProductlist;
        private ProductCheckoutAdapter adapter;
        private LinearLayoutManager layoutmngr;
        private Button btnPlaceorder;
        private TextView txtBadgeCount;
        private MainActivity mainActivity;
        private string userId, token;
        private List<WishListEntity> productCheckoutListEntities;

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
        /// This method is used to get detail of main activity to product detail
        /// </summary>
        /// <param name="mainActivity"></param>
        public ProductCheckOutFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
            if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null)))
            {
                userId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null);
            }
            if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
            {
                token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
            }
        }

        /// <summary>
        /// This method is used to initialize page load value and Get WishList Items
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.ProductCheckoutLayout, container, false);
            try
            {
                Bundle bundle = Arguments;
                TextView txtHeading = mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                TextView txtMessage = mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                LinearLayout lluserInfo = mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                txtBadgeCount = mainActivity.FindViewById<TextView>(Resource.Id.txtBadgeCount);
                BottomNavigationView bottomBar = mainActivity.FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
                bottomBar.Menu.GetItem(1).SetChecked(true);
                lluserInfo.Visibility = ViewStates.Gone;
                txtMessage.Visibility = ViewStates.Visible;
                txtHeading.Text = Resources.GetString(Resource.String.Bookingcheckout);
                txtMessage.Text = Resources.GetString(Resource.String.Bookingcheckoutmessage);
                btnPlaceorder = root.FindViewById<Button>(Resource.Id.btnPlaceOrder);
                btnPlaceorder.Visibility = ViewStates.Gone;
                btnPlaceorder.Click += BtnPlaceorder_Click;
                lvProductlist = root.FindViewById<RecyclerView>(Resource.Id.RcyCheckoutProductList);
                layoutmngr = new LinearLayoutManager(this.mainActivity);
                lvProductlist.SetLayoutManager(layoutmngr);
                GetWishListItems(userId, token);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.ProductCheckOutFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// On place order button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlaceorder_Click(object sender, EventArgs e)
        {
            HitCheckoutAPI();
        }

        /// <summary>
        /// Method to show wait cursor
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.ProductCheckOutFragment), null);
                }
            }
        }

        /// <summary>
        /// Getting data for product checkout model
        /// </summary>
        /// <returns></returns>
        public List<ProductCheckoutEntity> GetData()
        {
            List<ProductCheckoutEntity> checkoutEntity = new List<ProductCheckoutEntity>();
            try
            {
                foreach (var item in productCheckoutListEntities)
                {
                    checkoutEntity.Add(new ProductCheckoutEntity() { productId = item.productId, productName = item.productName, bookedBy = item.userId, userName = item.userName, distributorId = item.dId, distributorName = item.distributorName, quantity = item.quantity, brandPoints = item.brandPoints, pointsReceived = item.pointsReceived });
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetData), Resources.GetString(Resource.String.ProductCheckOutFragment), null);
                }
            }
            return checkoutEntity;
        }

        /// <summary>
        /// Method to Hit wish list API
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        public async void GetWishListItems(string userId, string token)
        {
            try
            {
                Show_Overlay();//SHOWS AN OVERLAY ON SCREEN TO PREVENT THE USER INTERACTION.
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
                        productCheckoutListEntities = await util.GetWishListItems(userId, token);
                        if (productCheckoutListEntities != null && productCheckoutListEntities.Count > 0)
                        {
                            btnPlaceorder.Visibility = ViewStates.Visible;
                            adapter = new ProductCheckoutAdapter(this.mainActivity, productCheckoutListEntities);
                            lvProductlist.SetAdapter(adapter);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetWishListItems), Resources.GetString(Resource.String.ProductCheckOutFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to call checkout API
        /// </summary>
        public async void HitCheckoutAPI()
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
                            ProductCheckoutResponseEntity checkoutResponseEntity = await util.Checkout(GetData(), token);
                            if (checkoutResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                            {
                                MyQueueResponseEntity myQueueResponse = await util.ClearQueue(userId, token);
                                if (myQueueResponse.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                                {
                                    txtBadgeCount.Text = Resources.GetString(Resource.String.noItem);
                                    txtBadgeCount.Visibility = ViewStates.Gone;

                                    Android.Support.V4.App.Fragment fragment = new BookingConfirmFragment(this.mainActivity);
                                    FragmentManager.BeginTransaction()
                                   .Replace(Resource.Id.content_frame, fragment)
                                   .Commit();
                                }
                            }
                            else
                            {
                                Toast.MakeText(this.mainActivity, checkoutResponseEntity.operationMessage, ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitCheckoutAPI), Resources.GetString(Resource.String.ProductCheckOutFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }
    }
}