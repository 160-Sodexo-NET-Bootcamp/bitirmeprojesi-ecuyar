using Entity.Identity;
using MLS_Data.DataModels;
using MLS_Data.Repositories.GeneralRepository;
using System;

namespace MLS_Data.Repositories.UserRepository
{
    public interface IUserRepository : IGenericRepository<Guid, User_DataModel, ApplicationUser>
    {
    }
}
