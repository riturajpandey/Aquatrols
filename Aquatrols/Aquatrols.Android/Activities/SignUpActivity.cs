using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Newtonsoft.Json;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used for sign up screen
    /// </summary>
    [Activity(WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
    public class SignUpActivity : Activity, View.IOnClickListener, View.IOnFocusChangeListener, View.IOnTouchListener
    {
        private WebView wbWebView;
        private Button btnSignUp, btnCancel, btnImagePicker, btndone;
        private TextInputLayout txtInputLayoutFirstname, txtInputLayoutCoursename, txtInputLayoutPhone, txtInputLayoutLastname, txtInputLayoutUsername, txtInputLayoutPassword, txtInputLayoutCPassword, txtInputLayoutCompanyNameReferal;
        private EditText editFirstname, editLastname, editPhone, editUsername, editPassword, editCPassword, editCompanyNameReferal, editReferalCode, editCourseDescription;
        private EditText[] editTxts;
        public static bool canSeePassword, canSeeConfirmPassword;
        public static string courseId = string.Empty;
        private string distributorId = string.Empty;
        private TextView txtAddress;
        private View vwBorder;
        private List<CourseEntity> lstCourseEntity = null;
        private List<DistinctDistributorEntity> lstDistributor = null;
        private AutoCompleteTextView autocompleteCoursename, autocompleteDSRname;
        public static readonly int PickImageId = 1000;
        private static Android.Net.Uri imgLocalUri = null;

        /// <summary>
        /// This method is used to initialize page load value and Get Token For Sign up
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                // Handle exception handling 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }

                // Create your application here
                SetContentView(Resource.Layout.SignUpLayout);
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode.
                FindControlsById();

                //Bind Signup url...
                wbWebView.LoadUrl("https://approachv3.aquatrols.com/WebAccount/Signup");
                
                //GetTokenForSignup();
                TextInputLayout[] textInputs = { txtInputLayoutFirstname, txtInputLayoutCoursename, txtInputLayoutPhone, txtInputLayoutLastname, txtInputLayoutUsername, txtInputLayoutPassword, txtInputLayoutCPassword, txtInputLayoutCompanyNameReferal };
                editTxts = new EditText[] { editUsername, editPassword, editCPassword, editFirstname, editLastname, editPhone, editCompanyNameReferal };
                #region //Google Analytics
                GoogleAnalytics.Current.Config.TrackingId = Resources.GetString(Resource.String.TrackingId);
                GoogleAnalytics.Current.Config.AppId = ApplicationContext.PackageName;
                GoogleAnalytics.Current.Config.AppName = this.ApplicationInfo.LoadLabel(PackageManager).ToString();
                GoogleAnalytics.Current.Config.AppVersion = this.PackageManager.GetPackageInfo(this.PackageName, 0).VersionName;
                GoogleAnalytics.Current.InitTracker();
                GoogleAnalytics.Current.Tracker.SendView("Signup");
                #endregion
                //Removing all error messages initially
                foreach (TextInputLayout txtInput in textInputs)
                {
                    txtInput.ErrorEnabled = false;
                    txtInput.Error = null;
                }
                foreach (EditText edit in editTxts)
                {
                    edit.OnFocusChangeListener = this;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Getting Reference of controls
        /// </summary>
        private void FindControlsById()
        {
            try
            {
                //txtAddress = FindViewById<TextView>(Resource.Id.txtAddress);
                //btnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
                btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
                btndone = FindViewById<Button>(Resource.Id.btnDONE);
                wbWebView = FindViewById<WebView>(Resource.Id.webViewSignUp);
                //editFirstname = FindViewById<EditText>(Resource.Id.editFirstname);
                //editLastname = FindViewById<EditText>(Resource.Id.editLastname);
                //editUsername = FindViewById<EditText>(Resource.Id.editUsername);
                //editPhone = FindViewById<EditText>(Resource.Id.editPhone);
                //editPassword = FindViewById<EditText>(Resource.Id.editPassword);
                //editCPassword = FindViewById<EditText>(Resource.Id.editCPassword);
                //editCompanyNameReferal = FindViewById<EditText>(Resource.Id.editCompanyNameReferal);
                //editReferalCode = FindViewById<EditText>(Resource.Id.editReferalCode);
                //vwBorder = FindViewById<View>(Resource.Id.vwborder);
                //autocompleteCoursename = FindViewById<AutoCompleteTextView>(Resource.Id.autocompleteCoursename);
                //autocompleteDSRname = FindViewById<AutoCompleteTextView>(Resource.Id.autocompleteDSRname);
                //editPassword.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                //editCPassword.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                //txtInputLayoutCoursename = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutCoursename);
                //txtInputLayoutFirstname = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutFirstname);
                //txtInputLayoutLastname = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutLastname);
                //txtInputLayoutUsername = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutUsername);
                //txtInputLayoutPhone = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutPhone);
                //txtInputLayoutPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutPassword);
                //txtInputLayoutCPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutCPassword);
                //txtInputLayoutCompanyNameReferal = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutCompanyNameReferal);
                //editCourseDescription = FindViewById<EditText>(Resource.Id.editCourseDescription);
                //btnImagePicker = FindViewById<Button>(Resource.Id.btnImagePicker);

                //#region Setting Events&Listeners
                //editPassword.AddTextChangedListener(new CustomTextWatcher(editPassword, this));
                //editCPassword.AddTextChangedListener(new CustomTextWatcher(editCPassword, this));
                //editPhone.AddTextChangedListener(new CustomTextWatcher(editPhone, this));
                //editFirstname.AddTextChangedListener(new CustomTextWatcher(editFirstname, this));
                //editLastname.AddTextChangedListener(new CustomTextWatcher(editLastname, this));
                //autocompleteCoursename.AddTextChangedListener(new CustomTextWatcher(autocompleteCoursename, this));
                ////button onclick listeners
                //btnSignUp.SetOnClickListener(this);
                btnCancel.SetOnClickListener(this);
                btndone.SetOnClickListener(this);
                //editPassword.SetOnTouchListener(this);
                //editCPassword.SetOnTouchListener(this);
                //btnImagePicker.SetOnClickListener(this);

                ////autocomplete textbox listeners
                //autocompleteCoursename.OnFocusChangeListener = this;
                //autocompleteCoursename.TextChanged += AutocompleteCoursename_TextChanged;
                //autocompleteCoursename.ItemClick += AutocompleteCoursename_ItemClick;
                //autocompleteDSRname.TextChanged += AutocompleteDSRname_TextChanged;
                //autocompleteDSRname.ItemClick += AutocompleteDSRname_ItemClick;


                //#endregion
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Autocomplete DSRname textview item click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteDSRname_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                AutoCompleteTextView autoText = (AutoCompleteTextView)sender;
                if (lstCourseEntity != null)
                {
                    distributorId = lstDistributor.Where(x => x.distributorName == autoText.Text).Select(x => x.did).FirstOrDefault();
                    if (string.IsNullOrEmpty(distributorId))
                    {
                        distributorId = string.Empty;
                    }
                    else
                    {
                        if (lstDistributor != null)
                        {
                            distributorId = lstDistributor.Where(x => x.distributorName == autocompleteDSRname.Text).Select(x => x.did).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteDSRname_ItemClick), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        ///  Raised when AutocompleteDSRname textbox value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteDSRname_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(autocompleteDSRname.Text))
                {
                    distributorId = string.Empty;
                }
                else
                {
                    if (lstDistributor != null)
                    {
                        distributorId = lstDistributor.Where(x => x.distributorName == autocompleteDSRname.Text).Select(x => x.did).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteDSRname_TextChanged), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when Autocompletecoursename textbox value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteCoursename_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(autocompleteCoursename.Text))
                {
                    txtAddress.Visibility = ViewStates.Gone;
                    vwBorder.Visibility = ViewStates.Gone;
                }
                else
                {
                    if (lstCourseEntity != null)
                    {
                        courseId = lstCourseEntity.Where(x => x.courseName == autocompleteCoursename.Text).Select(x => x.courseId).FirstOrDefault();
                        if (string.IsNullOrEmpty(courseId))
                        {
                            txtAddress.Visibility = ViewStates.Gone;
                            vwBorder.Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            GetAddress();
                            txtAddress.Visibility = ViewStates.Visible;
                            vwBorder.Visibility = ViewStates.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteCoursename_TextChanged), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Gets the address data and bind to Address textview
        /// </summary>
        public void GetAddress()
        {
            try
            {
                var country = lstCourseEntity.Where(x => x.courseId == courseId).Select(x => x.country).FirstOrDefault();
                var state = lstCourseEntity.Where(x => x.courseId == courseId).Select(x => x.state).FirstOrDefault();
                var city = lstCourseEntity.Where(x => x.courseId == courseId).Select(x => x.city).FirstOrDefault();
                var address = lstCourseEntity.Where(x => x.courseId == courseId).Select(x => x.address).FirstOrDefault();
                var zip = lstCourseEntity.Where(x => x.courseId == courseId).Select(x => x.zip).FirstOrDefault();
                txtAddress.Text = address + "," + city + "," + state + "," + zip + "," + country;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetAddress), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Autocomplete coursename item click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompleteCoursename_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                AutoCompleteTextView autoText = (AutoCompleteTextView)sender;
                if (lstCourseEntity != null)
                {
                    string[] strCourseName = autocompleteCoursename.Text.Split(new string[] { "(" }, StringSplitOptions.None);
                    string[] zipcode = strCourseName[1].Split(':');
                    zipcode[1] = zipcode[1].Trim().Replace(")", string.Empty);
                    courseId = lstCourseEntity.Where(x => x.courseName == strCourseName[0].TrimEnd() && x.zip == zipcode[1]).Select(x => x.courseId).FirstOrDefault();
                    if (!string.IsNullOrEmpty(courseId))
                    {
                        GetAddress();
                        txtAddress.Visibility = ViewStates.Visible;
                        vwBorder.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtAddress.Visibility = ViewStates.Gone;
                        vwBorder.Visibility = ViewStates.Gone;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.AutocompleteCoursename_ItemClick), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to call course API to get available courselist
        /// </summary>
        public async void HitCourseAPI(string signUpToken)
        {
            Show_Overlay();
            try
            {
                using (Utility util = new Utility(this))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        using (Utility utility = new Utility(this))
                        {
                            lstCourseEntity = await utility.GetCourseList(signUpToken);
                            //calling distributor list API
                            lstDistributor = await utility.GetDistinctDistributor(signUpToken);
                            List<string> distributors = lstDistributor.Select(x => x.distributorName).ToList();
                            var DSRadapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, distributors);
                            autocompleteDSRname.Adapter = DSRadapter;
                            string course = autocompleteCoursename.Text;
                            if (string.IsNullOrEmpty(course))
                            {
                                List<string> lstCourses = new List<string>();
                                foreach (var item in lstCourseEntity)
                                {
                                    lstCourses.Add(item.courseName + " (" + Resources.GetString(Resource.String.zipcode) + " : " + item.zip + ")");
                                }
                                var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, lstCourses);
                                autocompleteCoursename.Adapter = adapter;
                            }
                            else
                            {
                                List<string> lstCourses = lstCourseEntity.Where(x => x.courseName.ToLower().StartsWith(course.ToLower()) || x.courseName.Contains(course.ToLower())).Select(x => x.courseName).ToList();
                                if (lstCourseEntity != null || lstCourseEntity.Count != 0)
                                {
                                    var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, lstCourses);
                                    autocompleteCoursename.Adapter = adapter;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitCourseAPI), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// On click Event for button
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    //case Resource.Id.btnSignUp:
                    //    {
                    //        if (!string.IsNullOrEmpty(courseId))
                    //        {
                    //            SubmitForm();  //Calling the signup API
                    //        }
                    //        else
                    //        {
                    //            if (string.IsNullOrEmpty(autocompleteCoursename.Text.Trim()))
                    //            {
                    //                Toast.MakeText(this, Resources.GetString(Resource.String.RequireCoursename), ToastLength.Long).Show();
                    //            }
                    //            else
                    //            {
                    //                SubmitForm();
                    //            }
                    //        }
                    //        break;
                    //    }
                    case Resource.Id.btnCancel:
                        {
                            Finish();
                            break;
                        }
                    case Resource.Id.btnDONE:
                        {
                            Finish();
                            break;
                        }
                    //case Resource.Id.btnImagePicker:
                    //    {
                    //        OpenImagePicker();
                    //        break;
                    //    }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Checks Validation for autocomplete textbox 
        /// </summary>
        /// <param name="textview"></param>
        /// <param name="txtInputLayout"></param>
        /// <param name="hasFocus"></param>
        public void CheckValidationFor_AutocompleteTextview(AutoCompleteTextView textview, TextInputLayout txtInputLayout, bool hasFocus)
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    if (hasFocus == false)
                    {
                        if ((string.IsNullOrEmpty(textview.Text)))
                        {
                            txtAddress.Visibility = ViewStates.Gone;
                            vwBorder.Visibility = ViewStates.Gone;
                            txtInputLayoutCoursename.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.coursename));
                        }
                        else
                        {
                            if (lstCourseEntity != null)
                            {
                                string[] courseName = autocompleteCoursename.Text.Split('(');
                                courseId = lstCourseEntity.Where(x => x.courseName == courseName[0].TrimEnd()).Select(x => x.courseId).FirstOrDefault();
                                if (!string.IsNullOrEmpty(courseId))
                                {
                                    txtAddress.Visibility = ViewStates.Visible;
                                    vwBorder.Visibility = ViewStates.Visible;
                                }
                                else
                                {
                                    txtAddress.Visibility = ViewStates.Gone;
                                    vwBorder.Visibility = ViewStates.Gone;
                                }
                            }
                        }
                    }
                    else
                    {
                        txtInputLayoutCoursename.ErrorEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckValidationFor_AutocompleteTextview), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Checks Validation for edittext 
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="txtInputLayout"></param>
        /// <param name="hasFocus"></param>
        public void CheckValidation(EditText edit, TextInputLayout txtInputLayout, bool hasFocus)
        {
            try
            {
                //using (Utility utility = new Utility(this))
                //{
                //    if (hasFocus == false)
                //    {
                //        if (string.IsNullOrEmpty(edit.Text.Trim())) //Check if any edittext is empty except the redemption code
                //        {
                //            switch (edit.Id)
                //            {
                //                case Resource.Id.editFirstname:
                //                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.firstname));
                //                    break;
                //                case Resource.Id.editLastname:
                //                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.lastname));
                //                    break;
                //                case Resource.Id.editUsername:
                //                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.username));
                //                    break;
                //                case Resource.Id.editPhone:
                //                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.mobile));
                //                    break;
                //                case Resource.Id.editPassword:
                //                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.password));
                //                    break;
                //                case Resource.Id.editCPassword:
                //                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.cpassword));
                //                    break;
                //                default:
                //                    break;
                //            }
                //            return;
                //        }
                //        if (edit.Id == Resource.Id.editUsername)
                //        {
                //            if (!utility.IsValidEmail(editUsername.Text.Trim()))
                //            {
                //                txtInputLayout.Error = Resources.GetString(Resource.String.InvalidEmail);
                //            }
                //            else
                //            {
                //                txtInputLayout.ErrorEnabled = false;
                //            }
                //            return;
                //        }
                //        if (edit.Id == Resource.Id.editFirstname)
                //        {
                //            if (!string.IsNullOrEmpty(editFirstname.Text.Trim()))
                //                txtInputLayout.ErrorEnabled = false;
                //            //  editFirstname.Text=Utility.UppercaseWords(editFirstname.Text);
                //            return;
                //        }
                //        if (edit.Id == Resource.Id.editLastname)
                //        {
                //            if (!string.IsNullOrEmpty(editLastname.Text.Trim()))
                //                txtInputLayout.ErrorEnabled = false;
                //            //editLastname.Text = Utility.UppercaseWords(editLastname.Text);
                //            return;
                //        }
                //        if (edit.Id == Resource.Id.editPassword) //Check the valid password
                //        {
                //            if (!utility.IsValidPassword(editPassword.Text.Trim()))
                //            {
                //                txtInputLayout.Error = Resources.GetString(Resource.String.InvalidPassword);
                //            }
                //            else
                //            {
                //                txtInputLayout.ErrorEnabled = false;
                //            }
                //            return;
                //        }
                //        if (edit.Id == Resource.Id.editCPassword) //Check the valid password
                //        {
                //            if (!editCPassword.Text.Equals(editPassword.Text))
                //            {
                //                txtInputLayout.Error = Resources.GetString(Resource.String.PasswordNotMatch);
                //            }
                //            else
                //            {
                //                txtInputLayout.ErrorEnabled = false;
                //            }
                //            return;
                //        }
                //        if (edit.Id == Resource.Id.editPhone) //Check the valid Phone
                //        {
                //            if (!utility.IsValidPhone(editPhone.Text.Trim()))
                //            {
                //                txtInputLayout.Error = Resources.GetString(Resource.String.InvalidPhone);
                //            }
                //            else
                //            {
                //                txtInputLayout.ErrorEnabled = false;
                //            }
                //            return;
                //        }
                //    }
                //    else //when focus comes again
                //    {
                //        txtInputLayout.ErrorEnabled = false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckValidation), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to check validation while submit the details
        /// </summary>
        private void SubmitForm()
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    bool status = true;

                    //foreach (EditText edit in editTxts)
                    //{
                    //    if (edit.Id == Resource.Id.editFirstname)
                    //    {
                    //        if (string.IsNullOrEmpty(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutFirstname.Error = Resources.GetString(Resource.String.RequireFirstName);
                    //            editFirstname.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            txtInputLayoutFirstname.ErrorEnabled = false;
                    //        }
                    //    }
                    //    else if (edit.Id == Resource.Id.editLastname)
                    //    {
                    //        if (string.IsNullOrEmpty(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutLastname.Error = Resources.GetString(Resource.String.RequireLastName);
                    //            editLastname.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            txtInputLayoutLastname.ErrorEnabled = false;
                    //        }
                    //    }
                    //    else if (edit.Id == Resource.Id.editUsername)
                    //    {
                    //        if (string.IsNullOrEmpty(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutUsername.Error = Resources.GetString(Resource.String.RequireUserName);
                    //            editUsername.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //        else if (!utility.IsValidEmail(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutUsername.Error = Resources.GetString(Resource.String.InvalidEmail);
                    //            editUsername.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            txtInputLayoutUsername.ErrorEnabled = false;
                    //        }
                    //    }
                    //    else if (edit.Id == Resource.Id.editPhone)
                    //    {
                    //        if (string.IsNullOrEmpty(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutPhone.Error = Resources.GetString(Resource.String.RequirePhone);
                    //            editPhone.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //        else if (!utility.IsValidPhone(edit.Text.Trim()) || !edit.Text.Trim().Any(Char.IsDigit))
                    //        {
                    //            txtInputLayoutPhone.Error = Resources.GetString(Resource.String.InvalidPhone);
                    //            editPhone.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //    }
                    //    else if (edit.Id == Resource.Id.editPassword && string.IsNullOrEmpty(edit.Text.Trim()))
                    //    {
                    //        if (string.IsNullOrEmpty(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutPassword.Error = Resources.GetString(Resource.String.RequirePassword);
                    //            editPassword.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //        else if (!utility.IsValidPassword(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutPassword.Error = Resources.GetString(Resource.String.InvalidPassword);
                    //            editPassword.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //    }
                    //    else if (edit.Id == Resource.Id.editCPassword && string.IsNullOrEmpty(edit.Text.Trim()))
                    //    {
                    //        if (string.IsNullOrEmpty(edit.Text.Trim()))
                    //        {
                    //            txtInputLayoutCPassword.Error = Resources.GetString(Resource.String.RequireCPassword);
                    //            editCPassword.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //        else if (!editCPassword.Text.Equals(editPassword.Text))
                    //        {
                    //            txtInputLayoutCPassword.Error = Resources.GetString(Resource.String.PasswordNotMatch);
                    //            editCPassword.RequestFocus();
                    //            status = false;
                    //            return;
                    //        }
                    //    }
                    //}
                    //if (status)
                    //{
                    //    if (string.IsNullOrEmpty(courseId))
                    //    {
                    //        Toast.MakeText(this, Resource.String.newCourseEntered, ToastLength.Long).Show();
                    //        Intent intent = new Intent(this, typeof(AddCourseActivity));
                    //        SignUpRequestEntity userEntity = GetUserEntity();
                    //        if (imgLocalUri != null)
                    //            userEntity.CourseAffiliationProofFilePath = imgLocalUri.ToString();
                    //        intent.PutExtra("User", JsonConvert.SerializeObject(userEntity));
                    //        Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.about), autocompleteCoursename.Text).Commit();
                    //        StartActivity(intent);
                    //    }
                    //    else
                    //    {
                    //        Intent intent = new Intent(this, typeof(WebViewActivity));
                    //        SignUpRequestEntity userEntity = GetUserEntity();
                    //        if (imgLocalUri != null)
                    //            userEntity.CourseAffiliationProofFilePath = imgLocalUri.ToString();
                    //        intent.PutExtra("User", JsonConvert.SerializeObject(userEntity));
                    //        StartActivity(intent);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SubmitForm), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Calling API to get token for signup
        /// </summary>
        public async void GetTokenForSignup()
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    bool internetStatus = utility.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        AccessTokenResponseEntity accessTokenResponseEntity = await utility.GetTokenForSignup();
                        if (accessTokenResponseEntity != null)
                        {
                            HitCourseAPI(accessTokenResponseEntity.token);
                        }
                        else
                        {
                            Toast.MakeText(this, Resource.String.ServiceError, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTokenForSignup), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// Getting signup request data for sending to the signup API
        /// </summary>
        /// <returns></returns>
        public SignUpRequestEntity GetUserEntity()
        {
            SignUpRequestEntity signUpEntity = new SignUpRequestEntity();
            try
            {
                signUpEntity.firstName = editFirstname.Text;
                signUpEntity.lastName = editLastname.Text;
                signUpEntity.phoneNumber = editPhone.Text;
                signUpEntity.email = editUsername.Text;
                signUpEntity.password = editPassword.Text;
                signUpEntity.confirmPassword = editCPassword.Text;
                signUpEntity.courseId = courseId;
                signUpEntity.referralDSRName = distributorId;
                signUpEntity.referralCompany = editCompanyNameReferal.Text;
                signUpEntity.referralCode = editReferalCode.Text;
                signUpEntity.CourseAffiliationDescription = editCourseDescription.Text;
                signUpEntity.CourseAffiliationProofFilePath = "";   // default value 
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetUserEntity), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
            return signUpEntity;
        }

        /// <summary>
        /// Raised when focus gets changed of controls
        /// </summary>
        /// <param name="v"></param>
        /// <param name="hasFocus"></param>
        public void OnFocusChange(View v, bool hasFocus)
        {
            try
            {
                //switch (v.Id)
                //{
                //    case Resource.Id.autocompleteCoursename:
                //        CheckValidationFor_AutocompleteTextview(autocompleteCoursename, txtInputLayoutCoursename, hasFocus);
                //        break;
                //    case Resource.Id.editUsername:
                //        CheckValidation(editUsername, txtInputLayoutUsername, hasFocus);
                //        break;
                //    case Resource.Id.editFirstname:
                //        CheckValidation(editFirstname, txtInputLayoutFirstname, hasFocus);
                //        break;
                //    case Resource.Id.editLastname:
                //        CheckValidation(editLastname, txtInputLayoutLastname, hasFocus);
                //        break;
                //    case Resource.Id.editPhone:
                //        CheckValidation(editPhone, txtInputLayoutPhone, hasFocus);
                //        break;
                //    case Resource.Id.editPassword:
                //        CheckValidation(editPassword, txtInputLayoutPassword, hasFocus);
                //        break;
                //    case Resource.Id.editCPassword:
                //        CheckValidation(editCPassword, txtInputLayoutCPassword, hasFocus);
                //        break;
                //    case Resource.Id.editCompanyNameReferal:
                //        CheckValidation(editCompanyNameReferal, txtInputLayoutCompanyNameReferal, hasFocus);
                //        break;
                //    default:
                //        break;
                //}
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnFocusChange), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// This method shows an overlay dialog.
        /// </summary>
        OverlayActivity overlay;
        public void Show_Overlay()
        {
            try
            {
                overlay = new OverlayActivity(this);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        /// <summary>
        /// On touch listener for edittext if user touches the eye icon
        /// </summary>
        /// <param name="v"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnTouch(View v, MotionEvent e)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.editPassword:
                        {
                            if (e.Action == MotionEventActions.Down)
                            {
                                if (editPassword.GetCompoundDrawables()[2] != null)
                                {
                                    if (e.RawX >= (editPassword.Right - editPassword.GetCompoundDrawables()[2].Bounds.Width()))
                                    {
                                        ShowHidePassword(editPassword);
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    //case Resource.Id.editCPassword:
                    //    {
                    //        if (e.Action == MotionEventActions.Down)
                    //        {
                    //            if (editCPassword.GetCompoundDrawables()[2] != null)
                    //            {
                    //                if (e.RawX >= (editCPassword.Right - editCPassword.GetCompoundDrawables()[2].Bounds.Width()))
                    //                {
                    //                    ShowHidePassword(editCPassword);
                    //                    return true;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnTouch), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
            return false;
        }

        /// <summary>
        /// Show/Hide password when touch on eye icon
        /// </summary>
        /// <param name="edittext"></param>
        public void ShowHidePassword(EditText edittext)
        {
            try
            {
                EditText edit = edittext;
                switch (edit.Id)
                {
                    case Resource.Id.editPassword:
                        {
                            if (canSeePassword)
                            {
                                canSeePassword = false;
                                edit.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisible, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            else
                            {
                                canSeePassword = true;
                                edit.InputType = InputTypes.TextVariationPassword;
                                edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisible, 0);
                                edit.SetSelection(edit.Text.Length);
                            }
                            break;
                        }
                    //case Resource.Id.editCPassword:
                    //    {
                    //        if (canSeePassword)
                    //        {
                    //            canSeePassword = false;
                    //            edit.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
                    //            edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconVisible, 0);
                    //            edit.SetSelection(edit.Text.Length);
                    //        }
                    //        else
                    //        {
                    //            canSeePassword = true;
                    //            edit.InputType = InputTypes.TextVariationPassword;
                    //            edit.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.iconInvisible, 0);
                    //            edit.SetSelection(edit.Text.Length);
                    //        }
                    //        break;
                    //    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ShowHidePassword), Resources.GetString(Resource.String.SignUpActivity), null);
                }
            }
        }

        private void OpenImagePicker()
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        // Create a Method OnActivityResult(it is select the image controller)   
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                if (uri != null)
                {
                    imgLocalUri = uri;
                    try
                    {
                        var cursor = ContentResolver.Query(uri, null, null, null);
                        var fileNameIndex = cursor.GetColumnIndex(OpenableColumns.DisplayName);
                        cursor.MoveToFirst();
                        string filename = cursor.GetString(fileNameIndex);
                        btnImagePicker.Text = filename;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
    }
}