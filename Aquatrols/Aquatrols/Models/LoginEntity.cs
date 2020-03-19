using System.Collections.Generic;

namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Login request entity.
    /// </summary>
    public class LoginRequestEntity
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    /// <summary>
    /// This model class is used for Login response entity.
    /// </summary>
    public class LoginResponseEntity
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public bool? isActive { get; set; }
        public string isApproved { get; set; }
        public string token { get; set; }
        public List<string> role { get; set; }
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
}
