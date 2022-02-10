using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.CategoryRepository
{
    public class CategoryRepository : GenericRepository<Category_DataModel>, ICategoryRepository
    {
        public CategoryRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
