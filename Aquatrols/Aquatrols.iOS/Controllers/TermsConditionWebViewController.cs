using Aquatrols.iOS.Helper;
using CoreGraphics;
using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file is used for Terms and condition screen
    /// </summary>
    public partial class TermsConditionWebViewController : UIViewController
    {
        private Utility utility = Utility.GetInstance;
        public string pdfPath;
        public string visitWebPageLabel;
        private LoadingOverlay loadPop;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.TermsConditionWebViewController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public TermsConditionWebViewController (IntPtr handle) : base (handle)
        {
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
            string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
            if (!String.IsNullOrEmpty(exception))
            {
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.TermsConditionWebView, exception);
            }
            BackArrowClick();
            SetUISize();
            int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
            // set font for UILabel
            Utility.SetFontsforHeader(new UILabel[] { lblTermsAndConditions, }, null, Constant.lstDigit[11],viewWidth);
           // Perform any additional setup after loading the view, typically from a nib.
        }
        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            // call method
            WebViewTerms();
        }
        /// <summary>
        /// Set UI size sepcified in iPhone X.
        /// </summary>
        public void SetUISize()
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height;
                if (viewWidth == (int)DeviceScreenSize.ISix)
                {
                    if (viewHeight == Constant.lstDigit[31])
                    {
                        TermConditionsView.Frame = new CGRect(TermConditionsView.Frame.X, TermConditionsView.Frame.Y + Constant.lstDigit[10], TermConditionsView.Frame.Width, TermConditionsView.Frame.Height);
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetUISize, AppResources.TermsConditionWebView, null);
            }
        }
        /// <summary>
        /// to show pdf in web view
        /// </summary>
        protected async void WebViewTerms()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                string pdfValue = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.PDFValue);
                if (pdfValue == Constant.lstDigitString[0])
                {
                    var url = new NSUrl(AppResources.pdfReaderUrl+pdfPath);
                    var request = new NSUrlRequest(url);
                    WvTerms.LoadRequest(request);
                    lblTermsAndConditions.Text = AppResources.SDS;
                }
                else if (pdfValue == Constant.lstDigitString[1])
                {
                    var url = new NSUrl(AppResources.TermsConditions);
                    var request = new NSUrlRequest(url);
                    WvTerms.LoadRequest(request);
                    lblTermsAndConditions.Text = AppResources.RequireTermsConsLabel;
                }
                else if (pdfValue == Constant.lstDigitString[2])
                {
                    var url = new NSUrl(AppResources.pdfReaderUrl+pdfPath);
                    var request = new NSUrlRequest(url);
                    WvTerms.LoadRequest(request);
                    lblTermsAndConditions.Text = AppResources.RequireLabel;
                }
                else if (pdfValue == Constant.lstDigitString[3])
                {
                    var url = new NSUrl(pdfPath);
                    var request = new NSUrlRequest(url);

                    WvTerms.LoadRequest(request);
                    lblTermsAndConditions.Text = visitWebPageLabel;
                }
                else if (pdfValue == Constant.lstDigitString[4])
                {
                    var url = new NSUrl(AppResources.Policy);
                    var request = new NSUrlRequest(url);
                    WvTerms.LoadRequest(request);
                    lblTermsAndConditions.Text = AppResources.RequirePrivacylbl;
                }
                else if (pdfValue == Constant.lstDigitString[5])
                {
                    var url = new NSUrl(AppResources.FAQ);
                    var request = new NSUrlRequest(url);
                    WvTerms.LoadRequest(request);
                    lblTermsAndConditions.Text = AppResources.RequireFaqLbl;
                }
                else if (pdfValue == Constant.lstDigitString[6])
                {
                    var url = new NSUrl(AppResources.Support);
                    var request = new NSUrlRequest(url);
                    WvTerms.LoadRequest(request);
                    lblTermsAndConditions.Text = AppResources.RequireSupportlbl;
                }
                WvTerms.LoadFinished += (object sender, EventArgs e) => {
                    loadPop.Hide();
                };
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.WebViewTerms, AppResources.TermsConditionWebView, null);
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
            }
        }
        /// <summary>
        /// Gesture implementation for back arrow click
        /// back arrow click to navigate previous page.
        /// </summary>
        public void BackArrowClick()
        {
            UITapGestureRecognizer imgBackArrowClicked = new UITapGestureRecognizer(() =>
            {
                NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.isTermsConditions);
                this.NavigationController.PopViewController(true);
            });
            imgBackClick.UserInteractionEnabled = true;
            imgBackClick.AddGestureRecognizer(imgBackArrowClicked);
        }

        /// <summary>
        /// Dids the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}