namespace Aquatrols.Models
{
    /// <summary>
    /// This class file is used for set the base URl for API.
    /// </summary>
    public class ConfigEntity
    {
        public static string baseURL = "http://approachv3.aquatrols.com/";
        public static string lpBaseURL = "https://api-re2.prime-cloud.com/";
        public static string lpGenerateToken = "OAuth/Service/OAuth.svc/GenerateToken?clientCode=AQTRLS&clientPartner=NA&ForApiMethod=??&clientKey=CPHEA144-C77E-6839-886A-5F2413FT526&outputType=json&countryCulture=en-us";
        public static string lpPointStatus = "V4/CashBack/Service/Member.svc/PointStatus?clientCode=AQTRLS&membershipId=??&outputType=json&countryCulture=en-us";
        public enum DeviceScreenSize : int { IPadPro = 1024, IPad = 768, ISixPlus = 414, ISix = 375, IFive = 320 };
    }
}
