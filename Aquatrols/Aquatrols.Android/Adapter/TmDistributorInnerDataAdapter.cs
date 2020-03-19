using Android.Content;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// This class is used to show distributor header data
    /// </summary>
    class TmDistributorInnerDataAdapter : BaseAdapter<TmDistributorProductDetail>
    {
        Context context;
        List<TmDistributorProductDetail> tmDistributorProductDetail;

        /// <summary>
        /// This Method hold the Distributor header data detail
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tmDistributorProductDetail"></param>
        public TmDistributorInnerDataAdapter(Context context, List<TmDistributorProductDetail> tmDistributorProductDetail)
        {
            this.context = context;
            this.tmDistributorProductDetail = tmDistributorProductDetail;
        }

        /// <summary>
        /// This method is used to get item id of data
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// This method is used to show distributor header in tabular format
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = LayoutInflater.FromContext(this.context).Inflate(Resource.Layout.TmProductInnerListLayout, null, false);
            TextView txtProduct = view.FindViewById<TextView>(Resource.Id.txtProduct);
            TextView txtDate = view.FindViewById<TextView>(Resource.Id.txtQuantity);
            TextView txtQuantity = view.FindViewById<TextView>(Resource.Id.txtDate);
            txtDate.Visibility = ViewStates.Invisible;
            txtProduct.Text = this.tmDistributorProductDetail[position].productName;
            txtQuantity.Text = this.tmDistributorProductDetail[position].quantity + " " + this.tmDistributorProductDetail[position].unit;
            return view;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return tmDistributorProductDetail.Count;
            }
        }

        /// <summary>
        /// This method is used to get the posotion of product
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TmDistributorProductDetail this[int position]
        {
            get
            {
                return tmDistributorProductDetail[position];
            }
        }
    }
    
}