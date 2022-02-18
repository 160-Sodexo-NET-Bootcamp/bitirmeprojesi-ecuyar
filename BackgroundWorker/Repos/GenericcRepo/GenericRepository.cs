using BackgroundWorker.Context;
using Microsoft.EntityFrameworkCore;
using MLS_Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackgroundWorker.GenericcRepo
{
    public class GenericRepository<MainDataModel> : IGenericRepository<MainDataModel> where MainDataModel : class
    {
        protected BackgroundWorkerDbContext context;
        internal DbSet<MainDataModel> dbSet;

        public GenericRepository(BackgroundWorkerDbContext context)
        {
            this.context = context;
            dbSet = context.Set<MainDataModel>();
        }

        public async void Add(MainDataModel entity)
        {
            await dbSet.AddAsync(entity);
        }

        public bool DeleteById(int id)
        {
            var entity = dbSet.Find(id);
            dbSet.Remove(entity);
            return true;
        }

        public async Task<IEnumerable<MainDataModel>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<MainDataModel> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public bool Update(MainDataModel entity)
        {
            dbSet.Update(entity);
            return true;
        }
        public async Task<IEnumerable<MainDataModel>> Where(Expression<Func<MainDataModel, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
    }
}
