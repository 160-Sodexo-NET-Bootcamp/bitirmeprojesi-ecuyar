using Entity.Shared;

namespace MLS_Data.DataModels
{
    public class Product_DataModel : BaseEntity
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public short BrandId { get; set; }
        public short ColorId { get; set; }
        public int CategoryId { get; set; }
        public string SellerId { get; set; }
        public bool IsOfferable { get; set; }
        public bool IsSold { get; set; }
        public bool UsageStatus { get; set; }
        public decimal Price { get; set; }
        public string PicturePath { get; set; }
    }
}
