using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Product
{
    public class ShowProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public short BrandId { get; set; }
        public short ColorId { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public int CategoryId { get; set; }
        public Guid SellerId { get; set; }
        public bool IsOfferable { get; set; }
        public bool IsSold { get; set; }
        public bool UsageStatus { get; set; }
        public decimal Price { get; set; }
        public string PicturePath { get; set; }
    }
}
