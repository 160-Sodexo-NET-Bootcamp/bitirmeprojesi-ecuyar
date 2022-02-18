using Entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Category
{
    public class GetCategoryWithProductsDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int ParentCategoryId { get; set; }
        public IEnumerable<ShowProductDto> Products { get; set; }
    }
}
