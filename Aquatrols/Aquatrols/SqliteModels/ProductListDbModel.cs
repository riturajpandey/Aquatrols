using SQLite;

namespace Aquatrols.SqliteModels
{
    class ProductListDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int ProductRowId { get; set; }
        public string productId { get; set; }
        public string productCategoryId { get; set; }
        public string categoryName { get; set; }
        public string productImage { get; set; }
        public string productLogo { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public int brandPoints { get; set; }
        public int canadaBrandPoints { get; set; }
        public string productShortDescription { get; set; }
        public string unitId { get; set; }
        public string unit { get; set; }
        public string rates { get; set; }
        public string sds { get; set; }
        public string labelPermaLink { get; set; }
        public string productWebpage { get; set; }
        public bool isBookable { get; set; }
        public string bookableStatus { get; set; }
        public string quantity { get; set; }
        public string distributorId { get; set; }
        public string distributorName{get;set;}
        public string UserId { get; set; }
    }
}
