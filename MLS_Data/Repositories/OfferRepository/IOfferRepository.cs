using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.OfferRepository
{
    public interface IOfferRepository : IGenericRepository<int, Offer_DataModel, ApplicationUser_DataModel>
    {
    }
}
