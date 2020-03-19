using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Newtonsoft.Json;

namespace Aquatrols.Droid.Activities
{
    [Activity]
    public class LpRedeemWebViewActivity : Activity
    {
        private WebView webView;        
        private string courseId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.LpRedeemWebViewLayout);
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                FindControlsById();
                webView.Settings.JavaScriptEnabled = true;
                courseId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.tmCourseId), null);
                string url = "https://aquatrols.mp.prime-cloud.com/?lpidentifier=";
                url = url + courseId;
                webView.LoadUrl(url);                
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, "", "LPRedeemWebView", null);
                }
            }
        }

        public void FindControlsById()
        {
            try
            {                
                webView = FindViewById<WebView>(Resource.Id.webview);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
        }
    }
}