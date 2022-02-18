using AutoMapper;
using Entity.Category;
using Entity.Product;
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
using System.Threading.Tasks;

namespace MLS_Api.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser_DataModel> userManager;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser_DataModel> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get all subcategories. No auth.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApplicationResult<IEnumerable<GetCategoryDto>>> GetAllCategories()
        {
            try
            {
                //no need to authorize categories

                //main categories have a 0 value on parentcategoryid.
                //so we dont want main categories. we want subcategories
                var categories = await unitOfWork.Categories.WhereWithoutUser(x => x.ParentCategoryId != 0 && x.IsDeleted == false);

                var categoriesDto = mapper.Map<IEnumerable<GetCategoryDto>>(categories.Result);

                return new ApplicationResult<IEnumerable<GetCategoryDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = categoriesDto
                };
            }
            catch (Exception)
            {
                //logger
                return new ApplicationResult<IEnumerable<GetCategoryDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Add a category. With auth.
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns>categoryDto</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApplicationResult<CreateCategoryDto>> AddCategory([FromBody] CreateCategoryDto categoryDto)
        {
            //get current useer
            var userDataModel = GetCurrentUserAsync();

            if (userDataModel.Result == null)
            {
                return new ApplicationResult<CreateCategoryDto>
                {
                    ErrorMessage = ErrorCodes.UnauthorizedAccess,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }

            var categoryDataModel = mapper.Map<Category_DataModel>(categoryDto);

            //add other infos
            categoryDataModel.CreatedById = userDataModel.Result.Id;
            categoryDataModel.CreatedBy = userDataModel.Result.Id;

            await unitOfWork.Categories.Add(categoryDataModel, userDataModel.Result);
            unitOfWork.Complete();

            return new ApplicationResult<CreateCategoryDto>
            {
                Succeeded = true,
                ResponseTime = DateTime.UtcNow,
                Result = categoryDto
            };
        }

        /// <summary>
        /// Update a category. With auth.
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<ApplicationResult<UpdateCategoryDto>> UpdateCategory([FromBody] UpdateCategoryDto categoryDto)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<UpdateCategoryDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                var existCategory = await unitOfWork.Categories.GetById(categoryDto.Id, userDataModel.Result);
                existCategory.Result.CategoryName = categoryDto.CategoryName;

                //add other infos
                existCategory.Result.ModifiedBy = userDataModel.Result.Id;
                existCategory.Result.ModifiedById = userDataModel.Result.Id;
                existCategory.Result.ModifiedDate = DateTime.UtcNow;

                unitOfWork.Categories.Update(existCategory.Result, userDataModel.Result);
                unitOfWork.Complete();

                //map the object and return tu user
                var newCategory = mapper.Map<UpdateCategoryDto>(categoryDto);

                return new ApplicationResult<UpdateCategoryDto>
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    Result = newCategory
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<UpdateCategoryDto>
                {
                    ErrorMessage = ErrorCodes.GeneralError,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }
        }

        /// <summary>
        /// Get a category with its products. No auth.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("{categoryId}")]
        public async Task<ApplicationResult<GetCategoryWithProductsDto>> ShowCategory([FromRoute] int categoryId)
        {
            try
            {
                var categoryModel = await unitOfWork.Categories.GetByIdWithoutUser(categoryId);
                var productsModel = await unitOfWork.Products.WhereWithoutUser(x => x.CategoryId == categoryId && x.IsDeleted == false);

                var categoryDto = mapper.Map<GetCategoryWithProductsDto>(categoryModel.Result);
                var productsDto = mapper.Map<IEnumerable<ShowProductDto>>(productsModel.Result);

                //TODO: brandname ve color name yok, eklenebilir
                //TODO: brand, color, category redis de tutulacak
                categoryDto.Products = productsDto;

                return new ApplicationResult<GetCategoryWithProductsDto>
                {
                    Succeeded = true,
                    ResponseTime = DateTime.UtcNow,
                    Result = categoryDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<GetCategoryWithProductsDto>
                {
                    ErrorMessage = ErrorCodes.GeneralError,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }
        }






        //[HttpGet("products")]
        //public IActionResult GetCategoryProducts([FromQuery] int categoryId)
        //{
        //    //user will get chosen category's unsold products
        //    var products = unitOfWork.Products.Where(x => x.CategoryId == categoryId && x.IsSold == false);
        //    var productsDto = mapper.Map<List<Product_DataModel>, List<ShowProductDto>>(products);

        //    return Ok(productsDto);
        //}

        private async Task<ApplicationUser_DataModel> GetCurrentUserAsync()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}
