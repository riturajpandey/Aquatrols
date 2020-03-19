using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Square.Picasso;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// This class is used for view product.
    /// </summary>
    public class MyViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        ProductListAdapter adapter;
        private int position;
        string distributorId = string.Empty;
        string distributorName = string.Empty;
        string token = string.Empty;
        string role = string.Empty;
        public ImageView imgLogo { get; set; }
        public TextView txtProductInfo { get; set; }
        public TextView txtamounttext { get; set; }
        public TextView txtPointsPerGallon { get; set; }
        public Button btnAddtoOrder { get; set; }
        public Button btnBuynow { get; set; }
        public EditText editGallons { get; set; }
        public TextInputLayout txtInputLayoutGallons { get; set; }

        /// <summary>
        /// Holds the record of product
        /// </summary>
        /// <param name="view"></param>
        /// <param name="adapter"></param>
        public MyViewHolder(View view, ProductListAdapter adapter) : base(view)
        {
            try
            {
                this.adapter = adapter;
                imgLogo = view.FindViewById<ImageView>(Resource.Id.imageLogo);
                txtProductInfo = view.FindViewById<TextView>(Resource.Id.txtProductInfo);
                txtamounttext = view.FindViewById<TextView>(Resource.Id.txtAmounttext);
                txtPointsPerGallon = view.FindViewById<TextView>(Resource.Id.txtPointsPerGallon);
                btnAddtoOrder = view.FindViewById<Button>(Resource.Id.btnAddtoOrder);
                btnBuynow = view.FindViewById<Button>(Resource.Id.btnBuynow);
                editGallons = view.FindViewById<EditText>(Resource.Id.editGallons);
                txtInputLayoutGallons = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutGallons);
                btnBuynow.SetOnClickListener(this);
                btnAddtoOrder.SetOnClickListener(this);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.MyViewHolder), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Function to handle when click on addtoOrder button
        /// </summary>
        private void HandleAddToOrderButtonclick()
        {
            try
            {
                txtInputLayoutGallons.ErrorEnabled = false;
                if (!string.IsNullOrEmpty(editGallons.Text))
                {
                    if (editGallons.Text.Length <= 5 && Convert.ToInt32(editGallons.Text) > 0)
                    {
                        MyQueueEntity myQueueEntity = new MyQueueEntity();
                        myQueueEntity.productId = adapter.lstProductlist[position].productId;
                        myQueueEntity.quantity = Convert.ToInt32(editGallons.Text);
                        myQueueEntity.distributorId = distributorId;
                        myQueueEntity.userId = adapter.userId;
                        if (adapter.country.Equals(adapter.Context.Resources.GetString(Resource.String.USA)))
                        {
                            myQueueEntity.brandPoints = adapter.lstProductlist[position].brandPoints;
                        }
                        else
                        {
                            myQueueEntity.brandPoints = adapter.lstProductlist[position].canadaBrandPoints;
                        }
                        editGallons.Text = string.Empty;
                        AddToQueue(myQueueEntity, token); //Method call to add products to queue
                    }
                    else
                    {
                        txtInputLayoutGallons.Error = adapter.Context.Resources.GetString(Resource.String.invalidquantity);
                    }
                }
                else
                {
                    txtInputLayoutGallons.Error = adapter.Context.Resources.GetString(Resource.String.Required);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.Show_Overlay), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Function to handle when click on buynow button
        /// </summary>
        private void HandleBuynNowButtonclick()
        {
            try
            {
                MyQueueEntity myQueueEntity = null;
                txtInputLayoutGallons.ErrorEnabled = false;
                if (!string.IsNullOrEmpty(editGallons.Text))
                {
                    if (editGallons.Text.Length <= 5 && Convert.ToInt32(editGallons.Text) > 0)
                    {
                        myQueueEntity = new MyQueueEntity();
                        myQueueEntity.productId = adapter.lstProductlist[position].productId;
                        myQueueEntity.quantity = Convert.ToInt32(editGallons.Text);
                        myQueueEntity.distributorId = distributorId;
                        myQueueEntity.userId = adapter.userId;
                        if (adapter.country.Equals(adapter.Context.Resources.GetString(Resource.String.USA)))
                        {
                            myQueueEntity.brandPoints = adapter.lstProductlist[position].brandPoints;
                        }
                        else
                        {
                            myQueueEntity.brandPoints = adapter.lstProductlist[position].canadaBrandPoints;
                        }
                        editGallons.Text = string.Empty;
                        //Method call to add products to queue
                        AddToQueue(myQueueEntity, token);
                    }
                    else
                    {
                        Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.invalidquantity), ToastLength.Long).Show();
                    }
                }
                else
                {
                    if (adapter.country.Equals(adapter.Context.Resources.GetString(Resource.String.USA)))
                    {
                        Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.Add) + " " + adapter.lstProductlist[position].unit, ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.addlitre), ToastLength.Long).Show();
                    }
                }
                CallMyQueueFragment(myQueueEntity);//redirecting to myqueue screen
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.HandleBuynNowButtonclick), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Metthod to call Add to Queue API
        /// </summary>
        public async void AddToQueue(MyQueueEntity myQueueEntity, string token)
        {
            try
            {
                Show_Overlay();//SHOWS AN OVERLAY ON SCREEN TO PREVENT THE USER INTERACTION.               
                using (Utility util = new Utility(adapter.Context))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(adapter.Context, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        MyQueueResponseEntity myQueueResponseEntity = await util.AddToQueue(myQueueEntity, token);
                        if (myQueueResponseEntity.operationStatus.ToLower().Equals(adapter.Context.Resources.GetString(Resource.String.success)))
                        {
                            string count = await util.GetWishListItemCount(token);
                            if (string.IsNullOrEmpty(count) || count.Equals("0"))
                            {
                                adapter.txtCount.Visibility = ViewStates.Gone;
                            }
                            else
                            {
                                adapter.txtCount.Text = count;
                                adapter.txtCount.Visibility = ViewStates.Visible;
                                Toast.MakeText(adapter.Context, myQueueResponseEntity.operationMessage, ToastLength.Short).Show();
                            }
                        }
                        else
                        {
                            Toast.MakeText(adapter.Context, adapter.Context.Resources.GetString(Resource.String.ServiceError), ToastLength.Short).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.AddToQueue), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
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
                overlay = new OverlayActivity(adapter.Context);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.Show_Overlay), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Function for redirecting to MyQueue screen
        /// </summary>
        public void CallMyQueueFragment(MyQueueEntity myQueueEntity)
        {
            try
            {
                Android.Support.V4.App.Fragment fragment = new MyQueueFragment(adapter.Context);
                adapter.Context.llHeader.SetBackgroundResource(0);
                adapter.Context.parentLayout.SetBackgroundResource(0);
                adapter.Context.rlHeader.SetBackgroundColor(Android.Graphics.Color.ParseColor(adapter.Context.Resources.GetString(Resource.String.BlueBackground)));
                adapter.Context.txtHeading.Text = adapter.Context.Resources.GetString(Resource.String.myqueue);
                adapter.Context.SupportFragmentManager.BeginTransaction()
                .AddToBackStack(adapter.Context.txtHeading.Text)
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.CallMyQueueFragment), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Function to Bind data to view holder
        /// </summary>
        /// <param name="position"></param>
        public void Bind(int position)
        {
            try
            {
                this.position = position;
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.distributorId), null)))
                {
                    distributorId = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.distributorId), null);
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.distributorName), null)))
                {
                    distributorName = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.distributorName), null);
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.UserId), null)))
                {
                    adapter.userId = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.UserId), null);
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.country), null)))
                {
                    adapter.country = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.country), null);
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.token), null)))
                {
                    token = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.token), null);
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.Role), null)))
                {
                    role = Utility.sharedPreferences.GetString(adapter.Context.Resources.GetString(Resource.String.Role), null);
                }
                adapter.txtCount = adapter.Context.FindViewById<TextView>(Resource.Id.txtBadgeCount);
                TextView txtHeading = adapter.Context.FindViewById<TextView>(Resource.Id.txtHeading);
                LinearLayout llHeader = adapter.Context.FindViewById<LinearLayout>(Resource.Id.llHeader);
                RelativeLayout parentLayout = adapter.Context.FindViewById<RelativeLayout>(Resource.Id.parentLayout);
                RelativeLayout rlHeader = adapter.Context.FindViewById<RelativeLayout>(Resource.Id.rlHeader);
                #region 
                //checking country to display product Unit as litre if country is canada
                if (!adapter.country.Equals(adapter.Context.Resources.GetString(Resource.String.USA)))
                {
                    txtInputLayoutGallons.Hint = adapter.Context.Resources.GetString(Resource.String.Litre);
                    txtPointsPerGallon.Text = "at " + adapter.lstProductlist[position].canadaBrandPoints + " " + adapter.Context.Resources.GetString(Resource.String.pointsperLitre);
                }
                else
                {
                    txtInputLayoutGallons.Hint = adapter.lstProductlist[position].unit;
                    txtPointsPerGallon.Text = "at " + adapter.lstProductlist[position].brandPoints + " " + adapter.Context.Resources.GetString(Resource.String.pointsper) + " " + adapter.lstProductlist[position].unit;
                }
                #endregion

                txtProductInfo.Text = adapter.lstProductlist[position].productShortDescription;
                txtInputLayoutGallons.ErrorEnabled = false;
                string image = adapter.lstProductlist[position].productLogo.Replace("\\", "//");
                Android.Net.Uri url = Android.Net.Uri.Parse(adapter.Context.Resources.GetString(Resource.String.baseurl) + image);
                Picasso.With(adapter.Context).Load(url).Into(imgLogo);
                if (!string.IsNullOrEmpty(role))
                {
                    if (!role.ToLower().Equals(adapter.Context.Resources.GetString(Resource.String.Manager)))
                    {
                        IsProductBookable_ByLoggedInUser(false); //Method to hide book buttons if logged in account is User
                        if (position == 0)
                        {
                            ShowAlert();
                        }
                    }
                    else
                    {
                        IsProductBookable_ByLoggedInUser(true);
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.Bind), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Method to show Alert
        /// </summary>
        public void ShowAlert()
        {
            try
            {
                Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(adapter.Context, Android.Resource.Style.ThemeDeviceDefaultDialogAlert);
                alert.SetTitle(adapter.Context.Resources.GetString(Resource.String.message));
                alert.SetMessage(adapter.Context.Resources.GetString(Resource.String.usercannotbook));
                alert.SetPositiveButton(adapter.Context.Resources.GetString(Resource.String.Ok), (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.ShowAlert), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Function to restrict/Approve user to book a product if logged In as a USER
        /// </summary>
        /// <param name="flag"></param>
        public void IsProductBookable_ByLoggedInUser(bool flag)
        {
            try
            {
                if (flag == true)
                {
                    btnAddtoOrder.Visibility = ViewStates.Visible;
                    btnBuynow.Visibility = ViewStates.Visible;
                    txtamounttext.Visibility = ViewStates.Visible;
                    editGallons.Visibility = ViewStates.Visible;
                    txtPointsPerGallon.Visibility = ViewStates.Visible;
                }
                else
                {
                    btnAddtoOrder.Visibility = ViewStates.Gone;
                    btnBuynow.Visibility = ViewStates.Gone;
                    txtamounttext.Visibility = ViewStates.Gone;
                    editGallons.Visibility = ViewStates.Gone;
                    txtPointsPerGallon.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.IsProductBookable_ByLoggedInUser), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }

        /// <summary>
        /// Click event of controls
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.btnBuynow:
                        HandleBuynNowButtonclick();
                        break;
                    case Resource.Id.btnAddtoOrder:
                        HandleAddToOrderButtonclick();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(adapter.Context))
                {
                    utility.SaveExceptionHandling(ex, adapter.Context.Resources.GetString(Resource.String.OnClick), adapter.Context.Resources.GetString(Resource.String.ProductListAdapter), null);
                }
            }
        }
    }

    /// <summary>
    /// This class is used to show product fragment in main view.
    /// </summary>
    public class ProductListAdapter : RecyclerView.Adapter
    {
        public MainActivity Context;
        public List<ProductListEntity> lstProductlist;
        public TextView txtCount;
        public string userId = string.Empty, country = string.Empty;

        /// <summary>
        /// holds the product lis detail
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mproductlist"></param>
        public ProductListAdapter(MainActivity context, List<ProductListEntity> mproductlist)
        {
            Context = context;
            lstProductlist = mproductlist;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int ItemCount
        {
            get { return lstProductlist.Count; }
        }

        /// <summary>
        /// Return view
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.ProductListRowLayout, null, false);
            MyViewHolder myViewHolder = new MyViewHolder(row, this);
            return myViewHolder;
        }

        /// <summary>
        /// System on OnBindViewHolder
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyViewHolder viewHolder = (MyViewHolder)holder;
            viewHolder.Bind(position);
        }
    }
}