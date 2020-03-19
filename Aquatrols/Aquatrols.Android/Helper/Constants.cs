using System;
using System.Collections.Generic;

namespace Aquatrols.Droid.Helper
{
    /// <summary>
    /// To set constant value to be use in application
    /// </summary>
    public class Constants
    {
        public static Dictionary<string, string> provinces;
        public static Dictionary<string, string> states;       

        public static readonly List<String> liCountry = new List<String>()
            {
                "Select Country","USA","Canada"
            };

        public static readonly List<string> liMarket = new List<string>()
            {
                "All Products","Turf","Horticulture","Agriculture"
            };

        /// <summary>
        /// This Method is used to Bind the State List
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetStates()
        {
            states = new Dictionary<string, string>();
            states.Add("Select State", "Select State");
            states.Add("Alabama", "AL");
            states.Add("Alaska", "AK");
            states.Add("Arizona", "AZ");
            states.Add("Arkansas", "AR");
            states.Add("California", "CA");
            states.Add("Colorado", "CO");
            states.Add("Connecticut", "CT");
            states.Add("Delaware", "DE");
            states.Add("District Of Columbia", "DC");
            states.Add("Florida", "FL");
            states.Add("Georgia", "GA");
            states.Add("Hawaii", "HI");
            states.Add("Idaho", "ID");
            states.Add("Illinois", "IL");
            states.Add("Indiana", "IN");
            states.Add("Iowa", "IA");
            states.Add("Kansas", "KS");
            states.Add("Kentucky", "KY");
            states.Add("Louisiana", "LA");
            states.Add("Maine", "ME");
            states.Add("Maryland", "MD");
            states.Add("Massachusetts", "MA");
            states.Add("Michigan", "MI");
            states.Add("Minnesota", "MN");
            states.Add("Mississippi", "MS");
            states.Add("Missouri", "MO");
            states.Add("Montana", "MT");
            states.Add("Nebraska", "NE");
            states.Add("Nevada", "NV");
            states.Add("New Hampshire", "NH");
            states.Add("New Jersey", "NJ");
            states.Add("New Mexico", "NM");
            states.Add("New York", "NY");
            states.Add("North Carolina", "NC");
            states.Add("North Dakota", "ND");
            states.Add("Ohio", "OH");
            states.Add("Oklahoma", "OK");
            states.Add("Oregon", "OR");
            states.Add("Pennsylvania", "PA");
            states.Add("Rhode Island", "RI");
            states.Add("South Carolina", "SC");
            states.Add("South Dakota", "SD");
            states.Add("Tennessee", "TN");
            states.Add("Texas", "TX");
            states.Add("Utah", "UT");
            states.Add("Vermont", "VT");
            states.Add("Virginia", "VA");
            states.Add("Washington", "WA");
            states.Add("West Virginia</", "WV");
            states.Add("Wisconsin", "WI");
            states.Add("Wyoming", "WY");
            return states;
        }
        /// <summary>
        /// This method is used to Get Province List
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> GetProvince() {
            provinces = new Dictionary<string, string>();
            provinces.Add("Select Provinces", "Select Provinces");
            provinces.Add("Alberta","AB");
            provinces.Add("British Columbia","BC");
            provinces.Add("Manitoba","MB");
            provinces.Add("New Brunswick","NB");
            provinces.Add("Newfoundland and Labrador", "NL");
            provinces.Add("Nova Scotia", "NS");
            provinces.Add("Ontario", "ON");
            provinces.Add("Prince Edward Island", "PE");
            provinces.Add("Quebec", "QC");
            provinces.Add("Saskatchewan", "SK");
            provinces.Add("Northwest", "NT");
            provinces.Add("Nunavut", "NU");
            provinces.Add("Yukon", "YT");
            return provinces;
        }

        public const string receiverValue = "ReceiverValue";
        public const string matchContent = "<div[^>]*>Current Version</div><span[^>]*><div[^>]*><span[^>]*>(.*?)<";
        public const string Home = "Home";
        public const string Programs = "Programs";
        public const string Redeem = "Redeem";
        public const string About = "About";
        public const string MyFirebaseMsgService = "MyFirebaseMsgService";
    }
}