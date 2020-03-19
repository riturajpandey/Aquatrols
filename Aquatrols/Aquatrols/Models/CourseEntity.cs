using System.Collections.Generic;

namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Course entity.
    /// </summary>
    public class CourseEntity
    {
        public string courseId { get; set; }
        //public string CId { get; set; }
        public string courseName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public int earnedPoints { get; set; }
        public int pointsSpent { get; set; }
        public int balancedPoints { get; set; }
    }
    /// <summary>
    /// This model class is used for Course response entity.
    /// </summary>
    public class CourseResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
    /// <summary>
    /// This model class is used for TM course response entity.
    /// </summary>
    public class TmCourseResponseEntity
    {
        public string cId { get; set; }
        public string courseName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }
    /// <summary>
    /// This model class is used for TM courses vm.
    /// </summary>
    public class TmCoursesVm
    {
        public string cId { get; set; }
        public string courseName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }
    /// <summary>
    ///This model class is used for TM product detail.
    /// </summary>
    public class TmProductDetail
    {
        public int rowNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public object distributorId { get; set; }
        public string distributorName { get; set; }
        public string productName { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
        public string bookingDate { get; set; }
    }
    /// <summary>
    /// This model class is used for TM distributor product vm.
    /// </summary>
    public class TmDistributorProductVm
    {
        public string distributorName { get; set; }
        public List<TmProductDetail> tmProductDetail { get; set; }
    }
    /// <summary>
    /// This model class is used for TM course result response entity.
    /// </summary>
    public class TmCourseResultResponseEntity
    {
        public string isUserTiedToCourse { get; set; }
        public TmCoursesVm tmCoursesVm { get; set; }
        public List<TmDistributorProductVm> tmDistributorProductVm { get; set; }
    }
}
