using Android.App;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Aquatrols.Droid.Helper;
using System;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used to show loader to user
    /// </summary>
    public class OverlayActivity : Dialog
    {
        public Activity context;

        /// <summary>
        /// This method is used to show loader to user
        /// </summary>
        /// <param name="a"></param>
        public OverlayActivity(Activity a)
            : base(a)
        {
            this.context = a;
        }

        /// <summary>
        /// This method is used to work behind page loader
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                Window.RequestFeature(WindowFeatures.NoTitle);
                SetContentView(Resource.Layout.OverlayLayout);
                Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.OnCreate), context.Resources.GetString(Resource.String.OverlayActivity), null);
                }
            }
        }
    }
}