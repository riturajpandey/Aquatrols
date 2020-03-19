using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used to show terms and condition and other detail to user
    /// </summary>
    [Activity]
    public class TermsWebviewActivity : Activity, View.IOnClickListener
    {
        private ImageView imgBack;
        private WebView webView;
        private TextView txtHeading;

        /// <summary>
        /// This method is used to initialize page load value and check codition wise is that terms and condition or other detail
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.TermsWebviewLayout);
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                FindControlsById();
                webView.Settings.JavaScriptEnabled = true;
                webView.SetWebViewClient(new HelloWebViewClient(this));
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsTerms), null)))
                {
                    string terms = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.IsTerms), null);
                    if (terms.ToLower().Equals(Resources.GetString(Resource.String.yes)))
                    {
                        txtHeading.Text = Resources.GetString(Resource.String.terms);
                        webView.LoadUrl(Resources.GetString(Resource.String.termsLink));
                    }
                    else if (terms.ToLower().Equals(Resources.GetString(Resource.String.no)))
                    {
                        txtHeading.Text = Resources.GetString(Resource.String.Privacy);
                        webView.LoadUrl(Resources.GetString(Resource.String.privacyPolicyLink));
                    }
                    else if (terms.Equals(Resources.GetString(Resource.String.FAQ)))
                    {
                        txtHeading.Text = Resources.GetString(Resource.String.FAQ);
                        webView.LoadUrl(Resources.GetString(Resource.String.faqLink));
                    }
                    else if (terms.Equals(Resources.GetString(Resource.String.support)))
                    {
                        txtHeading.Text = Resources.GetString(Resource.String.support);
                        webView.LoadUrl(Resources.GetString(Resource.String.supportLink));
                    }
                    else if (terms.ToLower().StartsWith(Resources.GetString(Resource.String.web)))
                    {
                        string[] arr = terms.Split('$');
                        txtHeading.Text = arr[2];
                        webView.LoadUrl(arr[1]);
                    }
                    else if (terms.ToLower().StartsWith(Resources.GetString(Resource.String.sds)))
                    {
                        string[] arr = terms.Split('$');
                        txtHeading.Text = Resources.GetString(Resource.String.sds);
                        webView.Settings.JavaScriptEnabled = true;
                        webView.Settings.AllowFileAccess = true;
                        webView.LoadUrl("https://docs.google.com/viewer?embedded=true&url=" + arr[1]);

                    }
                    else if (terms.ToLower().StartsWith(Resources.GetString(Resource.String.label)))
                    {
                        string[] arr = terms.Split('$');
                        txtHeading.Text = Resources.GetString(Resource.String.label);
                        webView.Settings.AllowFileAccess = true;
                        webView.LoadUrl("https://docs.google.com/gview?embedded=true&url=" + arr[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.TermsWebviewActivity), null);
                }
            }
        }

        /// <summary>
        /// Getting the reference of the controls
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                webView = FindViewById<WebView>(Resource.Id.webview);
                txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
                imgBack.SetOnClickListener(this);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TermsWebviewActivity), null);
                }
            }
        }

        /// <summary>
        /// On click event for back icon
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.imgBack:
                        Finish();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.TermsWebviewActivity), null);
                }
            }
        }

        /// <summary>
        /// On device back button press
        /// </summary>
        public override void OnBackPressed()
        {
            Finish();
        }

        /// <summary>
        /// This internal class is used for behind functionality when user changes from termsLink to Privacy or other
        /// </summary>
        public class HelloWebViewClient : WebViewClient
        {
            OverlayActivity overlay;
            TermsWebviewActivity activity;

            /// <summary>
            /// This method is to prevent user to move in side the app with out click on i agree or not
            /// </summary>
            /// <param name="activity"></param>
            public HelloWebViewClient(TermsWebviewActivity activity)
            {
                this.activity = activity;
                try
                {
                    overlay = new OverlayActivity(this.activity);
                    overlay.Show();
                    overlay.SetCanceledOnTouchOutside(false);
                    overlay.SetCancelable(false);
                }
                catch (Exception ex)
                {
                    using (Utility utility = new Utility(activity))
                    {
                        utility.SaveExceptionHandling(ex, activity.Resources.GetString(Resource.String.HelloWebViewClient), activity.Resources.GetString(Resource.String.TermsWebviewActivity), null);
                    }
                }
            }

            /// <summary>
            /// This Method is for loading when data change
            /// </summary>
            /// <param name="view"></param>
            /// <param name="request"></param>
            /// <returns></returns>
            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                view.LoadUrl(request.Url.ToString());
                return false;
            }

            /// <summary>
            /// Invokes On load finished of webview
            /// </summary>
            /// <param name="view"></param>
            /// <param name="url"></param>
            public override void OnPageFinished(WebView view, string url)
            {
                try
                {
                    base.OnPageFinished(view, url);
                    overlay.Dismiss();
                }
                catch (Exception ex)
                {
                    using (Utility utility = new Utility(activity))
                    {
                        utility.SaveExceptionHandling(ex, activity.Resources.GetString(Resource.String.OnPageFinished), activity.Resources.GetString(Resource.String.TermsWebviewActivity), null);
                    }
                }
            }
        }
    }
}