namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Push data model entity.
    /// </summary>
    public class PushDataModelEntity
    {
        public string userId { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }
        public string region { get; set; } 
    }
    /// <summary>
    /// This model class is used for Push data model response entity.
    /// </summary>
    public class PushDataModelResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
}
