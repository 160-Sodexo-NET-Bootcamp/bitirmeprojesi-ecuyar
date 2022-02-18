using AutoMapper;
using Entity.Brand;
using Entity.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MLS_Data.DataModels;
using MLS_Data.Shared;
using MLS_Data.UoW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.Brand
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser_DataModel> userManager;

        public BrandController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser_DataModel> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get all brands. There should be role-based authorization.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApplicationResult<IEnumerable<GetBrandDto>>> GetAllColors()
        {
            try
            {
                var brands = await unitOfWork.Brands.WhereWithoutUser(x => x.IsDeleted == false);

                var brandsDto = mapper.Map<IEnumerable<GetBrandDto>>(brands.Result);

                return new ApplicationResult<IEnumerable<GetBrandDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = brandsDto
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult<IEnumerable<GetBrandDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Add a brand. There should be role-based authorization.
        /// </summary>
        /// <param name="getBrandDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApplicationResult<GetBrandDto>> AddBrand([FromBody] GetBrandDto getBrandDto)
        {
            //get current useer
            var userDataModel = GetCurrentUserAsync();

            if (userDataModel.Result == null)
            {
                return new ApplicationResult<GetBrandDto>
                {
                    ErrorMessage = ErrorCodes.UnauthorizedAccess,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }

            var brandDataModel = mapper.Map<Brand_DataModel>(getBrandDto);

            //add other infos
            brandDataModel.CreatedById = userDataModel.Result.Id;
            brandDataModel.CreatedBy = userDataModel.Result.Id;

            await unitOfWork.Brands.Add(brandDataModel, userDataModel.Result);
            unitOfWork.Complete();

            return new ApplicationResult<GetBrandDto>
            {
                Succeeded = true,
                ResponseTime = DateTime.UtcNow,
                Result = getBrandDto
            };
        }

        /// <summary>
        /// Update a brand. There should be role-based authorization.
        /// </summary>
        /// <param name="getBrandDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<ApplicationResult<GetBrandDto>> UpdateCategory([FromBody] GetBrandDto getBrandDto)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<GetBrandDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                var existBrand = await unitOfWork.Brands.GetById(getBrandDto.Id, userDataModel.Result);
                existBrand.Result.BrandName = getBrandDto.BrandName;

                //add other infos
                existBrand.Result.ModifiedBy = userDataModel.Result.Id;
                existBrand.Result.ModifiedById = userDataModel.Result.Id;
                existBrand.Result.ModifiedDate = DateTime.UtcNow;

                unitOfWork.Brands.Update(existBrand.Result, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<GetBrandDto>
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    Result = getBrandDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<GetBrandDto>
                {
                    ErrorMessage = ErrorCodes.GeneralError,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }
        }

        /// <summary>
        /// Delete a brand by updating isdeleted = true. There should be role-based authorization.
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [HttpPut("delete")]
        [Authorize]
        public async Task<ApplicationResult<GetBrandDto>> UpdateBrand([FromQuery][Required] int brandId)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<GetBrandDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                var existBrand = await unitOfWork.Brands.GetById(brandId, userDataModel.Result);
                existBrand.Result.IsDeleted = true;

                //add other infos
                existBrand.Result.ModifiedBy = userDataModel.Result.Id;
                existBrand.Result.ModifiedById = userDataModel.Result.Id;
                existBrand.Result.ModifiedDate = DateTime.UtcNow;

                unitOfWork.Brands.Update(existBrand.Result, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<GetBrandDto>
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    Result = null
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<GetBrandDto>
                {
                    ErrorMessage = ErrorCodes.GeneralError,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }
        }

        private async Task<ApplicationUser_DataModel> GetCurrentUserAsync()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}
