using Android.Content;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// Show total number of commits in distributor result
    /// </summary>
    public class TerritoryTotalCommitPerDistributorAdapter : BaseAdapter<TmCommitmentsPerDistributor>
    {
        Context context;
        List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributor;

        /// <summary>
        /// Contains total number of Territory Total Commit Per Distributor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tmCommitmentsPerDistributor"></param>
        public TerritoryTotalCommitPerDistributorAdapter(Context context, List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributor) : base()
        {
            this.context = context;
            this.tmCommitmentsPerDistributor = tmCommitmentsPerDistributor;
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
                view = LayoutInflater.FromContext(this.context).Inflate(Resource.Layout.TmSnapshotDistributorPerCommitRowLayout, null, false);
            }
            TextView txtDistributor = view.FindViewById<TextView>(Resource.Id.txtDistributor);
            TextView txtCount = view.FindViewById<TextView>(Resource.Id.txtCount);
            txtDistributor.Text = tmCommitmentsPerDistributor[position].distributorName;
            txtCount.Text = Convert.ToString(tmCommitmentsPerDistributor[position].courseCount);
            return view;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return tmCommitmentsPerDistributor.Count;
            }
        }

        /// <summary>
        /// To show the position of id
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TmCommitmentsPerDistributor this[int position]
        {
            get { return tmCommitmentsPerDistributor[position]; }
        }
    }
}