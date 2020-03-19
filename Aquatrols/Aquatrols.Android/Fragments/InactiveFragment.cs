using Android.OS;
using Android.Views;
using Aquatrols.Droid.Activities;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This class is used to bind in active fragment
    /// </summary>
    public class InactiveFragment : Android.Support.V4.App.Fragment
    {
        private MainActivity mainActivity;

        /// <summary>
        /// This method is used to fix the state
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        /// <summary>
        /// This method is used to get detail of main activity to confirm Inactive Fragment
        /// </summary>
        /// <param name="mainActivity"></param>
        public InactiveFragment(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        /// <summary>
        /// This method is used to return root
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View root = inflater.Inflate(Resource.Layout.InactiveFragmentLayout, container, false);
            return root;
        }
    }
}