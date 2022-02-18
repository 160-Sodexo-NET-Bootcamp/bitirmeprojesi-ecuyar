using Entity.Identity;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.BrandRepository
{
    public interface IBrandRepository : IGenericRepository<int, Brand_DataModel, ApplicationUser_DataModel>
    {
    }
}
