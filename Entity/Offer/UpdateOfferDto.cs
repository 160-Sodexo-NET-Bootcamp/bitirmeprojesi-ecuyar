using Entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Offer
{
    public class UpdateOfferDto
    {
        public int Id { get; set; }
        public decimal OfferedPrice { get; set; }
        public decimal? PercentageOffered { get; set; } = null;
        public bool IsActiveOffer { get; set; }

        public virtual ShowProductDto Product { get; set; }
    }
}
