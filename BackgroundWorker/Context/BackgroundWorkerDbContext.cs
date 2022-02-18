using BackgroundWorker.DataModels;
using Microsoft.EntityFrameworkCore;

namespace BackgroundWorker.Context
{
    public class BackgroundWorkerDbContext : DbContext, IBackgroundWorkerDbContext
    {
        public BackgroundWorkerDbContext(DbContextOptions<BackgroundWorkerDbContext> options) : base(options)
        {

        }

        public DbSet<Email> Emails { get; set; }
    }
}
