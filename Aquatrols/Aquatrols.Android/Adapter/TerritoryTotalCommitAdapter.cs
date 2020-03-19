using Android.Content;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// Show total number of commits in snapshot result
    /// </summary>
    public class TerritoryTotalCommitAdapter : BaseAdapter<TmTotalCommitments>
    {
        Context context;
        List<TmTotalCommitments> tmTotalCommitments;

        /// <summary>
        /// Contains total number of TerritoryTotalCommit
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tmTotalCommitments"></param>
        public TerritoryTotalCommitAdapter(Context context, List<TmTotalCommitments> tmTotalCommitments)
        {
            this.context = context;
            this.tmTotalCommitments = tmTotalCommitments;
        }

        /// <summary>
        /// To get the postion of Item
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Return view of total commit
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
                view = LayoutInflater.FromContext(this.context).Inflate(Resource.Layout.TmSnapshotTotalCommitLayout, null, false);
            }
            TextView txtProductName = view.FindViewById<TextView>(Resource.Id.txtProductName);
            TextView txtTmQuantity = view.FindViewById<TextView>(Resource.Id.txtTmQuantity);
            TextView txtTmCountry = view.FindViewById<TextView>(Resource.Id.txtTmCountry);
            txtProductName.Text = tmTotalCommitments[position].productName;
            txtTmQuantity.Text = tmTotalCommitments[position].quantity + " " + tmTotalCommitments[position].unit;
            txtTmCountry.Text = tmTotalCommitments[position].country;
            return view;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return tmTotalCommitments.Count;
            }
        }
        
        /// <summary>
        /// To show the position of id
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TmTotalCommitments this[int position]
        {
            get { return tmTotalCommitments[position]; }
        }
    }
}