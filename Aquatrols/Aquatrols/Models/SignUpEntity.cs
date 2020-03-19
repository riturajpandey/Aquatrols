namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Sign up request entity.
    /// </summary>
    public class SignUpRequestEntity
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string courseId { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string phoneNumber { get; set; } 
        public string referralDSRName { get; set; }
        public string referralCompany { get; set; }
        public string referralCode { get; set; }
        public string CourseAffiliationProofFilePath { get; set; }
        public string CourseAffiliationDescription { get; set; }
    }
    /// <summary>
    /// This model class is used for Sign up response entity.
    /// </summary>
    public class SignUpResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
        public string token { get; set; }
    }
}
