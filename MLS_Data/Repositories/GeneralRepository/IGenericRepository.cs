using MLS_Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MLS_Data.Repositories.GeneralRepository
{
    public interface IGenericRepository<Key, MainDto, ApplicationUser>
    {
        Task<ApplicationResult> Add(MainDto entity, ApplicationUser applicationUser);
        ApplicationResult Update(MainDto entity, ApplicationUser applicationUser);
        ApplicationResult UpdateGroup(List<MainDto> entities, ApplicationUser applicationUser);
        Task<ApplicationResult<MainDto>> GetById(Key id, ApplicationUser applicationUser);
        Task<ApplicationResult<IEnumerable<MainDto>>> GetAll(ApplicationUser applicationUser);
        ApplicationResult DeleteById(MainDto entity, ApplicationUser applicationUser);
        ApplicationResult<List<MainDto>> Where(Expression<Func<MainDto, bool>> predicate, ApplicationUser applicationUser);
    }
}
