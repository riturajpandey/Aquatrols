namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for Product list entity.
    /// </summary>
    public class ProductListEntity
    {
        public string productId { get; set; }
        public string productCategoryId { get; set; }
        public string categoryName { get; set; }
        public string productImage { get; set; }
        public string productLogo { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string productShortDescription { get; set; }
        public int brandPoints { get; set; }
        public string unitId { get; set; }
        public string unit { get; set; }
        public string rates { get; set; }
        public string sds { get; set; }
        public string labelPermaLink { get; set; }
        public string productWebpage { get; set; }
        public bool? isBookable { get; set; }
        public string bookableStatus { get; set; }
        public bool? isCanadianOnly { get; set; }
        public string canadaOnly { get; set; }
        public int canadaBrandPoints { get; set; }
    }
}
