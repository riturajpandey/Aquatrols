using System;
using System.Collections.Generic;

namespace Aquatrols.iOS.Helper
{
    public class Constant
    {
        public static Dictionary<string, string> provinces;
        public static Dictionary<string, string> states;
        /// <summary>
        /// markets List.
        /// </summary>
        public static readonly List<string> lstMarkets = new List<string>
        { 
            "All Products", "Turf", "Horticulture", "Agriculture"
        };
        /// <summary>
        /// state List.
        /// </summary>
        public static readonly List<string> lstStates = new List<string> {
            "Alabama","Alaska","Arizona","Arkansas","California","Colorado","Connecticut","Delaware", "Florida","Georgia",
            "Hawaii","Idaho","Illinois","Indiana","Iowa","Kansas","Kentucky","Louisiana", "Maine","Maryland",
            "Massachusetts","Michigan","Minnesota","Mississippi","Missouri","Montana","Nebraska","Nevada", "New Hampshire","New Jersey",
            "New Mexico","New York","North Carolina","North Dakota","Ohio","Oklahoma","Oregon","Pennsylvania", "Rhode Island","South Carolina",
            "South Dakota","Tennessee","Texas","Utah","Vermont","Virginia","Washington","West Virginia", "Wisconsin","Wyoming",
        };
        /// <summary>
        /// provinces List.
        /// </summary>
        public static readonly List<string> lstProvinces = new List<string>
        {
            "Alberta","British Columbia","Manitoba","New Brunswick","Newfoundland and Labrador","Nova Scotia","Ontario","Prince Edward Island", "Quebec","Saskatchewan",
        };
        /// <summary>
        /// country List.
        /// </summary>
        public static readonly List<string> lstCountry = new List<string>{
            "USA",
            "Canada"
        };
        /// <summary>
        /// digit List.
        /// </summary>
        public static readonly List<int> lstDigit = new List<int>
        {
           0,1,2,3,4,5,10,11,12,14,15,17,20,30,40,50,90,100,112,130,150,162,176,180,189,149,200,165,234,193,255,812,369,10000,11000,124,51,20000,30000,22000,33000,16,56,54,22,315,1200
        };
        /// <summary>
        /// digit List as a string.
        /// </summary>
        public static readonly List<String> lstDigitString = new List<String>
        {
            "1","2","3","4","5","0",",","\\","//","/","20"
        };
        /// <summary>
        /// duration of toast message.
        /// </summary>
        public static readonly int durationOfToastMessage = 5000;
        public static readonly int sizeOfGiftCardUserContent = 150;
        public static readonly int sizeOfGiftCardUserContentiPad = 250;
        public static readonly int sizeOfGiftCardUserContentiPadPro = 300;
        public static readonly int digitTwentyFive = 25;
        public static readonly int digitEighty = 80;
        public static readonly double digitNintyNinePointsFive = 99.50;
        public static readonly double digitNinteenPointsFive = 19.5;
        public static readonly double digitEighteenPointsFive = 18.5;
        public static readonly double digitTwelvePointsFive = 12.5;
        public static readonly double digitElevenPointsFive = 11.5;
        public static readonly int digitFourtyFive = 45;
        public static readonly double digitFiftySixPointFive = 56.5;
        /// <summary>
        /// Get the states.
        /// </summary>
        /// <returns>The states.</returns>
        public static Dictionary<string, string> GetStates()
        {
            states = new Dictionary<string, string>();
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
        /// Get the province.
        /// </summary>
        /// <returns>The province.</returns>
        public static Dictionary<string, string> GetProvince()
        {
            provinces = new Dictionary<string, string>();
            provinces.Add("Alberta", "AB");
            provinces.Add("British Columbia", "BC");
            provinces.Add("Manitoba", "MB");
            provinces.Add("New Brunswick", "NB");
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
    }
}
