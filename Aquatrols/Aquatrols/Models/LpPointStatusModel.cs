using System;
namespace Aquatrols.Models
{
    public class LpPointStatusModel
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string availablePointBalance { get; set; }
        public string availablePointBalanceInHomeCurrency { get; set; }
        public string availablePointBalanceInPrefferedCurrency { get; set; }
        public string currentStatusLevel { get; set; }
        public string currentStatusPointBalance { get; set; }
        public string customerId { get; set; }
        public string homeCurrencyCode { get; set; }
        public string lockedPointBalance { get; set; }
        public string lockedPointBalanceInHomeCurrency { get; set; }
        public string lockedPointBalanceInPrefferedCurrency { get; set; }
        public string membershipDate { get; set; }
        public string membershipId { get; set; }
        public string membershipStage { get; set; }
        public string pointExpiryDate { get; set; }
        public string pointsForNextStatus { get; set; }
        public string pointsToExpire { get; set; }
        public string preferredCurrencyCode { get; set; }
        public string statusExpiryDate { get; set; }
    }

    public class ExternalToken
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string token { get; set; }
        public string tokenExpiry { get; set; }
    }
}
