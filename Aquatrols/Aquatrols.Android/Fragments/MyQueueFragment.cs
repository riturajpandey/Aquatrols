using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Plugin.GoogleAnalytics;
using ShowcaseView.Interfaces;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show queue screen
    /// </summary>
    public class MyQueueFragment : Android.Support.V4.App.Fragment, RecyclerItemTouchHelperListener, OnViewInflateListener
    {
        private MainActivity mainActivity;
        private CartProductListAdapter cartAdapter;
        private RecyclerView.LayoutManager layoutMngr;
        private RecyclerView RecyclerCartProductList;
        private List<WishListEntity> lstWishListEntity;
        private Button btnConfirm;
        private LinearLayout llHeader, llTextMessage, llUserInfo;
        private RelativeLayout parentLayout, rlHeader;
        private TextView txtHeading, txtEmpty, txtBadgeCount, txtMessage;
        private ImageView btnTmView;
        private string UserId = string.Empty;
        private string token = string.Empty;

        /// <summary>
        /// This method is used to initialize page load value
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null)))
            {
                UserId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.UserId), null);
            }
            if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
            {
                token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
            }
        }

        /// <summary>
        /// This method is used to get detail of main activity to my queue detail
        /// </summary>
        /// <param name="mainActivity"></param>
        public MyQueueFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
            #region //Google Analytics
            GoogleAnalytics.Current.Config.TrackingId = this.mainActivity.Resources.GetString(Resource.String.TrackingId);
            GoogleAnalytics.Current.Config.AppId = this.mainActivity.ApplicationContext.PackageName;
            GoogleAnalytics.Current.Config.AppName = this.mainActivity.ApplicationInfo.LoadLabel(this.mainActivity.PackageManager).ToString();
            GoogleAnalytics.Current.Config.AppVersion = this.mainActivity.PackageManager.GetPackageInfo(this.mainActivity.PackageName, 0).VersionName;
            GoogleAnalytics.Current.InitTracker();
            GoogleAnalytics.Current.Tracker.SendView("My Queue");
            #endregion
        }

        /// <summary>
        /// This method is used to initialize page load value and show functionalities on confirm button click
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.MyQueueLayoutFragment, container, false);
            try
            {
                ShowCaseView();
                txtMessage = mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                txtHeading = mainActivity.FindViewById<TextView>(Resource.Id.txtHeading);
                llHeader = mainActivity.FindViewById<LinearLayout>(Resource.Id.llHeader);
                llUserInfo = mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                llTextMessage = mainActivity.FindViewById<LinearLayout>(Resource.Id.llTextMessage);
                parentLayout = mainActivity.FindViewById<RelativeLayout>(Resource.Id.parentLayout);
                rlHeader = mainActivity.FindViewById<RelativeLayout>(Resource.Id.rlHeader);
                txtBadgeCount = mainActivity.FindViewById<TextView>(Resource.Id.txtBadgeCount);
                btnTmView = mainActivity.FindViewById<ImageView>(Resource.Id.btnTmView);
                txtMessage.Text = Resources.GetString(Resource.String.myqueueMessage);
                llUserInfo.Visibility = ViewStates.Visible;
                txtEmpty = root.FindViewById<TextView>(Resource.Id.txtEmpty);
                RecyclerCartProductList = root.FindViewById<RecyclerView>(Resource.Id.RecyclerCartProductList);
                btnConfirm = root.FindViewById<Button>(Resource.Id.btnConfirm);
                btnTmView.Visibility = ViewStates.Gone;
                btnConfirm.Visibility = ViewStates.Gone;
                layoutMngr = new LinearLayoutManager(this.mainActivity);
                RecyclerCartProductList.SetLayoutManager(layoutMngr);
                GetWishListItems(UserId, token);
                btnConfirm.Click += BtnConfirm_Click;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.MyQueueFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// Showing tutorial for swipe to delete
        /// </summary>
        public void ShowCaseView()
        {
            try
            {
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.cartItem), null)))
                {
                    if (string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsManualVisible), null)))
                    {
                        ShowCaseView showcase = new ShowCaseView.Builder()
                            .Context(this.mainActivity)
                            .FocusOn(RecyclerCartProductList)
                            .CustomView(Resource.Layout.CustomShowcaseView, this)
                            .CloseOnTouch(true)
                            .FocusBorderSize(15)
                            .FocusCircleRadiusFactor(0.8)
                            .Build();

                        showcase.Show();
                        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsManualVisible), Resources.GetString(Resource.String.yes)).Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ShowCaseView), Resources.GetString(Resource.String.MyQueueFragment), null);
                }
            }
        }

        /// <summary>
        /// On confirm button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsValidAmount), null)) || Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsValidAmount), null).Equals(Resources.GetString(Resource.String.yes)))
                {
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.from), Resources.GetString(Resource.String.OneItem)).Commit();
                    Utility.sharedPreferences.Edit().Remove(Resources.GetString(Resource.String.IsValidAmount));
                    Android.Support.V4.App.Fragment fragment = new ProductCheckOutFragment(this.mainActivity);
                    llHeader.SetBackgroundResource(Resource.Drawable.BookingImage);
                    parentLayout.SetBackgroundResource(0);
                    llTextMessage.Visibility = ViewStates.Visible;
                    rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(Resources.GetString(Resource.String.BlueBackground)));
                    txtHeading.Text = Resources.GetString(Resource.String.Bookproducts);
                    FragmentManager.BeginTransaction()
                       .AddToBackStack(txtHeading.Text)
                       .Replace(Resource.Id.content_frame, fragment)
                       .Commit();
                }
                else
                {
                    Toast.MakeText(this.mainActivity, Resources.GetString(Resource.String.invalidquantity), ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BtnConfirm_Click), Resources.GetString(Resource.String.MyQueueFragment), null);
                }
            }
        }

        /// <summary>
        /// On swipe recyclerview Item
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="direction"></param>
        /// <param name="position"></param>
        public void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction, int position)
        {
            try
            {
                if (viewHolder != null)
                {
                    Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this.mainActivity);
                    alert.SetTitle(Resources.GetString(Resource.String.confirm));
                    alert.SetMessage(Resources.GetString(Resource.String.confirmationDeleteMessage));
                    // Yes delete
                    alert.SetPositiveButton(Resources.GetString(Resource.String.textYes), (senderAlert, args) =>
                    {
                        int deletedItem = lstWishListEntity[position].wishListId;
                        DeleteItemFromQueue(token, deletedItem);
                    });
                    // take me back button
                    alert.SetNegativeButton(Resources.GetString(Resource.String.textCancel), (senderAlert, args) =>
                    {
                        cartAdapter = new CartProductListAdapter(this.mainActivity, lstWishListEntity);
                        RecyclerCartProductList.SetAdapter(cartAdapter);

                    });

                    Dialog dialog = alert.Create();
                    if (true)
                    {
                        dialog.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnSwiped), Resources.GetString(Resource.String.MyQueueFragment), null);
                }
            }
        }

        /// <summary>
        /// Method to call API for deleting wishlist Item from Queue
        /// </summary>
        /// <param name="token"></param>
        /// <param name="wishListID"></param>
        public async void DeleteItemFromQueue(string token, int wishListID)
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
                        MyQueueResponseEntity myQueueResponse = await util.DeleteItemFromQueue(token, wishListID);
                        if (myQueueResponse.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                        {
                            GetWishListItems(UserId, token);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.DeleteItemFromQueue), Resources.GetString(Resource.String.MyQueueFragment), null);
                }
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.MyQueueFragment), null);
                }
            }
        }

        /// <summary>
        /// API call to get all wishlist Items from Queue
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
                        lstWishListEntity = await util.GetWishListItems(userId, token);
                        if (lstWishListEntity != null && lstWishListEntity.Count > 0)
                        {
                            txtBadgeCount.Visibility = ViewStates.Visible;
                            RecyclerCartProductList.Visibility = ViewStates.Visible;
                            txtBadgeCount.Text = lstWishListEntity.Count.ToString();
                            txtEmpty.Visibility = ViewStates.Gone;
                            btnConfirm.Visibility = ViewStates.Visible;
                            cartAdapter = new CartProductListAdapter(this.mainActivity, lstWishListEntity);
                            RecyclerCartProductList.SetAdapter(cartAdapter);
                            ItemTouchHelper.SimpleCallback itemTouchHelperCallback = new ProductListItemTouchListener(0, ItemTouchHelper.Left | ItemTouchHelper.Right, this);
                            new ItemTouchHelper(itemTouchHelperCallback).AttachToRecyclerView(RecyclerCartProductList);
                        }
                        else
                        {
                            txtBadgeCount.Visibility = ViewStates.Gone;
                            txtEmpty.Visibility = ViewStates.Visible;
                            btnConfirm.Visibility = ViewStates.Gone;
                            RecyclerCartProductList.Visibility = ViewStates.Gone;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetWishListItems), Resources.GetString(Resource.String.MyQueueFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// This method is used to make constructor of OnViewInflated
        /// </summary>
        /// <param name="view"></param>
        public void OnViewInflated(View view)
        {
            //implementation
        }

    }
}