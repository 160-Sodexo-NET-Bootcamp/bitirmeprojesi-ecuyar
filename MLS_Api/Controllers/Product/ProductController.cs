using AutoMapper;
using Entity.Product;
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

namespace MLS_Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser_DataModel> userManager;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser_DataModel> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// Add a product. With auth.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApplicationResult<RegisterProductDto>> AddProduct([FromBody] RegisterProductDto productDto)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<RegisterProductDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                var productDataModel = mapper.Map<Product_DataModel>(productDto);

                productDataModel.SellerId = userDataModel.Result.Id;
                productDataModel.IsSold = false;

                productDataModel.CreatedDate = DateTime.UtcNow;
                productDataModel.CreatedBy = userDataModel.Result.Id;
                productDataModel.CreatedById = userDataModel.Result.Id;

                await unitOfWork.Products.Add(productDataModel, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<RegisterProductDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = productDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<RegisterProductDto>
                {
                    ErrorMessage = ErrorCodes.GeneralError,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }
        }

        /// <summary>
        /// Get all unsold products. For general page. No auth.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApplicationResult<IEnumerable<ShowProductDto>>> GetAll()
        {
            //i will allow all users can see the products, so it wont be authorized
            try
            {
                var products = await unitOfWork.Products.WhereWithoutUser(x => x.IsDeleted == false && x.IsSold == false);

                var productsDto = mapper.Map<IEnumerable<ShowProductDto>>(products.Result);

                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = productsDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Get the specified product that is not sold or deleted. For general page. No auth.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("product")]
        public async Task<ApplicationResult<IEnumerable<ShowProductDto>>> GetProductById([FromQuery][Required] int productId)
        {
            //i will allow all users can see the product, so it wont be authorized
            try
            {
                var product = await unitOfWork.Products.WhereWithoutUser(x => x.IsDeleted == false && x.IsSold == false && x.Id == productId);
                var productDto = mapper.Map<IEnumerable<ShowProductDto>>(product.Result);

                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = productDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Get user's all products that not deleted. This is for my account page. User get him/her products. With auth.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<ApplicationResult<IEnumerable<ShowProductDto>>> GetOwnedProducts([FromQuery][Required] string userId)
        {
            try
            {
                //get current user
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<IEnumerable<ShowProductDto>>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //there is no sold limit for this method. user may want to see sold products
                var products = await unitOfWork.Products.WhereWithoutUser(x => x.SellerId == userId && x.IsDeleted == false);
                var productsDto = mapper.Map<IEnumerable<ShowProductDto>>(products.Result);

                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = productsDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Get user's all products that not deleted or unsold. This is for general page. No auth.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        //[HttpGet("{companyId}")]
        [HttpGet("seller")]
        public async Task<ApplicationResult<IEnumerable<ShowProductDto>>> GetCompanyProducts([FromQuery][Required] string companyId)
        {
            try
            {
                //there is no sold limit for this method. user may want to see sold products
                var products = await unitOfWork.Products.WhereWithoutUser(x => x.SellerId == companyId && x.IsDeleted == false && x.IsSold == false);
                var productsDto = mapper.Map<IEnumerable<ShowProductDto>>(products.Result);

                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = productsDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<IEnumerable<ShowProductDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        private async Task<ApplicationUser_DataModel> GetCurrentUserAsync()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}
