using Entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
    public class GetSaleDto
    {
        public int Id { get; set; }
        public string SellerId { get; private set; }
        public string BuyerId { get; private set; }
        public decimal Price { get; private set; }
        public int ProductId { get; set; }

        public virtual ShowProductDto Product { get; set; }
    }
}
