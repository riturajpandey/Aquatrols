namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Validate OTPR equest entity.
    /// </summary>
    public class ValidateOTPRequestEntity
    {
        public string UserId { get; set; }
        public int OTP { get; set; }
    }
    /// <summary>
    /// This model class is used for Validate user request entity.
    /// </summary>
    public class ValidateUserRequestEntity
    {
        public string username { get; set; }
    }
    /// <summary>
    /// This model class is used for Validate user response entity.
    /// </summary>
    public class ValidateUserResponseEntity
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string token { get; set; }
        public string role { get; set; }
        public string isActive { get; set; }
        public string isApproved { get; set; }
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
    /// <summary>
    /// This model class is used for Validate OTPR esponse entity.
    /// </summary>
    public class ValidateOTPResponseEntity
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string token { get; set; }
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
    /// <summary>
    /// This model class is used for Reset password request entity.
    /// </summary>
    public class ResetPasswordRequestEntity
    {
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string userId { get; set; }
        public string token { get; set; }
    }
    /// <summary>
    /// This model class is used for Reset password response entity.
    /// </summary>
    public class ResetPasswordResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }

    /// <summary>
    /// This model class is used for upload blob image response entity.
    /// </summary>
    public class UploadBlobImageResponseEntity
    {
        public string filePath { get; set; }
    }
    
}
