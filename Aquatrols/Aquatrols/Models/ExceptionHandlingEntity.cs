using System;
namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Exception handling request entity.
    /// </summary>
    public class ExceptionHandlingRequestEntity
      {
        public string exceptionType { get; set; }
        public string exceptionMessage { get; set; }
        public string exceptionOrgin { get; set; }
        public string platform { get; set; }
        public DateTime exceptionOccuredOn { get; set; }
        public string classFileName { get; set; }
        public string deviceName { get; set; }
        public string deviceModel { get; set; }
        public string osVersion { get; set; }
        public string userId { get; set; }
        public string username { get; set; }
    }
    /// <summary>
    /// This model class is used for Exception handling response entity.
    /// </summary>
    public class ExceptionHandlingResponseEntity
     {
          public string operationStatus { get; set; }
          public string operationMessage { get; set; }
     }
 }

