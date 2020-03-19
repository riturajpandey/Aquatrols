using Android.Content;
using Android.Views;
using Android.Widget;
using Aquatrols.Models;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    /// Show total number of commits in Sign up result
    /// </summary>
    class TerritoryTotalSignupAdapter : BaseAdapter<TmSnapshotSignUpCountVm>
    {
        Context context;
        List<TmSnapshotSignUpCountVm> tmSnapshotSignUpCountVms;

        /// <summary>
        /// Contains total number of Territory Total Commit Per user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tmSnapshotSignUpCountVms"></param>
        public TerritoryTotalSignupAdapter(Context context,List<TmSnapshotSignUpCountVm> tmSnapshotSignUpCountVms)
        {
            this.context = context;
            this.tmSnapshotSignUpCountVms = tmSnapshotSignUpCountVms;
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
                view = LayoutInflater.FromContext(this.context).Inflate(Resource.Layout.TmSnapshotTotalSignupRowLayout, null, false);
            }
            TextView txtTmNumber = view.FindViewById<TextView>(Resource.Id.txtTmNumber);
            TextView txtTmTotalSignups = view.FindViewById<TextView>(Resource.Id.txtTmTotalSignups);
            TextView txtTmTotalCommitments = view.FindViewById<TextView>(Resource.Id.txtTmTotalCommitments);
            TextView txtTmName= view.FindViewById<TextView>(Resource.Id.txtTmName); 
            txtTmNumber.Text = tmSnapshotSignUpCountVms[position].territoryNumber;
            txtTmName.Text = tmSnapshotSignUpCountVms[position].territoryName;
            txtTmTotalSignups.Text = Convert.ToString(tmSnapshotSignUpCountVms[position].countSignUps);
            txtTmTotalCommitments.Text = Convert.ToString(tmSnapshotSignUpCountVms[position].countCommitments);
            return view;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int Count
        {
            get
            {
                return tmSnapshotSignUpCountVms.Count;
            }
        }

        /// <summary>
        /// To show the position of id
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TmSnapshotSignUpCountVm this[int position]
        {
            get { return tmSnapshotSignUpCountVms[position]; }
        }
    }
}