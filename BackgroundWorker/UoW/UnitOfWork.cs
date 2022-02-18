using BackgroundWorker.Context;
using BackgroundWorker.Repos.EmailRepo;

namespace BackgroundWorker.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BackgroundWorkerDbContext context;

        public IEmailRepository Emails { get; private set; }

        public UnitOfWork(BackgroundWorkerDbContext context)
        {
            this.context = context;

            Emails = new EmailRepository(context);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
