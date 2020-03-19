using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used for home screen
    /// </summary>
    [Activity]
    public class TMViewHomeActivity : Activity
    {
        private TextView txtHeading;
        private ImageView imgMenu, btnTmCourse, btnTmDistributor, btnTmDashboard, btnTmSnapShot;

        /// <summary>
        /// This method is used to initialize page load value and set fonts to view
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.TMViewHomeLayout);
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

        /// <summary>
        /// Method to get reference of controls 
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
                btnTmCourse = FindViewById<ImageView>(Resource.Id.btnTmCourse);
                btnTmDashboard = FindViewById<ImageView>(Resource.Id.btnTmDashboard);
                btnTmDistributor = FindViewById<ImageView>(Resource.Id.btnTmDistributor);
                btnTmSnapShot = FindViewById<ImageView>(Resource.Id.btnTmSnapShot);
                imgMenu = FindViewById<ImageView>(Resource.Id.imgMenu);

            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TMViewHomeActivity), null);
                }
            }
            #region //Event handlers
            btnTmCourse.Click += BtnTmCourse_Click;
            btnTmDistributor.Click += BtnTmDistributor_Click;
            btnTmDashboard.Click += BtnTmDashboard_Click;
            btnTmSnapShot.Click += BtnTmSnapShot_Click;
            imgMenu.Click += ImgMenu_Click;
            #endregion
        }

        /// <summary>
        /// Event handler Method implementation for Snapshot button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTmSnapShot_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TmSnapshotActivity));
            StartActivity(intent);
        }

        /// <summary>
        /// Event handler Method implementation for Dashboard button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTmDashboard_Click(object sender, EventArgs e)
        {
            Finish();
        }

        /// <summary>
        /// Event handler Method implementation for Disributor button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTmDistributor_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TMDistributorHomeActivity));
            StartActivity(intent);
        }

        /// <summary>
        /// Event handler Method implementation for course button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTmCourse_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TMCourseHomeActivity));
            StartActivity(intent);
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ImgMenu_Click), Resources.GetString(Resource.String.TMViewHomeActivity), null);
                }
            }
        }

        /// <summary>
        /// Device Back button press Event
        /// </summary>
        public override void OnBackPressed()
        {
            Finish();
        }
    }
}