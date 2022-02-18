using Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MLS_Data.Repositories.GeneralRepository
{
    public interface IGenericRepository<Key, MainDataModel, ApplicationUser_DataModel>
    {
        Task<ApplicationResult> Add(MainDataModel entity, ApplicationUser_DataModel applicationUser);
        ApplicationResult Update(MainDataModel entity, ApplicationUser_DataModel applicationUser);
        ApplicationResult UpdateGroup(List<MainDataModel> entities, ApplicationUser_DataModel applicationUser);
        Task<ApplicationResult<MainDataModel>> GetById(Key id, ApplicationUser_DataModel applicationUser);
        Task<ApplicationResult<MainDataModel>> GetByIdWithoutUser(Key id);
        Task<ApplicationResult<IEnumerable<MainDataModel>>> GetAll(ApplicationUser_DataModel applicationUser);
        ApplicationResult DeleteById(MainDataModel entity, ApplicationUser_DataModel applicationUser);
        Task<ApplicationResult<IEnumerable<MainDataModel>>> Where(Expression<Func<MainDataModel, bool>> predicate, ApplicationUser_DataModel applicationUser);
        Task<ApplicationResult<IEnumerable<MainDataModel>>> WhereWithoutUser(Expression<Func<MainDataModel, bool>> predicate);
    }
}
