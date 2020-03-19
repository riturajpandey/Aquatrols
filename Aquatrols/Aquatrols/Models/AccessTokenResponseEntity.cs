namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for access token response entity
    /// </summary>
    public class AccessTokenResponseEntity
    {
        public string userId { get; set; }
        public string token { get; set; }
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
}
