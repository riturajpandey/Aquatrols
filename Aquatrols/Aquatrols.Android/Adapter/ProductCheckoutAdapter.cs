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
    /// This class is used to hold the Product detail in case of single record
    /// </summary>
    public class ProductCheckoutAdapter : RecyclerView.Adapter
    {
        private MainActivity Context;
        private List<WishListEntity> productlist;
        string country;

        /// <summary>
        /// This method hold the detail of Wish List Entity
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mproductlist"></param>
        public ProductCheckoutAdapter(MainActivity context, List<WishListEntity> mproductlist)
        {
            this.Context = context;
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
            View row = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.ProductCheckoutRowLayout, null, false);
            CheckoutAdapterViewHolder vh = new CheckoutAdapterViewHolder(row);
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
                CheckoutAdapterViewHolder myHolder = viewHolder as CheckoutAdapterViewHolder;
                myHolder.txtDistributor.Text = productlist[position].distributorName;
                if (country.Equals(Context.Resources.GetString(Resource.String.USA)))
                {
                    myHolder.txtQuantity.Text = productlist[position].quantity + " " + productlist[position].unitName;
                }
                else
                {
                    myHolder.txtQuantity.Text = productlist[position].quantity + " " + Context.Resources.GetString(Resource.String.Litre);
                }
                myHolder.txtpointPerGallon.Text = productlist[position].pointsReceived + " " + Context.Resources.GetString(Resource.String.Points);
                string image = productlist[position].productLogo.Replace("\\", "//");
                Android.Net.Uri url = Android.Net.Uri.Parse(this.Context.Resources.GetString(Resource.String.baseurl) + image);
                Picasso.With(Context).Load(url).Into(myHolder.imgLogo);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(Context))
                {
                    utility.SaveExceptionHandling(ex, Context.Resources.GetString(Resource.String.OnBindViewHolder), Context.Resources.GetString(Resource.String.ProductCheckoutAdapter), null);
                }
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

    /// <summary>
    /// This class holds new views detail
    /// </summary>
    public class CheckoutAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView imgLogo { get; set; }
        public TextView txtQuantity { get; set; }
        public TextView txtpointPerGallon { get; set; }
        public TextView txtDistributor { get; set; }

        /// <summary>
        /// Getting the reference of controls
        /// </summary>
        /// <param name="view"></param>
        public CheckoutAdapterViewHolder(View view) : base(view)
        {
            imgLogo = view.FindViewById<ImageView>(Resource.Id.imageLogo);
            txtQuantity = view.FindViewById<TextView>(Resource.Id.txtQuantity);
            txtpointPerGallon = view.FindViewById<TextView>(Resource.Id.textPointsPerGallon);
            txtDistributor = view.FindViewById<TextView>(Resource.Id.txtDistributor);
        }
    }
}