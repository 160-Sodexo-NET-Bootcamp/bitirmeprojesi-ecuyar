using BackgroundWorker.Context;
using BackgroundWorker.DataModels;
using BackgroundWorker.GenericcRepo;

namespace BackgroundWorker.Repos.EmailRepo
{
    public class EmailRepository : GenericRepository<Email>, IEmailRepository
    {
        public EmailRepository(BackgroundWorkerDbContext context) : base(context)
        {

        }
    }
}
