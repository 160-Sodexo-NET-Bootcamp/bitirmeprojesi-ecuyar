using BackgroundWorker.DataModels;
using Microsoft.EntityFrameworkCore;

namespace BackgroundWorker.Context
{
    public interface IBackgroundWorkerDbContext
    {
        DbSet<Email> Emails { get; set; }
    }
}
