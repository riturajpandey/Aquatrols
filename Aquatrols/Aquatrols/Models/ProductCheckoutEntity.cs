namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Product checkout entity.
    /// </summary>
    public class ProductCheckoutEntity
    {
        public string productId { get; set; }
        public string productName { get; set; }
        public string bookedBy { get; set; }
        public string userName { get; set; }
        public string distributorId { get; set; }
        public string distributorName { get; set; }
        public int quantity { get; set; }
        public int brandPoints { get; set; }
        public int pointsReceived { get; set; }
    }
    /// <summary>
    /// this model class is used for Product checkout response entity.
    /// </summary>
    public class ProductCheckoutResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage{ get; set; }
    }
}
