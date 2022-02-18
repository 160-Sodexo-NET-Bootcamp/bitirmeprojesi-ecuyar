using Entity.Identity;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.CategoryRepository
{
    public interface ICategoryRepository : IGenericRepository<int, Category_DataModel, ApplicationUser_DataModel>
    {
    }
}
