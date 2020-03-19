using Android;
using Android.App;
using Android.Content;
using Android.Util;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using Firebase.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HerdStrong.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = Constants.MyFirebaseMsgService;
        const string NOTIFICATION_ACTION = Constants.receiverValue;
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                Log.Debug(TAG, "From: " + message.From);
                Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                if (AppInForeground())
                {
                    Intent alerIntent = new Intent(NOTIFICATION_ACTION);
                    alerIntent.PutExtra("msg", message.GetNotification().Title + "\n" + message.GetNotification().Body);
                    Application.Context.SendBroadcast(alerIntent);
                }
                else
                {
                    SendNotification(message.GetNotification().Body, message.Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public bool AppInForeground()
        {
            try
            {
                ArrayList runningactivities = new ArrayList();
                Context context = Android.App.Application.Context;
                ActivityManager activityManager = (ActivityManager)context.GetSystemService(ActivityService);
                IList<ActivityManager.RunningAppProcessInfo> appProcesses = activityManager.RunningAppProcesses;
                if (appProcesses == null)
                {
                    return false;
                }
                string packageName = context.PackageName;
                foreach (ActivityManager.RunningAppProcessInfo appProcess in appProcesses)
                {
                    if (appProcess.Importance == Importance.Foreground && appProcess.ProcessName == packageName)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            //int id = Resource.Drawable.AquatrolsLogoCircle;
            var intent = new Intent(this, typeof(SplashActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (string key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.AlertDarkFrame)
                .SetContentTitle("Aquatrols")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}