using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show aboutus detail in about screen
    /// </summary>
    public class AboutusFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;

        /// <summary>
        /// This method is used to get detail of main activity to add aboutus detail
        /// </summary>
        /// <param name="mainActivity"></param>
        public AboutusFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// this method is used to fix the state
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        /// <summary>
        /// This method is used to initialize page load value and set drop down value data
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.AboutUsFragmentLayout, container, false);
            try
            {
                LinearLayout llTextMessage = this.mainActivity.FindViewById<LinearLayout>(Resource.Id.llTextMessage);
                llTextMessage.Visibility = ViewStates.Gone;
                Button btncontactus = root.FindViewById<Button>(Resource.Id.btncontactus);
                btncontactus.Click += Btncontactus_Click;
                WebView webview = root.FindViewById<WebView>(Resource.Id.webview);
                webview.Settings.JavaScriptEnabled = true;
                webview.SetWebViewClient(new HelloWebViewClient());
                webview.LoadUrl("file:///android_asset/aboutus.html");
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.AboutProductFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// On Contact us button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btncontactus_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType(Resources.GetString(Resource.String.textHtml));
                intent.PutExtra(Intent.ExtraEmail, new String[] { Resources.GetText(Resource.String.ApproachEmail) });
                intent.PutExtra(Intent.ExtraSubject, string.Empty);
                StartActivity(Intent.CreateChooser(intent, Resources.GetString(Resource.String.sendmail)));
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.mainActivity, this.mainActivity.Resources.GetString(Resource.String.noAppFound), ToastLength.Long).Show();
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Btncontactus_Click), Resources.GetString(Resource.String.AboutProductFragment), null);
                }
            }
        }
    }

    /// <summary>
    /// this internal class is used to replace web view detail by about detail
    /// </summary>
    public class HelloWebViewClient : WebViewClient
    {
        /// <summary>
        /// For API level 24 and later
        /// </summary>
        /// <param name="view"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl(request.Url.ToString());
            return false;
        }
    }
}