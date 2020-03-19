using Aquatrols.Models;
using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UIKit;
using static Aquatrols.Models.ConfigEntity;

namespace Aquatrols.iOS.Helper
{
    public class Utility:IDisposable
    {
        private static Utility instance = null;
        public static bool isBlank = false;
        public List<ExceptionHandlingRequestEntity> tempExceptionHandlingRequestEntity = new List<ExceptionHandlingRequestEntity>();
        /// <summary>
        /// create instance of utility.
        /// </summary>
        /// <value>The get instance.</value>
        public static Utility GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utility();
                }
                return instance;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aquatrols.iOS.Helper.Utility"/> class.
        /// </summary>
        private Utility()
        {
        }
        /// <summary>
        /// Check the internet connection.
        /// </summary>
        /// <returns><c>true</c>, if internet connection was checked, <c>false</c> otherwise.</returns>
        #region Checking Internet Connection
        public bool CheckInternetConnection()
        {
            bool isNetworkReachable = false;
            if (!Reachability.IsHostReachable(AppResources.NetworkUrl))
            {
                isNetworkReachable = false;
            }
            else
            {
                isNetworkReachable = true;
            }
            return isNetworkReachable;
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
            string  regexPattern= @"^([\w-\.]+[a-zA-Z0-9]+@([\w-]+\.)+[\w-]{2,4})?$";
            Match matches = Regex.Match(strValue, regexPattern);
            return matches.Success;
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
        /// Method to check valid zip code
        /// </summary>
        /// <returns><c>true</c>, if valid zip was ised, <c>false</c> otherwise.</returns>
        /// <param name="strValue">String value.</param>
        public bool IsValidZip(string strValue,string country)
        {
            if(country.Equals(AppResources.USA))
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
                else if (matches1.Success== true)
                {
                    flag = true;
                }
                return flag;  
            }
        }
        /// <summary>
        /// Method to check password validation
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool IsValidPassword(string strValue)
        {
            string regexPattern = @"^(?=.*\d)(?=.*[A-Z]).{4,}$";
            Match matches = Regex.Match(strValue, regexPattern);
            return matches.Success;
        }
        #endregion
        /// <summary>
        /// Method to login a registered user.
        /// </summary>
        /// <returns></returns>
        public async Task<LoginResponseEntity> UserLogin(LoginRequestEntity loginRequestEntity)
        {
            LoginResponseEntity loginResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(loginRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.SignIn, false, "");
                loginResponseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return loginResponseEntity;
        }
        /// <summary>
        /// Method to send the service token.
        /// </summary>
        /// <returns>The service token.</returns>
        /// <param name="pushDataModelEntity">Push data model entity.</param>
        public async Task<PushDataModelResponseEntity> SendServiceToken(PushDataModelEntity pushDataModelEntity,string token)
        {
            PushDataModelResponseEntity pushDataModelResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(pushDataModelEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.PushNotification, true, token);
                pushDataModelResponseEntity = JsonConvert.DeserializeObject<PushDataModelResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pushDataModelResponseEntity;
        }
        /// <summary>
        /// Method to Register the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="signUpRequestEntity">Sign up entity.</param>
        public async Task<SignUpResponseEntity> RegisterUser(SignUpRequestEntity signUpRequestEntity,string tokenSignUp)
        {
            SignUpResponseEntity signUpResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(signUpRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.SignUpAPI, true, tokenSignUp);
                signUpResponseEntity = JsonConvert.DeserializeObject<SignUpResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return signUpResponseEntity;
        }
        /// <summary>
        /// Method to Validate the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="validateUserRequestEntity">Validate user request entity.</param>
        public async Task<ValidateUserResponseEntity> ValidateUser(ValidateUserRequestEntity validateUserRequestEntity)
        {
            ValidateUserResponseEntity validateUserResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(validateUserRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.SendOtp, true, "");
                validateUserResponseEntity = JsonConvert.DeserializeObject<ValidateUserResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return validateUserResponseEntity;
        }
        /// <summary>
        /// Method to Validates the otp.
        /// </summary>
        /// <returns>The otp.</returns>
        /// <param name="validateOTPRequestEntity">Validate OTPR equest entity.</param>
        public async Task<ValidateOTPResponseEntity> ValidateOTP(ValidateOTPRequestEntity validateOTPRequestEntity)
        {
            ValidateOTPResponseEntity validateOTPResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(validateOTPRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.ValidateOTP, true, "");
                validateOTPResponseEntity = JsonConvert.DeserializeObject<ValidateOTPResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return validateOTPResponseEntity;
        }
        /// <summary>
        /// Method to Reset the password.
        /// </summary>
        /// <returns>The password.</returns>
        /// <param name="resetPasswordRequestEntity">Reset password request entity.</param>
        public async Task<ResetPasswordResponseEntity> ResetPassword(ResetPasswordRequestEntity resetPasswordRequestEntity)
        {
            ResetPasswordResponseEntity resetPasswordResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(resetPasswordRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.ResetPassword, true, "");
                resetPasswordResponseEntity = JsonConvert.DeserializeObject<ResetPasswordResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resetPasswordResponseEntity;
        }
        /// <summary>
        /// Method to get the user information.
        /// </summary>User
        /// <returns>The user info.</returns>
        /// <param name="userId">User identifier.</param>
        public async Task<UserInfoEntity> GetUserInfo(string userId, string token)
        {
            UserInfoEntity userInfoEntity = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.UserDetail + userId, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    userInfoEntity = JsonConvert.DeserializeObject<UserInfoEntity>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return userInfoEntity;
        }
        /// <summary>
        /// Method to get the distributor information.
        /// </summary>
        /// <returns>The distributor info.</returns>
        public async Task<List<DistributorInfoEntity>> GetDistributorInfo(string userid, string token)
        {
            List<DistributorInfoEntity> distributorInfoEntity = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.DistributorListUserWise + userid, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    distributorInfoEntity = JsonConvert.DeserializeObject<List<DistributorInfoEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return distributorInfoEntity;
        }
        /// <summary>
        /// method to get all distributor list.
        /// </summary>
        /// <returns>The all distributor list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<DistributorInfoEntity>> GetAllDistributorList(string token)
        {
            List<DistributorInfoEntity> distributorInfoEntity = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.DistributorList, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    distributorInfoEntity = JsonConvert.DeserializeObject<List<DistributorInfoEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return distributorInfoEntity;
        }
        /// <summary>
        /// Method to add the new course.
        /// </summary>
        /// <returns>The new course.</returns>
        /// <param name="courseEntity">Course entity.</param>
        public async Task<CourseResponseEntity> AddNewCourse(CourseEntity courseEntity,string tokenSignUp)
        {
            CourseResponseEntity courseResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(courseEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.NewCourse, true, tokenSignUp);
                courseResponseEntity = JsonConvert.DeserializeObject<CourseResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            return courseResponseEntity;
        }
        /// <summary>
        /// Method to get the course list.
        /// </summary>
        /// <returns>The course list.</returns>
        public async Task<List<CourseEntity>> GetCourseList(string token)
        {
            List<CourseEntity> courseEntity = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.CourseList, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    courseEntity = JsonConvert.DeserializeObject<List<CourseEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return courseEntity;
        }
        /// <summary>
        /// Method to Update the notification setting.
        /// </summary>
        /// <returns>The notification setting.</returns>
        /// <param name="notificationEntity">Notification entity.</param>
        public async Task<NotificationResponseEntity> UpdateNotificationSetting(NotificationEntity notificationEntity, string token)
        {
            NotificationResponseEntity notificationResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(notificationEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.Notifications, true, token);
                notificationResponseEntity = JsonConvert.DeserializeObject<NotificationResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return notificationResponseEntity;
        }
        /// <summary>
        /// Method to change the password.
        /// </summary>
        /// <returns>The password.</returns>
        /// <param name="changePasswordEntity">Change password entity.</param>
        public async Task<ChangePasswordResponseEntity> ChangePassword(ChangePasswordEntity changePasswordEntity, string token)
        {
            ChangePasswordResponseEntity changePasswordResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(changePasswordEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.ChangePassword, true, token);
                changePasswordResponseEntity = JsonConvert.DeserializeObject<ChangePasswordResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return changePasswordResponseEntity;
        }
        /// <summary>
        /// methof to get product list.
        /// </summary>
        /// <returns>The product list.</returns>
        public async Task<List<ProductListEntity>> GetProductList(string token)
        {
            List<ProductListEntity> productListEntities = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.ProductListApi, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                productListEntities = JsonConvert.DeserializeObject<List<ProductListEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return productListEntities;
        }
        /// <summary>
        /// method to Product checkout.
        /// </summary>
        /// <returns>The checkout.</returns>
        /// <param name="productCheckoutEntities">Product checkout entities.</param>
        /// <param name="token">Token.</param>
        public async Task<ProductCheckoutResponseEntity> ProductCheckout(List<ProductCheckoutEntity> productCheckoutEntities, string token)
        {
            ProductCheckoutResponseEntity productCheckoutResponse = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(productCheckoutEntities);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.ProductBook, true, token);
                productCheckoutResponse = JsonConvert.DeserializeObject<ProductCheckoutResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return productCheckoutResponse;
        }
        /// <summary>
        /// method to get the redeem points
        /// </summary>
        /// <returns>The points.</returns>
        /// <param name="redeemPointsEntity">Redeem points entity.</param>
        /// <param name="token">Token.</param>
        public async Task<RedeemPointResponseEntity> RedeemPoints(RedeemPointEntity redeemPointsEntity, string token)
        {
            RedeemPointResponseEntity redeemPointsResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(redeemPointsEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.RedeemAPI, true, token);
                redeemPointsResponseEntity = JsonConvert.DeserializeObject<RedeemPointResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return redeemPointsResponseEntity;
        }
      /// <summary>
      /// method to get the token case of sign up
      /// </summary>
      /// <returns>The request token.</returns>
      /// <param name="loginRequestEntity">Login request entity.</param>
        public async Task<AccessTokenResponseEntity> GetRequestToken(LoginRequestEntity loginRequestEntity)
        {
            AccessTokenResponseEntity accessTokenResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(loginRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.GetAccessToken, false, "");
                accessTokenResponseEntity = JsonConvert.DeserializeObject<AccessTokenResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return accessTokenResponseEntity;
        }
        /// <summary>
        /// method to get the purchase history information.
        /// </summary>
        /// <returns>The purchase history information.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<PurchaseHistoryEntity>> GetPurchaseHistoryInformation(string userId,string token)
        {
            List<PurchaseHistoryEntity> purchaseHistoryEntities = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.BookingDetail + userId, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    purchaseHistoryEntities = JsonConvert.DeserializeObject<List<PurchaseHistoryEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return purchaseHistoryEntities;
        }
        /// <summary>
        /// Method to add the My Queue.
        /// </summary>
        /// <returns>The My Queue.</returns>
        /// <param name="myQueueEntity">MyQueue entity.</param>
        public async Task<MyQueueResponseEntity> AddMyQueue(MyQueueEntity myQueueEntity, string tokenSignUp)
        {
            MyQueueResponseEntity myQueueResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(myQueueEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.MyQueueAPI, true, tokenSignUp);
                myQueueResponseEntity = JsonConvert.DeserializeObject<MyQueueResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return myQueueResponseEntity;
        }
        /// <summary>
        /// Gets the wish list deatil.
        /// </summary>
        /// <returns>The wish list deatil.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Token.</param>
        public async Task<List<WishListEntity>> GetWishListItem(string userId, string token)
        {
            List<WishListEntity> wishListEntities = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.WishListAPI + userId, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    wishListEntities = JsonConvert.DeserializeObject<List<WishListEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return wishListEntities;
        }
        /// <summary>
        /// Gets the wish list item count.
        /// </summary>
        /// <returns>The wish list item count.</returns>
        /// <param name="token">Token.</param>
        public async Task<string> GetWishListItemCount(string token)
        {
            string count = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.WishListItemCountAPI, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    count = JsonConvert.DeserializeObject<string>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }
        /// <summary>
        /// Clears the queue.
        /// </summary>
        /// <returns>The queue.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Token.</param>
        public async Task<MyQueueResponseEntity> ClearQueue(string userId, string token)
        {
            MyQueueResponseEntity myQueueResponseEntity = null;
            try
            {
                string strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.ClearQueueAPI+ userId, true, token);
                myQueueResponseEntity = JsonConvert.DeserializeObject<MyQueueResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return myQueueResponseEntity;
        }
        /// <summary>
        /// Deletes the selected item.
        /// </summary>
        /// <returns>The selected item.</returns>
        /// <param name="wishListId">User identifier.</param>
        /// <param name="token">Token.</param>
        public async Task<MyQueueResponseEntity> DeleteSelectedItem(int wishListId, string token)
        {
            MyQueueResponseEntity myQueueResponseEntity = null;
            try
            {
                string strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.DeleteSelectedItemAPI + wishListId, true, token);
                myQueueResponseEntity = JsonConvert.DeserializeObject<MyQueueResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return myQueueResponseEntity;
        }
        /// <summary>
        /// Deletes the selected item.
        /// </summary>
        /// <returns>The selected item.</returns>
        /// <param name="token">Token.</param>
        public async Task<string> GetTerritoryDataFile(string token)
        {
            string filepath = null;
            try
            {
                string strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData,AppResources.GetSnapshotExcelAPI, true, token);
                filepath = JsonConvert.DeserializeObject<string>(response);
           }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return filepath;
        }
        /// <summary>
        /// Deletes the selected item.
        /// </summary>
        /// <returns>The selected item.</returns>
        /// <param name="token">Token.</param>
        public async Task<string> GetTerritoryDataFileDistributor(TmDistributorResultRequestEntity tmDistributorResultRequestExcel,string token)
        {
            string filepath = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(tmDistributorResultRequestExcel);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.ExportToExcelAPI, true, token);
                filepath = JsonConvert.DeserializeObject<string>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return filepath;
        }
        /// <summary>
        /// Method to get the distinct distributor list.
        /// </summary>
        /// <returns>The course list.</returns>
        public async Task<List<DistinctDistributorEntity>> GetDistinctDistributorList(string token)
        {
            List<DistinctDistributorEntity> distinctDistributorEntity = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.DistinctDistributorList, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    distinctDistributorEntity = JsonConvert.DeserializeObject<List<DistinctDistributorEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return distinctDistributorEntity;
        }
        /// <summary>
        /// Get the reward item list.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<RedeemGiftCardEntity>> GetRewardItemList(string token)
        {
            List<RedeemGiftCardEntity> redeemGiftCardEntity = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.RewardItemList, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    redeemGiftCardEntity = JsonConvert.DeserializeObject<List<RedeemGiftCardEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return redeemGiftCardEntity;
        }
        /// <summary>
        /// Get the Territory course list item.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<TmCourseResponseEntity>> GetTmCourseList(string token)
        {
            List<TmCourseResponseEntity> tmCourseResponseEntity = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.TmCourseListAPI, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmCourseResponseEntity = JsonConvert.DeserializeObject<List<TmCourseResponseEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmCourseResponseEntity;
        }
        /// <summary>
        /// Get the Territory Superintendent list item.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<TmSuperIntendantResponseEntity>> GetSuperIntendentList(string token)
        {
            List<TmSuperIntendantResponseEntity> tmSuperIntendantResponseEntities = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.TmSuperIntendentListAPI, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmSuperIntendantResponseEntities = JsonConvert.DeserializeObject<List<TmSuperIntendantResponseEntity>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }
            return tmSuperIntendantResponseEntities;
        }
        /// <summary>
        /// Get the Territory Course result data.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<TmCourseResultResponseEntity> GetTmCourseResultData(string courseID,string token)
        {
            TmCourseResultResponseEntity tmCourseResultResponse = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.TmCourseResultAPI+courseID, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmCourseResultResponse = JsonConvert.DeserializeObject<TmCourseResultResponseEntity>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmCourseResultResponse;
        }
        /// <summary>
        /// Get the Territory Superintendent result data.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<TmCourseResultResponseEntity> GetTmSuperIntendentResultData(string superIntendentId, string token)
        {
            TmCourseResultResponseEntity tmCourseResultResponse = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.TmSuperIntendentResultAPI + superIntendentId, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmCourseResultResponse = JsonConvert.DeserializeObject<TmCourseResultResponseEntity>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmCourseResultResponse;
        }
        /// <summary>
        /// Get the Territory State list item.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<TmStateList>> GetTmStateList(string token)
        {
            List<TmStateList> tmStateLists = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.TmStateSearchAPI, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmStateLists = JsonConvert.DeserializeObject<List<TmStateList>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmStateLists;
        }
        /// <summary>
        /// Get the Territory State list item.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<TmDistributorList>> GetTmDistributorList(string tmStateID,string token)
        {
            List<TmDistributorList> tmDistributors = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.TmDistributorSearchAPI+tmStateID, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmDistributors = JsonConvert.DeserializeObject<List<TmDistributorList>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmDistributors;
        }
        /// <summary>
        /// Get the Distributor search Result data.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<TmDistributorResultResponseEntity> GetTmDistributorSearchResultData(TmDistributorResultRequestEntity tmDistributorResultRequestEntity, string token)
        {
            TmDistributorResultResponseEntity tmDistributorResultResponseEntities = null;
              try
            {
                string strSerializedData = JsonConvert.SerializeObject(tmDistributorResultRequestEntity);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.TmDistributorSearchResultAPI, true, token);
                tmDistributorResultResponseEntities = JsonConvert.DeserializeObject<TmDistributorResultResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmDistributorResultResponseEntities;
        }
        /// <summary>
        /// Get the Distributor search Result data.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<TmSnapshotSignUpCountVm>> GetTmSnapShotResultCountData(string token)
        {
            ListTmSnapshotSignUp tmCommitmentsPerDistributor = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.GetSignupCommitmentCountAPI, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmCommitmentsPerDistributor = JsonConvert.DeserializeObject<ListTmSnapshotSignUp>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmCommitmentsPerDistributor.tmSnapshotSignUpCountVm;
        }
        /// <summary>
        /// Get the Distributor search Result data.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<TmCommitmentsPerDistributor>> ProductCommtPerDistributor(string token)
        {
            List<TmCommitmentsPerDistributor> tmCommitmentsPerDistributor = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.GetCommitmentPerDistributorAPI, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmCommitmentsPerDistributor = JsonConvert.DeserializeObject<List<TmCommitmentsPerDistributor>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmCommitmentsPerDistributor;
        }
        /// <summary>
        /// Get the Distributor search Result data.
        /// </summary>
        /// <returns>The reward item list.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<TmTotalCommitments>> GetTotalGallonperLtr(string token)
        {
            List<TmTotalCommitments> tmTotalCommitments = null;
            try
            {
                string strSerializedData = null;
                ServiceHelper objService = new ServiceHelper();
                ServiceHelper.ServiceResponse serviceResponse = await objService.GetRequest(strSerializedData, AppResources.GetTotalQuantityCommitedAPI, true, token);
                if (serviceResponse.status.Equals(AppResources.OK))
                    tmTotalCommitments = JsonConvert.DeserializeObject<List<TmTotalCommitments>>(serviceResponse.response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tmTotalCommitments;
        }
        /// <summary>
        /// Dismiss the keyboard on tap.
        /// </summary>
        /// <param name="item">Item.</param>
        public void DismissKeyboardOnTap(UIView[] item)
        {
            try
            {
                foreach (UIView view in item)
                {
                    var g = new UITapGestureRecognizer(() => view.EndEditing(true));
                    g.CancelsTouchesInView = false; //for iOS5
                    view.AddGestureRecognizer(g);
                } 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// method to add Done button to numeric keyboard.
        /// </summary>
        /// <param name="textField">Text field.</param>
        public static void AddDoneButtonToNumericKeyboard(UITextField[] textField)
        {
            try
            {
                foreach (var item in textField)
                {
                    UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));
                    var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
                    {
                        item.ResignFirstResponder();
                    });
                    toolbar.Items = new UIBarButtonItem[] {
                    new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                    doneButton
                };
                    item.InputAccessoryView = toolbar;
                } 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// method to add Done button to numeric keyboard.
        /// </summary>
        /// <param name="textField">Text field.</param>
        public static void AddDoneButtonToKeyboard(UITextField[] textField)
        {
            try
            {
                foreach (var item in textField)
                {
                    item.ReturnKeyType = UIReturnKeyType.Done;
                    item.ShouldReturn = (txtField) =>
                    {
                        txtField.ResignFirstResponder();
                        return true;
                    };
                } 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// method to remove NSUserDefaults object value
        /// </summary>
        public static void ClearCachedData()
        {
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.userId);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.IsLogedIn);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isEmailPreference);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isNotification);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isCheckbox);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.CourseName);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.CourseId);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.approved);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.userName);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.password);
            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.isTermsConditions);
        }
        /// <summary>
        /// method to set the padding of UITextField.
        /// </summary>
        /// <param name="f">F.</param>
        /// <param name="padding">Padding.</param>
        public static void SetPadding(UITextField[] f, int padding)
        {
            try
            {
                if (f != null)
                {
                    foreach (UITextField textfield in f)
                    {
                        UIView paddingView = new UIView(new RectangleF(Constant.lstDigit[0], Constant.lstDigit[0], padding, Constant.lstDigit[12]));
                        textfield.LeftView = paddingView;
                        textfield.LeftViewMode = UITextFieldViewMode.Always;
                    }
                }   
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// method to set the fonts of Label, textfield and text view.
        /// </summary>
        /// <param name="txtview">Txtview.</param>
        /// <param name="label">Label.</param>
        /// <param name="txtfield">Txtfield.</param>
        /// <param name="size">Size.</param>
        public static void SetFonts(UITextView[] txtview, UILabel[] label, UITextField[] txtfield, nfloat size, int width)
        {
            try
            {
                nfloat Size = size;
                switch (width)
                {
                    case (int)DeviceScreenSize.ISixPlus: //6 Plus, 7 Plu
                        Size = size + Constant.lstDigit[2];
                        break;

                    case (int)DeviceScreenSize.ISix:  //6, 6s,
                        Size = size + Constant.lstDigit[3];
                        break;

                    case (int)DeviceScreenSize.IFive: //5, 5s, 5
                        Size = size;
                        break;
                    case (int)DeviceScreenSize.IPadPro: //iPad Pro
                        Size = size + Constant.lstDigit[8];
                        break;
                    case (int)DeviceScreenSize.IPad: //5iPad
                        Size = size + Constant.lstDigit[6];
                        break;

                }
                if (txtview != null)
                {
                    foreach (UITextView t in txtview)
                    {
                        t.Font = UIFont.FromName(AppResources.RobotoRegular, Size);
                    }
                }
                if (label != null)
                {
                    foreach (UILabel lbl in label)
                    {
                        lbl.Font = UIFont.FromName(AppResources.RobotoRegular, Size);
                    }
                }
                if (txtfield != null)
                {
                    foreach (UITextField textfield in txtfield)
                    {
                        textfield.Font = UIFont.FromName(AppResources.RobotoRegular, Size);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// method to sets the fonts sen serif.
        /// </summary>
        /// <param name="label">Label.</param>
        /// <param name="size">Size.</param>
        /// <param name="width">Width.</param>
        public static void SetFontsSenSerif( UILabel[] label, nfloat size, int width)
        {
            
            try
            {
                nfloat Size = size;
                switch (width)
                {
                    case (int)DeviceScreenSize.ISixPlus: //6 Plus, 7 Plu
                        Size = size + Constant.lstDigit[2];
                        break;

                    case (int)DeviceScreenSize.ISix:  //6, 6s,
                        Size = size + Constant.lstDigit[3];
                        break;

                    case (int)DeviceScreenSize.IFive: //5, 5s, 5
                        Size = size;
                        break;

                    case (int)DeviceScreenSize.IPadPro: //iPad Pro
                        Size = size + Constant.lstDigit[8];
                        break;

                    case (int)DeviceScreenSize.IPad: //iPad
                        Size = size + Constant.lstDigit[6];
                        break;

                }

                if (label != null)
                {
                    foreach (UILabel lbl in label)
                    {
                       lbl.Font = UIFont.FromName(AppResources.RobotoRegular,Size);
                    }
                }  
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Method to Sets the fonts italic.
        /// </summary>
        /// <param name="label">Label.</param>
        /// <param name="size">Size.</param>
        public static void SetFontsItalic(UILabel[] label, nfloat size, int width)
        {
            try
            {
                nfloat Size = size;
                switch (width)
                {
                    case (int)DeviceScreenSize.ISixPlus: //6 Plus, 7 Plu
                        Size = size + Constant.lstDigit[2];
                        break;

                    case (int)DeviceScreenSize.ISix:  //6, 6s,
                        Size = size + Constant.lstDigit[3];
                        break;

                    case (int)DeviceScreenSize.IFive: //5, 5s, 5
                        Size = size;
                        break;

                    case (int)DeviceScreenSize.IPadPro: // iPad Pro
                        Size = size + Constant.lstDigit[8];
                        break;

                    case (int)DeviceScreenSize.IPad: //IPad
                        Size = size + Constant.lstDigit[6];
                        break;

                }
                if (label != null)
                {
                    foreach (UILabel lbl in label)
                    {
                        lbl.Font = UIFont.FromName(AppResources.RobotoItalic, Size);
                    }
                } 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// method to sets the fonts for header for label and button.
        /// </summary>
        /// <param name="label">Label.</param>
        /// <param name="button">Button.</param>
        /// <param name="size">Size.</param>
        public static void SetFontsforHeader(UILabel[] label, UIButton[] button, nfloat size, int width)
        {
            try
            {
                nfloat Size = size;
                switch (width)
                {
                    case (int)DeviceScreenSize.ISixPlus: //6 Plus, 7 Plu
                        Size = size + Constant.lstDigit[2];
                        break;

                    case (int)DeviceScreenSize.ISix:  //6, 6s,
                        Size = size + Constant.lstDigit[3];
                        break;

                    case (int)DeviceScreenSize.IFive: //5, 5s, 5
                        Size = size;
                        break;

                    case (int)DeviceScreenSize.IPadPro: //IPad pro
                        Size = size + Constant.lstDigit[8];
                        break;

                    case (int)DeviceScreenSize.IPad: //iPad
                        Size = size + Constant.lstDigit[6];
                        break;

                }
                if (label != null)
                {
                    foreach (UILabel lbl in label)
                    {
                        lbl.Font = UIFont.FromName(AppResources.RobotoBold, Size);
                    }
                }
                if (button != null)
                {
                    foreach (UIButton btn in button)
                    {
                        btn.Font = UIFont.FromName(AppResources.RobotoBold, Size);
                    }
                } 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// method to show the alert message.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        public static void DebugAlert(string title, string message)
        {
            try
            {
                UIAlertView alert = new UIAlertView();
                alert.Title = title;
                alert.AddButton(AppResources.OK);
                alert.Message = message;
                alert.Show(); 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Hits the exception handling API.
        /// </summary>
        public async void SaveExceptionHandling(Exception exception, string exceptionOrgin, string classFileName, string previouData)
        {
            try
            {
                string token = string.Empty;
                string userId = string.Empty;
                string userName = string.Empty;
                bool internetStatus = CheckInternetConnection();
                // get token
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token)))
                {
                    token = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.token);
                }
                // get user id 
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId)))
                {
                    userId = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userId);
                }
                // get user name 
                if (!string.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userName)))
                {
                    userName = NSUserDefaults.StandardUserDefaults.StringForKey(AppResources.userName);
                }
                List<ExceptionHandlingRequestEntity> exceptionHandlingRequestEntity = new List<ExceptionHandlingRequestEntity>();
                if (internetStatus) // check the internet connection
                {
                    ServiceHelper objService = new ServiceHelper();
                    if (!(String.IsNullOrEmpty(previouData)))
                    {
                        string response = await objService.PostRequest(previouData, AppResources.ExceptionhandlingAPI, true, token);
                        ExceptionHandlingResponseEntity exceptionHandlingResponseEntity = JsonConvert.DeserializeObject<ExceptionHandlingResponseEntity>(response);
                        if (exceptionHandlingResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                        {
                            tempExceptionHandlingRequestEntity = null;
                            NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.Exception);
                        }  
                        else
                        {
                            exceptionHandlingRequestEntity = JsonConvert.DeserializeObject<List<ExceptionHandlingRequestEntity>>(previouData);
                            if (exceptionHandlingRequestEntity!=null)
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
                                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.Exception);
                                    }
                                }
                            }
                           
                        }
                        if(exceptionHandlingResponseEntity==null)
                        {
                            exceptionHandlingRequestEntity = JsonConvert.DeserializeObject<List<ExceptionHandlingRequestEntity>>(previouData);
                            if (exceptionHandlingRequestEntity!=null)
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
                                        NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.Exception);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        exceptionHandlingRequestEntity.Add(new ExceptionHandlingRequestEntity()
                        {
                            exceptionType = exception.GetType().Name,
                            exceptionMessage = exception.Message,
                            exceptionOrgin = exceptionOrgin,
                            exceptionOccuredOn = DateTime.UtcNow,
                            platform = UIDevice.CurrentDevice.SystemName,
                            classFileName = classFileName,
                            deviceName = UIDevice.CurrentDevice.Name,
                            deviceModel = Xamarin.iOS.DeviceHardware.Model,
                            osVersion = UIDevice.CurrentDevice.SystemVersion,
                            userId = userId,
                            username = userName
                        });
                        string strSerializedData = JsonConvert.SerializeObject(exceptionHandlingRequestEntity);
                        string response = await objService.PostRequest(strSerializedData, AppResources.ExceptionhandlingAPI, true, token);
                        ExceptionHandlingResponseEntity exceptionHandlingResponseEntity = JsonConvert.DeserializeObject<ExceptionHandlingResponseEntity>(response);
                        if (exceptionHandlingResponseEntity.operationStatus.ToLower().Equals(AppResources.success))
                        {
                           // Toast.MakeText(exception.GetType().Name).Show();
                        }
                    }
                }
                else
                {
                    if (!(String.IsNullOrEmpty(previouData)))
                    {
                        exceptionHandlingRequestEntity = JsonConvert.DeserializeObject<List<ExceptionHandlingRequestEntity>>(previouData);
                        if (exceptionHandlingRequestEntity!=null)
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
                                    NSUserDefaults.StandardUserDefaults.RemoveObject(AppResources.Exception);
                                }
                            }
                        }
                    }

                    exceptionHandlingRequestEntity.Add(new ExceptionHandlingRequestEntity()
                    {
                        exceptionType = exception.GetType().Name,
                        exceptionMessage = exception.Message,
                        exceptionOrgin = exceptionOrgin,
                        exceptionOccuredOn = DateTime.UtcNow,
                        platform = UIDevice.CurrentDevice.SystemName,
                        classFileName = classFileName,
                        deviceName = UIDevice.CurrentDevice.Name,
                        deviceModel = Xamarin.iOS.DeviceHardware.Model,
                        osVersion = UIDevice.CurrentDevice.SystemVersion,
                        userId = userId,
                        username = userName
                    });
                    tempExceptionHandlingRequestEntity.AddRange(exceptionHandlingRequestEntity);
                    string strSerializedData = JsonConvert.SerializeObject(tempExceptionHandlingRequestEntity);
                    NSUserDefaults.StandardUserDefaults.SetString(strSerializedData,AppResources.Exception);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Releases all resource used by the <see cref="T:Aquatrols.iOS.Helper.Utility"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:Aquatrols.iOS.Helper.Utility"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="T:Aquatrols.iOS.Helper.Utility"/> in an unusable state.
        /// After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Aquatrols.iOS.Helper.Utility"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:Aquatrols.iOS.Helper.Utility"/> was occupying.</remarks>
        public void Dispose()
        {
            instance = null;
            instance.Dispose();
        }
         
        public async Task<string> UploadImageToBlobAsync(Stream image, string fileName, string token, string type)
        {
            string blobUrl = "";
            try
            {
                ServiceHelper objService = new ServiceHelper();
                blobUrl = await objService.PostFileRequest(image, fileName, "api/account/Upload/", true, token, type);
            }
            catch (Exception ex)
            {
                return null;
                //SaveExceptionHandling(ex, context.Resources.GetString(Resource.String.AddNewCourse), context.Resources.GetString(Resource.String.Utility), null);
            }
            return blobUrl;
        }

        /// <summary>
        /// Method to check validity of loggedin token.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckTokenValidity(string userToken)
        {
            LoginResponseEntity loginResponseEntity = null;
            try
            {
                string strSerializedData = JsonConvert.SerializeObject(userToken);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, AppResources.SignIn, false, "");
                loginResponseEntity = JsonConvert.DeserializeObject<LoginResponseEntity>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }

        /// <summary>
        /// Method to Register the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="membershipId">User membership Id.</param>
        public async Task<string> GetLoyaltyPoints(string membershipId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(membershipId))
                    return null;

                string token = await GetLpToken("PointStatus",lpBaseURL+lpGenerateToken);
                if (token == null)
                    return null;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("access_token", token);
                string lpPointUrl = lpBaseURL + lpPointStatus;
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
