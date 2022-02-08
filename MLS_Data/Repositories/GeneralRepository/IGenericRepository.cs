using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MLS_Data.Repositories.GeneralRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> Add(T entity);
        bool Update(T entity);
        bool UpdateGroup(List<T> entities);
        Task<T> GetById(int id);
        Task<T> GetByGuid(Guid guid);
        Task<IEnumerable<T>> GetAll();
        bool DeleteById(T entity);
        bool DeleteByGuid(T entity);
        List<T> Where(Expression<Func<T, bool>> predicate);
    }
}
