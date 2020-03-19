using Aquatrols.iOS.Helper;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using Plugin.GoogleAnalytics;
using System;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.Controllers
{
    /// <summary>
    /// This forgot password controller is used for set up the new password for user. 
    /// </summary>
    public partial class ForgotPasswordController : UIViewController
    {
        private LoadingOverlay loadPop;
        private Utility utility = Utility.GetInstance;
        public ForgotPasswordController(IntPtr handle) : base(handle)
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
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.ForgotPassword, exception); // exception handling 
            }
            PasswordResetClick();
            ConfirmPasswordResetClick();
            ResendPasswordClick();
            AddDoneButton();
            DismissKeyboardClick();
            SetLogoSize();
            AddDoneToNumericButton();
            SetFonts();
            try
            {
                // implement google analytics
                GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                GoogleAnalytics.Current.InitTracker();
                GoogleAnalytics.Current.Tracker.SendView(AppResources.ForgotPassword);
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.ForgotPassword, null);
            }
            // Perform any additional setup after loading the view, typically from a nib.
        }
        /// <summary>
        /// Set the fonts of UILabel,UITextField and UIButton.
        /// set the padding of UITextField
        /// </summary>
        public void SetFonts()
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetPadding(new UITextField[] { txtEmailAddress, txtOTPCode, txtPassword, txtConfirmPassword }, Constant.lstDigit[4]);
                Utility.SetFonts(null, new UILabel[] { lblEmailAddress, lblVerific, lblEmail }, new UITextField[] { txtEmailAddress, txtOTPCode, txtPassword, txtConfirmPassword, }, Constant.lstDigit[11], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblSendVerificationCode, lblVerification, lblresetPassword }, new UIButton[] { btnCancelEmailInput, btnNextEmailInput, btnCancelOTP, btnNextOTP, btnCancelResetPassword, btnReset }, Constant.lstDigit[11], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblcode, lblShare, lblReceive, lblResendPassword }, null, Constant.lstDigit[9], viewWidth);
                Utility.SetFonts(null, new UILabel[] { lblCommentPassword }, null, Constant.lstDigit[8], viewWidth);
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.ForgotPassword, null);
            }
        }
        /// <summary>
        /// Set UI in iPhone X.
        /// </summary>
        public void SetLogoSize()
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width; // get the width the screen
                int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height; // get the height the screen.
                if (viewWidth == (int)DeviceScreenSize.ISix)
                {
                    if (viewHeight == Constant.lstDigit[31]) // check condition on iPhone X
                    {
                        ForgotPasswordView.Frame = new CGRect(ForgotPasswordView.Frame.X, ForgotPasswordView.Frame.Y + 40, ForgotPasswordView.Frame.Width, ForgotPasswordView.Frame.Height);
                    }
                }  
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.setLogoSize, AppResources.ForgotPassword, null); // exception handling 
            }
        }
        /// <summary>
        /// Hit Validate User API
        /// Validate the user to enter email is exist or not if exist then send otp to user mail id 
        /// </summary>
        protected async void Next()
        {
            loadPop = new LoadingOverlay(View.Frame);
            try
            {
                if (String.IsNullOrEmpty(txtEmailAddress.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireEmailAddress, Constant.durationOfToastMessage).Show();
                }
                else
                {
                    this.View.Add(loadPop);
                    if (utility.IsValidEmail(txtEmailAddress.Text.Trim()))
                    {
                        ValidateUserRequestEntity validateUserRequestEntity = new ValidateUserRequestEntity();
                        validateUserRequestEntity.username = txtEmailAddress.Text.Trim();
                        bool isConnected = utility.CheckInternetConnection();
                        if (isConnected)
                        {
                            ValidateUserResponseEntity validateUserResponseEntity = await utility.ValidateUser(validateUserRequestEntity);
                            if (validateUserResponseEntity.operationStatus.ToLower() == AppResources.success)
                            {
                                NSUserDefaults.StandardUserDefaults.SetString(validateUserResponseEntity.userId, AppResources.userId);
                                NSUserDefaults.StandardUserDefaults.SetString(validateUserRequestEntity.username, AppResources.userName);
                                lblEmail.Text = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userName);
                                EmailInputView.Hidden = true;
                                OTPView.Hidden = false;
                            }
                            else
                            {
                                Toast.MakeText(validateUserResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                            }
                        }
                        else
                        {
                            Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.InvalidEmail, Constant.durationOfToastMessage).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.Next, AppResources.ForgotPassword, null);
            }
            finally
            {
                loadPop.Hide(); 
            }
        }
        /// <summary>
        /// Hit Validate OTP API
        /// Verify the OTP who entered the user 
        /// </summary>
        protected async void OTP()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                if (String.IsNullOrEmpty(txtOTPCode.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireOTP, Constant.durationOfToastMessage).Show();
                }
                else
                {
                    ValidateOTPRequestEntity validateOTPRequestEntity = new ValidateOTPRequestEntity();
                    validateOTPRequestEntity.UserId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
                    validateOTPRequestEntity.OTP = Convert.ToInt32(txtOTPCode.Text.Trim());
                        bool isConnected = utility.CheckInternetConnection();
                        if (isConnected)
                        {
                        ValidateOTPResponseEntity validateOTPResponseEntity = await utility.ValidateOTP(validateOTPRequestEntity);
                        if (validateOTPResponseEntity.operationStatus.ToLower() == AppResources.success)
                        {
                            NSUserDefaults.StandardUserDefaults.SetString(validateOTPResponseEntity.token,AppResources.token);
                            Toast.MakeText(validateOTPResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                            EmailInputView.Hidden = true;
                            OTPView.Hidden = true;
                            ResetPasswordView.Hidden = false;
                        }
                        else
                        {
                            Toast.MakeText(validateOTPResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.OTP, AppResources.ForgotPassword, null); // exception handling 
            }
            finally
            {
                loadPop.Hide(); // hide loader 
            }
        }
        /// <summary>
        /// Hit Reset Password API
        /// Reset Password of the user 
        /// </summary>
        protected async void Reset()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequirePassword, Constant.durationOfToastMessage).Show();
                }
                else
                {
                    if (utility.IsValidPassword(txtPassword.Text.Trim()))
                    {
                        if (String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                        {
                            Toast.MakeText(AppResources.RequireCPassword, Constant.durationOfToastMessage).Show();
                        }
                        else
                        {
                            if (!txtPassword.Text.Equals(txtConfirmPassword.Text.Trim()))
                            {
                                Toast.MakeText(AppResources.InvalidConfirmPassword, Constant.durationOfToastMessage).Show();
                            }
                        }
                        ResetPasswordRequestEntity resetPasswordRequestEntity = new ResetPasswordRequestEntity();
                        resetPasswordRequestEntity.userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
                        resetPasswordRequestEntity.password = txtPassword.Text.Trim();
                        resetPasswordRequestEntity.confirmPassword = txtConfirmPassword.Text.Trim();
                        resetPasswordRequestEntity.token =NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                        bool isConnected = utility.CheckInternetConnection();
                        if (isConnected)
                        {
                            ResetPasswordResponseEntity resetPasswordResponseEntity = await utility.ResetPassword(resetPasswordRequestEntity);
                            if (resetPasswordResponseEntity.operationStatus.ToLower() == AppResources.success)
                            {
                                Toast.MakeText(resetPasswordResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                                this.NavigationController.PopViewController(true);
                            }
                            else
                            {
                                Toast.MakeText(resetPasswordResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                            }
                        }
                        else
                        {
                            Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.InvalidPassword, Constant.durationOfToastMessage).Show(); 
                    }
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.Reset, AppResources.ForgotPassword, null);
            }
            finally
            {
                loadPop.Hide();
            } 
        }
        /// <summary>
        /// next button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnNextEmailInput_TouchUpInside(UIButton sender)
        {
            txtEmailAddress.ResignFirstResponder();
            Next();
        }
        /// <summary>
        /// Cancel button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnCancelEmailInput_TouchUpInside(UIButton sender)
        {
            this.NavigationController.PopViewController(true);
        }
        /// <summary>
        /// Next Otp button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnNextOTP_TouchUpInside(UIButton sender)
        {
            txtOTPCode.ResignFirstResponder();
            OTP();
        }
        /// <summary>
        /// Cancel otp button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnCancelOTP_TouchUpInside(UIButton sender)
        {
            EmailInputView.Hidden = false;
            OTPView.Hidden = true;
            txtOTPCode.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
        }
        /// <summary>
        /// Reset button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnReset_TouchUpInside(UIButton sender)
        {
            Reset();
        }
        /// <summary>
        /// cancel reset password button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnCancelResetPassword_TouchUpInside(UIButton sender)
        {
            this.NavigationController.PopViewController(true);
        }
        /// <summary>
        /// Changed event to text box on reset password
        /// hide or show eye 
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtResetPassword_EditingChanged(UITextField sender)
        {
            if (!String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                imgResetPasswordEye.Hidden = false;
            }
            else
            {
                imgResetPasswordEye.Hidden = true;
            }
        }
        /// <summary>
        /// Changed event to text box on reset confirm password 
        /// hide or show eye
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtResetCPassword_EditingChanged(UITextField sender)
        {
            if (!String.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                imgResetConfirmPasswordEye.Hidden = false;
            }
            else
            {
                imgResetConfirmPasswordEye.Hidden = true;
            }
        }
        /// <summary>
        /// Click the visible or invisible eye reset password.
        /// </summary>
        public void PasswordResetClick()
        {
            bool imageshow = false;
            UITapGestureRecognizer passwordResetImgClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (imageshow == false)
                    {
                        imageshow = true;
                        imgResetPasswordEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                        txtPassword.SecureTextEntry = false;
                    }
                    else
                    {
                        imageshow = false;
                        imgResetPasswordEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                        txtPassword.SecureTextEntry = true;
                    } 
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.PasswordResetClick, AppResources.ForgotPassword, null);
                }
            });
            imgResetPasswordEye.UserInteractionEnabled = true;
            imgResetPasswordEye.AddGestureRecognizer(passwordResetImgClicked);
        }
        /// <summary>
        /// Click the visible or invisible eye reset confirm password.
        /// </summary>
        public void ConfirmPasswordResetClick()
        {
            bool imageshow = false;
            UITapGestureRecognizer ConfirmPasswordResetImgClicked = new UITapGestureRecognizer(() =>
            {
                try
                {
                    if (imageshow == false)
                    {
                        imageshow = true;
                        imgResetConfirmPasswordEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                        txtConfirmPassword.SecureTextEntry = false;
                    }
                    else
                    {
                        imageshow = false;
                        imgResetConfirmPasswordEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                        txtConfirmPassword.SecureTextEntry = true;
                    }  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.ConfirmPasswordResetClick, AppResources.ForgotPassword, null);
                }
            });
            imgResetConfirmPasswordEye.UserInteractionEnabled = true;
            imgResetConfirmPasswordEye.AddGestureRecognizer(ConfirmPasswordResetImgClicked);
        }
        /// <summary>
        /// Click the resend password
        /// </summary>
        public void ResendPasswordClick()
        {
            UITapGestureRecognizer resendPasswordClicked = new UITapGestureRecognizer(() =>
            {
                Next(); 
            });
            lblResendPassword.UserInteractionEnabled = true;
            lblResendPassword.AddGestureRecognizer(resendPasswordClicked);
        }
        /// <summary>
        /// Dismiss the keyboard click on forgot password view.
        /// </summary>
        public void DismissKeyboardClick()
        {
            UITapGestureRecognizer dismissKeyboardClicked = new UITapGestureRecognizer(() =>
            {
                utility.DismissKeyboardOnTap(new UIView[] { ForgotPasswordView });
            });
            ForgotPasswordView.UserInteractionEnabled = true;
            ForgotPasswordView.AddGestureRecognizer(dismissKeyboardClicked);
        }
        /// <summary>
        /// Add done button on keyboard at runtime
        /// </summary>
        public void AddDoneButton()
        {
            UITextField[] textFields = new UITextField[] { txtEmailAddress,txtPassword, txtConfirmPassword };
            Utility.AddDoneButtonToKeyboard(textFields);
        }
        /// <summary>
        /// Add done to numeric button at runtime.
        /// </summary>
        public void AddDoneToNumericButton()
        {
            UITextField[] textFields = new UITextField[] { txtOTPCode };
            Utility.AddDoneButtonToNumericKeyboard(textFields);
        }
        /// <summary>
        /// Did the receive memory warning.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

