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

namespace Aquatrols.Droid.Activities
{
    [Activity(Label = "LoggedInUserWebViewActivity")]
    public class LoggedInUserWebViewActivity : Activity
    {
        private WebView webView;
        private string redirectUrl;
        public ImageView imgMenu;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.LoggedInUserWebViewLayout);
                FindControlsById();
                webView.Settings.JavaScriptEnabled = true;
                webView.SetWebViewClient(new HelloWebViewClient(this));

                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                string token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);
                if(string.IsNullOrWhiteSpace(token))
                {
                    Intent intent;
                    intent = new Intent(this, typeof(AboutAppActivity));
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                    Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.about), Resources.GetString(Resource.String.about)).Commit();
                    this.StartActivity(intent);
                }
                redirectUrl = ConfigEntity.baseURL + "WebAccount/signin?token=" + token;
                webView.LoadUrl(redirectUrl);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, "", "LoggedInUserWebView", null);
                }
            }
        }
        public void FindControlsById()
        {
            try
            {
                webView = FindViewById<WebView>(Resource.Id.webView1);
                imgMenu = FindViewById<ImageView>(Resource.Id.imgMenu);
                imgMenu.Click += ImgMenu_Click;

            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised on Menu icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Android.Support.V7.Widget.PopupMenu menu = new Android.Support.V7.Widget.PopupMenu(this, imgMenu);
                menu.Inflate(Resource.Menu.OptionMenu);
                menu.Show();
                Android.Support.V4.App.Fragment fragment = null;
                menu.MenuItemClick += (s1, arg1) =>
                {
                    Intent intent;
                    switch (arg1.Item.ItemId)
                    {
                        case Resource.Id.logoutID:
                            intent = new Intent(this, typeof(LoginActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.isApproved), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerritory), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.Role), null).Commit();
                            StartActivity(intent);
                            Finish();
                            break;
                        case Resource.Id.termsID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.yes)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.privacyID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.no)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.FAQ:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.FAQ)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.SupportID:
                            intent = new Intent(this, typeof(TermsWebviewActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.support)).Commit();
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.About:
                            intent = new Intent(this, typeof(AboutAppActivity));
                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.about), Resources.GetString(Resource.String.about)).Commit();
                            this.StartActivity(intent);
                            break;
                        default:
                            break;
                    }
                };
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ImgMenu_Click), Resources.GetString(Resource.String.MainActivity), null);
                }
            }
        }

        public override bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            if (keyCode == Keycode.Back && webView.CanGoBack())
            {
                webView.GoBack();
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        public override void OnBackPressed()
        {
            if (webView.CanGoBack())            
                webView.GoBack();            
            else
                base.OnBackPressed();            
        }

        /// <summary>
        /// This internal class is used for behind functionality when user changes from termsLink to Privacy or other
        /// </summary>
        public class HelloWebViewClient : WebViewClient
        {
            OverlayActivity overlay;
            LoggedInUserWebViewActivity activity;

            /// <summary>
            /// This method is to prevent user to move in side the app with out click on i agree or not
            /// </summary>
            /// <param name="activity"></param>
            public HelloWebViewClient(LoggedInUserWebViewActivity activity)
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
                        utility.SaveExceptionHandling(ex, activity.Resources.GetString(Resource.String.HelloWebViewClient), "LoggedInUserWebView", null);
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
