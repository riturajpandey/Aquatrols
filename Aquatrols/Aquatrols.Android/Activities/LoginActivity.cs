using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Aquatrols.Droid.Fragments;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Plugin.GoogleAnalytics;
using System;
using System.Linq;
using Xamarin.Essentials;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    /// This Activity class file is used for login screen
    /// </summary>
    [Activity(WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
    public class LoginActivity : Activity, View.IOnClickListener, View.IOnTouchListener, View.IOnFocusChangeListener
    {
        private Button btnSignin, btnJoinNow, btnForgotPassword;
        private EditText editUsername, editPassword;
        private TextInputLayout txtInputLayoutUsername, txtInputLayoutPassword;
        public static bool canSeePassword;
        private const string NOTIFICATION_ACTION = Constants.receiverValue;
        NotificationBroadcastReceiver notificationBroadcastReceiver = new NotificationBroadcastReceiver();

        /// <summary>
        /// This method is used to initialize page load value and implement google analytics
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
                SetContentView(Resource.Layout.LoginLayout);           //setting Layout View=SignInLayout
                RequestedOrientation = ScreenOrientation.Portrait;      //disabling Landscape mode.
                FindControlsById();
                if (!string.IsNullOrEmpty(Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null)))
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                    StartActivity(intent);
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                alert.SetTitle(Resources.GetString(Resource.String.error));
                alert.SetMessage(ex.StackTrace);
                alert.SetPositiveButton(Resources.GetString(Resource.String.Ok), (senderAlert, args) =>
                {
                });
                alert.SetNegativeButton(Resources.GetString(Resource.String.cancel), (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
            // implement google analytics
            GoogleAnalytics.Current.Config.TrackingId = Resources.GetString(Resource.String.TrackingId);
            GoogleAnalytics.Current.Config.AppId = ApplicationContext.PackageName;
            GoogleAnalytics.Current.Config.AppName = this.ApplicationInfo.LoadLabel(PackageManager).ToString();
            GoogleAnalytics.Current.Config.AppVersion = this.PackageManager.GetPackageInfo(this.PackageName, 0).VersionName;
            GoogleAnalytics.Current.InitTracker();
            GoogleAnalytics.Current.Tracker.SendView(Resources.GetString(Resource.String.Login));
        }

        /// <summary>
        /// Getting the reference of controls
        /// </summary>
        private void FindControlsById()
        {
            try
            {
                btnSignin = FindViewById<Button>(Resource.Id.btnSignin);
                btnJoinNow = FindViewById<Button>(Resource.Id.btnJoinNow);
                btnForgotPassword = FindViewById<Button>(Resource.Id.btnForgotPassword);
                editPassword = FindViewById<EditText>(Resource.Id.editPassword);
                editPassword.SetCompoundDrawablesWithIntrinsicBounds(0, 0, 0, 0);
                editUsername = FindViewById<EditText>(Resource.Id.editUsername);
                txtInputLayoutUsername = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutUsername);
                txtInputLayoutPassword = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutPassword);
                editPassword.AddTextChangedListener(new CustomTextWatcher(editPassword, this));

                #region Setting Events&Listeners
                editPassword.SetOnTouchListener(this);
                btnSignin.SetOnClickListener(this);
                btnJoinNow.SetOnClickListener(this);
                btnForgotPassword.SetOnClickListener(this);
                TextInputLayout[] textInputs = { txtInputLayoutUsername, txtInputLayoutPassword };
                //Removing all error messages initially
                foreach (TextInputLayout txtInput in textInputs)
                {
                    txtInput.ErrorEnabled = false;
                    txtInput.Error = null;
                }
                EditText[] editTxts = { editPassword, editUsername };
                foreach (EditText edit in editTxts)
                {
                    edit.OnFocusChangeListener = this;
                }
                //btn.OnFocusChangeListener = this;
                #endregion
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
        }

        /// <summary>
        /// Click event handler for Button
        /// </summary>
        /// <param name="v"></param>
        public async void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.btnSignin:
                        {
                            HitSignInAPI();  //Calling the Login API
                            break;
                        }
                    case Resource.Id.btnJoinNow:
                        {
                            var uri = new Uri("https://approachv3.aquatrols.com/WebAccount/Signup");
                            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                            //Intent intent = new Intent(this, typeof(SignUpActivity));
                            //intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                            //StartActivity(intent);
                            break;
                        }
                    case Resource.Id.btnForgotPassword:
                        {
                            //Open Fragment dialog
                            FragmentTransaction transaction = FragmentManager.BeginTransaction();
                            ForgetDialogFragment dialog = new ForgetDialogFragment(this);
                            dialog.Show(transaction, "");
                            dialog.Cancelable = false;
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.LoginActivity), null);
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
        }

        /// <summary>
        /// Checking validation for controls
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            Utility utility = new Utility(this);
            if (string.IsNullOrEmpty(editUsername.Text.Trim()))
            {
                txtInputLayoutUsername.Error = Resources.GetString(Resource.String.RequireUserName);
                editUsername.RequestFocus();
                return false;
            }
            else if (string.IsNullOrEmpty(editPassword.Text.Trim()))
            {
                txtInputLayoutPassword.Error = Resources.GetString(Resource.String.RequirePassword);
                editPassword.RequestFocus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method validates the user inputs and hits the SignIn API
        /// </summary>
        private async void HitSignInAPI()
        {
            try
            {
                Show_Overlay();//SHOWS AN OVERLAY ON SCREEN TO PREVENT THE USER INTERACTION.
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                if (IsValid())
                {
                    txtInputLayoutUsername.ErrorEnabled = false;
                    txtInputLayoutPassword.ErrorEnabled = false;

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
                            LoginRequestEntity loginEntity = new LoginRequestEntity();
                            loginEntity.username = editUsername.Text.Trim();
                            loginEntity.password = editPassword.Text;
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.Smusername), editUsername.Text.Trim()).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.password), editPassword.Text).Commit();
                            LoginResponseEntity loginResponseEntity = await util.UserLogin(loginEntity);
                            if (loginResponseEntity != null)
                            {
                                if (loginResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                                {
                                    var usertype = loginResponseEntity.role.Where(a => a.Equals("Manager")).FirstOrDefault();
                                    if (usertype != null)
                                    {
                                        if (loginResponseEntity.role[0].ToLower().Equals(Resources.GetString(Resource.String.Admin)))
                                        {
                                            //Toast.MakeText(this, Resources.GetString(Resource.String.unauthorized), ToastLength.Long).Show();
                                            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                                            alert.SetTitle("Alert");
                                            alert.SetMessage("There is already an Approach account registered with this golf course or business. Please contact approach@aquatrols.com for more information about this issue. Thank you.");
                                            Dialog dialog = alert.Create();
                                            dialog.Show();
                                            dialog.SetCancelable(true);
                                            editPassword.Text = string.Empty;
                                            editUsername.Text = string.Empty;
                                        }
                                        else
                                        {
                                            Intent intent = new Intent(this, typeof(MainActivity));
                                            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), loginResponseEntity.token).Commit();
                                            //Intent intent = new Intent(this, typeof(MainActivity));
                                            //intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.IsTerritory), loginResponseEntity.role[1]).Commit();
                                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.Role), loginResponseEntity.role[0]).Commit();
                                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), loginResponseEntity.userId).Commit();
                                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), loginResponseEntity.token).Commit();
                                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.isApproved), Convert.ToString(loginResponseEntity.isApproved)).Commit();
                                            StartActivity(intent);
                                            Finish();
                                        }
                                    }
                                    else
                                    {
                                        Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                                        alert.SetTitle("Alert");
                                        alert.SetMessage("There is already an Approach account registered with this golf course or business. Please contact approach@aquatrols.com for more information about this issue. Thank you.");
                                        
                                        Dialog dialog = alert.Create();
                                        dialog.Show();
                                        dialog.SetCancelable(true);
                                        editPassword.Text = string.Empty;
                                        editUsername.Text = string.Empty;
                                    }
                                }
                                else
                                {
                                    editPassword.Text = string.Empty;
                                    Toast.MakeText(this, loginResponseEntity.operationMessage, ToastLength.Long).Show();
                                }
                            }
                            else
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.ServiceError), ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitSignInAPI), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
            finally
            {
                if (overlay != null)
                    overlay.Dismiss();
            }
        }

        /// <summary>
        /// Show/hide password when click on eye icon
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
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnTouch), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
            return false;
        }

        /// <summary>
        /// Show/hide the password
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
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ShowHidePassword), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
        }

        /// <summary>
        /// raised when focus changes of edittext
        /// </summary>
        /// <param name="v"></param>
        /// <param name="hasFocus"></param>
        public void OnFocusChange(View v, bool hasFocus)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.editUsername:
                        CheckValidation(editUsername, txtInputLayoutUsername, hasFocus);
                        break;
                    case Resource.Id.editPassword:
                        CheckValidation(editPassword, txtInputLayoutPassword, hasFocus);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnFocusChange), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
        }

        /// <summary>
        /// Checking Validation for Edittext controls
        /// </summary>
        /// <param name="edittxt"></param>
        /// <param name="txtInputLayout"></param>
        /// <param name="hasFocus"></param>
        private void CheckValidation(EditText edittxt, TextInputLayout txtInputLayout, bool hasFocus)
        {
            try
            {
                using (Utility utility = new Utility(this))
                {
                    if (hasFocus == false)
                    {
                        if (string.IsNullOrEmpty(edittxt.Text.Trim())) //Check if any edittext is empty except the redemption code
                        {
                            switch (edittxt.Id)
                            {
                                case Resource.Id.editUsername:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.RequireUserName);
                                    break;
                                case Resource.Id.editPassword:
                                    txtInputLayout.Error = Resources.GetString(Resource.String.RequirePassword);
                                    break;
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckValidation), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
        }

        /// <summary>
        /// system On Start method
        /// </summary>
        protected override void OnStart()
        {
            try
            {
                base.OnStart();
                IntentFilter intentFilter = new IntentFilter(NOTIFICATION_ACTION);
                RegisterReceiver(notificationBroadcastReceiver, intentFilter);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnStart), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
        }

        /// <summary>
        /// system On Stop method
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                base.OnStop();
                UnregisterReceiver(notificationBroadcastReceiver);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnStop), Resources.GetString(Resource.String.LoginActivity), null);
                }
            }
        }
    }
}