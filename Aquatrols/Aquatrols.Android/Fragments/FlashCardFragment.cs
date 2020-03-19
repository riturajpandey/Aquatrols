using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using ShowcaseView.Interfaces;
using Square.Picasso;
using System;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show deatil of product
    /// </summary>
    public class FlashCardFragment : Android.Support.V4.App.Fragment
    {
        private static MainActivity mainactivity;
        private ImageView imgMain;
        private LinearLayout llProductDetail;

        /// <summary>
        /// To create constructor of FlashCardFragment
        /// </summary>
        public FlashCardFragment()
        {

        }
        
        public Activity context { get; private set; }

        /// <summary>
        /// This method is to show  product detail
        /// </summary>
        /// <param name="description"></param>
        /// <param name="mainActivity"></param>
        /// <returns></returns>
        public static FlashCardFragment newInstance(string[] description, MainActivity mainActivity)
        {
            mainactivity = mainActivity;
            FlashCardFragment fragment = new FlashCardFragment();
            Bundle args = new Bundle();
            args.PutStringArray("detail", description);
            fragment.Arguments = args;
            return fragment;
        }

        /// <summary>
        /// This method is used to initialize page load value and and return product data
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FlashCard_Layout, container, false);
            try
            {
                string[] description = Arguments.GetStringArray("detail");
                TextView txtView = view.FindViewById<TextView>(Resource.Id.txtDescription);
                ImageView imgLogo = view.FindViewById<ImageView>(Resource.Id.ImgLogo);
                TextView txtRates = view.FindViewById<TextView>(Resource.Id.txtrates);
                Button btnSds = view.FindViewById<Button>(Resource.Id.btnsds);
                Button btnWebpage = view.FindViewById<Button>(Resource.Id.btnVisitWebpage);
                Button btnLabel = view.FindViewById<Button>(Resource.Id.btnLabel);
                llProductDetail = view.FindViewById<LinearLayout>(Resource.Id.llProductDetail);
                imgMain = view.FindViewById<ImageView>(Resource.Id.ImgMain);
                txtView.Text = description[0];
                txtRates.Text = description[6];
                string web = description[3];
                string sds = description[1];
                string label = description[2];
                string name = description[5];
                string imageLogo = description[7].Replace("\\", "//");
                string img = description[4].Replace("\\", "//");
                Android.Net.Uri url = Android.Net.Uri.Parse(this.Context.Resources.GetString(Resource.String.baseurl) + imageLogo);
                Picasso.With(Context).Load(url).Into(imgLogo);
                Android.Net.Uri urlMain = Android.Net.Uri.Parse(this.Context.Resources.GetString(Resource.String.baseurl) + img);
                Picasso.With(Context).Load(urlMain).Into(imgMain);
                if (string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsManualVisibleOnProduct), null)))
                {
                    ShowCase();
                }
                //sds button click
                btnSds.Click += delegate
                {
                    if (!string.IsNullOrEmpty(sds))
                    {
                        Intent intent = new Intent(Activity.ApplicationContext, typeof(TermsWebviewActivity));
                        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.sds) + Resources.GetString(Resource.String.splitSymbol) + sds).Commit();
                        this.StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(mainactivity, Resources.GetString(Resource.String.linkNotfound), ToastLength.Long).Show();
                    }
                };
                //label button click
                btnLabel.Click += delegate
                {
                    if (!string.IsNullOrEmpty(sds))
                    {
                        Intent intent = new Intent(Activity.ApplicationContext, typeof(TermsWebviewActivity));
                        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.label) + Resources.GetString(Resource.String.splitSymbol) + label).Commit();
                        this.StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(mainactivity, Resources.GetString(Resource.String.linkNotfound), ToastLength.Long).Show();
                    }
                };
                //webpage button click
                btnWebpage.Click += delegate
                {
                    if (!string.IsNullOrEmpty(sds))
                    {
                        Intent intent = new Intent(Activity.ApplicationContext, typeof(TermsWebviewActivity));
                        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.web) + Resources.GetString(Resource.String.splitSymbol) + web + Resources.GetString(Resource.String.splitSymbol) + name).Commit();
                        this.StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(mainactivity, Resources.GetString(Resource.String.linkNotfound), ToastLength.Long).Show();
                    }
                };
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainactivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.FlashCardFragment), null);
                }
            }
            return view;
        }

        /// <summary>
        /// Method to display showcase
        /// </summary>
        public void ShowCase()
        {
            try
            {
                ShowCaseView showcase = new ShowCaseView.Builder()
                    .Context(mainactivity)
                    .CustomView(Resource.Layout.CustomeShowCaseDelete, mainactivity)
                    .CloseOnTouch(true)
                    .BackgroundColor(Android.Graphics.Color.Transparent)
                    .FocusBorderColor(Android.Graphics.Color.White)
                    .FocusBorderSize(15)
                    .FocusCircleRadiusFactor(0.8)
                    .Build();

                showcase.Show();
                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsManualVisibleOnProduct), Resources.GetString(Resource.String.yes)).Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainactivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ShowCase), Resources.GetString(Resource.String.FlashCardFragment), null);
                }
            }
        }
    }
}