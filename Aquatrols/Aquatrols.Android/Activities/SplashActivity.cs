using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Aquatrols.Droid.Helper;
using System;
using System.Threading;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This splash screen is used to user as a launch screen
    /// </summary>
    [Activity(MainLauncher = true)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "approach.aquatrols.com",
              DataPathPrefix = "/WebAccount/Signup",
              AutoVerify = true)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "approach.aquatrols.com",
              DataPathPrefix = "/Home/Index",
              AutoVerify = true)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "approach.aquatrols.com",
              DataPathPrefix = "/Profile/Index",
              AutoVerify = true)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "approach.aquatrols.com",
              DataPathPrefix = "/Redeem/Index",
              AutoVerify = true)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "approach.aquatrols.com",
              DataPathPrefix = "/EOP/Index",
              AutoVerify = true)]
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "approach.aquatrols.com",
              DataPathPrefix = "/Product",
              AutoVerify = true)]
    public class SplashActivity : Activity
    {
        const string NOTIFICATION_ACTION = Constants.receiverValue;
        NotificationBroadcastReceiver notificationBroadcastReceiver = new NotificationBroadcastReceiver();

        /// <summary>
        /// This method is used to initialize page load value and chesk if internally every thing is ok then pass to next screen
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.SplashLayout);
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode.       
                if (Intent.Data != null && Intent.Data.Path.Contains(Resources.GetString(Resource.String.Signup)))
                {
                    ///handle deep link for signup
                    StartActivity(typeof(SignUpActivity));
                    Finish();
                }
                else
                {
                    Helper.Utility.sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                    ThreadPool.QueueUserWorkItem(o => DoDelay());
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.SplashActivity), null);
                }
            }
        }

        /// <summary>
        /// This Method is used for delay till 2000 ms
        /// </summary>
        void DoDelay()
        {
            try
            {
                Thread.Sleep(2000);
                StartActivity(typeof(LoginActivity));
                Finish();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.DoDelay), Resources.GetString(Resource.String.SplashActivity), null);
                }
            }
        }

        /// <summary>
        /// System On Start method
        /// </summary>
        protected override void OnStart()
        {
            try
            {
                base.OnStart();
                if (Intent.Data != null && !(Intent.Data.Path.Equals(Resources.GetString(Resource.String.Signup))))
                {
                    try
                    {
                        Android.Net.Uri uri = Intent.Data;
                        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.LinkData), uri.Path).Commit();
                    }
                    catch (Exception ex)
                    {
                        using (Utility utility = new Utility(this))
                        {
                            utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnStart), Resources.GetString(Resource.String.SplashActivity), null);
                        }
                    }
                }
                IntentFilter intentFilter = new IntentFilter(NOTIFICATION_ACTION);
                RegisterReceiver(notificationBroadcastReceiver, intentFilter);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnStart), Resources.GetString(Resource.String.SplashActivity), null);
                }
            }
        }

        /// <summary>
        /// System On Stop method
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                base.OnStop();
                UnregisterReceiver(notificationBroadcastReceiver);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnStop), Resources.GetString(Resource.String.SplashActivity), null);
                }
            }
        }

    }
}