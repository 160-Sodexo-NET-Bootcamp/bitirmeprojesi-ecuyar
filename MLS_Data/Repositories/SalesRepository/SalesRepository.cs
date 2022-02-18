using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.SalesRepository
{
    public class SalesRepository : GenericRepository<int, Sales_DataModel, ApplicationUser_DataModel>, ISalesRepository
    {
        public SalesRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
