using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Newtonsoft.Json;
using Plugin.GoogleAnalytics;
using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This class is used to show the course list on Sign Up page.
    /// </summary>
    [Activity]
    public class AddCourseActivity : Activity, View.IOnClickListener, View.IOnFocusChangeListener
    {
        private ArrayAdapter countryAdapter, stateAdapter, provincesAdapter;
        private Spinner spinnerCountry, spinnerState, spinnerProvinces;
        private LinearLayout linearLayoutState, linearLayoutProvinces, linearLayoutCountry;
        private Button btnAdd, btnCancel;
        private TextInputLayout txtInputLayoutcity, txtInputLayoutStreetAddress, txtInputLayoutZip;
        private EditText editCity, editStreetAddress, editZip;
        private string spinnerCountryValue, spinnerProvincesValue, spinnerStateValue;
        private SignUpRequestEntity signUpEntity;
        private EditText[] editTxts = null;
        private Dictionary<string, string> lstProvinces, lstStates;

        /// <summary>
        /// This method is used to initialize page load value and  bind country and state value
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.AddCourseLayout);// Create your application here
                // Handle exception handling 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                FindControlsById();
                TextInputLayout[] textInputs = { txtInputLayoutStreetAddress, txtInputLayoutcity, txtInputLayoutZip };
                editTxts = new EditText[] { editCity, editStreetAddress, editZip };
                BindCountry();
                BindStates();
                Bindprovinces();
                signUpEntity = JsonConvert.DeserializeObject<SignUpRequestEntity>(Intent.GetStringExtra("User"));
                //added item selected events
                spinnerCountry.ItemSelected += SpinnerCountry_ItemSelected;
                spinnerState.ItemSelected += SpinnerState_ItemSelected;
                spinnerProvinces.ItemSelected += SpinnerProvinces_ItemSelected;
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
                // setting onclick listener for buttons
                btnAdd.SetOnClickListener(this);
                btnCancel.SetOnClickListener(this);
                #region //Google Analytics
                GoogleAnalytics.Current.Config.TrackingId = Resources.GetString(Resource.String.TrackingId);
                GoogleAnalytics.Current.Config.AppId = this.ApplicationContext.PackageName;
                GoogleAnalytics.Current.Config.AppName = this.ApplicationInfo.LoadLabel(this.PackageManager).ToString();
                GoogleAnalytics.Current.Config.AppVersion = this.PackageManager.GetPackageInfo(this.PackageName, 0).VersionName;
                GoogleAnalytics.Current.InitTracker();
                GoogleAnalytics.Current.Tracker.SendView("AddCourse");
                #endregion
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Geeting the reference of the controls
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                spinnerCountry = FindViewById<Spinner>(Resource.Id.spinnerCountry);
                spinnerState = FindViewById<Spinner>(Resource.Id.spinnerState);
                spinnerProvinces = FindViewById<Spinner>(Resource.Id.spinnerProvinces);
                linearLayoutState = FindViewById<LinearLayout>(Resource.Id.linearLayoutState);
                linearLayoutProvinces = FindViewById<LinearLayout>(Resource.Id.linearLayoutProvinces);
                linearLayoutCountry = FindViewById<LinearLayout>(Resource.Id.linearLayoutCountry);
                txtInputLayoutcity = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutCity);
                txtInputLayoutStreetAddress = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutStreetAddress);
                txtInputLayoutZip = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutZip);
                editCity = FindViewById<EditText>(Resource.Id.editCity);
                editZip = FindViewById<EditText>(Resource.Id.editZip);
                editStreetAddress = FindViewById<EditText>(Resource.Id.editStreetAddress);
                btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
                btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when provinces dropdown value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinnerProvinces_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                string result;
                Spinner spinner = (Spinner)sender;
                spinnerProvincesValue = spinner.GetItemAtPosition(e.Position).ToString();
                if (lstProvinces.ContainsKey(spinnerProvincesValue))
                {
                    result = lstProvinces[spinnerProvincesValue];
                    spinnerProvincesValue = result;
                }
                else
                {
                    spinnerProvincesValue = string.Empty;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SpinnerProvinces_ItemSelected), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when state dropdown value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinnerState_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                string result;
                Spinner spinner = (Spinner)sender;
                spinnerStateValue = spinner.GetItemAtPosition(e.Position).ToString();
                if (lstStates.ContainsKey(spinnerStateValue))
                {
                    result = lstStates[spinnerStateValue];
                    spinnerStateValue = result;
                }
                else
                {
                    spinnerStateValue = string.Empty;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SpinnerState_ItemSelected), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when country dropdown value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinnerCountry_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                Spinner spinner = (Spinner)sender;
                spinnerCountryValue = spinner.GetItemAtPosition(e.Position).ToString();
                if (!spinnerCountryValue.Equals(Resources.GetString(Resource.String.selectCountry)))
                {
                    if (spinnerCountryValue.Equals(Resources.GetString(Resource.String.USA)))
                    {
                        linearLayoutState.Visibility = ViewStates.Visible;
                        linearLayoutProvinces.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        linearLayoutState.Visibility = ViewStates.Gone;
                        linearLayoutProvinces.Visibility = ViewStates.Visible;
                    }
                }
                else
                {
                    linearLayoutState.Visibility = ViewStates.Gone;
                    linearLayoutProvinces.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SpinnerCountry_ItemSelected), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Binding country list to country dropdown
        /// </summary>
        public void BindCountry()
        {
            try
            {
                countryAdapter = new ArrayAdapter<String>(this, Resource.Layout.spinner_item_selected, Constants.liCountry);
                countryAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                spinnerCountry.Adapter = countryAdapter;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BindCountry), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Binding state list to state dropdown
        /// </summary>
        public void BindStates()
        {
            try
            {
                lstStates = Constants.GetStates();
                List<string> liStates = new List<string>();
                foreach (var item in lstStates)
                {
                    liStates.Add(item.Key);
                }
                stateAdapter = new ArrayAdapter<String>(this, Resource.Layout.spinner_item_selected, liStates);
                stateAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                spinnerState.Adapter = stateAdapter;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.BindStates), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        ///Binding Provinces list to Provinces dropdown
        /// </summary>
        public void Bindprovinces()
        {
            try
            {
                lstProvinces = Constants.GetProvince();
                List<string> liProvinces = new List<string>();
                foreach (var item in lstProvinces)
                {
                    liProvinces.Add(item.Key);
                }
                provincesAdapter = new ArrayAdapter<String>(this, Resource.Layout.spinner_item_selected, liProvinces);
                provincesAdapter.SetDropDownViewResource(Resource.Layout.spinner_item);
                spinnerProvinces.Adapter = provincesAdapter;
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Bindprovinces), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Getting the course entity data for passing to the Add course API
        /// </summary>
        /// <returns></returns>
        public CourseEntity GetCourseEntity()
        {
            CourseEntity courseEntity = new CourseEntity();
            try
            {
                courseEntity.courseName = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.about), null);
                courseEntity.city = editCity.Text;
                courseEntity.address = editStreetAddress.Text;
                courseEntity.zip = editZip.Text;
                courseEntity.country = spinnerCountryValue;
                if (linearLayoutState.Visibility == ViewStates.Visible)
                {
                    courseEntity.state = spinnerStateValue;
                }
                if (linearLayoutProvinces.Visibility == ViewStates.Visible)
                {
                    courseEntity.state = spinnerProvincesValue;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetCourseEntity), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
            return courseEntity;
        }

        /// <summary>
        /// Showing wait cursor
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.AddCourseActivity), null);
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
                    Show_Overlay();
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
                            HitAddCourseAPI(accessTokenResponseEntity.token);
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTokenForSignup), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// Calling API to add course
        /// </summary>
        public async void HitAddCourseAPI(string signuptoken)
        {
            CourseResponseEntity courseResponseEntity;
            try
            {
                using (Utility utility = new Utility(this))
                {
                    Show_Overlay();
                    bool internetStatus = utility.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        courseResponseEntity = await utility.AddNewCourse(GetCourseEntity(), signuptoken);
                        if (courseResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                        {
                            Toast.MakeText(this, Resources.GetString(Resource.String.courseAdded), ToastLength.Long).Show();
                            Intent intent = new Intent(this, typeof(WebViewActivity));
                            signUpEntity.courseId = courseResponseEntity.operationMessage;
                            intent.PutExtra(Resources.GetString(Resource.String.User), JsonConvert.SerializeObject(signUpEntity));
                            StartActivityForResult(intent, 10);
                        }
                        else
                        {
                            Toast.MakeText(this, courseResponseEntity.operationMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitAddCourseAPI), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
            finally
            {
                overlay.Dismiss();
            }
        }

        /// <summary>
        /// On activity result Method
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if (resultCode == Result.Ok)
                {
                    Intent intent = new Intent(this, typeof(SignUpActivity));
                    Finish();
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnActivityResult), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// system Back button press event
        /// </summary>
        public override void OnBackPressed()
        {
            Finish();
        }

        /// <summary>
        /// Button click event 
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.btnAdd:
                        Submit();
                        break;
                    case Resource.Id.btnCancel:
                        Finish();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Method to check validation and call API to get token for signup
        /// </summary>
        public void Submit()
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    bool status = true;
                    if (spinnerCountryValue.Equals(Resources.GetString(Resource.String.selectCountry)))
                    {
                        Toast.MakeText(this, Resources.GetString(Resource.String.selectCountry), ToastLength.Long).Show();
                        status = false;
                    }
                    else if (linearLayoutState.Visibility == ViewStates.Visible && spinnerStateValue.Equals(Resources.GetString(Resource.String.selectState)))
                    {
                        Toast.MakeText(this, Resources.GetString(Resource.String.selectState), ToastLength.Long).Show();
                        status = false;
                    }
                    else if (linearLayoutProvinces.Visibility == ViewStates.Visible && spinnerProvincesValue.Equals(Resources.GetString(Resource.String.selectProvinces)))
                    {
                        Toast.MakeText(this, Resources.GetString(Resource.String.selectProvinces), ToastLength.Long).Show();
                        status = false;
                    }
                    else
                    {
                        foreach (EditText edit in editTxts)
                        {
                            if (edit.Id == Resource.Id.editCity)
                            {
                                if (string.IsNullOrEmpty(edit.Text.Trim()))
                                {
                                    txtInputLayoutcity.ErrorEnabled = true;
                                    txtInputLayoutcity.Error = Resources.GetString(Resource.String.RequireCity);
                                    editCity.RequestFocus();
                                    status = false;
                                    return;
                                }
                                else
                                {
                                    txtInputLayoutcity.ErrorEnabled = false;
                                }
                            }
                            else if (edit.Id == Resource.Id.editStreetAddress)
                            {
                                if (string.IsNullOrEmpty(edit.Text.Trim()))
                                {
                                    txtInputLayoutStreetAddress.Error = Resources.GetString(Resource.String.RequireStreetAddress);
                                    editStreetAddress.RequestFocus();
                                    status = false;
                                    return;
                                }
                                else
                                {
                                    txtInputLayoutcity.ErrorEnabled = false;
                                }
                            }
                            else if (edit.Id == Resource.Id.editZip)
                            {
                                if (string.IsNullOrEmpty(edit.Text.Trim()))
                                {
                                    txtInputLayoutZip.Error = Resources.GetString(Resource.String.RequireZip);
                                    editZip.RequestFocus();
                                    status = false;
                                    return;
                                }
                                else if (utility.IsValidZip(edit.Text, spinnerCountryValue) == false)
                                {
                                    txtInputLayoutZip.Error = Resources.GetString(Resource.String.InvalidZip);
                                    editZip.RequestFocus();
                                    status = false;
                                    return;
                                }
                                else
                                {
                                    txtInputLayoutZip.ErrorEnabled = false;
                                }
                            }
                        }
                    }
                    if (status)
                    {
                        GetTokenForSignup();
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Submit), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Checking validation for controls when focus change
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="txtInputLayout"></param>
        /// <param name="hasFocus"></param>
        public void CheckValidation(EditText edit, TextInputLayout txtInputLayout, bool hasFocus)
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    if (hasFocus == false)
                    {
                        if (string.IsNullOrEmpty(edit.Text.Trim())) //Check if any edittext is empty except the redemption code
                        {
                            switch (edit.Id)
                            {
                                case Resource.Id.editCity:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.cityname));
                                    break;
                                case Resource.Id.editStreetAddress:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.address));
                                    break;
                                case Resource.Id.editZip:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireField).Replace("##", Resources.GetString(Resource.String.zipcode));
                                    break;
                                default:
                                    break;
                            }
                            return;
                        }
                        if (edit.Id == Resource.Id.editZip)
                        {
                            if (!spinnerCountryValue.Equals(Resources.GetString(Resource.String.selectCountry)))
                            {
                                if (string.IsNullOrEmpty(editZip.Text) || (!utility.IsValidZip(edit.Text, spinnerCountryValue)))
                                {
                                    txtInputLayoutZip.Error = Resources.GetString(Resource.String.InvalidZip);
                                    editZip.RequestFocus();
                                }
                                else
                                {
                                    txtInputLayoutZip.ErrorEnabled = false;
                                }
                            }
                            return;
                        }
                    }
                    else //when focus comes again
                    {
                        txtInputLayout.ErrorEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckValidation), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }

        /// <summary>
        /// Raised when focus gets changed of edit text
        /// </summary>
        /// <param name="v"></param>
        /// <param name="hasFocus"></param>
        public void OnFocusChange(View v, bool hasFocus)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.editCity:
                        CheckValidation(editCity, txtInputLayoutcity, hasFocus);
                        break;
                    case Resource.Id.editStreetAddress:
                        CheckValidation(editStreetAddress, txtInputLayoutStreetAddress, hasFocus);
                        break;
                    case Resource.Id.editZip:
                        CheckValidation(editZip, txtInputLayoutZip, hasFocus);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnFocusChange), Resources.GetString(Resource.String.AddCourseActivity), null);
                }
            }
        }
    }
}