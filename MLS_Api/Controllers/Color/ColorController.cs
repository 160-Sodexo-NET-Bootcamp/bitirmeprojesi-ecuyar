using AutoMapper;

using Entity.Color;
using Entity.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MLS_Data.DataModels;
using MLS_Data.Shared;
using MLS_Data.UoW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.Color
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser_DataModel> userManager;

        public ColorController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser_DataModel> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get all colors. There should be role-based authorization.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApplicationResult<IEnumerable<GetColorDto>>> GetAllColors()
        {
            try
            {
                var colors = await unitOfWork.Colors.WhereWithoutUser(x => x.IsDeleted == false);

                var brandsDto = mapper.Map<IEnumerable<GetColorDto>>(colors.Result);

                return new ApplicationResult<IEnumerable<GetColorDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = brandsDto
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult<IEnumerable<GetColorDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Add a color. There should be role-based authorization.
        /// </summary>
        /// <param name="getColorDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApplicationResult<GetColorDto>> AddBrand([FromBody] GetColorDto getColorDto)
        {
            //get current useer
            var userDataModel = GetCurrentUserAsync();

            if (userDataModel.Result == null)
            {
                return new ApplicationResult<GetColorDto>
                {
                    ErrorMessage = ErrorCodes.UnauthorizedAccess,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }

            var colorDataModel = mapper.Map<Color_DataModel>(getColorDto);

            //add other infos
            colorDataModel.CreatedById = userDataModel.Result.Id;
            colorDataModel.CreatedBy = userDataModel.Result.Id;

            await unitOfWork.Colors.Add(colorDataModel, userDataModel.Result);
            unitOfWork.Complete();

            return new ApplicationResult<GetColorDto>
            {
                Succeeded = true,
                ResponseTime = DateTime.UtcNow,
                Result = getColorDto
            };
        }

        /// <summary>
        /// Update a color. There should be role-based authorization.
        /// </summary>
        /// <param name="getColorDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<ApplicationResult<GetColorDto>> UpdateCategory([FromBody] GetColorDto getColorDto)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<GetColorDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                var existColor = await unitOfWork.Colors.GetById(getColorDto.Id, userDataModel.Result);
                existColor.Result.ColorName = getColorDto.ColorName;

                //add other infos
                existColor.Result.ModifiedBy = userDataModel.Result.Id;
                existColor.Result.ModifiedById = userDataModel.Result.Id;
                existColor.Result.ModifiedDate = DateTime.UtcNow;

                unitOfWork.Colors.Update(existColor.Result, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<GetColorDto>
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    Result = getColorDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<GetColorDto>
                {
                    ErrorMessage = ErrorCodes.GeneralError,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }
        }

        /// <summary>
        /// Delete a color by updating isdeleted = true. There should be role-based authorization.
        /// </summary>
        /// <param name="colorId"></param>
        /// <returns></returns>
        [HttpPut("delete")]
        [Authorize]
        public async Task<ApplicationResult<GetColorDto>> UpdateBrand([FromQuery][Required] int colorId)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<GetColorDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                var existColor = await unitOfWork.Colors.GetById(colorId, userDataModel.Result);
                existColor.Result.IsDeleted = true;

                //add other infos
                existColor.Result.ModifiedBy = userDataModel.Result.Id;
                existColor.Result.ModifiedById = userDataModel.Result.Id;
                existColor.Result.ModifiedDate = DateTime.UtcNow;

                unitOfWork.Colors.Update(existColor.Result, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<GetColorDto>
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    Result = null
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<GetColorDto>
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
