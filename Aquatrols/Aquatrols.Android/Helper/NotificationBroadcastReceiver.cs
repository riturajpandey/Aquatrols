using Android.App;
using Android.Content;

namespace Aquatrols.Droid.Helper
{
    /// <summary>
    /// This Class file is used for Notification broadcase Receiver
    /// </summary>
    public class NotificationBroadcastReceiver : BroadcastReceiver
    {
        /// <summary>
        /// This override method OnRecive will call whenever user recieve any notification
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            var desc = intent.Extras.GetString("msg");
            AlertDialog.Builder alert = new AlertDialog.Builder(context);
            alert.SetTitle("Notification");
            alert.SetMessage(desc);
            alert.SetPositiveButton("OK", (senderAlert, args) =>
            {
                
            });
            
            Dialog dialog = alert.Create();
            dialog.SetCanceledOnTouchOutside(false);
            dialog.Show();
        }
    }
}