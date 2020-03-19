using System.Collections.Generic;

namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Distributor info entity.
    /// </summary>
    public class DistributorInfoEntity
    {
        public string distributorId { get; set; }
        public string distributorName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }
    /// <summary>
    /// This model class is used for Distinct distributor entity.
    /// </summary>
    public class DistinctDistributorEntity
    {
        public string did { get; set; }
        public string distributorName { get; set; }
    }
    /// <summary>
    /// This model class is used for TM state list.
    /// </summary>
    public class TmStateList
    {
        public string regionState { get; set; }
    }
    /// <summary>
    /// This model class is used for TM distributor list.
    /// </summary>
    public class TmDistributorList
    {
        public string dId { get; set; }
        public string distributorName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }
    /// <summary>
    /// This model class is used for TM distributors.
    /// </summary>
    public class TmDistributors
    {
        public string dId { get; set; }
        public string distributorName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }
    /// <summary>
    /// This model class is used for TM distributor product detail.
    /// </summary>
    public class TmDistributorProductDetail
    {
        public int    rowNumber { get; set; }
        public string distributorId { get; set; }
        public string email { get; set; }
        public string courseId { get; set; }
        public string courseName { get; set; }
        public string productName { get; set; }
        public int    quantity { get; set; }
        public string unit { get; set; }
    }
    /// <summary>
    /// This model class is used for TM course product vm.
    /// </summary>
    public class TmCourseProductVm
    {
        public string courseName { get; set; }
        public List<TmDistributorProductDetail> tmProductDetail { get; set; }
    }
    /// <summary>
    /// This model class is used for TM distributor result request entity.
    /// </summary>
    public class TmDistributorResultRequestEntity
    {
        public string distributorName { get; set; }
        public string state { get; set; }
    }
    /// <summary>
    /// This model class is used for TM distributor result response entity.
    /// </summary>
    public class TmDistributorResultResponseEntity
    {
        public TmDistributors tmDistributors { get; set; }
        public List<TmCourseProductVm> tmCourseProductVm { get; set; }
    }
}
