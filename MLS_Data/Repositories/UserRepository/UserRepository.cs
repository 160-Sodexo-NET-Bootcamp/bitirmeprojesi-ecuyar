using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;

namespace MLS_Data.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<User_DataModel>, IUserRepository
    {
        public UserRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
