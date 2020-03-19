using System;

namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for User info entity.
    /// </summary>
    public class UserInfoEntity
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string userStatusId { get; set; }
        public string statusName { get; set; }
        public string courseId { get; set; }
        public string courseName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }  
        public int earnedPoints { get; set; }
        public int pointsSpent { get; set; }
        public int balancedPoints { get; set; }
        public string roleId { get; set; }
        public string role { get; set; }
        public string isEmailPreference { get; set; }
        public string isNotification { get; set; }
        public int isApproved { get; set; }
        public string approvedStatus { get; set; }
        public string isActive { get; set; }
        public DateTime createdOn { get; set; }
        public string MembershipId { get; set; }

    }
    /// <summary>
    /// This model class is used for Tm super intendant response entity.
    /// </summary>
    public class TmSuperIntendantResponseEntity
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; } 
        public string lastName { get; set; }
    }
}
