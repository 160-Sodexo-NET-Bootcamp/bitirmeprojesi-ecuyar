using Microsoft.EntityFrameworkCore;
using MLS_Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MLS_Data.Repositories.GeneralRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected MyLittleShopDbContext context;
        internal DbSet<T> dbSet;

        public GenericRepository(MyLittleShopDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception)
            {
                //logger
                return false;
            }
        }

        public bool DeleteByGuid(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                }

                dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                //logger
                return false;
            }
        }

        public bool DeleteById(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                }

                dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                //logger
                return false;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByGuid(Guid guid)
        {
            return await dbSet.FindAsync(guid);
        }

        public virtual async Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public bool Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                }

                dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                //logger
                return false;
            }
        }

        public bool UpdateGroup(List<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    return false;
                }

                dbSet.UpdateRange(entities);
                return true;
            }
            catch (Exception)
            {
                //logger
                return false;
            }
        }

        public List<T> Where(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).ToList();
        }
    }
}
