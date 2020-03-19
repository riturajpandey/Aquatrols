using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.Net;
using Android.Widget;
using Aquatrols.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aquatrols.Droid.Helper
{
    /// <summary>
    /// This utility class is used for common method and API calling method.  
    /// </summary>
    class Utility : IDisposable
    {
        private Activity context;
        public static ISharedPreferences sharedPreferences = null;
        public static ISharedPreferences ExceptionData = null;
        List<ExceptionHandlingRequestEntity> tempExceptionHandlingRequestEntity = new List<ExceptionHandlingRequestEntity>();

        public Utility(Activity context)
        {
            this.context = context;
        }

        /// <summary>
        /// This method is to check wheather Internet is connected or not
        /// </summary>
        /// <returns></returns>
        #region Checking Internet Connection
        public bool CheckInternetConnection()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Android.Content.Context.ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;
            return isOnline;
        }
        #endregion

        #region Checking validation for Password/Mobile/Email
        /// <summary>
        /// Method to check valid Email address.
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool IsValidEmail(string strValue)
        {
            string regexPattern = @"^([\w-\.]+[a-zA-Z0-9]+@([\w-]+\.)+[\w-]{2,4})?$";
            Match matches = Regex.Match(strValue, regexPattern);
            return matches.Success;
        }

        /// <summary>
        /// Method to Create firstletter in uppercase
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        /// <summary>
        /// Method to check valid phone number
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool IsValidPhone(string strValue)
        {
            if (strValue.Contains("-"))
            {
                strValue = strValue.Replace("-", "");
            }
            if (strValue.Contains("("))
            {
                strValue = strValue.Replace("(", "");
            }
            if (strValue.Contains(")"))
            {
                strValue = strValue.Replace(")", "");
            }
            if (strValue.Length >= 5 && strValue.Length <= 10)
                return true;
            else
                return false;
        }

        /// <summary>
        /// checks that zip is valid
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool IsValidZip(string strValue, string country)
        {
            if (country.Equals("USA"))
            {
                string regexPattern = @"^[0-9]{5}(?:-[0-9]{4})?$";
                Match matches = Regex.Match(strValue, regexPattern);
                return matches.Success;
            }
            else
            {
                bool flag = false;
                string regexPattern = @"^(?!.*[DFIOQU])[A-VXY][0-9][A-Z] ?[0-9][A-Z][0-9]$";
                string regexPattern1 = @"^(?!.*[DFIOQU])[A-VXY][0-9][A-Z]$";
                Match matches = Regex.Match(strValue, regexPattern);
                Match matches1 = Regex.Match(strValue, regexPattern1);
                if (matches.Success == true)
                {
                    flag = true;
                }
                else if (matches1.Success == true)
                {
                    flag = true;
                }
                return flag;
            }
        }

        /// <summary>
        /// check if app is installed
        /// </summary>
        /// <param name="c"></param>
        /// <param name="targetPackage"></param>
        /// <returns></returns>
        public static bool isPackageInstalled(Context c, String targetPackage)
        {
            bool flag = false;
            try
            {
                ApplicationInfo info = c.PackageManager.GetApplicationInfo(targetPackage, 0);
                flag = true;
            }
            catch
            {
                flag = false;
                // Facebook not installed
            }
            return flag;
        }

        /// <summary>
        /// get facebook Url
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static String getFacebookPageURL(Context context)
        {
            PackageManager packageManager = context.PackageManager;
            try
            {
                int versionCode = packageManager.GetPackageInfo(context.Resources.GetString(Resource.String.facebookpackage), 0).VersionCode;
                if (versionCode >= 3002850)
                { //newer versions of fb app
                    return "fb://facewebmodal/f?href=" + context.Resources.GetString(Resource.String.facebooklink);
                }
                else
                { //older versions of fb app
                    return "fb://page/" + context.Resources.GetString(Resource.String.app_name);
                }
            }
            catch
            {
                return context.Resources.GetString(Resource.String.facebooklink); //normal web url
            }
        }

        /// <summary>
        /// Method to check password validation
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool IsValidPassword(string strValue)
        {
            //string regexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{4,}";
            string regexPattern = @"^(?=.*\d)(?=.*[A-Z]).{4,}$";
            Match matches = Regex.Match(strValue, regexPattern);
            return matches.Success;
        }
        #endregion

        #region Apply Font Here
        public static void SettingFonts(EditText[] editTxtList, TextView[] txtViewsList, Button[] btnList)
        {
            Typeface fontOSReg = Typeface.CreateFromAsset(Application.Context.Assets, "DroidSerif-Regular.ttf");
            Typeface fontOSLight = Typeface.CreateFromAsset(Application.Context.Assets, "DroidSerif-Italic.ttf");
            Typeface fontOSBold = Typeface.CreateFromAsset(Application.Context.Assets, "DroidSerif-Bold.ttf");
            Typeface fontOSBoldItalic = Typeface.CreateFromAsset(Application.Context.Assets, "DroidSerif-BoldItalic.ttf");
            if (txtViewsList != null)
            {
                foreach (TextView txtview in txtViewsList)
                {
                    if (txtview.Tag != null && txtview.Tag.ToString() == "Heading")
                        txtview.SetTypeface(fontOSBold, Android.Graphics.TypefaceStyle.Normal);
                    else if (txtview.Tag != null && txtview.Tag.ToString().ToLower().Equals("bitalic"))
                    {
                        txtview.SetTypeface(fontOSBoldItalic, Android.Graphics.TypefaceStyle.Normal);
                    }
                    else if (txtview.Tag != null && txtview.Tag.ToString().ToLower().Equals("italic"))
                    {
                        txtview.SetTypeface(fontOSLight, Android.Graphics.TypefaceStyle.Normal);
                    }
                    else
                        txtview.SetTypeface(fontOSReg, Android.Graphics.TypefaceStyle.Normal);
                }
            }
            if (btnList != null)
            {
                foreach (Button btn in btnList)
                    btn.SetTypeface(fontOSBold, Android.Graphics.TypefaceStyle.Normal);
            }
            if (editTxtList != null)
            {
                foreach (EditText editTxt in editTxtList)
                    editTxt.SetTypeface(fontOSReg, Android.Graphics.TypefaceStyle.Normal);
            }
        }
        #endregion

        /// <summary>
        /// Method to clear Items from Queue
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MyQueueResponseEntity> ClearQueue(string userId, string token)
        {
            MyQueueResponseEntity myQueueResponse = null;
            try
            {
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(string.Empty, context.Resources.GetString(Resource.String.WishListClearQueueURL) + userId, true, token);
                myQueueResponse = JsonConvert.DeserializeObject<MyQueueResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.ClearQueue), context.Resources.GetString(Resource.String.Utility), null);
            }
            return myQueueResponse;
        }

        /// <summary>
        /// Method to Get All WishList Items
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<RedeemGiftCardEntity>> GetRewardItem(string token)
        {
            List<RedeemGiftCardEntity> redeemGiftCardEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.RewardItemListURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    redeemGiftCardEntities = JsonConvert.DeserializeObject<List<RedeemGiftCardEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetRewardItem), context.Resources.GetString(Resource.String.Utility), null);
            }
            return redeemGiftCardEntities;
        }

        /// <summary>
        /// Method to Get All WishList Items
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<WishListEntity>> GetWishListItems(string userId, string token)
        {
            List<WishListEntity> wishListEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.WishListURL) + userId, true, token);
                if (serviceResponse.status.Equals("OK"))
                    wishListEntities = JsonConvert.DeserializeObject<List<WishListEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetWishListItems), context.Resources.GetString(Resource.String.Utility), null);
            }
            return wishListEntities;
        }

        /// <summary>
        /// Method to remove Item from Queue
        /// </summary>
        public async Task<MyQueueResponseEntity> DeleteItemFromQueue(string token, int wishListID)
        {
            MyQueueResponseEntity myQueueResponse = null;
            try
            {
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(string.Empty, context.Resources.GetString(Resource.String.WishListDeleteURL) + wishListID, true, token);
                myQueueResponse = JsonConvert.DeserializeObject<MyQueueResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.DeleteItemFromQueue), context.Resources.GetString(Resource.String.Utility), null);
            }
            return myQueueResponse;
        }

        /// <summary>
        /// Method to Add items to Queue
        /// </summary>
        /// <param name="myQueueEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MyQueueResponseEntity> AddToQueue(MyQueueEntity myQueueEntity, string token)
        {
            MyQueueResponseEntity myQueueResponse = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(myQueueEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.WishListSaveURL), true, token);
                myQueueResponse = JsonConvert.DeserializeObject<MyQueueResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.AddToQueue), context.Resources.GetString(Resource.String.Utility), null);
            }
            return myQueueResponse;
        }

        /// <summary>
        /// Method to get Distinct Distributor
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<DistinctDistributorEntity>> GetDistinctDistributor(string token)
        {
            List<DistinctDistributorEntity> distinctDistributors = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.DistinctDisListURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    distinctDistributors = JsonConvert.DeserializeObject<List<DistinctDistributorEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetDistinctDistributor), context.Resources.GetString(Resource.String.Utility), null);
            }
            return distinctDistributors;
        }

        /// <summary>
        /// Method to get count of cartItems
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetWishListItemCount(string token)
        {
            string count = string.Empty;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.ItemCountURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    count = serviceResponse.response;
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetWishListItemCount), context.Resources.GetString(Resource.String.Utility), null);
            }
            return count;
        }

        /// <summary>
        /// Method to Redeem Points
        /// </summary>
        /// <param name="redeemPointEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RedeemPointResponseEntity> RedeemPoints(RedeemPointEntity redeemPointEntity, string token)
        {
            RedeemPointResponseEntity redeemPointResponse = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(redeemPointEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.LoyaltyPointsRedeemURL), true, token);
                redeemPointResponse = JsonConvert.DeserializeObject<RedeemPointResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.RedeemPoint), context.Resources.GetString(Resource.String.Utility), null);
            }
            return redeemPointResponse;
        }

        /// <summary>
        /// Method to send device token to FCM
        /// </summary>
        /// <param name="pushDataModelEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PushDataModelResponseEntity> SendDeviceToken(PushDataModelEntity pushDataModelEntity, string token)
        {
            PushDataModelResponseEntity pushDataModelResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(pushDataModelEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.PushNotificationSaveURL), true, token);
                pushDataModelResponseEntity = JsonConvert.DeserializeObject<PushDataModelResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.SendDeviceToken), context.Resources.GetString(Resource.String.Utility), null);
            }
            return pushDataModelResponseEntity;
        }

        /// <summary>
        /// Method for product checkout 
        /// </summary>
        /// <param name="productCheckoutEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ProductCheckoutResponseEntity> Checkout(List<ProductCheckoutEntity> productCheckoutEntity, string token)
        {
            ProductCheckoutResponseEntity productCheckoutResponse = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(productCheckoutEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.ProductBookURL), true, token);
                productCheckoutResponse = JsonConvert.DeserializeObject<ProductCheckoutResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.Checkout), context.Resources.GetString(Resource.String.Utility), null);
            }
            return productCheckoutResponse;
        }

        /// <summary>
        /// Method to get all products
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<ProductListEntity>> GetAllProducts(string token)
        {
            List<ProductListEntity> productListEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.ProductListURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    productListEntities = JsonConvert.DeserializeObject<List<ProductListEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetAllProducts), context.Resources.GetString(Resource.String.Utility), null);
            }
            return productListEntities;
        }

        /// <summary>
        /// Method to get purchase history
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<PurchaseHistoryEntity>> GetPurchaseHistory(string token, string userId)
        {
            List<PurchaseHistoryEntity> lstPurchaseHistoryEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.ProductBookingDetailURL) + userId, true, token);
                if (serviceResponse.status.Equals("OK"))
                    lstPurchaseHistoryEntities = JsonConvert.DeserializeObject<List<PurchaseHistoryEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetPurchaseHistory), context.Resources.GetString(Resource.String.Utility), null);
            }
            return lstPurchaseHistoryEntities;
        }

        /// <summary>
        /// Method to get All distributors
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<DistributorInfoEntity>> GetAllDistributors(string token)
        {
            List<DistributorInfoEntity> distributorInfoEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.DistributorLstURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    distributorInfoEntities = JsonConvert.DeserializeObject<List<DistributorInfoEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetAllDistributors), context.Resources.GetString(Resource.String.Utility), null);
            }
            return distributorInfoEntities;
        }

        /// <summary>
        /// Method to update notification settings
        /// </summary>
        /// <param name="notificationEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<NotificationResponseEntity> UpdateNotificationSetting(NotificationEntity notificationEntity, string token)
        {
            NotificationResponseEntity notificationResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(notificationEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.NotificationsURL), true, token);
                notificationResponseEntity = JsonConvert.DeserializeObject<NotificationResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.UpdateNotificationSetting), context.Resources.GetString(Resource.String.Utility), null);
            }
            return notificationResponseEntity;
        }

        /// <summary>
        /// Method to change Password
        /// </summary>
        /// <param name="changePasswordEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ChangePasswordResponseEntity> ChangePassword(ChangePasswordEntity changePasswordEntity, string token)
        {
            ChangePasswordResponseEntity changePasswordResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(changePasswordEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.ChangePasswordURL), true, token);
                changePasswordResponseEntity = JsonConvert.DeserializeObject<ChangePasswordResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.ChangePassword), context.Resources.GetString(Resource.String.Utility), null);
            }
            return changePasswordResponseEntity;
        }

        /// <summary>
        /// Method to change Password
        /// </summary>
        /// <param name="changePasswordEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetTerritorySnapshotExcelUrl(string token, SnapShotRequestEntity snapShotRequestEntity)
        {
            string result = null;
            try
            {
                var serializedData = JsonConvert.SerializeObject(snapShotRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(serializedData, context.Resources.GetString(Resource.String.GetSnapshotExcelURL), true, token);
                result = response.ToString();
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTerritorySnapshotExcelUrl), context.Resources.GetString(Resource.String.Utility), null);
            }
            return result;
        }

        /// <summary>
        /// Method to change Password
        /// </summary>
        /// <param name="changePasswordEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetTerritoryDistributorExcelUrl(string token, SnapShotRequestEntity snapShotRequestEntity)
        {
            string result = null;
            try
            {
                var serializedData = JsonConvert.SerializeObject(snapShotRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(serializedData, context.Resources.GetString(Resource.String.ExportDistURL), true, token);
                result = response.ToString();
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTerritoryDistributorExcelUrl), context.Resources.GetString(Resource.String.Utility), null);
            }
            return result;
        }

        /// <summary>
        /// Add new course
        /// </summary>
        /// <param name="courseEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<CourseResponseEntity> AddNewCourse(CourseEntity courseEntity, string token)
        {
            CourseResponseEntity courseResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(courseEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.CoursesSaveURL), true, token);
                courseResponseEntity = JsonConvert.DeserializeObject<CourseResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.AddNewCourse), context.Resources.GetString(Resource.String.Utility), null);
            }
            return courseResponseEntity;
        }

        /// <summary>
        /// Method to get Token for signup process
        /// </summary>
        /// <param name="loginEntity"></param>
        /// <returns></returns>
        public async Task<AccessTokenResponseEntity> GetTokenForSignup()
        {
            AccessTokenResponseEntity AccesstokenResponseEntity = null;
            try
            {
                LoginRequestEntity loginRequestEntity = new LoginRequestEntity();
                loginRequestEntity.username = this.context.Resources.GetString(Resource.String.signUpUser);
                loginRequestEntity.password = this.context.Resources.GetString(Resource.String.signUpPass);
                var strSerializedData = JsonConvert.SerializeObject(loginRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.GetAccessTokenURL), false, "");
                AccesstokenResponseEntity = JsonConvert.DeserializeObject<AccessTokenResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTokenForSignup), context.Resources.GetString(Resource.String.Utility), null);
            }
            return AccesstokenResponseEntity;
        }

        /// <summary>
        /// Method to login a registered user.
        /// </summary>
        /// <param name="strSerializedData"></param>
        /// <returns></returns>
        public async Task<LoginResponseEntity> UserLogin(LoginRequestEntity loginEntity)
        {
            LoginResponseEntity loginResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(loginEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.AccountSigninURL), false, "");
                loginResponseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.UserLogin), context.Resources.GetString(Resource.String.Utility), null);
            }
            return loginResponseEntity;
        }

        /// <summary>
        /// Method to update user profile information
        /// </summary>
        /// <param name="strSerializedData"></param>
        /// <param name="saveEarlierData"></param>
        /// <returns></returns>
        public async Task<SignUpResponseEntity> UserSignUp(SignUpRequestEntity signupEntity, string signuptoken)
        {
            SignUpResponseEntity signUpResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(signupEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.UserSignupURL), true, signuptoken);
                signUpResponseEntity = JsonConvert.DeserializeObject<SignUpResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.UserSignUp), context.Resources.GetString(Resource.String.Utility), null);
            }
            return signUpResponseEntity;
        }

        /// <summary>
        /// Sends the OTP.
        /// </summary>
        /// <returns>The OT.</returns>
        /// <param name="Email">Email.</param>
        public async Task<ValidateUserResponseEntity> SendOTP(ValidateUserRequestEntity validateUserRequestEntity)
        {
            ValidateUserResponseEntity validateOTPResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(validateUserRequestEntity);
                ServiceHelper objservice = new ServiceHelper();
                string response = await objservice.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.SendOTPURL), false, null);
                validateOTPResponseEntity = JsonConvert.DeserializeObject<ValidateUserResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.SendOTP), context.Resources.GetString(Resource.String.Utility), null);
            }
            return validateOTPResponseEntity;
        }

        /// <summary>
        /// Method to Get the user Information
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfoEntity> GetUserInfo(string userId, string token)
        {
            UserInfoEntity userInfo = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.UserDetailURL) + userId, true, token);
                if (serviceResponse.status.Equals("OK"))
                    userInfo = JsonConvert.DeserializeObject<UserInfoEntity>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetUserInfo), context.Resources.GetString(Resource.String.Utility), null);
            }
            return userInfo;
        }

        /// <summary>
        /// Method to Get Distributor List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<DistributorInfoEntity>> GetDistributorList(string userId, string token)
        {
            List<DistributorInfoEntity> distributorInfoEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest(null, context.Resources.GetString(Resource.String.DistributorListUserURL) + userId, true, token);
                if (serviceResponse.status.Equals("OK"))
                    distributorInfoEntities = JsonConvert.DeserializeObject<List<DistributorInfoEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetDistributorList), context.Resources.GetString(Resource.String.Utility), null);
            }
            return distributorInfoEntities;
        }

        /// <summary>
        /// Resets the user password.
        /// </summary>
        /// <returns>The user password.</returns>
        /// <param name="Email">Email.</param>
        /// <param name="Password">Password.</param>
        public async Task<ResetPasswordResponseEntity> ResetUserPassword(ResetPasswordRequestEntity resetPasswordRequestEntity)
        {
            ResetPasswordResponseEntity resetPasswordResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(resetPasswordRequestEntity);
                ServiceHelper objservice = new ServiceHelper();
                string response = await objservice.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.ResetPasswordURL), false, null);
                resetPasswordResponseEntity = JsonConvert.DeserializeObject<ResetPasswordResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.ResetUserPassword), context.Resources.GetString(Resource.String.Utility), null);
            }
            return resetPasswordResponseEntity;
        }

        /// <summary>
        /// Verifies the OTP.
        /// </summary>
        /// <returns>True .</returns>
        /// <param name="Email">Email.</param>
        /// <param name="OTP">OTP.</param>
        public async Task<ValidateOTPResponseEntity> VerifyOTP(ValidateOTPRequestEntity validateOTPRequestEntity)
        {
            ValidateOTPResponseEntity validateOTPResponseEntity = null;
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(validateOTPRequestEntity);
                ServiceHelper objservice = new ServiceHelper();
                string response = await objservice.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.ValidateOTPURL), false, null);
                validateOTPResponseEntity = JsonConvert.DeserializeObject<ValidateOTPResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.VerifyOTP), context.Resources.GetString(Resource.String.Utility), null);
            }
            return validateOTPResponseEntity;
        }

        /// <summary>
        /// Method to Get Course List
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<CourseEntity>> GetCourseList(string token)
        {
            List<CourseEntity> courseEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.ListURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    courseEntities = JsonConvert.DeserializeObject<List<CourseEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetCourseList), context.Resources.GetString(Resource.String.Utility), null);
            }
            return courseEntities;
        }

        /// <summary>
        /// Method to Get Territory Course List
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<TmCourseResponseEntity>> GetTmCourseList(string token)
        {
            List<TmCourseResponseEntity> tmCourseEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetCoursesURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    tmCourseEntities = JsonConvert.DeserializeObject<List<TmCourseResponseEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmCourseList), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmCourseEntities;
        }

        /// <summary>
        /// Method to Get Territory Course result 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TmCourseResultResponseEntity> GetTmCourseSearchResult(string token, string TmCourseId)
        {
            TmCourseResultResponseEntity tmCourseResultEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetCourseSearchResultURL) + TmCourseId, true, token);
                if (serviceResponse.status.Equals("OK"))
                    tmCourseResultEntities = JsonConvert.DeserializeObject<TmCourseResultResponseEntity>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmCourseSearchResult), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmCourseResultEntities;
        }

        /// <summary>
        /// Method to Get Territory Course result 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TmCourseResultResponseEntity> GetTmSuperIntendantSearchResult(string token, string tmUserId)
        {
            TmCourseResultResponseEntity tmCourseResultEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetUserSearchResultURL) + tmUserId, true, token);
                if (serviceResponse.status.Equals("OK"))
                    tmCourseResultEntities = JsonConvert.DeserializeObject<TmCourseResultResponseEntity>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmSuperIntendantSearchResult), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmCourseResultEntities;
        }

        /// <summary>
        /// Method to Get Territory SuperIntendant List
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<TmSuperIntendantResponseEntity>> GetTmSuperIntendantList(string token)
        {
            List<TmSuperIntendantResponseEntity> TmSuperIntendantEntities = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetAllTmRegionUsersURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    TmSuperIntendantEntities = JsonConvert.DeserializeObject<List<TmSuperIntendantResponseEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmSuperIntendantList), context.Resources.GetString(Resource.String.Utility), null);
            }
            return TmSuperIntendantEntities;
        }

        /// <summary>
        /// Method to Get Territory state List
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<TmStateList>> GetTmStateList(string token)
        {
            List<TmStateList> tmStateLists = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetAllStatesURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    tmStateLists = JsonConvert.DeserializeObject<List<TmStateList>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmStateList), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmStateLists;
        }

        /// <summary>
        /// Method to Get Territory Distributor List
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<TmDistributorList>> GetTmDistributorList(string token, string tmState)
        {
            List<TmDistributorList> tmDistributorLists = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetTmDistributorsURL) + tmState, true, token);
                if (serviceResponse.status.Equals("OK"))
                    tmDistributorLists = JsonConvert.DeserializeObject<List<TmDistributorList>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                //SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmDistributorList), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmDistributorLists;
        }

        /// <summary>
        /// Method to Get Data on the behalf of Distributor
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TmDistributorResultResponseEntity> GetTmDistributorSearchResultData(TmDistributorResultRequestEntity tmDistributorResultRequestEntity, string token)
        {
            TmDistributorResultResponseEntity tmDistributorResultResponseEntities = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(tmDistributorResultRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.GetDistributorSearchResultURL), true, token);
                tmDistributorResultResponseEntities = JsonConvert.DeserializeObject<TmDistributorResultResponseEntity>(response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmDistributorSearchResultData), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmDistributorResultResponseEntities;
        }

        /// <summary>
        /// Method to Get total number of signups and commitments
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<TmSnapshotSignUpCountVm>> GetTmTotalSignupAndCommitments(string token)
        {
            List<TmSnapshotSignUpCountVm> tmSnapshotSignUpCountVm = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetSignUpCommitmentCountURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                {
                    var response = JsonConvert.DeserializeObject<ListTmSnapshotSignUp>(serviceResponse.response);
                    tmSnapshotSignUpCountVm = response.tmSnapshotSignUpCountVm;
                }
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmTotalSignupAndCommitments), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmSnapshotSignUpCountVm;
        }

        /// <summary>
        /// Method to Get Territory total number of commitments per country
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<TmTotalCommitments>> GetTmTotalCommitmentsPerRegion(string token)
        {
            List<TmTotalCommitments> tmTotalCommitments = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetTotalQuantityCommitedURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    tmTotalCommitments = JsonConvert.DeserializeObject<List<TmTotalCommitments>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmTotalCommitmentsPerRegion), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmTotalCommitments;
        }

        /// <summary>
        /// Method to Get Territory total number of commitments per distributor
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<TmCommitmentsPerDistributor>> GetTmTotalCommitmentsPerDistributor(string token)
        {
            List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributors = null;
            try
            {
                ServiceHelper objservice = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objservice.GetRequest("", context.Resources.GetString(Resource.String.GetCommitmentPerDistributorURL), true, token);
                if (serviceResponse.status.Equals("OK"))
                    tmCommitmentsPerDistributors = JsonConvert.DeserializeObject<List<TmCommitmentsPerDistributor>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.GetTmTotalCommitmentsPerDistributor), context.Resources.GetString(Resource.String.Utility), null);
            }
            return tmCommitmentsPerDistributors;
        }
        public void Dispose()
        {

        }

        /// <summary>
        /// Method to save exception in data base
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="exceptionOrgin"></param>
        /// <param name="classFileName"></param>
        /// <param name="previouData"></param>
        public async void SaveExceptionHandling(Exception exc, string exceptionOrgin, string classFileName, string previouData)
        {
            try
            {
                bool internetStatus = CheckInternetConnection();
                string token = string.Empty;
                List<ExceptionHandlingRequestEntity> exceptionHandlingRequestEntity = new List<ExceptionHandlingRequestEntity>();
                if (sharedPreferences.GetString(context.Resources.GetString(Resource.String.token), null) != null)
                {
                    token = sharedPreferences.GetString(context.Resources.GetString(Resource.String.token), null);
                }
                if (sharedPreferences.GetString(context.Resources.GetString(Resource.String.ExceptionData), null) != null)
                {
                    string exception = sharedPreferences.GetString(context.Resources.GetString(Resource.String.ExceptionData), null);
                    exceptionHandlingRequestEntity = JsonConvert.DeserializeObject<List<ExceptionHandlingRequestEntity>>(exception);
                    foreach (var item in exceptionHandlingRequestEntity)
                    {
                        DateTime exceptionDate = item.exceptionOccuredOn;
                        DateTime currentDate = DateTime.UtcNow;
                        TimeSpan ts = currentDate - exceptionDate;
                        double NrOfDays = ts.TotalDays;

                        if (NrOfDays > 7)
                        {
                            tempExceptionHandlingRequestEntity = null;
                            sharedPreferences.Edit().PutString(context.Resources.GetString(Resource.String.ExceptionData), null).Commit();
                        }
                    }
                }

                if (internetStatus)
                {
                    ServiceHelper objServicePrevious = new ServiceHelper();
                    exceptionHandlingRequestEntity.Add(new ExceptionHandlingRequestEntity()
                    {
                        exceptionType = exc.GetType().Name,
                        exceptionMessage = exc.Message,
                        exceptionOrgin = exceptionOrgin,
                        exceptionOccuredOn = DateTime.UtcNow,
                        platform = Xamarin.Essentials.DeviceInfo.Platform.ToString(),
                        classFileName = classFileName,
                        deviceName = Xamarin.Essentials.DeviceInfo.Name,
                        deviceModel = Xamarin.Essentials.DeviceInfo.Model,
                        osVersion = Xamarin.Essentials.DeviceInfo.Version.ToString(),
                        userId = sharedPreferences.GetString(context.Resources.GetString(Resource.String.UserId), null),
                        username = sharedPreferences.GetString(context.Resources.GetString(Resource.String.Smusername), null)
                    });
                    var strSerializedData = JsonConvert.SerializeObject(exceptionHandlingRequestEntity);
                    var response = await objServicePrevious.PostRequest(strSerializedData, context.Resources.GetString(Resource.String.ExceptionHandlingURL), true, token);
                    ExceptionHandlingResponseEntity exceptionHandlingResponseEntity = JsonConvert.DeserializeObject<ExceptionHandlingResponseEntity>(response);
                    if (exceptionHandlingResponseEntity.operationStatus.ToLower().Equals("success"))
                    {
                        sharedPreferences.Edit().PutString(context.Resources.GetString(Resource.String.ExceptionData), null).Commit();
                    }
                    else
                    {
                        foreach (var item in exceptionHandlingRequestEntity)
                        {
                            DateTime exceptionDate = item.exceptionOccuredOn;
                            DateTime currentDate = DateTime.UtcNow;
                            TimeSpan ts = currentDate - exceptionDate;
                            double NrOfDays = ts.TotalDays;

                            if (NrOfDays > 7)
                            {
                                tempExceptionHandlingRequestEntity = null;
                                sharedPreferences.Edit().PutString(context.Resources.GetString(Resource.String.ExceptionData), null).Commit();
                            }
                        }
                    }
                    if (exceptionHandlingResponseEntity == null)
                    {
                        foreach (var item in exceptionHandlingRequestEntity)
                        {
                            DateTime exceptionDate = item.exceptionOccuredOn;
                            DateTime currentDate = DateTime.UtcNow;
                            TimeSpan ts = currentDate - exceptionDate;
                            double NrOfDays = ts.TotalDays;

                            if (NrOfDays > 7)
                            {
                                tempExceptionHandlingRequestEntity = null;
                                sharedPreferences.Edit().PutString(context.Resources.GetString(Resource.String.ExceptionData), null).Commit();
                            }
                        }
                    }
                }
                else
                {
                    if (exceptionHandlingRequestEntity != null)
                    {
                        foreach (var item in exceptionHandlingRequestEntity)
                        {
                            DateTime exceptionDate = item.exceptionOccuredOn;
                            DateTime currentDate = DateTime.UtcNow;
                            TimeSpan ts = currentDate - exceptionDate;
                            double NrOfDays = ts.TotalDays;

                            if (NrOfDays > 7)
                            {
                                tempExceptionHandlingRequestEntity = null;
                                sharedPreferences.Edit().PutString(context.Resources.GetString(Resource.String.ExceptionData), null).Commit();
                            }
                        }
                    }
                    exceptionHandlingRequestEntity.Add(new ExceptionHandlingRequestEntity()
                    {
                        exceptionType = exc.GetType().Name,
                        exceptionMessage = exc.Message,
                        exceptionOrgin = exceptionOrgin,
                        exceptionOccuredOn = DateTime.UtcNow,
                        platform = Xamarin.Essentials.DeviceInfo.Platform.ToString(),
                        classFileName = classFileName,
                        deviceName = Xamarin.Essentials.DeviceInfo.Name,
                        deviceModel = Xamarin.Essentials.DeviceInfo.Model,
                        osVersion = Xamarin.Essentials.DeviceInfo.Version.ToString(),
                        userId = sharedPreferences.GetString(context.Resources.GetString(Resource.String.UserId), null),
                        username = sharedPreferences.GetString(context.Resources.GetString(Resource.String.Smusername), null)
                    });
                    tempExceptionHandlingRequestEntity.AddRange(exceptionHandlingRequestEntity);
                    var strSerialized = JsonConvert.SerializeObject(tempExceptionHandlingRequestEntity);
                    sharedPreferences.Edit().PutString(context.Resources.GetString(Resource.String.ExceptionData), strSerialized.ToString()).Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Add new course
        /// </summary>
        /// <param name="courseEntity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> UploadImageToBlob(Stream image, string fileName, string token, string type)
        {
            string blobUrl = "";
            try
            {
                ServiceHelper objService = new ServiceHelper();
                blobUrl = await objService.PostFileRequest(image, fileName, "api/account/Upload/", true, token, type);
            }
            catch (Exception ex)
            {
                SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.AddNewCourse), context.Resources.GetString(Resource.String.Utility), null);
            }
            return blobUrl;
        }

        public async Task<string> GetLoyaltyPoints(string membershipId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(membershipId))
                    return null;

                string token = await GetLpToken("PointStatus", ConfigEntity.lpBaseURL + ConfigEntity.lpGenerateToken);
                if (token == null)
                    return null;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("access_token", token);
                string lpPointUrl = ConfigEntity.lpBaseURL + ConfigEntity.lpPointStatus;
                HttpResponseMessage response = await client.GetAsync(lpPointUrl.Replace("??", membershipId));
                var result = await response.Content.ReadAsStringAsync();
                LpPointStatusModel pointStatusModel = JsonConvert.DeserializeObject<LpPointStatusModel>(result);
                if (string.Equals(pointStatusModel.errorCode, "0"))
                    return pointStatusModel.availablePointBalance;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<string> GetLpToken(string apiMethod, string tokenUrl)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(tokenUrl.Replace("??", apiMethod));
            var result = await response.Content.ReadAsStringAsync();
            ExternalToken externalToken = JsonConvert.DeserializeObject<ExternalToken>(result);
            if (string.Equals(externalToken.errorCode, "0"))
                return externalToken.token;
            return null;
        }
    }
}