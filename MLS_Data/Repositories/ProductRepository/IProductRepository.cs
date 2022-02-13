using Entity.Identity;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<int, Product_DataModel, ApplicationUser>
    {
    }
}
