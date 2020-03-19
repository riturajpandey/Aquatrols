using System;

using UIKit;

namespace Aquatrols.iOS.Controllers
{
    public partial class RegistrationSuccessViewController : UIViewController
    {
        public RegistrationSuccessViewController() : base("RegistrationSuccessViewController", null)
        {
        }
        public RegistrationSuccessViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void BtnOk_TouchUpInside(UIButton sender)
        {
            this.NavigationController.PopToRootViewController(true);
        }
    }
}

