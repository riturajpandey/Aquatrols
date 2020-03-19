using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Helper;

namespace Aquatrols.Droid.Activities
{
    [Activity(Label = "AboutAppActivity")]
    public class AboutAppActivity : Activity
    {
        private TextView txtHeading;
        private ImageView imgMenu, imgBack, imgProcessFlow;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.AboutAppLayout);
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode.
                // Handle exception handling  
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                FindControlsById();
                Utility.SettingFonts(null, new TextView[] { txtHeading }, null);
                imgProcessFlow.SetImageResource(Resource.Drawable.aboutapproachart);
                // Create your application here
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.TMViewHomeActivity), null);
                }
            }
        }
        public void FindControlsById()
        {
            try
            {
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
                imgMenu = FindViewById<ImageView>(Resource.Id.imgMenu);
                imgProcessFlow = FindViewById<ImageView>(Resource.Id.imgAboutFlow);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TMViewHomeActivity), null);
                }
            }
            #region //Event handlers           
            imgMenu.Click += ImgMenu_Click;
            imgBack.Click += ImgBack_Click;
            #endregion
        }

        private void ImgBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        /// <summary>
        ///raised on Menu icon click
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
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerms), Resources.GetString(Resource.String.support)).Commit();
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ImgMenu_Click), Resources.GetString(Resource.String.TMViewHomeActivity), null);
                }
            }
        }
    }
}