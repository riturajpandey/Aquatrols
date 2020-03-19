namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Notification response entity.
    /// </summary>
    public class NotificationResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
    /// <summary>
    /// This model class is used for Notification entity.
    /// </summary>
    public class NotificationEntity
    {
        public string UserId { get; set; }
        public bool isEmailPreference { get; set; }
        public bool isNotification { get; set; }
    }
}
