using Entity.Identity;
using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;
using System;

namespace MLS_Data.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<Guid, User_DataModel, ApplicationUser>, IUserRepository
    {
        public UserRepository(MyLittleShopDbContext context) : base(context)
        {
        }
    }
}
