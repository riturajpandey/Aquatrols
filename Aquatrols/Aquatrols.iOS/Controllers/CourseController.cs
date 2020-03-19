using Aquatrols.iOS.Helper;
using Aquatrols.iOS.Picker;
using Aquatrols.Models;
using CoreGraphics;
using Foundation;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;
using ToastIOS;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.Controllers
{
    /// <summary>
    /// This class file course controller is used for show the course list
    /// </summary>
    public partial class CourseController : UIViewController
    {
        private UIPickerView statePicker= new UIPickerView();
        private UIPickerView countryPicker= new UIPickerView();
        private UIPickerView proviencePicker= new UIPickerView();
        private Utility utility = Utility.GetInstance;
        private LoadingOverlay loadPop;
        public SignUpRequestEntity signUpRequestEntity;
        private Dictionary<string, string> lstProvinces,lstStates;
        UIButton btnDone= new UIButton();
        public CourseController(IntPtr handle) : base(handle)
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
                utility.SaveExceptionHandling(null, AppResources.ViewDidLoad, AppResources.AddCourse, exception);
            }
            lblState.Text = AppResources.State;
            TxtStateClick();
            TxtCountryClick();
            ImgBackCourseClick();
            DismissKeyboardClick();
            SetUISize();
            GetRequestTokenForSignUp();
            AddDoneButtonToKeyboard();
            SetFonts();
            CourseChildViewClick();
            try
            {
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    CourseScroll.ContentSize = new CGSize(CourseChildView.Bounds.Width, CourseChildView.Bounds.Height);
                    // implement the google analytics
                    GoogleAnalytics.Current.Config.TrackingId = AppResources.TrackingId;
                    GoogleAnalytics.Current.Config.AppId = AppResources.AppId;
                    GoogleAnalytics.Current.Config.AppName = AppResources.AppName;
                    GoogleAnalytics.Current.Config.AppVersion = (NSString)NSBundle.MainBundle.InfoDictionary[AppResources.AppVersion];
                    GoogleAnalytics.Current.InitTracker();
                    GoogleAnalytics.Current.Tracker.SendView(AppResources.AddCourse);
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.ViewDidLoad, AppResources.AddCourse, null);
            }
            // Perform any additional setup after loading the view, typically from a nib.
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
                        Courseheaderview.Frame = new CGRect(Courseheaderview.Frame.X, Courseheaderview.Frame.Y + Constant.lstDigit[10], Courseheaderview.Frame.Width, Courseheaderview.Frame.Height);
                        CourseScroll.Frame = new CGRect(CourseScroll.Frame.X, Courseheaderview.Frame.Y + Courseheaderview.Frame.Height, CourseScroll.Frame.Width, CourseScroll.Frame.Height);
                        CourseScroll.ContentSize = new CGSize(CourseChildView.Bounds.Width, CourseChildView.Bounds.Height);
                    }
                } 
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetUISize, AppResources.AddCourse, null);
            }
        }
        /// <summary>
        /// Set the fonts of UILabel,UITextField and UIButton.
        /// </summary>
        public void SetFonts()
        {
            try
            {
                int viewWidth = (int)UIScreen.MainScreen.Bounds.Size.Width;
                Utility.SetPadding(new UITextField[] { txtCountry, txtState, txtCity, txtStreetAddress, txtZipCode }, Constant.lstDigit[4]);
                Utility.SetFonts(null, new UILabel[] { lblCountry, lblState, lblCity, lblStreet, lblZip }, new UITextField[] { txtCountry, txtState, txtCity, txtStreetAddress, txtZipCode }, Constant.lstDigit[11], viewWidth);
                Utility.SetFontsforHeader(new UILabel[] { lblAddCourse }, new UIButton[] { btnAdd }, Constant.lstDigit[11], viewWidth); 
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.SetFonts, AppResources.AddCourse, null);
            }
        }
        /// <summary>
        /// Get the request token for sign up.
        /// Hit API to get token
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
                utility.SaveExceptionHandling(ex, AppResources.GetRequestTokenForSignUp, AppResources.AddCourse, null);
            }
        }
        /// <summary>
        /// Courses the child view click.
        /// </summary>
        public void CourseChildViewClick()
        {
            UITapGestureRecognizer txtStateClicked = new UITapGestureRecognizer(() =>
            {
                btnDone.Hidden = true;
                statePicker.Hidden = true;
                proviencePicker.Hidden = true;
                countryPicker.Hidden = true;
            });
            CourseChildView.UserInteractionEnabled = true;
            CourseChildView.AddGestureRecognizer(txtStateClicked);
        }
        /// <summary>
        /// click the state text box and open state picker.
        /// </summary>
        public void TxtStateClick()
        {
            UITapGestureRecognizer txtStateClicked = new UITapGestureRecognizer(() =>
            {
                btnDone.Hidden = true;
                countryPicker.Hidden = true;
                StatePicker(txtCountry.Text);
            });
            txtState.UserInteractionEnabled = true;
            txtState.AddGestureRecognizer(txtStateClicked);
        }
        /// <summary>
        /// To bind the data on State using the picker.
        /// </summary>
        public void StatePicker(string country)
        {
            try
            {
                if (country == AppResources.USA)
                {
                    statePicker = new UIPickerView(new CGRect(Constant.lstDigit[0], UIScreen.MainScreen.Bounds.Height - Constant.lstDigit[23], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[23]));
                    statePicker.BackgroundColor = UIColor.White;
                    statePicker.ShowSelectionIndicator = true;
                    btnDone = new UIButton(new CGRect(statePicker.Frame.X, statePicker.Frame.Y - Constant.lstDigit[15], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[15]));
                    btnDone.BackgroundColor = UIColor.Gray;
                    btnDone.SetTitle(AppResources.Done, UIControlState.Normal);
                    lstStates = Constant.GetStates();
                    List<string> lstState = new List<string>();
                    foreach (var item in lstStates)
                    {
                        lstState.Add(item.Key);
                    }
                    var picker = new StateModel(lstState,Constant.GetStates());
                    statePicker.Model = picker;
                    View.AddSubviews(statePicker, btnDone);
                    picker.ValueChanged += (s, e) =>
                    {
                        txtState.Text = picker.SelectedValue;
                    };
                    btnDone.TouchUpInside += (s, e) =>
                    {
                        statePicker.Hidden = true;
                        btnDone.Hidden = true;
                    };
                    View.Add(statePicker);
                }
                else
                {
                    proviencePicker = new UIPickerView(new CGRect(Constant.lstDigit[0], UIScreen.MainScreen.Bounds.Height - Constant.lstDigit[23], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[23]));
                    proviencePicker.BackgroundColor = UIColor.White;
                    proviencePicker.ShowSelectionIndicator = true;
                    btnDone = new UIButton(new CGRect(proviencePicker.Frame.X, proviencePicker.Frame.Y -Constant.lstDigit[15], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[15]));
                    btnDone.BackgroundColor = UIColor.Gray;
                    btnDone.SetTitle(AppResources.Done, UIControlState.Normal);
                    lstProvinces = Constant.GetProvince();
                    List<string> lstProvince = new List<string>();
                    foreach(var item in lstProvinces)
                    {
                        lstProvince.Add(item.Key);
                    }
                    var picker = new ProvinceModel(lstProvince,Constant.GetProvince());
                    proviencePicker.Model = picker;
                    View.AddSubviews(proviencePicker, btnDone);
                    picker.ValueChanged += (s, e) =>
                    {
                        txtState.Text = picker.SelectedValue;
                    };
                    btnDone.TouchUpInside += (s, e) =>
                    {
                        proviencePicker.Hidden = true;
                        btnDone.Hidden = true;
                    };
                    View.Add(proviencePicker);
                }
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.StatePicker, AppResources.AddCourse, null); // exception handling
            }
        }
        /// <summary>
        /// Country picker.
        /// Bind the country USA and Canada
        /// </summary>
        public void CountryPicker()
        {
            try
            {
                countryPicker = new UIPickerView(new CGRect(Constant.lstDigit[0], UIScreen.MainScreen.Bounds.Height - Constant.lstDigit[23], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[23]));
                countryPicker.BackgroundColor = UIColor.White;
                countryPicker.ShowSelectionIndicator = true;
                btnDone = new UIButton(new CGRect(countryPicker.Frame.X, countryPicker.Frame.Y - Constant.lstDigit[15], UIScreen.MainScreen.Bounds.Width, Constant.lstDigit[15]));
                btnDone.BackgroundColor = UIColor.Gray;
                btnDone.SetTitle(AppResources.Done, UIControlState.Normal);
                var picker = new CountryModel(Constant.lstCountry);
                countryPicker.Model = picker;
                View.AddSubviews(countryPicker, btnDone);
                picker.ValueChanged += (s, e) =>
                {
                    txtCountry.Text = picker.SelectedValue;
                    txtCountry_EditingChanged(txtCountry);
                    if (txtCountry.Text == AppResources.USA)
                    {
                        lblState.Text = AppResources.State;
                    }
                    else
                    {
                        lblState.Text = AppResources.Province;
                    }
                };
                btnDone.TouchUpInside += (s, e) =>
                {
                    countryPicker.Hidden = true;
                    btnDone.Hidden = true;
                };
                View.Add(countryPicker);
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.CountryPicker, AppResources.AddCourse, null);
            }
        }
        /// <summary>
        /// Changed Events to country text box to show state based on country.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtCountry_EditingChanged(UITextField sender)
        {
            try
            {
                if (txtCountry.Text == AppResources.USA)
                {
                    lblState.Text = AppResources.State;
                    txtState.Text = String.Empty;
                    TxtStateClick();
                }
                else
                {
                    lblState.Text = AppResources.Province;
                    txtState.Text = String.Empty;
                    TxtStateClick();
                }    
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.txtCountry_EditingChanged, AppResources.AddCourse, null);
            }
        }
        /// <summary>
        /// Add Button touch up inside.
        /// check validation to all fields.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void BtnAdd_TouchUpInside(UIButton sender)
        {
            try
            {
                btnDone.Hidden = true;
                countryPicker.Hidden = true;
                statePicker.Hidden = true;
                proviencePicker.Hidden = true;
                txtZipCode.ResignFirstResponder();
                if (String.IsNullOrEmpty(txtCountry.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireCountry, Constant.durationOfToastMessage).Show();
                    return;
                }
                if (String.IsNullOrEmpty(txtState.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireState, Constant.durationOfToastMessage).Show();
                    return;
                }
                if (String.IsNullOrEmpty(txtCity.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireCity, Constant.durationOfToastMessage).Show();
                    return;
                }
                if (String.IsNullOrEmpty(txtStreetAddress.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireStreetAddress, Constant.durationOfToastMessage).Show();
                    return;
                }
                if (String.IsNullOrEmpty(txtZipCode.Text.Trim()))
                {
                    Toast.MakeText(AppResources.RequireZip, Constant.durationOfToastMessage).Show();
                    return;
                }
                else
                {
                    bool flag = utility.IsValidZip(txtZipCode.Text.Trim(),txtCountry.Text.Trim());
                    if (flag == false)
                    {
                        Toast.MakeText(AppResources.InvalidZip, Constant.durationOfToastMessage).Show();
                        return;
                    }
                }
                AddCourse();  
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.BtnAdd_TouchUpInside, AppResources.AddCourse, null);
            }
        }
        /// <summary>
        /// Add the course if Course is empty in the list.
        /// </summary>
        protected async void AddCourse()
        {
            loadPop = new LoadingOverlay(View.Frame);
            this.View.Add(loadPop);
            try
            {
                string coursename = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CourseName);
                CourseEntity courseEntity = new CourseEntity();
                courseEntity.courseName = coursename;
                courseEntity.country = txtCountry.Text.Trim();
                if(txtCountry.Text == AppResources.USA)
                {
                    courseEntity.state = lstStates[txtState.Text];  
                }
                else
                {
                    courseEntity.state = lstProvinces[txtState.Text];    
                }
                courseEntity.city = txtCity.Text.Trim();
                courseEntity.address = txtStreetAddress.Text.Trim();
                courseEntity.zip = txtZipCode.Text.Trim();
                bool isConnected = utility.CheckInternetConnection();
                if (isConnected)
                {
                    string tokenSignUp=NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.tokenSignUp);
                    CourseResponseEntity courseResponseEntity = await utility.AddNewCourse(courseEntity,tokenSignUp);
                    if (courseResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                    {
                        Toast.MakeText(AppResources.SuccessCourse, Constant.durationOfToastMessage).Show();
                        NSUserDefaults.StandardUserDefaults.SetString(courseResponseEntity.operationMessage, AppResources.CourseId);
                        PerformSegue(AppResources.SegueFromCourseToAffidavit, this);
                    }
                    else
                    {
                        Toast.MakeText(courseResponseEntity.operationMessage, Constant.durationOfToastMessage).Show();
                    }
                }
                else
                {
                    Toast.MakeText(AppResources.Error, Constant.durationOfToastMessage).Show();
                }
            }
            catch (Exception ex)
            {
                utility.SaveExceptionHandling(ex, AppResources.AddCourse, AppResources.AddCourse, null);
            }
            finally
            {
                loadPop.Hide();
            }
        }
        /// <summary>
        /// Prepare for segue Course to Terms and Conditions screen.
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            try
            {
                base.PrepareForSegue(segue, sender);
                if (segue.Identifier == AppResources.SegueFromCourseToAffidavit)
                {
                    var navctlr = segue.DestinationViewController as WebViewController;
                    if (navctlr != null)
                    {
                        this.signUpRequestEntity.courseId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.CourseId);
                        navctlr.signUpRequestEntity = this.signUpRequestEntity;
                    }
                }  
            }
            catch(Exception ex)
            {
                utility.SaveExceptionHandling(ex,AppResources.PrepareForSegue, AppResources.AddCourse, null);
            }
        }
        /// <summary>
        /// click the text box of country to show country picker and display the country list in the UIPicker.
        /// </summary>
        public void TxtCountryClick()
        {
            UITapGestureRecognizer txtCountryClicked = new UITapGestureRecognizer(() =>
            {
                CountryPicker();
            });
            txtCountry.UserInteractionEnabled = true;
            txtCountry.AddGestureRecognizer(txtCountryClicked);
        }
        /// <summary>
        /// Gesture implementation for click on back image to go the previous page
        /// </summary>
        public void ImgBackCourseClick()
        {
            UITapGestureRecognizer imgBackArrowClicked = new UITapGestureRecognizer(() =>
            {
                this.NavigationController.PopViewController(true);
            });
            imgBackCourse.UserInteractionEnabled = true;
            imgBackCourse.AddGestureRecognizer(imgBackArrowClicked);
        }
        /// <summary>
        /// Dismiss the keyboard on click of view.
        /// </summary>
        public void DismissKeyboardClick()
        {
            UITapGestureRecognizer dismissKeyboardClicked = new UITapGestureRecognizer(() =>
            {
                utility.DismissKeyboardOnTap(new UIView[] { CourseView });
            });
            CourseView.UserInteractionEnabled = true;
            CourseView.AddGestureRecognizer(dismissKeyboardClicked);
        }
        /// <summary>
        /// Add done button to keyboard.
        /// </summary>
        public void AddDoneButtonToKeyboard()
        {
            UITextField[] textFields = new UITextField[] { txtCountry, txtState, txtCity, txtStreetAddress,txtZipCode };
            Utility.AddDoneButtonToKeyboard(textFields);
        }
       /// <summary>
       /// Text the zip code did begin editing.
       /// </summary>
       /// <param name="sender">Sender.</param>
        partial void txtZipCode_DidBeginEditing(UITextField sender)
        {
            btnDone.Hidden = true;
            statePicker.Hidden = true;
            proviencePicker.Hidden = true;
            countryPicker.Hidden = true;
        }
        /// <summary>
        /// Text the street address did begin editing.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtStreetAddress_DidBeginEditing(UITextField sender)
        {
            btnDone.Hidden = true;
            statePicker.Hidden = true;
            proviencePicker.Hidden = true;
            countryPicker.Hidden = true;
        }
        /// <summary>
        /// Text the city did begin editing.
        /// </summary>
        /// <param name="sender">Sender.</param>
        partial void txtCity_DidBeginEditing(UITextField sender)
        {
            btnDone.Hidden = true;
            statePicker.Hidden = true;
            proviencePicker.Hidden = true;
            countryPicker.Hidden = true;
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

