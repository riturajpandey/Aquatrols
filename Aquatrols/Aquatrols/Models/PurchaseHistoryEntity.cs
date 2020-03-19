namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Purchase history entity.
    /// </summary>
    public class PurchaseHistoryEntity
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string courseName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string productLogo { get; set; }
        public string productName { get; set; }
        public int brandPoints { get; set; }
        public string categoryName { get; set; }
        public string unitName { get; set; }
        public string distributorName { get; set; }
        public int quantity { get; set; }
        public int pointsReceived { get; set; }
        public string bookingDate { get; set; }
        public string bookingStatus { get; set; }
    }
}
