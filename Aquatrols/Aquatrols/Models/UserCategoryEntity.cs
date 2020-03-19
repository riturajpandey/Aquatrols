namespace Aquatrols.Models
{
    /// <summary>
    /// This model class is used for User category entity.
    /// </summary>
    public class UserCategoryEntity
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public int pointsPerUnit { get; set; }
    }
}
