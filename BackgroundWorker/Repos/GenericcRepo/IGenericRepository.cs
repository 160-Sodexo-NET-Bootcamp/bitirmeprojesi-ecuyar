using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackgroundWorker.GenericcRepo
{
    public interface IGenericRepository<MainDataModel>
    {
        void Add(MainDataModel entity);
        bool Update(MainDataModel entity);
        Task<MainDataModel> GetById(int id);
        Task<IEnumerable<MainDataModel>> GetAll();
        bool DeleteById(int id);
        Task<IEnumerable<MainDataModel>> Where(Expression<Func<MainDataModel, bool>> predicate);
    }
}
