// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Aquatrols.iOS
{
    [Register ("AbpoutAppViewController")]
    partial class AbpoutAppViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView AboutUsFlowImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton imgBack { get; set; }

        [Action ("ImgBack_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ImgBack_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AboutUsFlowImage != null) {
                AboutUsFlowImage.Dispose ();
                AboutUsFlowImage = null;
            }

            if (imgBack != null) {
                imgBack.Dispose ();
                imgBack = null;
            }
        }
    }
}