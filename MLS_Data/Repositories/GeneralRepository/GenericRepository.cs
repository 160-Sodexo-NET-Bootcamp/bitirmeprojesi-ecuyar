using Entity.Shared;
using Microsoft.EntityFrameworkCore;
using MLS_Data.Context;
using MLS_Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MLS_Data.Repositories.GeneralRepository
{
    public class GenericRepository<Key, MainDataModel, ApplicationUser_DataModel> : IGenericRepository<Key, MainDataModel, ApplicationUser_DataModel> where MainDataModel : class
    {
        protected MyLittleShopDbContext context;
        internal DbSet<MainDataModel> dbSet;

        public GenericRepository(MyLittleShopDbContext context)
        {
            this.context = context;
            dbSet = context.Set<MainDataModel>();
        }

        public virtual async Task<ApplicationResult> Add(MainDataModel entity, ApplicationUser_DataModel applicationUser)
        {
            try
            {
                await dbSet.AddAsync(entity);

                return new ApplicationResult
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual ApplicationResult DeleteById(MainDataModel entity, ApplicationUser_DataModel applicationUser)
        {
            try
            {
                if (entity == null)
                {
                    return new ApplicationResult
                    {
                        Succeeded = false,
                        ResponseTime = DateTime.UtcNow,
                        ErrorMessage = ErrorCodes.RecordNotFound
                    };
                }

                //dont delete use isdeleted property
                dbSet.Update(entity);

                return new ApplicationResult
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual async Task<ApplicationResult<IEnumerable<MainDataModel>>> GetAll(ApplicationUser_DataModel applicationUser)
        {
            try
            {
                if (applicationUser == null)
                {
                    return new ApplicationResult<IEnumerable<MainDataModel>>
                    {
                        Result = null,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false,
                        ErrorMessage = ErrorCodes.GeneralError
                    };
                }

                var allitems = await dbSet.ToListAsync();

                return new ApplicationResult<IEnumerable<MainDataModel>>
                {
                    Result = allitems,
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult<IEnumerable<MainDataModel>>
                {
                    Result = null,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual async Task<ApplicationResult<MainDataModel>> GetById(Key id, ApplicationUser_DataModel applicationUser)
        {
            try
            {
                var item = await dbSet.FindAsync(id);

                return new ApplicationResult<MainDataModel>
                {
                    Result = item,
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<MainDataModel>
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual async Task<ApplicationResult<MainDataModel>> GetByIdWithoutUser(Key id)
        {
            try
            {
                var item = await dbSet.FindAsync(id);

                return new ApplicationResult<MainDataModel>
                {
                    Result = item,
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<MainDataModel>
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual ApplicationResult Update(MainDataModel entity, ApplicationUser_DataModel applicationUser)
        {
            try
            {
                if (entity == null)
                {
                    return new ApplicationResult
                    {
                        Succeeded = false,
                        ResponseTime = DateTime.UtcNow,
                        ErrorMessage = ErrorCodes.RecordNotFound
                    };
                }

                dbSet.Update(entity);

                return new ApplicationResult
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual ApplicationResult UpdateGroup(List<MainDataModel> entities, ApplicationUser_DataModel applicationUser)
        {
            try
            {
                if (entities == null)
                {
                    return new ApplicationResult
                    {
                        Succeeded = false,
                        ResponseTime = DateTime.UtcNow,
                        ErrorMessage = ErrorCodes.RecordNotFound
                    };
                }

                dbSet.UpdateRange(entities);

                return new ApplicationResult
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual async Task<ApplicationResult<IEnumerable<MainDataModel>>> Where(Expression<Func<MainDataModel, bool>> predicate, ApplicationUser_DataModel applicationUser)
        {
            try
            {
                IEnumerable<MainDataModel> items = await dbSet.Where(predicate).ToListAsync();

                return new ApplicationResult<IEnumerable<MainDataModel>>
                {
                    Result = items,
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult<IEnumerable<MainDataModel>>
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        public virtual async Task<ApplicationResult<IEnumerable<MainDataModel>>> WhereWithoutUser(Expression<Func<MainDataModel, bool>> predicate)
        {
            try
            {
                IEnumerable<MainDataModel> items = await dbSet.Where(predicate).ToListAsync();

                return new ApplicationResult<IEnumerable<MainDataModel>>
                {
                    Result = items,
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult<IEnumerable<MainDataModel>>
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }
    }
}
