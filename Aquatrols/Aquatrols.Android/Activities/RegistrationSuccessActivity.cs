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
    [Activity(Label = "RegistrationSuccessActivity")]
    public class RegistrationSuccessActivity : Activity
    {
        private Button btnCancel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegistrationSuccessLayout);
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
        }
        public void FindControlsById()
        {
            try
            {
                btnCancel = FindViewById<Button>(Resource.Id.btnCancelForEmail);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.TMViewHomeActivity), null);
                }
            }
            #region //Event handlers           
            btnCancel.Click += BtnCancel_Click;
            #endregion
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            StartActivity(intent);
        }
    }
}