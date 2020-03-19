using Android.Content.Res;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Square.Picasso;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    ///  This class is used to bind data Cart Product List view holder detail
    /// </summary>
    public class CartProductListAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView imgLogo { get; set; }
        public EditText editGallons { get; set; }
        public RelativeLayout mview_foreground { get; set; }
        public RelativeLayout mview_background { get; set; }
        public TextView txtPoints { get; set; }
        public TextView txtDistributor { get; set; }
        public TextInputLayout txtInputLayoutGallons { get; set; }

        /// <summary>
        /// Getting the reference of controls
        /// </summary>
        /// <param name="view"></param>
        public CartProductListAdapterViewHolder(View view) : base(view)
        {
            imgLogo = view.FindViewById<ImageView>(Resource.Id.img);
            editGallons = view.FindViewById<EditText>(Resource.Id.editGallons);
            txtPoints = view.FindViewById<TextView>(Resource.Id.textPoints);
            txtDistributor = view.FindViewById<TextView>(Resource.Id.txtDistributor);
            txtInputLayoutGallons = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutGallons);
            mview_foreground = view.FindViewById<RelativeLayout>(Resource.Id.view_foreground);
            mview_background = view.FindViewById<RelativeLayout>(Resource.Id.view_background);
        }
    }

    /// <summary>
    ///  This class is used to bind data Cart Product List view
    /// </summary>
    class CartProductListAdapter : RecyclerView.Adapter
    {
        private MainActivity Context;
        private List<WishListEntity> productlist;
        string country, token;

        /// <summary>
        /// In this method WishListEntity holds the cart product detail
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mproductlist"></param>
        public CartProductListAdapter(MainActivity context, List<WishListEntity> mproductlist)
        {
            Context = context;
            productlist = mproductlist;
        }

        /// <summary>
        /// Create new views (invoked by the layout manager)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View row = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.MyQueueProductRowLayout, null, false);            
            CartProductListAdapterViewHolder vh = new CartProductListAdapterViewHolder(row);
            return vh;
        }

        /// <summary>
        /// Replace the contents of a view (invoked by the layout manager)
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Context.Resources.GetString(Resource.String.country), null)))
                {
                    country = Utility.sharedPreferences.GetString(Context.Resources.GetString(Resource.String.country), null);
                }
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Context.Resources.GetString(Resource.String.token), null)))
                {
                    token = Utility.sharedPreferences.GetString(Context.Resources.GetString(Resource.String.token), null);
                }
                CartProductListAdapterViewHolder myHolder = viewHolder as CartProductListAdapterViewHolder;
                if (country.Equals(Context.Resources.GetString(Resource.String.USA)))
                {
                    myHolder.txtInputLayoutGallons.Hint = productlist[position].unitName;
                }
                else
                {
                    myHolder.txtInputLayoutGallons.Hint = Context.Resources.GetString(Resource.String.Litre);
                }
                myHolder.txtPoints.Text = productlist[position].pointsReceived.ToString();
                myHolder.editGallons.Text = productlist[position].quantity.ToString();
                myHolder.editGallons.SetSelection(myHolder.editGallons.Text.Length);
                myHolder.txtDistributor.Text = productlist[position].distributorName;
                myHolder.editGallons.TextChanged += (s, e) =>
                {
                    if ((!string.IsNullOrEmpty(myHolder.editGallons.Text) && myHolder.editGallons.Text.Length <= 8) && (Convert.ToInt32(myHolder.editGallons.Text) > 0))
                    {
                        Utility.sharedPreferences.Edit().PutString(this.Context.Resources.GetString(Resource.String.IsValidAmount), Context.Resources.GetString(Resource.String.yes)).Commit();
                        myHolder.txtPoints.Text = ((productlist[position].brandPoints) * Convert.ToInt32(myHolder.editGallons.Text)).ToString();
                        MyQueueEntity myQueueEntity = new MyQueueEntity();
                        myQueueEntity.userId = productlist[position].userId;
                        myQueueEntity.wishListId = productlist[position].wishListId;
                        myQueueEntity.quantity = Convert.ToInt32(myHolder.editGallons.Text);
                        myQueueEntity.brandPoints = productlist[position].brandPoints;
                        HitAddtoQueueAPI(myQueueEntity, token);
                    }
                    else
                    {
                        Utility.sharedPreferences.Edit().PutString(this.Context.Resources.GetString(Resource.String.IsValidAmount), Context.Resources.GetString(Resource.String.no)).Commit();
                        myHolder.txtPoints.Text = string.Empty;
                        Toast.MakeText(this.Context, this.Context.Resources.GetString(Resource.String.invalidquantity), ToastLength.Short).Show();
                    }
                };
                string image = productlist[position].productLogo.Replace("\\", "//");
                Android.Net.Uri url = Android.Net.Uri.Parse(this.Context.Resources.GetString(Resource.String.baseurl) + image);
                Picasso.With(Context).Load(url).Into(myHolder.imgLogo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Method to call Add to queue API
        /// </summary>
        /// <param name="myQueueEntity"></param>
        /// <param name="token"></param>
        public async void HitAddtoQueueAPI(MyQueueEntity myQueueEntity,string token)
        {
            try
            {
                using (Utility util = new Utility(this.Context))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this.Context, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        MyQueueResponseEntity myQueueResponse= await util.AddToQueue(myQueueEntity,token);
                        Toast.MakeText(this.Context, myQueueResponse.operationMessage, ToastLength.Long).Show();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int ItemCount
        {
            get { return productlist.Count; }
        }
        
    }
}