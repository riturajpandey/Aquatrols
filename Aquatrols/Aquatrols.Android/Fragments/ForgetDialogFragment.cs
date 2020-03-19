using Android.App;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Aquatrols.Droid.Activities;
using Aquatrols.Droid.Helper;
using Aquatrols.Models;
using System;
using System.Linq;

namespace Aquatrols.Droid.Fragments
{
    /// <summary>
    /// This calss used in case of forget button
    /// </summary>
    public class ForgetDialogFragment : DialogFragment, View.IOnClickListener
    {
        private Activity context;
        private LinearLayout llForPasswordReset, llForVerification, llForEmail;
        private Button btnResetForPasswordReset, btnCancelForPasswordReset, btnVerifyForVerification, btnCancelForVerification, btnNextForEmail, btnCancelForEmail;
        private EditText editConfirmPassword, editPassword, editVerificationCode, editEmail;
        private ImageButton imgBtnSeeConfirmPassword, imgBtnSeePassword;
        private TextView txtEnterCode, txtWesentto, txtMaskedMobile, txtCodemaytake, txtDonotshare, txtDidntreceive, txtResetPassword, txtSendVerificationCode, txtEntertheemail;
        private bool canSeeConfrmPassword, canSeePassword;

        /// <summary>
        /// this dialog is show when user click on forget button
        /// </summary>
        /// <param name="a"></param>
        public ForgetDialogFragment(Activity a)
        {
            context = a;
        }

        /// <summary>
        /// This method is used to initialize page load value and take action in case of forget button
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ForgotPasswordLayout, container, false);
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                llForPasswordReset = view.FindViewById<LinearLayout>(Resource.Id.llForPasswordReset);
                llForVerification = view.FindViewById<LinearLayout>(Resource.Id.llForVerification);
                llForEmail = view.FindViewById<LinearLayout>(Resource.Id.llForEmail);

                txtResetPassword = view.FindViewById<TextView>(Resource.Id.txtResetPassword);
                txtSendVerificationCode = view.FindViewById<TextView>(Resource.Id.txtSendVerificationCode);
                txtEntertheemail = view.FindViewById<TextView>(Resource.Id.txtEntertheemail);
                txtEnterCode = view.FindViewById<TextView>(Resource.Id.txtEnterCode);
                txtWesentto = view.FindViewById<TextView>(Resource.Id.txtWesentto);
                txtMaskedMobile = view.FindViewById<TextView>(Resource.Id.txtMaskedMobile);
                txtCodemaytake = view.FindViewById<TextView>(Resource.Id.txtCodemaytake);
                txtDonotshare = view.FindViewById<TextView>(Resource.Id.txtDonotshare);
                txtDidntreceive = view.FindViewById<TextView>(Resource.Id.txtDidntreceive);
                //Edit text
                editPassword = view.FindViewById<EditText>(Resource.Id.editPassword);
                editConfirmPassword = view.FindViewById<EditText>(Resource.Id.editConfirmPassword);
                editEmail = view.FindViewById<EditText>(Resource.Id.editEmail);
                editVerificationCode = view.FindViewById<EditText>(Resource.Id.editVerificationCode);
                ///Eye icon
                imgBtnSeeConfirmPassword = view.FindViewById<ImageButton>(Resource.Id.imgBtnSeeConfirmPassword);
                imgBtnSeePassword = view.FindViewById<ImageButton>(Resource.Id.imgBtnSeePassword);
                //Button
                btnResetForPasswordReset = view.FindViewById<Button>(Resource.Id.btnResetForPasswordReset);
                btnCancelForPasswordReset = view.FindViewById<Button>(Resource.Id.btnCancelForPasswordReset);
                btnVerifyForVerification = view.FindViewById<Button>(Resource.Id.btnVerifyForVerification);
                btnCancelForVerification = view.FindViewById<Button>(Resource.Id.btnCancelForVerification);
                btnNextForEmail = view.FindViewById<Button>(Resource.Id.btnNextForEmail);
                btnCancelForEmail = view.FindViewById<Button>(Resource.Id.btnCancelForEmail);

                #region Setting Events&Listeners
                btnResetForPasswordReset.SetOnClickListener(this);
                btnCancelForPasswordReset.SetOnClickListener(this);
                btnVerifyForVerification.SetOnClickListener(this);
                btnCancelForVerification.SetOnClickListener(this);
                btnNextForEmail.SetOnClickListener(this);
                btnCancelForEmail.SetOnClickListener(this);
                imgBtnSeeConfirmPassword.SetOnClickListener(this);
                imgBtnSeePassword.SetOnClickListener(this);

                editPassword.AddTextChangedListener(new MyTextWatcher(editPassword, imgBtnSeePassword));
                editConfirmPassword.AddTextChangedListener(new MyTextWatcher(editConfirmPassword, imgBtnSeeConfirmPassword));
                #endregion

                #region Setting Fonts Styles
                Button[] btns = { btnCancelForEmail, btnCancelForPasswordReset, btnCancelForVerification, btnNextForEmail, btnResetForPasswordReset, btnVerifyForVerification };
                EditText[] editTxts = { editConfirmPassword, editPassword, editVerificationCode, editEmail };
                TextView[] txtViews = { txtResetPassword, txtSendVerificationCode, txtEntertheemail, txtCodemaytake, txtDidntreceive, txtDonotshare, txtEnterCode, txtMaskedMobile, txtWesentto };
                #endregion

                string strText = Resources.GetString(Resource.String.didnotRecievedcode);
                var ss = new SpannableString(Resources.GetString(Resource.String.didnotRecievedcode) + Resources.GetString(Resource.String.resend));
                var clickableSpan = new MyClickableSpan();
                clickableSpan.Click += delegate
                {
                    SendOTPCode();
                };
                ss.SetSpan(clickableSpan, strText.Length, strText.Length + 6, SpanTypes.ExclusiveExclusive);
                txtDidntreceive.TextFormatted = ss;
                txtDidntreceive.MovementMethod = new LinkMovementMethod();
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnCreateView), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
            return view;
        }
        
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }

        /// <summary>
        /// On click event
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.imgBtnSeeConfirmPassword:  //show/hide confirm password
                        {
                            if (canSeeConfrmPassword)
                            {
                                canSeeConfrmPassword = false;
                                editConfirmPassword.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
                                imgBtnSeeConfirmPassword.SetImageResource(Resource.Drawable.iconVisibleBlue);
                            }
                            else
                            {
                                canSeeConfrmPassword = true;
                                editConfirmPassword.InputType = Android.Text.InputTypes.TextVariationPassword;
                                imgBtnSeeConfirmPassword.SetImageResource(Resource.Drawable.iconInvisibleBlue);
                            }
                            break;
                        }
                    case Resource.Id.imgBtnSeePassword:       //show/hide password
                        {
                            if (canSeePassword)
                            {
                                canSeePassword = false;
                                editPassword.InputType = InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
                                imgBtnSeePassword.SetImageResource(Resource.Drawable.iconVisibleBlue);
                            }
                            else
                            {
                                canSeePassword = true;
                                editPassword.InputType = InputTypes.TextVariationPassword;
                                imgBtnSeePassword.SetImageResource(Resource.Drawable.iconInvisibleBlue);
                            }
                            break;
                        }
                    case Resource.Id.btnResetForPasswordReset:
                        {
                            if (ValidateInputs())
                            {
                                ResetUserPassword();
                            }
                            break;
                        }
                    case Resource.Id.btnCancelForPasswordReset:
                        {
                            llForPasswordReset.Visibility = ViewStates.Gone;
                            llForVerification.Visibility = ViewStates.Visible;
                            llForEmail.Visibility = ViewStates.Gone;
                            editVerificationCode.Text = "";
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), null).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), null).Commit();
                            break;
                        }
                    case Resource.Id.btnVerifyForVerification:
                        {
                            if (!string.IsNullOrEmpty(editVerificationCode.Text))
                            {
                                CheckOTP();
                            }
                            else
                            {
                                Toast.MakeText(this.context, Resources.GetString(Resource.String.entercode), ToastLength.Long).Show();
                            }
                            break;
                        }
                    case Resource.Id.btnCancelForVerification:
                        {
                            llForEmail.Visibility = ViewStates.Visible;
                            llForVerification.Visibility = ViewStates.Gone;
                            llForPasswordReset.Visibility = ViewStates.Gone;
                            editVerificationCode.Text = "";
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.UserId), null).Commit();
                            break;
                        }
                    case Resource.Id.btnNextForEmail:  //Send OTP for the first time
                        {
                            SendOTPCode();
                            break;
                        }
                    case Resource.Id.btnCancelForEmail:
                        {
                            Dismiss();   //dismiss fragment 
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.OnClick), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
        }

        /// <summary>
        /// Method to call Reset password API
        /// </summary>
        private async void ResetUserPassword()
        {
            try
            {
                using (Utility util = new Utility(this.context))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this.context, Resources.GetString(Resource.String.NetworkError), ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        Show_Overlay();

                        ResetPasswordRequestEntity resetPasswordRequestEntity = new ResetPasswordRequestEntity();
                        resetPasswordRequestEntity.password = editPassword.Text;
                        resetPasswordRequestEntity.confirmPassword = editConfirmPassword.Text;
                        resetPasswordRequestEntity.userId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.SmUserId), null);
                        resetPasswordRequestEntity.token = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.token), null);

                        ResetPasswordResponseEntity resetPasswordResponseEntity = await util.ResetUserPassword(resetPasswordRequestEntity);
                        if (resetPasswordResponseEntity.operationStatus.ToLower() == Resources.GetString(Resource.String.success))
                        {
                            Toast.MakeText(this.context, resetPasswordResponseEntity.operationMessage, ToastLength.Long).Show();
                            Dismiss();   //dismiss the forget dialog
                        }
                        else
                        {
                            Toast.MakeText(this.context, resetPasswordResponseEntity.operationMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ResetUserPassword), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
            finally
            {
                if (overlay != null)
                    overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to check OTP
        /// </summary>
        private async void CheckOTP()
        {
            try
            {
                using (Utility util = new Utility(this.context))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this.context, Resources.GetString(Resource.String.NetworkError), ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        Show_Overlay();
                        ValidateOTPRequestEntity validateOTPRequestEntity = new ValidateOTPRequestEntity();
                        validateOTPRequestEntity.UserId = Utility.sharedPreferences.GetString(Resources.GetString(Resource.String.SmUserId), null);
                        validateOTPRequestEntity.OTP = Convert.ToInt32(editVerificationCode.Text);
                        ValidateOTPResponseEntity validateOTPResponseEntity = await util.VerifyOTP(validateOTPRequestEntity);
                        if (validateOTPResponseEntity.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                        {
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.SmUserId), validateOTPResponseEntity.userId).Commit();
                            Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.token), validateOTPResponseEntity.token).Commit();
                            llForPasswordReset.Visibility = ViewStates.Visible;
                            llForVerification.Visibility = ViewStates.Gone;
                            llForEmail.Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            Toast.MakeText(this.context, validateOTPResponseEntity.operationMessage, ToastLength.Long).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckOTP), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
            finally
            {
                if (overlay != null)
                    overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to send OTP on registered Mobile number
        /// </summary>
        private async void SendOTPCode()
        {
            try
            {
                using (Utility util = new Utility(this.context))
                {
                    bool internetStatus = util.CheckInternetConnection();
                    if (!internetStatus)
                    {
                        Toast.MakeText(this.context, Resources.GetString(Resource.String.NetworkError), ToastLength.Long).Show();
                        return;
                    }
                    else
                    {
                        if (CheckValidation(util))
                        {
                            Show_Overlay();

                            ValidateUserRequestEntity validateUserRequestEntity = new ValidateUserRequestEntity();
                            validateUserRequestEntity.username = editEmail.Text;
                            ValidateUserResponseEntity validateOTPResponse = await util.SendOTP(validateUserRequestEntity);
                            if (validateOTPResponse.operationStatus.ToLower().Equals(Resources.GetString(Resource.String.success)))
                            {
                                Utility.sharedPreferences.Edit().PutString(Resources.GetString(Resource.String.SmUserId), validateOTPResponse.userId).Commit();

                                txtMaskedMobile.Text = editEmail.Text;

                                llForEmail.Visibility = ViewStates.Gone;
                                llForVerification.Visibility = ViewStates.Visible;
                                llForPasswordReset.Visibility = ViewStates.Gone;
                            }
                            else
                            {
                                Toast.MakeText(context, validateOTPResponse.operationMessage, ToastLength.Long).Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.SendOTPCode), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
            finally
            {
                if (overlay != null)
                    overlay.Dismiss();
            }
        }

        /// <summary>
        /// Method to Check Validation
        /// </summary>
        /// <param name="util"></param>
        /// <returns></returns>
        private bool CheckValidation(Utility util)
        {
            try
            {
                if (string.IsNullOrEmpty(editEmail.Text))
                {
                    Toast.MakeText(context, Resources.GetString(Resource.String.RequireEmail), ToastLength.Long).Show();
                    return false;
                }
                else if (!util.IsValidEmail(editEmail.Text))
                {
                    Toast.MakeText(context, Resources.GetString(Resource.String.InvalidEmail), ToastLength.Long).Show();
                    return false;
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.CheckValidation), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
            return true;
        }

        /// <summary>
        /// Showing wait cursor
        /// </summary>
        OverlayActivity overlay;
        public void Show_Overlay()
        {
            try
            {
                overlay = new OverlayActivity(this.context);
                overlay.Show();
                overlay.SetCanceledOnTouchOutside(false);
                overlay.SetCancelable(false);
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.Show_Overlay), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
        }

        /// <summary>
        /// Checks the enetered text is valid or not
        /// </summary>
        /// <returns></returns>
        private bool ValidateInputs()
        {
            try
            {
                using (Utility utility = new Utility(this.context))
                {
                    if (string.IsNullOrEmpty(editPassword.Text) || !utility.IsValidPassword(editPassword.Text))
                    {
                        Toast.MakeText(this.context, Resources.GetString(Resource.String.EnterValidPassowrd), ToastLength.Long).Show();
                        return false;
                    }
                    if (string.IsNullOrEmpty(editConfirmPassword.Text) || editPassword.Text != editConfirmPassword.Text)
                    {
                        Toast.MakeText(this.context, Resources.GetString(Resource.String.PasswordNotMatch), ToastLength.Long).Show();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                using (Utility utility = new Utility(context))
                {
                    utility.SaveExceptionHandling(ex, Resources.GetString(Resource.String.ValidateInputs), Resources.GetString(Resource.String.ForgetDialogFragment), null);
                }
            }
            return true;
        }
    }

    /// <summary>
    /// This method is listen when edit text view
    /// </summary>
    #region Implementation of Android.Text.ITextWatcher
    public class MyTextWatcher : Java.Lang.Object, ITextWatcher
    {
        public EditText view;
        public ImageButton imgBtn;

        /// <summary>
        /// this method is listen when edit text view
        /// </summary>
        /// <param name="view"></param>
        /// <param name="imgBtn"></param>
        public MyTextWatcher(EditText view, ImageButton imgBtn)
        {
            this.view = view;
            this.imgBtn = imgBtn;
        }

        /// <summary>
        /// this method is used after edit text view
        /// </summary>
        /// <param name="s"></param>
        public void AfterTextChanged(IEditable s)
        {
        }

        /// <summary>
        /// this method is used before edit text view
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="after"></param>
        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
        }

        /// <summary>
        /// this method is used on edit text view
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="before"></param>
        /// <param name="count"></param>
        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            switch (view.Id)
            {
                case Resource.Id.editPassword:
                    {
                        if (s.ToArray().Count() > 0)
                            imgBtn.Visibility = ViewStates.Visible;
                        else
                            imgBtn.Visibility = ViewStates.Gone;
                    }
                    break;
                case Resource.Id.editConfirmPassword:
                    {
                        if (s.ToArray().Count() > 0)
                            imgBtn.Visibility = ViewStates.Visible;
                        else
                            imgBtn.Visibility = ViewStates.Gone;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    /// <summary>
    /// This class shows effets on text changed
    /// </summary>
    public class MyClickableSpan : ClickableSpan
    {
        public Action<View> Click;

        /// <summary>
        /// This method is used to show out put on text changed
        /// </summary>
        /// <param name="widget"></param>
        public override void OnClick(View widget)
        {
            if (Click != null)
                Click(widget);
        }
    }
}