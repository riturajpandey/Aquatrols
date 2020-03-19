using Android.App;
using Android.Content;
using Android.Util;
using Aquatrols.Droid;
using Aquatrols.Droid.Helper;
using Firebase.Iid;

namespace HerdStrong.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug("TOken", "Refreshed token: " + refreshedToken);
            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.deviceToken), refreshedToken).Commit();
        }
    }
}