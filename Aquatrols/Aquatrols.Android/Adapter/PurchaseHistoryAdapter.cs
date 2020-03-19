using Android.Content;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// This class is used to show detail of product purchased by which user
    /// </summary>
    class PurchaseHistoryAdapter : BaseAdapter<PurchaseHistoryEntity>
    {
        private Context context;
        private List<PurchaseHistoryEntity> lstPurchaseHistory;

        /// <summary>
        /// To hold the detail of purchase history
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lstPurchaseHistory"></param>
        public PurchaseHistoryAdapter(Context context, List<PurchaseHistoryEntity> lstPurchaseHistory)
        {
            this.context = context;
            this.lstPurchaseHistory = lstPurchaseHistory;
        }

        /// <summary>
        /// Show position of purchase history
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override PurchaseHistoryEntity this[int position]
        {
            get { return this.lstPurchaseHistory[position]; }
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return this.lstPurchaseHistory.Count;
            }
        }

        /// <summary>
        /// Show position of user
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// To show view of purchase history
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.FromContext(this.context).Inflate(Resource.Layout.PurchaseHistoryRowLayout, null, false);
            }
            TextView txtProductName = view.FindViewById<TextView>(Resource.Id.txtProductName);
            TextView txtProductQuantity = view.FindViewById<TextView>(Resource.Id.txtProductQuantity);
            TextView txtDistributorName = view.FindViewById<TextView>(Resource.Id.txtDistributorName);
            txtProductName.Text = lstPurchaseHistory[position].productName;
            txtProductQuantity.Text = lstPurchaseHistory[position].quantity.ToString();
            txtDistributorName.Text = lstPurchaseHistory[position].distributorName;
            return view;
        }
    }
}