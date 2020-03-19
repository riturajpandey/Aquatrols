using SQLite;
namespace Aquatrols.SqliteModels
{
    public class CartItemList
    {
        [PrimaryKey, AutoIncrement]
        public int ProductRowId { get; set; }
        public string cartItemCount { get; set; }
        public string userId { get; set; }
    }
}
