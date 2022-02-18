using AutoMapper;
using Entity.Product;
using Entity.Sale;
using Entity.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MLS_Data.DataModels;
using MLS_Data.Shared;
using MLS_Data.UoW;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.Sale
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser_DataModel> userManager;

        public SaleController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser_DataModel> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// Buy a product with product id. With auth.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("buy")]
        [Authorize]
        public async Task<ApplicationResult<bool>> CreateSale([FromQuery][Required] int productId)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<bool>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //check if product is not sale
                var product = await unitOfWork.Products.GetById(productId, userDataModel.Result);

                if (product.Result.IsDeleted == true && product.Result.IsSold == true)
                {
                    return new ApplicationResult<bool>
                    {
                        ErrorMessage = ErrorCodes.RecordNotFound,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //create sale object
                Sales_DataModel sales_DataModel = new()
                {
                    Price = product.Result.Price,
                    BuyerId = userDataModel.Result.Id,
                    SellerId = product.Result.SellerId,
                    CreatedBy = userDataModel.Result.Id,
                    CreatedById = userDataModel.Result.Id,
                    CreatedDate = DateTime.UtcNow,
                    ProductId = productId
                };

                //make product sold
                product.Result.IsSold = true;
                unitOfWork.Products.Update(product.Result, userDataModel.Result);

                //add sale to the table
                await unitOfWork.Sales.Add(sales_DataModel, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<bool>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = true
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<bool>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Get a sale. With auth.
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize]
        public async Task<ApplicationResult<GetSaleDto>> GetSale([FromQuery][Required] int saleId)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<GetSaleDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //check if sale is user's sale
                var saleDM = await unitOfWork.Sales.GetByIdWithoutUser(saleId);
                var productDM = await unitOfWork.Products.GetByIdWithoutUser(saleDM.Result.ProductId);

                if (saleDM.Result.BuyerId != userDataModel.Result.Id)
                {
                    return new ApplicationResult<GetSaleDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                var saleDto = mapper.Map<GetSaleDto>(saleDM.Result);
                var productDto = mapper.Map<ShowProductDto>(productDM.Result);

                saleDto.Product = productDto;

                return new ApplicationResult<GetSaleDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = saleDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<GetSaleDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Cancel a sale request. With auth.
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [HttpGet("cancel")]
        [Authorize]
        public async Task<ApplicationResult<bool>> CancelSale([FromQuery][Required] int saleId)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<bool>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //check if sale is user's sale
                var saleDM = await unitOfWork.Sales.GetByIdWithoutUser(saleId);
                var productDM = await unitOfWork.Products.GetByIdWithoutUser(saleDM.Result.ProductId);

                if (saleDM.Result.BuyerId != userDataModel.Result.Id)
                {
                    return new ApplicationResult<bool>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                saleDM.Result.IsDeleted = true;

                saleDM.Result.ModifiedById = userDataModel.Result.Id;
                saleDM.Result.ModifiedBy = userDataModel.Result.Id;
                saleDM.Result.ModifiedDate = DateTime.UtcNow;

                //delete sale
                unitOfWork.Sales.Update(saleDM.Result, userDataModel.Result);

                //update product like unsold
                productDM.Result.IsSold = false;
                unitOfWork.Products.Update(productDM.Result, userDataModel.Result);

                unitOfWork.Complete();

                return new ApplicationResult<bool>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = true
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<bool>
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
