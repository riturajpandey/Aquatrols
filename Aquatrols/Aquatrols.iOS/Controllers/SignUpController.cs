    using Aquatrols.iOS.Controllers;
    using Aquatrols.iOS.Helper;
    using Aquatrols.iOS.TableVewSource;
    using Aquatrols.Models;
    using CoreGraphics;
    using Foundation;
    using Plugin.GoogleAnalytics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ToastIOS;
    using UIKit;
    using static Aquatrols.Models.ConfigEntity;

    namespace Aquatrols.iOS
    {
        /// <summary>
        /// this class file is used for sign up screen
        /// </summary>
        public partial class SignUpController : UIViewController
        {
            private Utility utility = Utility.GetInstance;
            private LoadingOverlay loadPop;
            private List<CourseEntity> courseEntity = null;
            private List<DistinctDistributorEntity> lstDistinctDistributorEntities = null;
            private SignUpRequestEntity signUpRequestEntity = new SignUpRequestEntity();
            bool isLengthOne = false;
            bool isLengthFour = false;
            bool isLengthEight = false;
            UIImagePickerController picker;
            Stream imageStriem = null;
            NSUrl imagePath = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.SignUpController"/> class.
            /// </summary>
            /// <param name="handle">Handle.</param>
            public SignUpController(IntPtr handle) : base(handle)
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
                    utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.SignUp, exception); // exception handling 
                }
                SignupScroll.ContentSize = new CGSize(SignUpChildView.Bounds.Width, SignUpChildView.Bounds.Height);
                BackArrowClick();
                SetFonts();
                PasswordSignUpImgClick();
                ConfirmPasswordSignUpImgClick();
                tblCoursename.Hidden = true;
                MoveScrollView();
                SignUpChildViewClick();
                SetUISize();
                AddDoneButtonToNumericKeyboard();
                AddDoneButtonToKeyboard();
                GetRequestTokenForSignUp();
                tblDistributorList.Hidden = true;
                try
                {
                    bool isConnected = utility.CheckInternetConnection(); // check the internet connection 
                    if (isConnected)
                    {
                        // implement the google analytics
                        GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                        GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                        GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                        GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                        GoogleAnalytics.Current.InitTracker();
                        GoogleAnalytics.Current.Tracker.SendView(AppResources.SignUp);
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show(); // show the toast message 
                    }
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.SignUp, null); // check the exception handling 
                }
                // Perform any additional setup after loading the view, typically from a nib. 
            }
            /// <summary>
            /// Set UI size sepcified in iPhone X.
            /// set UI in iPhone X
            /// </summary>
            public void SetUISize()
            {
                try
                {
                    int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width; // get the device width 
                    int viewHeight = (int)UIScreen.MainScreen.Bounds.Size.Height; // get the device height 
                    if (viewWidth == (int)DeviceScreenSize.ISix)
                    {
                        if (viewHeight == Constant.lstDigit[31])
                        {
                            headerView.Frame = new CGRect(headerView.Frame.X, headerView.Frame.Y + Constant.lstDigit[10], headerView.Frame.Width, headerView.Frame.Height);
                            SignupScroll.Frame = new CGRect(SignupScroll.Frame.X, headerView.Frame.Y + headerView.Frame.Height, SignupScroll.Frame.Width, SignupScroll.Frame.Height);
                            SignupScroll.ContentSize = new CGSize(SignUpChildView.Bounds.Width, SignUpChildView.Bounds.Height);
                            tblDistributorList.Frame = new CGRect(
                            tblDistributorList.Frame.X, lblDistributorName.Frame.Y + lblDistributorName.Frame.Height - Constant.lstDigit[8], tblDistributorList.Frame.Width, tblDistributorList.Frame.Height);
                        }
                    }  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.SetUISize, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Hit the Request token API
            /// Get the request token for sign up.
            /// </summary>
            public async void GetRequestTokenForSignUp()
            {
                loadPop = new LoadingOverlay(View.Frame);
                this.View.Add(loadPop);
                await Task.Delay(Constant.lstDigit[17]);
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
                            HitCourseNameListAPI(accessTokeResponseEntity.token);
                            HitAllDistributorListAPI(accessTokeResponseEntity.token);
                        }
                        else
                        {
                            Toast.MakeText(accessTokeResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error,Constant.durationOfToastMessage).Show();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.GetRequestTokenForSignUp, AppResources.SignUp, null);
                    if (loadPop != null)
                    {
                        loadPop.Hide();
                    }
                }
            }
            /// <summary>
            /// Set the fonts of UILabel,UITextField,UIButton.
            /// set the padding of UITextField.
            /// </summary>
            public void SetFonts()
            {
                try
                {
                    int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                    Utility.SetPadding(new UITextField[] { txtCourseName, txtUsername, txtPassword, txtCPassword, txtPhoneNumber, txtFirstname, txtLastName }, Constant.lstDigit[4]);
                    Utility.SetFonts(null, new UILabel[] { lblCName, lblUsername, lblPassword, lblCPassword, lblPhoneNumber, lblfirstname, lblLastname, lblDistributorName, lblDSRName,lblReferralCode }, new UITextField[] { txtCourseName, txtUsername, txtPassword, txtCPassword, txtPhoneNumber, txtFirstname, txtLastName, txtDistributorName, txtDSRName ,txtReferralCode, txtCourseDesc}, Constant.lstDigit[11], viewWidth);
                    Utility.SetFonts(null, new UILabel[] { lblAddress,lblCommentPassword }, null, Constant.lstDigit[8], viewWidth);
                    Utility.SetFontsforHeader(new UILabel[] { lblSignUp, lblReferredBy }, new UIButton[] { btnSubmit }, Constant.lstDigit[11], viewWidth); 
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Hit the course name list API.
            /// </summary>
            protected async void HitCourseNameListAPI(string tokenSignUp)
            {
                try
                {
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        courseEntity = await utility.GetCourseList(tokenSignUp);
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.HitCourseNameListAPI, AppResources.SignUp, null);
                }
                finally
                {
                    loadPop.Hide(); 
                }
            }
            /// <summary>
            /// Hits all distributor list API.
            /// </summary>
            /// <param name="token">Token.</param>
            protected async void HitAllDistributorListAPI(string token)
            {
                try
                {
                    bool isConnected = utility.CheckInternetConnection();
                    if (isConnected)
                    {
                        lstDistinctDistributorEntities = await utility.GetDistinctDistributorList(token);
                    }
                    else
                    {
                        Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();  
                    }
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.HitAllDistributorListAPI, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Set the UI on depend on course address.
            /// change frame of fields
            /// </summary>
            public void SetUILblCourseAddress()
            {
                try
                {
                    if (lblAddress.Hidden == true)
                    {
                        lblUsername.Frame = new CGRect(
                            lblUsername.Frame.X, txtCourseName.Frame.Y + txtCourseName.Frame.Height + Constant.lstDigit[9], lblUsername.Frame.Width, lblUsername.Frame.Height
                            );
                        txtUsername.Frame = new CGRect(
                            txtUsername.Frame.X, lblUsername.Frame.Y + lblUsername.Frame.Height, txtUsername.Frame.Width, txtUsername.Frame.Height
                            );
                        lblPassword.Frame = new CGRect(
                            lblPassword.Frame.X, txtUsername.Frame.Y + txtUsername.Frame.Height + Constant.lstDigit[9], lblPassword.Frame.Width, lblPassword.Frame.Height
                            );
                        txtPassword.Frame = new CGRect(
                            txtPassword.Frame.X, lblPassword.Frame.Y + lblPassword.Frame.Height, txtPassword.Frame.Width, txtPassword.Frame.Height
                            );
                        imgPasswordSignUpEye.Frame = new CGRect(
                            imgPasswordSignUpEye.Frame.X, txtPassword.Frame.Y + Constant.lstDigit[3], imgPasswordSignUpEye.Frame.Width, imgPasswordSignUpEye.Frame.Height
                            );
                        lblCommentPassword.Frame = new CGRect(
                            lblCommentPassword.Frame.X, txtPassword.Frame.Y + txtPassword.Frame.Height, lblCommentPassword.Frame.Width, lblCommentPassword.Frame.Height
                           );
                        lblCPassword.Frame = new CGRect(
                            lblCPassword.Frame.X, lblCommentPassword.Frame.Y + lblCommentPassword.Frame.Height + Constant.lstDigit[9], lblCPassword.Frame.Width, lblCPassword.Frame.Height
                            );
                        txtCPassword.Frame = new CGRect(
                            txtCPassword.Frame.X, lblCPassword.Frame.Y + lblCPassword.Frame.Height, txtCPassword.Frame.Width, txtCPassword.Frame.Height
                            );
                        imgCPasswordSignUpEye.Frame = new CGRect(
                            imgCPasswordSignUpEye.Frame.X, txtCPassword.Frame.Y + Constant.lstDigit[3], imgCPasswordSignUpEye.Frame.Width, imgCPasswordSignUpEye.Frame.Height
                           );
                        lblfirstname.Frame = new CGRect(
                            lblfirstname.Frame.X, txtCPassword.Frame.Y + txtCPassword.Frame.Height + Constant.lstDigit[9], lblfirstname.Frame.Width, lblfirstname.Frame.Height
                            );
                        txtFirstname.Frame = new CGRect(
                            txtFirstname.Frame.X, lblfirstname.Frame.Y + lblfirstname.Frame.Height, txtFirstname.Frame.Width, txtFirstname.Frame.Height
                            );
                        lblLastname.Frame = new CGRect(
                            lblLastname.Frame.X, txtFirstname.Frame.Y + txtFirstname.Frame.Height + Constant.lstDigit[9], lblLastname.Frame.Width, lblLastname.Frame.Height
                            );
                        txtLastName.Frame = new CGRect(
                            txtLastName.Frame.X, lblLastname.Frame.Y + lblLastname.Frame.Height, txtLastName.Frame.Width, txtLastName.Frame.Height
                            );
                        lblPhoneNumber.Frame = new CGRect(
                            lblPhoneNumber.Frame.X, txtLastName.Frame.Y + txtLastName.Frame.Height + Constant.lstDigit[9], lblPhoneNumber.Frame.Width, lblPhoneNumber.Frame.Height
                          );
                        txtPhoneNumber.Frame = new CGRect(
                            txtPhoneNumber.Frame.X, lblPhoneNumber.Frame.Y + lblPhoneNumber.Frame.Height, txtPhoneNumber.Frame.Width, txtPhoneNumber.Frame.Height
                            );
                        lblReferredBy.Frame = new CGRect(
                            lblReferredBy.Frame.X, txtPhoneNumber.Frame.Y + txtPhoneNumber.Frame.Height + Constant.lstDigit[9], lblReferredBy.Frame.Width, lblReferredBy.Frame.Height
                           );
                        lblDistributorName.Frame = new CGRect(
                            lblDistributorName.Frame.X, lblReferredBy.Frame.Y + lblReferredBy.Frame.Height + Constant.lstDigit[9], lblDistributorName.Frame.Width, lblDistributorName.Frame.Height
                          );
                        txtDistributorName.Frame = new CGRect(
                            txtDistributorName.Frame.X, lblDistributorName.Frame.Y + lblDistributorName.Frame.Height, txtDistributorName.Frame.Width, txtDistributorName.Frame.Height
                            );
                        tblDistributorList.Frame = new CGRect(
                            tblDistributorList.Frame.X, lblDistributorName.Frame.Y + lblDistributorName.Frame.Height- Constant.lstDigit[9], tblDistributorList.Frame.Width, tblDistributorList.Frame.Height
                          );
                        lblDSRName.Frame = new CGRect(
                            lblDSRName.Frame.X, txtDistributorName.Frame.Y + txtDistributorName.Frame.Height+ Constant.lstDigit[9], lblDSRName.Frame.Width, lblDSRName.Frame.Height
                            );
                        txtDSRName.Frame = new CGRect(
                            txtDSRName.Frame.X, lblDSRName.Frame.Y + lblDSRName.Frame.Height, txtDSRName.Frame.Width, txtDSRName.Frame.Height
                            );
                        lblReferralCode.Frame = new CGRect(
                           lblReferralCode.Frame.X, txtDSRName.Frame.Y + txtDSRName.Frame.Height + Constant.lstDigit[9], lblReferralCode.Frame.Width, lblReferralCode.Frame.Height
                         );
                        txtReferralCode.Frame = new CGRect(
                           txtReferralCode.Frame.X, lblReferralCode.Frame.Y + lblReferralCode.Frame.Height, txtReferralCode.Frame.Width, txtReferralCode.Frame.Height
                           );
                        lblCourseAff.Frame = new CGRect(
                            lblCourseAff.Frame.X, txtReferralCode.Frame.Y + txtReferralCode.Frame.Height, lblCourseAff.Frame.Width, lblCourseAff.Frame.Height
                            );
                        btnSelectImage.Frame = new CGRect(
                         btnSelectImage.Frame.X, lblCourseAff.Frame.Y + lblCourseAff.Frame.Height + Constant.lstDigit[12], btnSelectImage.Frame.Width, btnSelectImage.Frame.Height
                         );
                        lblCourseDesc.Frame = new CGRect(
                           lblCourseDesc.Frame.X, btnSelectImage.Frame.Y + btnSelectImage.Frame.Height, lblCourseDesc.Frame.Width, lblCourseDesc.Frame.Height
                           );
                        txtCourseDesc.Frame = new CGRect(
                          txtCourseDesc.Frame.X, lblCourseDesc.Frame.Y + lblCourseDesc.Frame.Height, txtCourseDesc.Frame.Width, txtCourseDesc.Frame.Height
                          );
                        btnSubmit.Frame = new CGRect(
                            btnSubmit.Frame.X, txtCourseDesc.Frame.Y + txtCourseDesc.Frame.Height + Constant.lstDigit[12], btnSubmit.Frame.Width, btnSubmit.Frame.Height
                            );
                    }
                    else
                    {
                        lblUsername.Frame = new CGRect(
                            lblUsername.Frame.X, lblAddress.Frame.Y + lblAddress.Frame.Height + Constant.lstDigit[5], lblUsername.Frame.Width, lblUsername.Frame.Height
                            );
                        txtUsername.Frame = new CGRect(
                            txtUsername.Frame.X, lblUsername.Frame.Y + lblUsername.Frame.Height, txtUsername.Frame.Width, txtUsername.Frame.Height
                            );
                        lblPassword.Frame = new CGRect(
                            lblPassword.Frame.X, txtUsername.Frame.Y + txtUsername.Frame.Height + Constant.lstDigit[9], lblPassword.Frame.Width, lblPassword.Frame.Height
                            );
                        txtPassword.Frame = new CGRect(
                            txtPassword.Frame.X, lblPassword.Frame.Y + lblPassword.Frame.Height, txtPassword.Frame.Width, txtPassword.Frame.Height
                            );
                        lblCommentPassword.Frame = new CGRect(
                            lblCommentPassword.Frame.X, txtPassword.Frame.Y + txtPassword.Frame.Height, lblCommentPassword.Frame.Width, lblCommentPassword.Frame.Height
                           );
                        imgPasswordSignUpEye.Frame = new CGRect(
                            imgPasswordSignUpEye.Frame.X, txtPassword.Frame.Y + Constant.lstDigit[3], imgPasswordSignUpEye.Frame.Width, imgPasswordSignUpEye.Frame.Height
                            );
                        lblCPassword.Frame = new CGRect(
                            lblCPassword.Frame.X, lblCommentPassword.Frame.Y + lblCommentPassword.Frame.Height + Constant.lstDigit[9], lblCPassword.Frame.Width, lblCPassword.Frame.Height
                            );
                        txtCPassword.Frame = new CGRect(
                            txtCPassword.Frame.X, lblCPassword.Frame.Y + lblCPassword.Frame.Height, txtCPassword.Frame.Width, txtCPassword.Frame.Height
                            );
                        imgCPasswordSignUpEye.Frame = new CGRect(
                            imgCPasswordSignUpEye.Frame.X, txtCPassword.Frame.Y + Constant.lstDigit[3], imgCPasswordSignUpEye.Frame.Width, imgCPasswordSignUpEye.Frame.Height
                           );
                        lblPhoneNumber.Frame = new CGRect(
                            lblPhoneNumber.Frame.X, txtCPassword.Frame.Y + txtCPassword.Frame.Height + Constant.lstDigit[9], lblPhoneNumber.Frame.Width, lblPhoneNumber.Frame.Height
                            );
                        txtPhoneNumber.Frame = new CGRect(
                            txtPhoneNumber.Frame.X, lblPhoneNumber.Frame.Y + lblPhoneNumber.Frame.Height, txtPhoneNumber.Frame.Width, txtPhoneNumber.Frame.Height
                            );
                        lblfirstname.Frame = new CGRect(
                            lblfirstname.Frame.X, txtCPassword.Frame.Y + txtCPassword.Frame.Height + Constant.lstDigit[9], lblfirstname.Frame.Width, lblfirstname.Frame.Height
                            );
                        txtFirstname.Frame = new CGRect(
                            txtFirstname.Frame.X, lblfirstname.Frame.Y + lblfirstname.Frame.Height, txtFirstname.Frame.Width, txtFirstname.Frame.Height
                            );
                        lblLastname.Frame = new CGRect(
                            lblLastname.Frame.X, txtFirstname.Frame.Y + txtFirstname.Frame.Height + Constant.lstDigit[9], lblLastname.Frame.Width, lblLastname.Frame.Height
                            );
                        txtLastName.Frame = new CGRect(
                            txtLastName.Frame.X, lblLastname.Frame.Y + lblLastname.Frame.Height, txtLastName.Frame.Width, txtLastName.Frame.Height
                            );
                        lblPhoneNumber.Frame = new CGRect(
                           lblPhoneNumber.Frame.X, txtLastName.Frame.Y + txtLastName.Frame.Height + Constant.lstDigit[9], lblPhoneNumber.Frame.Width, lblPhoneNumber.Frame.Height
                         );
                        txtPhoneNumber.Frame = new CGRect(
                           txtPhoneNumber.Frame.X, lblPhoneNumber.Frame.Y + lblPhoneNumber.Frame.Height, txtPhoneNumber.Frame.Width, txtPhoneNumber.Frame.Height
                           );
                        lblReferredBy.Frame = new CGRect(
                            lblReferredBy.Frame.X, txtPhoneNumber.Frame.Y + txtPhoneNumber.Frame.Height + Constant.lstDigit[9], lblReferredBy.Frame.Width, lblReferredBy.Frame.Height
                           );
                        lblDistributorName.Frame = new CGRect(
                            lblDistributorName.Frame.X, lblReferredBy.Frame.Y + lblReferredBy.Frame.Height + Constant.lstDigit[9], lblDistributorName.Frame.Width, lblDistributorName.Frame.Height
                          );
                        txtDistributorName.Frame = new CGRect(
                            txtDistributorName.Frame.X, lblDistributorName.Frame.Y + lblDistributorName.Frame.Height, txtDistributorName.Frame.Width, txtDistributorName.Frame.Height
                            );
                        tblDistributorList.Frame = new CGRect(
                            tblDistributorList.Frame.X, lblDistributorName.Frame.Y + lblDistributorName.Frame.Height-Constant.lstDigit[6], tblDistributorList.Frame.Width, tblDistributorList.Frame.Height
                          );
                        lblDSRName.Frame = new CGRect(
                            lblDSRName.Frame.X, txtDistributorName.Frame.Y+txtDistributorName.Frame.Height + Constant.lstDigit[9], lblDSRName.Frame.Width, lblDSRName.Frame.Height
                          );
                        txtDSRName.Frame = new CGRect(
                            txtDSRName.Frame.X, lblDSRName.Frame.Y + lblDSRName.Frame.Height, txtDSRName.Frame.Width, txtDSRName.Frame.Height
                            );
                        lblReferralCode.Frame = new CGRect(
                            lblReferralCode.Frame.X, txtDSRName.Frame.Y + txtDSRName.Frame.Height + Constant.lstDigit[9], lblReferralCode.Frame.Width, lblReferralCode.Frame.Height
                          );
                        txtReferralCode.Frame = new CGRect(
                           txtReferralCode.Frame.X, lblReferralCode.Frame.Y + lblReferralCode.Frame.Height, txtReferralCode.Frame.Width, txtReferralCode.Frame.Height
                           );
                        lblCourseAff.Frame = new CGRect(
                            lblCourseAff.Frame.X, txtReferralCode.Frame.Y + txtReferralCode.Frame.Height, lblCourseAff.Frame.Width, lblCourseAff.Frame.Height
                            );
                        btnSelectImage.Frame = new CGRect(
                           btnSelectImage.Frame.X, lblCourseAff.Frame.Y + lblCourseAff.Frame.Height + Constant.lstDigit[12], btnSelectImage.Frame.Width, btnSelectImage.Frame.Height
                           );
                        lblCourseDesc.Frame = new CGRect(
                           lblCourseDesc.Frame.X, btnSelectImage.Frame.Y + btnSelectImage.Frame.Height, lblCourseDesc.Frame.Width, lblCourseDesc.Frame.Height
                           );
                        txtCourseDesc.Frame = new CGRect(
                          txtCourseDesc.Frame.X, lblCourseDesc.Frame.Y + lblCourseDesc.Frame.Height, txtCourseDesc.Frame.Width, txtCourseDesc.Frame.Height
                          );
                        btnSubmit.Frame = new CGRect(
                            btnSubmit.Frame.X, txtCourseDesc.Frame.Y + txtCourseDesc.Frame.Height + Constant.lstDigit[12], btnSubmit.Frame.Width, btnSubmit.Frame.Height
                            );
                    }
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.SetUILblCourseAddress, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// put the data in signUpRequestEntity 
		    /// </summary>
		    public void SignUp()
            {
                loadPop = new LoadingOverlay(View.Frame);
                this.View.Add(loadPop);
                try
                {
                    string[] CourseName= txtCourseName.Text.Trim().Split('(');
                    if(courseEntity!=null)
                    {
                        string courseId = courseEntity.Where(a => a.courseName.ToLower().Trim() == CourseName[0].ToLower().Trim()).Select(a => a.courseId).FirstOrDefault();
                        signUpRequestEntity.courseId = courseId;  
                    }
                    signUpRequestEntity.email = txtUsername.Text.Trim();
                    signUpRequestEntity.password = txtPassword.Text.Trim();
                    signUpRequestEntity.confirmPassword = txtCPassword.Text.Trim();
                    signUpRequestEntity.phoneNumber = txtPhoneNumber.Text.Trim();
                    signUpRequestEntity.firstName = txtFirstname.Text.Trim();
                    signUpRequestEntity.lastName = txtLastName.Text.Trim();
                    signUpRequestEntity.referralDSRName =NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.SignUpDistributrId);
                    signUpRequestEntity.referralCompany = txtDSRName.Text.Trim();
                    signUpRequestEntity.referralCode = txtReferralCode.Text.Trim();
                    signUpRequestEntity.CourseAffiliationDescription = txtCourseDesc.Text.Trim();
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, "SignUp", AppResources.SignUp, null);
                }
                finally
                {
                    loadPop.Hide();
                }
            }
            /// <summary>
            /// Check the validation of all fields in sign up page
            /// </summary>
            /// <returns>The valid.</returns>
            public async Task<bool> IsValid()
            {
                await Task.Delay(Constant.lstDigit[17]);
                bool flag = false;
                if (String.IsNullOrEmpty(txtUsername.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireUserName, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if(utility.IsValidEmail(txtUsername.Text.Trim())==false)
                {
                    Toast.MakeText(AppResources.InvalidUserName, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequirePassword, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if(utility.IsValidPassword(txtPassword.Text.Trim())==false)
                {
                    Toast.MakeText(AppResources.InvalidPassword, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if (String.IsNullOrEmpty(txtCPassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireCPassword, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if(!txtPassword.Text.Equals(txtCPassword.Text.Trim()))
                {
                    Toast.MakeText(AppResources.InvalidConfirmPassword, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if (String.IsNullOrEmpty(txtFirstname.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireFirstName, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if (String.IsNullOrEmpty(txtLastName.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireLastName, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if (String.IsNullOrEmpty(txtPhoneNumber.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequirePhone, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else if (utility.IsValidPhone(txtPhoneNumber.Text.Trim()) == false)
                {
                    Toast.MakeText(AppResources.InvalidPhone, Constant.durationOfToastMessage).Show();
                    flag = false;
                }
                else
                {
                    flag = true;
                    SignUp();
                }
                return flag;
            }
            /// <summary>
            /// Texts the phone number editing changed.
            /// </summary>
            /// <param name="sender">Sender.</param>
            partial void txtPhoneNumber_EditingChanged(UITextField sender)
            {
              try
              {
                    if (txtPhoneNumber.Text.Length==1 && isLengthOne==false && (!txtPhoneNumber.Text.Contains("(")))
                    {  
                        string formatted = "(" + txtPhoneNumber.Text;
                        txtPhoneNumber.Text = formatted;
                        isLengthOne = true;
                        isLengthFour = false;
                        isLengthEight = false;
                    }
                    else if(txtPhoneNumber.Text.Length == 4 && isLengthFour == false)
                    {
                      string formatted = txtPhoneNumber.Text + ")";
                       txtPhoneNumber.Text = formatted;
                       isLengthFour = true;
                       isLengthOne = false;
                       isLengthEight = false;

                    }
                    else if(txtPhoneNumber.Text.Length == 8 && isLengthEight == false)
                    {
                        isLengthEight = true;
                        isLengthOne = false;
                        isLengthFour = false;
                        string formatted = txtPhoneNumber.Text + "-";
                        txtPhoneNumber.Text = formatted;
                    }
                }
              catch (Exception ex)
              {
                    utility.SaveExceptionHandling(ex,AppResources.txtPhoneNumber_EditingChanged, AppResources.SignUp, null);
              }
            }
            /// <summary>
            /// Automatically capitalize first Name.
            /// </summary>
            /// <param name="sender">Sender.</param>
            partial void txtFirstname_EditingChanged(UITextField sender)
            {
                try
                {
                    string firstName = sender.Text;
                    char[] c = firstName.ToArray();
                    bool CapitalNext = true;
                    string blankValue = null;
                    foreach (char ch in c)
                    {
                        if (CapitalNext)
                            blankValue += ch.ToString().ToUpper();
                        else
                            blankValue += ch.ToString();
                        CapitalNext = false;
                        if (char.IsWhiteSpace(ch))
                        {
                            CapitalNext = true;
                        }
                    }
                    txtFirstname.Text = blankValue;
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.txtFirstname_EditingChanged, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Automatically capitalize Last Name.
            /// </summary>
            /// <param name="sender">Sender.</param>
            partial void txtLastName_EditingChanged(UITextField sender)
            {
                try
                {
                    string lastName = sender.Text;
                    char[] c = lastName.ToArray();
                    bool CapitalNext = true;
                    string blankValue = null;
                    foreach (char ch in c)
                    {
                        if (CapitalNext)
                            blankValue += ch.ToString().ToUpper();
                        else
                            blankValue += ch.ToString();
                        CapitalNext = false;
                        if (char.IsWhiteSpace(ch))
                        {
                            CapitalNext = true;
                        }
                    }
                    txtLastName.Text = blankValue;
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.txtLastName_EditingChanged, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// submit button touch up inside.
            /// </summary>
            /// <param name="sender">Sender.</param>
            async partial void  BtnSubmit_TouchUpInside(UIButton sender)
            {
                try
                {
                    txtDSRName.ResignFirstResponder();
                    if (String.IsNullOrEmpty(txtCourseName.Text.Trim()))
                    {
                        Toast.MakeText(AppResources.RequireCourseName, Constant.durationOfToastMessage).Show();
                    }
                    else
                    {
                        string[] CourseName= txtCourseName.Text.Trim().Split('(');
                        string Course=CourseName[0];
                        NSUserDefaults.StandardUserDefaults.SetString(Course, AppResources.CourseName);
                        if(courseEntity!=null)
                        {
                            string courseId = courseEntity.Where(a => a.courseName.ToLower().Trim() == CourseName[0].ToLower().Trim()).Select(a => a.courseId).FirstOrDefault();
                            bool res = await IsValid();
                            if (res == true)
                            {
                                if (String.IsNullOrEmpty(courseId))
                                {
                                    string CourseId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CourseId);
                                    Toast.MakeText(AppResources.newCourseEntered, Constant.durationOfToastMessage).Show();
                                    this.PerformSegue(AppResources.SegueFromSignUpToCourse, null);
                                }
                                else
                                {
                                    this.PerformSegue(AppResources.SegueFromSignUpToWebView, null);
                                }
                            }
                        }
                        else
                        {
                            bool res = await IsValid();
                            if (res == true)
                            {
                                string CourseId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CourseId);
                                Toast.MakeText(AppResources.newCourseEntered, Constant.durationOfToastMessage).Show();
                                this.PerformSegue(AppResources.SegueFromSignUpToCourse, null);
                            }
                        }   
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.BtnSubmit_TouchUpInside, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Prepares for segue Sign Up to Webview.
            /// </summary>
            /// <param name="segue">Segue.</param>
            /// <param name="sender">Sender.</param>
            public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		    {
                base.PrepareForSegue(segue, sender);
                try
                {
                    if (segue.Identifier == AppResources.SegueFromSignUpToWebView)
                    {
                        var navctlr = segue.DestinationViewController as WebViewController;
                        if (navctlr != null)
                        {
                            navctlr.signUpRequestEntity = this.signUpRequestEntity;
                        }
                    }
                    else if (segue.Identifier == AppResources.SegueFromSignUpToCourse)
                    {
                        var navctlr1 = segue.DestinationViewController as CourseController;
                        if (navctlr1 != null)
                        {
                            navctlr1.signUpRequestEntity = this.signUpRequestEntity;
                        }
                    }  
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.PrepareForSegue, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// course name text changed events.
            /// to show course list in a table view source
            /// </summary>
            /// <param name="sender">Sender.</param>
            partial void txtCourseName_Changed(UITextField sender)
            {
                try
                {
                    string courseName1 = sender.Text;
                    char[] c = courseName1.ToArray();
                    bool CapitalNext = true;
                    string blankValue = null;
                    foreach (char ch in c)
                    {
                        if (CapitalNext)
                            blankValue += ch.ToString().ToUpper();
                        else
                            blankValue += ch.ToString();
                        CapitalNext = false;
                        if (char.IsWhiteSpace(ch))
                        {
                            CapitalNext = true;
                        }
                    }
                    txtCourseName.Text = blankValue;
                    if (courseEntity != null)
                    {
                        string str = txtCourseName.Text.Trim();
                        if (!string.IsNullOrEmpty(str))
                        {
                            tblCoursename.Hidden = false;
                        }
                        else
                        {
                            tblCoursename.Hidden = true;
                            lblAddress.Text = String.Empty;
                        }
                        if (!string.IsNullOrEmpty(str))
                        {
                            List<CourseEntity> lstcourseEntity = courseEntity.Where(b => b.courseName.ToLower().StartsWith(str.ToLower()) || b.courseName.ToLower().Contains(str.ToLower())).ToList();
                            if (lstcourseEntity.Count != Constant.lstDigit[0])
                            {
                                List<string> courseName = new List<string>();
                                foreach (var item in lstcourseEntity)
                                {
                                    courseName.Add(item.courseName + " (ZipCode :" + item.zip + ")");
                                }
                                tblCoursename.Hidden = false;
                                TableViewCourse tableViewCourse = new TableViewCourse(this, courseName);
                                tblCoursename.Source = tableViewCourse;
                                tblCoursename.ReloadData();
                                tableViewCourse.OnRowSelect += HandleOnRowSelect;
                            }
                            else
                            {
                                tblCoursename.Hidden = true;
                                lblAddress.Text = String.Empty;
                            }
                        }
                        else
                        {
                            if (courseEntity.Count != Constant.lstDigit[0])
                            {
                                List<string> courseName = new List<string>();
                                foreach(var item in courseEntity)
                                {
                                    courseName.Add(item.courseName + " (ZipCode :" + item.zip + ")");
                                }
                                tblCoursename.Hidden = true;
                                TableViewCourse tableViewCourse = new TableViewCourse(this, courseName);
                                tblCoursename.Source = tableViewCourse;
                                tblCoursename.ReloadData();
                                tableViewCourse.OnRowSelect += HandleOnRowSelect;
                            }
                            else
                            {
                                tblCoursename.Hidden = true;
                            }
                            lblAddress.Hidden = true;
                            lblAddress.Text = String.Empty;
                            SetUILblCourseAddress();
                        }
                    }
                    else
                    {
                        lblAddress.Hidden = true;
                        SetUILblCourseAddress();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.txtCourseName_Changed, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Handles the on row select on click to course list.
            /// </summary>
            /// <param name="Coursename">Coursename.</param>
            public void HandleOnRowSelect(string Coursename)
            {
                try
                {
                    string[] CourseName= Coursename.Split('(');
                    string[] Zipcode = CourseName[1].Split(':');
                    string zipValue = Zipcode[1].ToString().Replace(')',' ').Trim();
                    txtCourseName.Text = Coursename;
                    string courseId=  courseEntity.Where(a => a.courseName.ToLower().Trim() == CourseName[0].ToLower().Trim() && a.zip== zipValue).Select(a => a.courseId).FirstOrDefault();
                    string zip = courseEntity.Where(x => x.courseId == courseId).Select(x => x.zip).FirstOrDefault();
                    string address = courseEntity.Where(x => x.courseId == courseId).Select(x => x.address).FirstOrDefault();
                    string city = courseEntity.Where(x => x.courseId == courseId).Select(x => x.city).FirstOrDefault();
                    string state = courseEntity.Where(x => x.courseId == courseId).Select(x => x.state).FirstOrDefault();
                    string country = courseEntity.Where(x => x.courseId == courseId).Select(x => x.country).FirstOrDefault();
                    //string zip = courseEntity.Where(x => x.courseId == courseId).Select(x => x.zip).FirstOrDefault();
                    lblAddress.Hidden = false;
                    lblAddress.Text = address + Constant.lstDigitString[6] + city + Constant.lstDigitString[6] + state + Constant.lstDigitString[6] + country + Constant.lstDigitString[6] + zip;
                    tblCoursename.Hidden = true;
                    SetUILblCourseAddress();
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.HandleOnRowSelect, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Distributor name text changed events.
            /// to show distributor list in a table view source
            /// </summary>
            /// <param name="sender">Sender.</param>
            partial void txtDistributorName_Changed(UITextField sender)
            {
                try
                {
                    if (lstDistinctDistributorEntities != null)
                    {
                        string str = txtDistributorName.Text;
                        if (!string.IsNullOrEmpty(str))
                        {
                            List<DistinctDistributorEntity> lstDistributorInfoEntity = lstDistinctDistributorEntities.Where(b => b.distributorName.ToLower().StartsWith(str.ToLower()) || b.distributorName.ToLower().Contains(str.ToLower())).ToList();
                            if (lstDistributorInfoEntity.Count != Constant.lstDigit[0])
                            {
                                tblDistributorList.Hidden = false;
                                TableViewDistributorList tableViewDistributorList1 = new TableViewDistributorList(this, lstDistributorInfoEntity);
                                tblDistributorList.Source = tableViewDistributorList1;
                                tblDistributorList.ReloadData();
                                tableViewDistributorList1.OnRowSelect += HandleOnRowSelected;
                            }
                            else
                            {
                                tblDistributorList.Hidden = true;
                            }
                        }
                        else
                        {
                            lstDistinctDistributorEntities = lstDistinctDistributorEntities.ToList();
                            if (lstDistinctDistributorEntities.Count != Constant.lstDigit[0])
                            {
                                tblDistributorList.Hidden = true;
                                TableViewDistributorList tableViewDistributorList = new TableViewDistributorList(this, lstDistinctDistributorEntities);
                                tblDistributorList.Source = tableViewDistributorList;
                                tblDistributorList.ReloadData();
                                tableViewDistributorList.OnRowSelect += HandleOnRowSelected;
                            }
                            else
                            {
                                tblDistributorList.Hidden = true;
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(AppResources.NoDistributors, Constant.durationOfToastMessage).Show();
                    }
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.txtDistributorName_Changed, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Handles the on row select to click on table view source on distributor.
            /// </summary>
            /// <param name="distributorName">Coursename.</param>
            public void HandleOnRowSelected(string distributorName)
            {
                try  
                {
                    txtDistributorName.Text = distributorName;
                    if (!string.IsNullOrEmpty(txtDistributorName.Text))
                    {
                        string DistributorId = lstDistinctDistributorEntities.Where(x => x.distributorName == txtDistributorName.Text).Select(x => x.did).FirstOrDefault().ToString();
                        NSUserDefaults.StandardUserDefaults.SetString(DistributorId, AppResources.SignUpDistributrId);
                    }
                    tblDistributorList.Hidden = true;
                }
                catch (Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.HandleOnRowSelected, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Moves the scroll view to open keyboard.
            /// </summary>
            public void MoveScrollView()
            {
                try
                {
                    bool isScrollViewUp = false;
                    UIKeyboard.Notifications.ObserveWillShow((s, e) =>
                    {
                        if (!isScrollViewUp)
                        {
                            isScrollViewUp = true;
                            var keyboardFrame = UIKeyboard.FrameBeginFromNotification(e.Notification);
                            SignUpChildView.Frame = new CGRect(SignUpChildView.Frame.X, SignUpChildView.Frame.Y - Constant.lstDigit[14], SignUpChildView.Frame.Width, SignUpChildView.Frame.Height);
                            tblDistributorList.Frame = new CGRect(
                           tblDistributorList.Frame.X, lblDistributorName.Frame.Y + lblDistributorName.Frame.Height - Constant.lstDigit[8], tblDistributorList.Frame.Width, tblDistributorList.Frame.Height
                         );
                        }
                    });
                    UIKeyboard.Notifications.ObserveWillHide((s, e) =>
                    {
                        SignUpChildView.Frame = new CGRect(SignUpChildView.Frame.X, Constant.lstDigit[0], SignUpChildView.Frame.Width, SignUpChildView.Frame.Height);
                        tblDistributorList.Frame = new CGRect(
                           tblDistributorList.Frame.X, lblDistributorName.Frame.Y + lblDistributorName.Frame.Height + Constant.lstDigit[12], tblDistributorList.Frame.Width, tblDistributorList.Frame.Height
                         );
                        isScrollViewUp = false;
                    }); 
                }
                catch(Exception ex)
                {
                    utility.SaveExceptionHandling(ex, AppResources.MoveScrollView, AppResources.SignUp, null);
                }
            }
            /// <summary>
            /// Gesture implementation for back arrow click to navigate screen.
            /// </summary>
            public void BackArrowClick()
            {
                UITapGestureRecognizer imgBackArrowClicked = new UITapGestureRecognizer(() =>
                {
                    bool isDeepLinkSignUp = NSUserDefaults.StandardUserDefaults.BoolForKey(AppResources.deepLink);
                    if (isDeepLinkSignUp)
                    {
                        Utility.ClearCachedData();
                        UIViewController[] uIViewControllers = NavigationController.ViewControllers;
                        foreach (UIViewController item in uIViewControllers)
                        {
                            if (item is LoginController)
                            {
                                NavigationController.PopToViewController(item, true);
                                break;
                            }
                        }
                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.deepLink);
                    }
                    else
                    {
                        this.NavigationController.PopViewController(true);
                    }
                });
                imgBackArrowSignup.UserInteractionEnabled = true;
                imgBackArrowSignup.AddGestureRecognizer(imgBackArrowClicked);
            }
            /// <summary>
            /// Gesture implementation for Sign up child view click.
            /// Dismiss the keyboard click on sign up.
            /// </summary>
            public void SignUpChildViewClick()
            {
                UITapGestureRecognizer signUpChildViewClicked = new UITapGestureRecognizer(() =>
                {
                    tblCoursename.Hidden = true;
                    tblDistributorList.Hidden = true;
                    utility.DismissKeyboardOnTap(new UIView[] { SignUpChildView });
                });
                SignUpChildView.UserInteractionEnabled = true;
                SignUpChildView.AddGestureRecognizer(signUpChildViewClicked);
            }
            /// <summary>
            /// Change Events to UITextField of password
            /// to hide or show password eye icon.
            /// </summary>
            /// <param name="sender">Sender.</param>
            partial void txtPasswordSignUp_EditingChanged(UITextField sender)
            {
                if (!String.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    imgPasswordSignUpEye.Hidden = false;
                }
                else
                {
                    imgPasswordSignUpEye.Hidden = true;
                }
            }
            /// <summary>
            /// Change Events to UITextField of confirm Password 
            /// to hide or show confirm password eye icon.
            /// </summary>
            /// <param name="sender">Sender.</param>
            partial void txtCPasswordSignUp_EditingChanged(UITextField sender)
            {
                if (!String.IsNullOrEmpty(txtCPassword.Text.Trim()))
                {
                    imgCPasswordSignUpEye.Hidden = false;
                }
                else
                {
                    imgCPasswordSignUpEye.Hidden = true;
                }
            }
            /// <summary>
            /// Gesture implementation for eye icon
            /// show visible or invisible password eye
            /// </summary>
            public void PasswordSignUpImgClick()
            {
                bool imageshow = false;
                UITapGestureRecognizer passwordSignUpImgClicked = new UITapGestureRecognizer(() =>
                {
                    try
                    {
                        if (imageshow == false)
                        {
                            imageshow = true;
                            imgPasswordSignUpEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                            txtPassword.SecureTextEntry = false;
                        }
                        else
                        {
                            imageshow = false;
                            imgPasswordSignUpEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                            txtPassword.SecureTextEntry = true;
                        }  
                    }
                    catch(Exception ex)
                    {
                        utility.SaveExceptionHandling(ex, AppResources.PasswordSignUpImgClick, AppResources.SignUp, null);
                    }
                });
                imgPasswordSignUpEye.UserInteractionEnabled = true;
                imgPasswordSignUpEye.AddGestureRecognizer(passwordSignUpImgClicked);
            }
            /// <summary>
            /// Gesture implementation for eye icon
            /// show visible or invisible confirm password eye
            /// </summary>
            public void ConfirmPasswordSignUpImgClick()
            {
                bool imageshow = false;
                UITapGestureRecognizer confirmPasswordSignUpImgClicked = new UITapGestureRecognizer(() =>
                {
                    try
                    {
                        if (imageshow == false)
                        {
                            imageshow = true;
                            imgCPasswordSignUpEye.Image = UIImage.FromBundle(AppResources.imgInvisibleEye);
                            txtCPassword.SecureTextEntry = false;
                        }
                        else
                        {
                            imageshow = false;
                            imgCPasswordSignUpEye.Image = UIImage.FromBundle(AppResources.imgVisibleEye);
                            txtCPassword.SecureTextEntry = true;
                        }   
                    }
                    catch(Exception ex)
                    {
                        utility.SaveExceptionHandling(ex, AppResources.ConfirmPasswordSignUpImgClick, AppResources.SignUp, null);
                    }
                });
                imgCPasswordSignUpEye.UserInteractionEnabled = true;
                imgCPasswordSignUpEye.AddGestureRecognizer(confirmPasswordSignUpImgClicked);
            }
            /// <summary>
            /// Add done button to numeric keyboard.
            /// </summary>
            public void AddDoneButtonToNumericKeyboard()
            {
                UITextField[] textFields = new UITextField[] { txtPhoneNumber };
                Utility.AddDoneButtonToNumericKeyboard(textFields);
            }
            /// <summary>
            /// Add done button to keyboard.
            /// </summary>
            public void AddDoneButtonToKeyboard()
            {
                UITextField[] textFields = new UITextField[] { txtCourseName, txtUsername, txtPassword, txtCPassword, txtFirstname, txtLastName,txtDSRName ,txtDistributorName,txtReferralCode, txtCourseDesc};
                Utility.AddDoneButtonToKeyboard(textFields);
            }
            /// <summary>
            /// Dids the receive memory warning.
            /// </summary>
            public override void DidReceiveMemoryWarning()
            {
                base.DidReceiveMemoryWarning();
                // Release any cached data, images, etc that aren't in use.
            }

            partial void Btn_Img_Select(UIButton sender)
            {
                picker = new UIImagePickerController();
                picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
                picker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
                picker.FinishedPickingMedia += Finished;
                picker.Canceled += OnImagePickerCancelled;
                PresentViewController(picker, animated: true, completionHandler: null);
            }

            public void Finished(object sender, UIImagePickerMediaPickedEventArgs e)
            {
                UIImage image = e.EditedImage ?? e.OriginalImage;

            if (image != null)
                {
                signUpRequestEntity.CourseAffiliationProofFilePath = (e.Info[UIImagePickerController.ImageUrl] as NSUrl).Path;
                string imgName = signUpRequestEntity.CourseAffiliationProofFilePath.Split('/').LastOrDefault();
                btnSelectImage.SetTitle(imgName, UIControlState.Normal);                

                UnregisterEventHandlers();
              
                }
                else
                {
                    signUpRequestEntity.CourseAffiliationProofFilePath = null;
                    UnregisterEventHandlers();
                }
                picker.DismissModalViewController(true);
            }
            void OnImagePickerCancelled(object sender, EventArgs args)
            {
                UnregisterEventHandlers();
                picker.DismissModalViewController(true);
            }

            void UnregisterEventHandlers()
            {
                picker.FinishedPickingMedia -= Finished;
                picker.Canceled -= OnImagePickerCancelled;
            }
        }
    }