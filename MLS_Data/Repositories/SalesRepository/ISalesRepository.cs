using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.SalesRepository
{
    public interface ISalesRepository : IGenericRepository<int, Sales_DataModel, ApplicationUser_DataModel>
    {
    }
}
