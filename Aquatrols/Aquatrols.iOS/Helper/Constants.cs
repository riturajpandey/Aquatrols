using System;
using System.Collections.Generic;

namespace Aquatrols.iOS.Helper
{
    public class Constants
    {
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
           0,1,2,3,4,5,10,11,12,14,15,17,20,30,40,50,90,100,112,130,150,162,176,180,189,149,200,166,238,193,255,812,369,10000,11000,124,51,20000,30000,22000,33000,16,56,54,22,315,1200
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
    }
}
