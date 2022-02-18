using Entity.Shared;

namespace MLS_Data.DataModels
{
    public class Offer_DataModel : BaseEntity
    {
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
        public decimal OfferedPrice { get; set; }
        public int ProductId { get; set; }
        public bool IsActiveOffer { get; set; }
        public bool IsSuccessfullOffer { get; set; }
    }
}
