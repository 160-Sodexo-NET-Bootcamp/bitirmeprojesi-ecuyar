using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.UserRepository
{
    public interface IUserRepository : IGenericRepository<User_DataModel>
    {
    }
}
