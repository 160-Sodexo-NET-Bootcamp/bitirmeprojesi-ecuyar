using Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLS_Data.DataModels
{
    public class Sales_DataModel: BaseEntity
    {
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }
}
