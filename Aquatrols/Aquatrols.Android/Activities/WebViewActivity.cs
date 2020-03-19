using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Views.InputMethods;
using Android.Webkit;
using Android.Widget;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Activities
{
    /// <summary>
    ///  This class is the web view where we pass terms and condition or other detail of user
    /// </summary>
    [Activity]
    public class WebViewActivity : Activity, View.IOnClickListener
    {
        private WebView webview;
        private Button btnAccept, btnReject;
        private CheckBox chkAgree;
        private ImageView imgBack;
        private SignUpRequestEntity signUpEntity;

        /// <summary>
        /// This method is used to initialize page load value and Set Web View Client data
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.WebViewLayout);
                // Handle exception handling 
                if (Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    using (Utility utility = new Utility(this))
                    {
                        utility.SaveExceptionHandling(null, null, null, Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.ExceptionData), null));
                    }
                }
                FindControlsById();
                webview.Settings.JavaScriptEnabled = true;
                webview.SetWebViewClient(new HelloWebViewClient(this));
                webview.LoadUrl(Resources.GetString(Resource.String.termsLink));
                signUpEntity = JsonConvert.DeserializeObject<SignUpRequestEntity>(Intent.GetStringExtra("User"));
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreate), Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
        }

        /// <summary>
        /// Getting the reference of controls
        /// </summary>
        public void FindControlsById()
        {
            try
            {
                imgBack = FindViewById<ImageView>(Resource.Id.imgBack);
                btnAccept = FindViewById<Button>(Resource.Id.btnAccept);
                btnReject = FindViewById<Button>(Resource.Id.btnReject);
                chkAgree = FindViewById<CheckBox>(Resource.Id.chkAgree);
                webview = FindViewById<WebView>(Resource.Id.webview);
                btnAccept.SetOnClickListener(this);
                btnReject.SetOnClickListener(this);
                imgBack.SetOnClickListener(this);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.FindControlsById), Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
        }

        /// <summary>
        /// On click listener for button and back icon
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.btnAccept:
                        IsAgreeChecked();
                        break;
                    case Resource.Id.btnReject:
                        SetResult(Result.Ok);
                        Finish();
                        break;
                    case Resource.Id.imgBack:
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.WebViewActivity), null);
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
                            if (signUpEntity.CourseAffiliationProofFilePath != null)
                                signUpEntity.CourseAffiliationProofFilePath = await UploadImageToBlobAsync(accessTokenResponseEntity.token, signUpEntity.CourseAffiliationProofFilePath);
                            HitSignUpAPI(accessTokenResponseEntity.token);
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTokenForSignup), Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
        }

        /// <summary>
        /// Checks wheather agree button is checked or not
        /// </summary>
        public void IsAgreeChecked()
        {
            if (chkAgree.Checked)
            {
                GetTokenForSignup();
            }
            else
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.acceptCondition), ToastLength.Long).Show();
            }
        }

        /// <summary>
        /// Calling signup API
        /// </summary>
        /// <param name="signuptoken"></param>
        private async void HitSignUpAPI(string signuptoken)
        {
            Show_Overlay();
            try
            {
                if(CurrentFocus != null)
                {// to hide onscreen keyboard
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                }
                
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
                        SignUpResponseEntity signUpResponseEntity = await util.UserSignUp(signUpEntity, signuptoken);
                        if (signUpResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                        {
                            Toast.MakeText(this, signUpResponseEntity.operationMessage, ToastLength.Long).Show();
                            SignUpActivity.courseId = string.Empty;
                            LoginRequestEntity loginEntity = new LoginRequestEntity();
                            loginEntity.username = signUpEntity.email;
                            loginEntity.password = signUpEntity.password;
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.Smusername), signUpEntity.email).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.password), signUpEntity.password).Commit();
                            LoginResponseEntity loginResponseEntity = await util.UserLogin(loginEntity);
                            if (loginResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                            {
                                if (loginResponseEntity.role[0].ToLower().Equals(Resources.GetString(Resource.String.Admin)))
                                {
                                    Toast.MakeText(this, Resources.GetString(Resource.String.unauthorized), ToastLength.Long).Show();
                                }
                                else
                                {
                                    Intent intent = new Intent(this, typeof(MainActivity));
                                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
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
                                // if login not successful than , user needs approval , show message to user
                                Intent intent = new Intent(this, typeof(RegistrationSuccessActivity));
                                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                                StartActivity(intent);
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, signUpResponseEntity.operationMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.HitSignUpAPI), Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
            finally
            {
                if (overlay != null)
                {
                    overlay.Dismiss();
                }
            }
        }

        /// <summary>
        /// On back button click
        /// </summary>
        public override void OnBackPressed()
        {
            SetResult(Android.App.Result.Ok);
            Finish();
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
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
        }
        public async Task<string> UploadImageToBlobAsync(string signuptoken, string imgLocalUri)
        {
            Show_Overlay();
            try
            {
                Stream imgLocalStream = ContentResolver.OpenInputStream(Android.Net.Uri.Parse(imgLocalUri));
                var type = ContentResolver.GetType(Android.Net.Uri.Parse(imgLocalUri));
                var cursor = ContentResolver.Query(Android.Net.Uri.Parse(imgLocalUri), null, null, null);
                var fileNameIndex = cursor.GetColumnIndex(OpenableColumns.DisplayName);
                cursor.MoveToFirst();
                string filename = cursor.GetString(fileNameIndex);

                if (imgLocalStream == null)
                    return null;

                using (Utility utility = new Utility(this))
                {
                    string imageUrl = await utility.UploadImageToBlob(imgLocalStream, filename, signuptoken, type);
                    UploadBlobImageResponseEntity urlResponse = null;
                    if (imageUrl != null)
                    {
                        urlResponse = JsonConvert.DeserializeObject<UploadBlobImageResponseEntity>(imageUrl);
                        return urlResponse.filePath;
                    }                        
                    else
                    {
                        Toast.MakeText(this, Resource.String.ServiceError, ToastLength.Long).Show();
                        return null;
                    }
                }

            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(this))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.GetTokenForSignup), Resources.GetString(Resource.String.SignUpActivity), null);
                }
                return null;
            }
            finally
            {
                if (overlay != null)
                {
                    overlay.Dismiss();
                }
            }
        }
    }

    /// <summary>
    /// This internal class is used for behind functionality when user changes from termsLink to Privacy or other
    /// </summary>
    public class HelloWebViewClient : WebViewClient
    {
        OverlayActivity overlay;
        WebViewActivity activity;

        /// <summary>
        /// This method is to prevent user to move in side the app with out click on i agree or not
        /// </summary>
        /// <param name="activity"></param>
        public HelloWebViewClient(WebViewActivity activity)
        {
            this.activity = activity;
            try
            {
                overlay = new OverlayActivity(this.activity);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (System.Exception ex)
            {
                using (Utility utility = new Utility(activity))
                {
                    utility.SaveExceptionHandling(ex, activity.Resources.GetString(Resource.String.HelloWebViewClient), activity.Resources.GetString(Resource.String.WebViewActivity), null);
                }
            }
        }

        /// <summary>
        /// This Method is for loading when data change
        /// </summary>
        /// <param name="view"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl(request.Url.ToString());
            return false;
        }

        /// <summary>
        /// Invokes On load finished of webview
        /// </summary>
        /// <param name="view"></param>
        /// <param name="url"></param>
        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            overlay.Dismiss();
        }
    }
}