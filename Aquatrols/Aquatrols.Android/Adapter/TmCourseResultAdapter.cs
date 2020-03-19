using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    ///  This class is used to show Course data
    /// </summary>
    class TmCourseResultAdapter : BaseAdapter<TmDistributorProductVm>
    {
        Context context;
        List<TmDistributorProductVm> tmDistributorProduct;

        /// <summary>
        /// This Method hold the Course data detail
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tmDistributorProduct"></param>
        public TmCourseResultAdapter(Context context, List<TmDistributorProductVm> tmDistributorProduct)
        {
            this.tmDistributorProduct = tmDistributorProduct;
            this.context = context;
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
        /// This method is used to show course data in tabular format
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
               view=LayoutInflater.FromContext(this.context).Inflate(Resource.Layout.TmCourseResultRowLayout, null, false);
            TextView rllQuantityHeader = view.FindViewById<TextView>(Resource.Id.rllQuantityHeader);
            rllQuantityHeader.Text = this.context.Resources.GetString(Resource.String.Quantity);
            TextView rllBookingDateHeader = view.FindViewById<TextView>(Resource.Id.rllBookingDateHeader);
            rllBookingDateHeader.Text = this.context.Resources.GetString(Resource.String.BookingDateHeadertxtValue);
            TextView txtDistributorName = view.FindViewById<TextView>(Resource.Id.txtDistributorName);
            TextView txtProduct= view.FindViewById<TextView>(Resource.Id.txtProduct);
            TextView txtQuantity = view.FindViewById<TextView>(Resource.Id.txtQuantity);
            ListView lstInnerData = view.FindViewById<ListView>(Resource.Id.lvInnerProductData);
            txtDistributorName.Text = this.tmDistributorProduct[position].distributorName;
            TmProductInnerDataAdapter adapter = new TmProductInnerDataAdapter(this.context, this.tmDistributorProduct[position].tmProductDetail);
            lstInnerData.Adapter = adapter;
            return view;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return tmDistributorProduct.Count;
            }
        }

        /// <summary>
        /// This method is used to get the position of product
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TmDistributorProductVm this[int position]
        {
            get
            {
                return this.tmDistributorProduct[position];
            }
        }
    }
}