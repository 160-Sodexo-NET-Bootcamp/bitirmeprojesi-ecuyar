using System;
using System.ComponentModel.DataAnnotations;

namespace MLS_Data.DataModels
{
    public class Product_DataModel
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public Guid UserId { get; set; }
        public bool IsOfferable { get; set; }
        public bool IsSold { get; set; }
        public bool UsageStatus { get; set; }
        public decimal Price { get; set; }
        public string PicturePath { get; set; }
    }
}
