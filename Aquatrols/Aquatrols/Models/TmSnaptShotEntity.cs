using System.Collections.Generic;

namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Tm snapt shot entity.
    /// </summary>
    public class TmSnaptShotEntity
    {
        
    }
    /// <summary>
    /// This model class is used for Tm commitments per distributor.
    /// </summary>
    public class TmCommitmentsPerDistributor
    {
        public string distributorName { get; set; }
        public int courseCount { get; set; }
    }
    /// <summary>
    /// This model class is used for  Tm total commitments.
    /// </summary>
    public class TmTotalCommitments
    {
        public string productName { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
        public string country { get; set; }
    }
    /// <summary>
    /// This model class is used for Tm snapshot sign up count vm.
    /// </summary>
    public class TmSnapshotSignUpCountVm
    {
        public string territoryNumber { get; set; }
        public string territoryName { get; set; }
        public int countSignUps { get; set; }
        public int countCommitments { get; set; }
    }
    /// <summary>
    /// This model class is used for List tm snapshot sign up.
    /// </summary>
    public class ListTmSnapshotSignUp
    {
        public List<TmSnapshotSignUpCountVm> tmSnapshotSignUpCountVm { get; set; }
    }
    /// <summary>
    /// This model class is used for Snap shot request entity.
    /// </summary>
    public class SnapShotRequestEntity
    {
        public string distributorName { get; set; }
        public string state { get; set; }
    }
}
