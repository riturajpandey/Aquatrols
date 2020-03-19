using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using Plugin.GoogleAnalytics;
using System;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to show about detail
    /// </summary>
    public class AboutFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;

        /// <summary>
        /// This method is used to initialize Instance State of about screen
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// This method is used to initialize task of about and implement google analytics
        /// </summary>
        /// <param name="mainActivity"></param>
        public AboutFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
            #region //Google Analytics
            GoogleAnalytics.Current.Config.TrackingId = this.mainActivity.Resources.GetString(Resource.String.TrackingId);
            GoogleAnalytics.Current.Config.AppId = this.mainActivity.ApplicationContext.PackageName;
            GoogleAnalytics.Current.Config.AppName = this.mainActivity.ApplicationInfo.LoadLabel(this.mainActivity.PackageManager).ToString();
            GoogleAnalytics.Current.Config.AppVersion = this.mainActivity.PackageManager.GetPackageInfo(this.mainActivity.PackageName, 0).VersionName;
            GoogleAnalytics.Current.InitTracker();
            GoogleAnalytics.Current.Tracker.SendView("About Page");
            #endregion
        }

        /// <summary>
        /// This method is used to initialize page load value and getting the reference of controls
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.AboutLayoutFragment, container, false);
            try
            {
                LinearLayout lluserInfo = mainActivity.FindViewById<LinearLayout>(Resource.Id.llUserInfo);
                LinearLayout llTextMessage = mainActivity.FindViewById<LinearLayout>(Resource.Id.llTextMessage);
                LinearLayout llHeader = this.mainActivity.FindViewById<LinearLayout>(Resource.Id.llHeader);
                if (llHeader.Visibility == ViewStates.Gone)
                {
                    llHeader.Visibility = ViewStates.Visible;
                }
                ImageView imgTmView = mainActivity.FindViewById<ImageView>(Resource.Id.btnTmView);
                imgTmView.Visibility = ViewStates.Gone;
                TextView txtMessage = mainActivity.FindViewById<TextView>(Resource.Id.txtMessage);
                llTextMessage.Visibility = ViewStates.Visible;
                lluserInfo.Visibility = ViewStates.Visible;
                txtMessage.Text = this.mainActivity.Resources.GetString(Resource.String.AboutusHomeMessage);
                Button btnSendEmail = root.FindViewById<Button>(Resource.Id.btnsendEmail);
                ImageView imgAllproduct = root.FindViewById<ImageView>(Resource.Id.imgProducts);
                ImageView imgAboutus = root.FindViewById<ImageView>(Resource.Id.imgAbout);
                ImageView imgTerritoryManager = root.FindViewById<ImageView>(Resource.Id.imgTManager);
                ImageView imgFacebook = root.FindViewById<ImageView>(Resource.Id.facebook);
                ImageView imgYoutube = root.FindViewById<ImageView>(Resource.Id.youtube);
                ImageView imgRSS = root.FindViewById<ImageView>(Resource.Id.rss);
                ImageView imgTwitter = root.FindViewById<ImageView>(Resource.Id.twitter);
                ImageView imgInstagram = root.FindViewById<ImageView>(Resource.Id.instagram);

                #region
                //click Events
                btnSendEmail.Click += BtnSendEmail_Click;
                imgAboutus.Click += ImgAboutus_Click;
                imgAllproduct.Click += ImgAllproduct_Click;
                imgTerritoryManager.Click += ImgterritoryManager_Click;
                imgFacebook.Click += Imgfb_Click;
                imgTwitter.Click += ImgTwitter_Click;
                imgYoutube.Click += ImgYoutube_Click;
                imgInstagram.Click += ImgInstagram_Click;
                imgRSS.Click += ImgRss_Click;
                #endregion
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.AboutFragment), null);
                }
            }
            return root;
        }

        /// <summary>
        /// On Send Email button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType("text/html");
                intent.PutExtra(Intent.ExtraEmail, new String[] { Resources.GetText(Resource.String.ApproachEmail) });
                intent.PutExtra(Intent.ExtraSubject, string.Empty);
                StartActivity(Intent.CreateChooser(intent, this.mainActivity.Resources.GetString(Resource.String.sendmail)));
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "BtnSendEmail_Click", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// Territory manager button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgterritoryManager_Click(object sender, EventArgs e)
        {
            try
            {
                Android.Support.V4.App.Fragment fragment = new TerritoryManagerFragment(this.mainActivity);
                FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.content_frame, fragment)
                        .Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "ImgterritoryManager_Click", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// Products button click to open products fragment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgAllproduct_Click(object sender, EventArgs e)
        {
            try
            {
                Android.Support.V4.App.Fragment fragment = new AboutProductFragment(this.mainActivity);
                FragmentManager.BeginTransaction()
                   .Replace(Resource.Id.content_frame, fragment)
                   .Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "ImgAllproduct_Click", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// About us button click to open aboutus fragment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgAboutus_Click(object sender, EventArgs e)
        {
            try
            {
                Android.Support.V4.App.Fragment fragment = new AboutusFragment(this.mainActivity);
                FragmentManager.BeginTransaction()
                   .Replace(Resource.Id.content_frame, fragment)
                   .Commit();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "ImgAboutus_Click", "AboutFragment", null);
                }
            }
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
                    utility.SaveExceptionHandling(ex, "Show_Overlay", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// Rss feed Icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgRss_Click(object sender, EventArgs e)
        {
            try
            {
                bool res = Utility.isPackageInstalled(this.mainActivity, Resources.GetString(Resource.String.rsspackage));
                if (res == true)
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(Resources.GetString(Resource.String.rsslink)));
                    intent.SetPackage(Resources.GetString(Resource.String.rsspackage));
                    StartActivity(intent);
                }
                else
                {
                    var uri = Android.Net.Uri.Parse(Resources.GetString(Resource.String.rsslink));
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "ImgRss_Click", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// On Instagram icon click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgInstagram_Click(object sender, EventArgs e)
        {
            try
            {
                bool res = Utility.isPackageInstalled(this.mainActivity, Resources.GetString(Resource.String.instagrampackage));
                if (res == true)
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(Resources.GetString(Resource.String.instagramlink)));
                    intent.SetPackage(Resources.GetString(Resource.String.instagrampackage));
                    StartActivity(intent);
                }
                else
                {
                    var uri = Android.Net.Uri.Parse(Resources.GetString(Resource.String.instagramlink));
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "ImgInstagram_Click", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// On youtube icon  click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgYoutube_Click(object sender, EventArgs e)
        {
            try
            {
                bool res = Utility.isPackageInstalled(this.mainActivity, Resources.GetString(Resource.String.youtubepackage));
                if (res == true)
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(Resources.GetString(Resource.String.youtubelink)));
                    intent.SetPackage(Resources.GetString(Resource.String.youtubepackage));
                    StartActivity(intent);

                }
                else
                {
                    var uri = Android.Net.Uri.Parse(Resources.GetString(Resource.String.youtubelink));
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "ImgYoutube_Click", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// On twitter icon  click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgTwitter_Click(object sender, EventArgs e)
        {
            try
            {
                bool res = Utility.isPackageInstalled(this.mainActivity, Resources.GetString(Resource.String.twitterpackage));
                if (res == true)
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(Resources.GetString(Resource.String.twitterlink)));
                    intent.SetPackage(Resources.GetString(Resource.String.twitterpackage));
                    StartActivity(intent);
                }
                else
                {
                    var uri = Android.Net.Uri.Parse(Resources.GetString(Resource.String.twitterlink));
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "ImgTwitter_Click", "AboutFragment", null);
                }
            }
        }

        /// <summary>
        /// On facebook icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Imgfb_Click(object sender, EventArgs e)
        {
            try
            {
                Intent facebookIntent = new Intent(Intent.ActionView);
                String facebookUrl = Utility.getFacebookPageURL(this.mainActivity);
                facebookIntent.SetData(Android.Net.Uri.Parse(facebookUrl));
                StartActivity(facebookIntent);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(mainActivity))
                {
                    utility.SaveExceptionHandling(ex, "Imgfb_Click", "AboutFragment", null);
                }
            }
        }
    }
}