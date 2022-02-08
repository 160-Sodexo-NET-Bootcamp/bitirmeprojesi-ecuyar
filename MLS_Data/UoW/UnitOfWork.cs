using MLS_Data.Context;
using MLS_Data.Repositories.UserRepository;

namespace MLS_Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyLittleShopDbContext context;
        public IUserRepository Users { get; private set; }

        public UnitOfWork(MyLittleShopDbContext context)
        {
            this.context = context;

            Users = new UserRepository(context);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
