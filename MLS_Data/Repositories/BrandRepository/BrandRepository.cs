using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.BrandRepository
{
    public class BrandRepository : GenericRepository<Brand_DataModel>, IBrandRepository
    {
        public BrandRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
