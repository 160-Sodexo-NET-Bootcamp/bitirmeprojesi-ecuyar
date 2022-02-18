using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.OfferRepository
{
    public class OfferRepository : GenericRepository<int, Offer_DataModel, ApplicationUser_DataModel>, IOfferRepository
    {
        public OfferRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
