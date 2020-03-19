using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// This class is used to show distributor data
    /// </summary>
    class TmDistributorResultAdapter : BaseAdapter<TmCourseProductVm>
    {
        Context context;
        List<TmCourseProductVm> tmCourseProduct;

        /// <summary>
        /// This Method hold the Distributor data detail
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tmCourseProduct"></param>
        public TmDistributorResultAdapter(Context context, List<TmCourseProductVm> tmCourseProduct)
        {
            this.context = context;
            this.tmCourseProduct = tmCourseProduct;
        }

        /// <summary>
        /// This method is used to get the particular position of product
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
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
        /// This method is used to show distributor data in tabular format
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = LayoutInflater.FromContext(this.context).Inflate(Resource.Layout.TmCourseResultRowLayout, null, false);
            TextView rllQuantityHeader = view.FindViewById<TextView>(Resource.Id.rllQuantityHeader);
            rllQuantityHeader.Visibility = ViewStates.Invisible;
            TextView rllBookingDateHeader = view.FindViewById<TextView>(Resource.Id.rllBookingDateHeader);
            rllBookingDateHeader.Text = this.context.Resources.GetString(Resource.String.Quantity);
            TextView txtDistributorName = view.FindViewById<TextView>(Resource.Id.txtDistributorName);
            TextView txtProduct = view.FindViewById<TextView>(Resource.Id.txtProduct);
            TextView txtQuantity = view.FindViewById<TextView>(Resource.Id.txtQuantity);
            ListView lstInnerData = view.FindViewById<ListView>(Resource.Id.lvInnerProductData);
            txtDistributorName.Text = this.tmCourseProduct[position].courseName;
            TmDistributorInnerDataAdapter adapter = new TmDistributorInnerDataAdapter(this.context, this.tmCourseProduct[position].tmProductDetail);
            lstInnerData.Adapter = adapter;
            return view;
        }

        /// <summary>
        ///Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return tmCourseProduct.Count;
            }
        }

        /// <summary>
        /// This method is used to get the posotion of product
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TmCourseProductVm this[int position]
        {
            get
            {
                return tmCourseProduct[position];
            }
        }
    }
}