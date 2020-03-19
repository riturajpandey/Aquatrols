using Android.Content;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// This class is used to show course header data
    /// </summary>
    class TmProductInnerDataAdapter : BaseAdapter<TmProductDetail>
    {
        Context context;
        List<TmProductDetail> tmProductDetail;

        /// <summary>
        /// This Method hold the course header data detail
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tmProductDetail"></param>
        public TmProductInnerDataAdapter(Context context, List<TmProductDetail> tmProductDetail)
        {
            this.context = context;
            this.tmProductDetail = tmProductDetail;
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
        /// This method is used to show course header in tabular format
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
            TextView txtQuantity = view.FindViewById<TextView>(Resource.Id.txtQuantity);   
            TextView txtDate= view.FindViewById<TextView>(Resource.Id.txtDate);
            txtProduct.Text = this.tmProductDetail[position].productName;
            txtDate.Text = this.tmProductDetail[position].bookingDate;
            txtQuantity.Text = this.tmProductDetail[position].quantity + " " + this.tmProductDetail[position].unit;
            return view;
        }

        /// <summary>
        ///Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return this.tmProductDetail.Count;
            }
        }

        /// <summary>
        /// This method is used to get the posotion of product
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TmProductDetail this[int position]
        {
            get
            {
                return this.tmProductDetail[position];
            }
        }
    }
    
}