using BackgroundWorker.Repos.EmailRepo;

namespace BackgroundWorker.UoW
{
    public interface IUnitOfWork
    {
        IEmailRepository Emails { get; }
        int Complete();
    }
}
