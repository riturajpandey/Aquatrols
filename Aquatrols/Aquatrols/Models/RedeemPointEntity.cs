using System;

namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Redeem gift card entity.
    /// </summary>
    public class RedeemGiftCardEntity
    {
        public string rewardId { get; set; }
        public string rewardItem { get; set; }
        public string itemImage { get; set; }
        public string description { get; set; }
        public int minimumPoints { get; set; }
        public int pointPricePerUnit { get; set; }
        public string itemType { get; set; }
        public bool isAvailableUs { get; set; }
        public bool isAvailableCanada { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public string labelForAmount { get; set; }
    }
    /// <summary>
    /// This model class is used for Redeem point entity.
    /// </summary>
    public class RedeemPointEntity
    {
        public string pointsSpentBy { get; set; }
        public string rewardItemId { get; set; }
        public string rewardItem { get; set; }
        public int minimumBalance { get; set; }
        public int pointsPricePerUnit { get; set; }
        public int totalPointsSpent { get; set; }
    }
    /// <summary>
    /// This model class is used for Redeem point response entity.
    /// </summary>
    public class RedeemPointResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
}
