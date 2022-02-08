using MLS_Data.Repositories.UserRepository;

namespace MLS_Data.UoW
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        int Complete();
    }
}
