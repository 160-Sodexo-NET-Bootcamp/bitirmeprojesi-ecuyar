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
    public class GenericRepository<Key, MainDto, ApplicationUser> : IGenericRepository<Key, MainDto, ApplicationUser> where MainDto : class
    {
        protected MyLittleShopDbContext context;
        internal DbSet<MainDto> dbSet;

        public GenericRepository(MyLittleShopDbContext context)
        {
            this.context = context;
            dbSet = context.Set<MainDto>();
        }

        public virtual async Task<ApplicationResult> Add(MainDto entity, ApplicationUser applicationUser)
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
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public virtual ApplicationResult DeleteById(MainDto entity, ApplicationUser applicationUser)
        {
            try
            {
                if (entity == null)
                {
                    return new ApplicationResult
                    {
                        Succeeded = false,
                        ResponseTime = DateTime.UtcNow,
                        ErrorMessage = "Error Occured!"
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
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public virtual async Task<ApplicationResult<IEnumerable<MainDto>>> GetAll(ApplicationUser applicationUser)
        {

            var allitems = await dbSet.ToListAsync();

            return new ApplicationResult<IEnumerable<MainDto>>
            {
                Result = allitems,
                Succeeded = true,
                ResponseTime = DateTime.UtcNow
            };
        }

        public virtual async Task<ApplicationResult<MainDto>> GetById(Key id, ApplicationUser applicationUser)
        {
            var item = await dbSet.FindAsync(id);

            return new ApplicationResult<MainDto>
            {
                Result = item,
                Succeeded = true,
                ResponseTime = DateTime.UtcNow
            };
        }

        public virtual ApplicationResult Update(MainDto entity, ApplicationUser applicationUser)
        {
            try
            {
                if (entity == null)
                {
                    return new ApplicationResult
                    {
                        Succeeded = false,
                        ResponseTime = DateTime.UtcNow,
                        ErrorMessage = "Error Occured!"
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
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public virtual ApplicationResult UpdateGroup(List<MainDto> entities, ApplicationUser applicationUser)
        {
            try
            {
                if (entities == null)
                {
                    return new ApplicationResult
                    {
                        Succeeded = false,
                        ResponseTime = DateTime.UtcNow,
                        ErrorMessage = "Error Occured!"
                    };
                }

                dbSet.UpdateRange(entities);

                return new ApplicationResult
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = "Error Occured!"
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult
                {
                    Succeeded = false,
                    ResponseTime = DateTime.UtcNow,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public ApplicationResult<List<MainDto>> Where(Expression<Func<MainDto, bool>> predicate, ApplicationUser applicationUser)
        {
            List<MainDto> items = dbSet.Where(predicate).ToList();

            return new ApplicationResult<List<MainDto>>
            {
                Result = items,
                Succeeded = true,
                ResponseTime = DateTime.UtcNow
            };
        }
    }
}
