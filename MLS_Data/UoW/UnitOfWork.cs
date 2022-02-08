using MLS_Data.Context;

namespace MLS_Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyLittleShopDbContext context;

        public UnitOfWork(MyLittleShopDbContext context)
        {
            this.context = context;
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
