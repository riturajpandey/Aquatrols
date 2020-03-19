namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Change password entity.
    /// </summary>
    public class ChangePasswordEntity
    {
        public string userId { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
    }
    /// <summary>
    /// This model class is used for Change password response entity.
    /// </summary>
    public class ChangePasswordResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }

    }
}
