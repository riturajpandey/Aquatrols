namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for My queue entity.
    /// </summary>
    public class MyQueueEntity
    {
        public int wishListId { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; }
        public int brandPoints { get; set; }
        public string distributorId { get; set; }
        public string userId { get; set; }
    }
    /// <summary>
    /// This model class is used for Wish list entity.
    /// </summary>
    public class WishListEntity
    {
        public int wishListId { get; set; }
        public string productId { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string productName { get; set; }
        public string productLogo { get; set; }
        public string dId { get; set; }
        public string distributorName { get; set; }
        public int quantity { get; set; }
        public int brandPoints { get; set; }
        public int pointsReceived { get; set; }
        public string unitName { get; set; }
    }
    /// <summary>
    /// This model class is used for My queue response entity.
    /// </summary>
    public class MyQueueResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
}
