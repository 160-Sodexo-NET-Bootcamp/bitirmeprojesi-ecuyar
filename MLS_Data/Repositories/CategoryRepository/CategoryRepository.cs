using Entity.Identity;
using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.CategoryRepository
{
    public class CategoryRepository : GenericRepository<int, Category_DataModel, ApplicationUser_DataModel>, ICategoryRepository
    {
        public CategoryRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
