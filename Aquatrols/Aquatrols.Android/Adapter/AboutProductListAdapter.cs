using Android.Content.Res;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Square.Picasso;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Adapter
{
    /// <summary>
    ///  This class is used to bind data comming in list in to view
    /// </summary>
    public class AboutProductViewHolder : RecyclerView.ViewHolder
    {
        public ImageView imgIcon { get; set; }

        /// <summary>
        /// This method is to bind image according to id
        /// </summary>
        /// <param name="view"></param>
        public AboutProductViewHolder(View view) : base(view)
        {
            imgIcon = view.FindViewById<ImageView>(Resource.Id.imgProductIcon);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class AboutProductListAdapter : RecyclerView.Adapter
    {
        MainActivity Context;
        List<ProductListEntity> lstProductlist;

        /// <summary>
        /// This Method is to bind product list
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mproductlist"></param>
        public AboutProductListAdapter(MainActivity context, List<ProductListEntity> mproductlist)
        {
            Context = context;
            lstProductlist = mproductlist;
        }

        /// <summary>
        /// Fill in cound here, currently 0
        /// </summary>
        public override int ItemCount
        {
            get { return lstProductlist.Count; }
        }

        /// <summary>
        /// This method is to bind view of product and return
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.FromContext(Context).Inflate(Resource.Layout.AboutProductRowLayout, null, false);
            AboutProductViewHolder myAboutViewHolder = new AboutProductViewHolder(row);
            return myAboutViewHolder;
        }

        /// <summary>
        /// This method is to Bind Record in View
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            try
            {
                AboutProductViewHolder AboutHolder = holder as AboutProductViewHolder;
                string image = lstProductlist[position].productLogo.Replace("\\", "//");
                Android.Net.Uri url = Android.Net.Uri.Parse(this.Context.Resources.GetString(Resource.String.baseurl) + image);
                Picasso.With(Context).Load(url).Into(AboutHolder.imgIcon);
                AboutHolder.imgIcon.Click += delegate
                {
                    Bundle bundle = new Bundle();
                    bundle.PutString(this.Context.Resources.GetString(Resource.String.position), position.ToString());
                    Android.Support.V4.App.Fragment fragment = new AboutProductDetailFragment(Context);
                    fragment.Arguments = bundle;
                    Context.SupportFragmentManager.BeginTransaction()
                   .Replace(Resource.Id.content_frame, fragment)
                   .Commit();
                };
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(Context))
                {
                    utility.SaveExceptionHandling(ex, Context.Resources.GetString(Resource.String.OnBindViewHolder), Context.Resources.GetString(Resource.String.AboutProductViewHolder), null);
                }
            }
        }

    }
}