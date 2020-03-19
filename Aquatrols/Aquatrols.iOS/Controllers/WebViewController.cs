using Aquatrols.iOS.Controllers;
using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using Plugin.GoogleAnalytics;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS
{
    /// <summary>
    /// This class file is used for Webview Controller
    /// </summary>
    public partial class WebViewController : UIViewController
    {
        private Utility utility = Utility.GetInstance;
        public SignUpRequestEntity signUpRequestEntity;
        private bool imageshow = false;
        private LoadingOverlay loadPop;
        private bool flag = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.WebViewController"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public WebViewController(IntPtr handle) : base(handle)
        {
            
        }
        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            // Perform any additional setup after loading the view, typically from a nib.
            base.ViewDidLoad();
            //Check exception data if net is not copnnected and stored the exception in nsuserdefault if user redirect the page first hit the exception in previous saved in shared preferences.
            string exception = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.Exception);
            if (!String.IsNullOrEmpty(exception))
            {
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.WebView, exception);
            }
            CheckBoxClick();
            BackArrowClick();
            SetUISize();
            GetRequestTokenForSignUp();
            NSUserDefaults.StandardUserDefaults.SetBool(false, AppResources.isCheckbox);
            int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
            //set fonts for UILabel and UiButton
            Utility.SetFonts(null, new UILabel[] { lblAgree }, null, Constant.lstDigit[8],viewWidth);
            Utility.SetFontsforHeader(new UILabel[] { lblAffidavit, }, new UIButton[] { btnReject, btnSignUp }, Constant.lstDigit[11],viewWidth);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                    GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                    GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                    GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                    GoogleAnalytics.Current.InitTracker();
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.Affidavit);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.WebView, null);
            }
        }
        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
        {
            // call method
            WebViewAffadavit();
        }
        /// <summary>
        /// to show affidavit detail in web view.
        /// </summary>
        protected async void WebViewAffadavit()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                var url = new NSUrl(AppResources.Affidavit1);
                var request = new NSUrlRequest(url);
                WvAffadavit.LoadRequest(request);
                WvAffadavit.LoadFinished += (object sender, EventArgs e) => {
                    loadPop.Hide();
                };
            }
            catch (Exception ex)
            {
                if (loadPop != null)
                {
                    loadPop.Hide();
                }
                utility.SaveExceptionHandling(ex, AppResources.WebViewAffadavit, AppResources.WebView, null);
            }  
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
                        ChildViewAffidavit.Frame = new CGRect(ChildViewAffidavit.Frame.X, ChildViewAffidavit.Frame.Y + Constant.lstDigit[10], ChildViewAffidavit.Frame.Width, ChildViewAffidavit.Frame.Height - +Constant.lstDigit[12]);
                    }
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetUISize, AppResources.WebView, null);
            }
        }
        /// <summary>
        /// Hit request token API
        /// Get the request token for sign up.
        /// </summary>
        public async void GetRequestTokenForSignUp()
        {
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    LoginRequestEntity loginRequestEntity = new LoginRequestEntity();
                    loginRequestEntity.username = AppResources.SignUpUserName;
                    loginRequestEntity.password = AppResources.SignUpPassword;
                    AccessTokenResponseEntity accessTokeResponseEntity = await utility.GetRequestToken(loginRequestEntity);
                    if (accessTokeResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                    {
                        NSUserDefaults.StandardUserDefaults.SetString(accessTokeResponseEntity.token, AppResources.tokenSignUp);
                    }
                    else
                    {
                        Toast.MakeText(accessTokeResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.GetRequestTokenForSignUp, AppResources.WebView, null);
            } 
        }
        /// <summary>
        /// Hit the sign up API.
        /// Sign up
        /// </summary>
        protected async void HitSignUpAPI()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            await Task.Delay(Constant.lstDigit[17]);
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    flag = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.isCheckbox);
                    if (flag == true)
                    {
                        string tokenSignUp= NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.tokenSignUp);

                        if (!string.IsNullOrWhiteSpace(signUpRequestEntity.CourseAffiliationProofFilePath))
                            signUpRequestEntity.CourseAffiliationProofFilePath = await UploadImageToBlob(tokenSignUp, signUpRequestEntity.CourseAffiliationProofFilePath);

                        SignUpResponseEntity signUpResponseEntity = await utility.RegisterUser(signUpRequestEntity,tokenSignUp);
                        if (signUpResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                        {
                            Toast.MakeText(signUpResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                            NSUserDefaults.StandardUserDefaults.SetString(signUpRequestEntity.email, AppResources.userName);
                            NSUserDefaults.StandardUserDefaults.SetString(signUpRequestEntity.password, AppResources.password);
                            signUpRequestEntity.courseId = string.Empty;
                            NSUserDefaults.StandardUserDefaults.SetBool(false,AppResources.isCheckbox);
                            DashboardController dashboardController = (DashboardController)Storyboard.InstantiateViewController(AppResources.DashboardController);
                            this.NavigationController.PushViewController(dashboardController, true);
                        }
                        else
                        {
                            Toast.MakeText(signUpResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                            NSUserDefaults.StandardUserDefaults.SetBool(true, AppResources.isCheckbox);
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.RequireCheckBox, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.HitSignUpAPI, AppResources.WebView, null);
            }
            finally
            {
               loadPop.Hide();
            }
          }
        /// <summary>
        /// signup button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnSignUp_TouchUpInside(UIButton sender)
        {
            var signupRequestEntity = signUpRequestEntity;
            HitSignUpAPI();
        }
        /// <summary>
        /// reject button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnReject_TouchUpInside(UIButton sender)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(false, AppResources.isCheckbox);
            UIViewController [] view=NavigationController.ViewControllers;
            foreach(UIViewController item in view)
            {
                if(item is SignUpController)
                {
                    this.NavigationController.PopToViewController(item, true);
                    break;
                }
            }
        }
        /// <summary>
        /// Gesture implementation for check box click.
        /// </summary>
        public void CheckBoxClick()
        {
            UITapGestureRecognizer imgCheckBoxClicked = new UITapGestureRecognizer(() =>
            {
                if (imageshow == false)
                {
                    imageshow = true;
                    NSUserDefaults.StandardUserDefaults.SetBool(imageshow, AppResources.isCheckbox);
                    imgCheckBox.Image = UIImage.FromBundle(AppResources.ImgCheckbox);
                }
                else
                {
                    imageshow = false;
                    NSUserDefaults.StandardUserDefaults.SetBool(imageshow, AppResources.isCheckbox);
                    imgCheckBox.Image = UIImage.FromBundle(AppResources.ImgUnCheckbox);
                }
            });
            imgCheckBox.UserInteractionEnabled = true;
            imgCheckBox.AddGestureRecognizer(imgCheckBoxClicked);
        }
        /// <summary>
        /// back arrow click to navigate previous page.
        /// </summary>
        public void BackArrowClick()
        {
            UITapGestureRecognizer imgBackArrowClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
                NSUserDefaults.StandardUserDefaults.SetBool(false, AppResources.isCheckbox);
            });
            imgBackClick.UserInteractionEnabled = true;
            imgBackClick.AddGestureRecognizer(imgBackArrowClicked);
        }

        private async Task<string> UploadImageToBlob(string signupToken, string imgPath)
        {
            try
            {
                UIImage image;
                Stream imgStream;
                UploadBlobImageResponseEntity urlResponse = null;
                using (var data = NSData.FromFile(imgPath))
                    image = UIImage.LoadFromData(data);

                NSData imgData = image.AsJPEG(1);
                imgStream = imgData.AsStream(); 

                string imageName = imgPath.Split('/').LastOrDefault();
                string imageType = imageName.Split('.').LastOrDefault();

                string imageUrl = await utility.UploadImageToBlobAsync(imgStream, imageName, signupToken, imageType);
                if (imageUrl != null)
                {
                    urlResponse = JsonConvert.DeserializeObject<UploadBlobImageResponseEntity>(imageUrl);
                    return urlResponse.filePath;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}