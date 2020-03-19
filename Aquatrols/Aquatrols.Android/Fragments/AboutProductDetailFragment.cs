using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Adapter;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show product detail in about screen
    /// </summary>
    public class AboutProductDetailFragment : Android.Support.V4.App.Fragment
    {
        private List<ProductListEntity> flashCards;
        private MainActivity mainActivity;
        private ViewPager viewPager;
        private ScrollView scViewMain;

        /// <summary>
        /// This method is used to get detail of main activity to add product detail
        /// </summary>
        /// <param name="mainActivity"></param>
        public AboutProductDetailFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// This method is used to fix the state
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        /// <summary>
        /// This method is used to initialize page load value and Hit Product List API
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var root = inflater.Inflate(Resource.Layout.AboutProductDetailLayout, container, false);
            try
            {
                // Use this to return your custom view for this Fragment
                viewPager = root.FindViewById<ViewPager>(Resource.Id.viewpager);
                scViewMain = this.mainActivity.FindViewById<ScrollView>(Resource.Id.scViewMain);
                string position = Arguments.GetString(Resources.GetString(Resource.String.tmCourseId));
                HitProductListAPI(position);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.AboutProductDetailFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// Showing overlay screen
        /// </summary>
        OverlayActivity overlay;
        public void Show_Overlay()
        {
            try
            {
                overlay = new OverlayActivity(this.mainActivity);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.AboutProductDetailFragment), null);
                }
            }
        }

        /// <summary>
        /// Calling product list API
        /// </summary>
        /// <param name="position"></param>
        public async void HitProductListAPI(string position)
        {
            try
            {
                Show_Overlay();
                using (Utility util = new Utility(this.mainActivity))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this.mainActivity, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                        {
                            string token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                            flashCards = await util.GetAllProducts(token);
                            if (flashCards.Count == 0)
                            {
                                Toast.MakeText(this.mainActivity, Resource.String.NoRecord, ToastLength.Long).Show();
                            }
                            else
                            {
                                FlashCardDeckAdapter adapter = new FlashCardDeckAdapter(ChildFragmentManager, flashCards.OrderBy(x => x.productName).ToList(), this.mainActivity);
                                viewPager.Adapter = adapter;
                                viewPager.SetCurrentItem(Convert.ToInt32(position), true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitProductListAPI), Resources.GetString(Resource.String.AboutProductDetailFragment), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }
    }
}