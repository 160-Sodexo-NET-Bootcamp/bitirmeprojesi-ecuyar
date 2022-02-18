using Entity.Product;

namespace Entity.Offer
{
    public class ShowOfferDto
    {
        public int Id { get; set; }
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
        public decimal OfferedPrice { get; set; }
        public int ProductId { get; set; }
        public bool IsActiveOffer { get; set; }
        public bool IsSuccessfullOffer { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ShowProductDto Product { get; set; }
    }
}
