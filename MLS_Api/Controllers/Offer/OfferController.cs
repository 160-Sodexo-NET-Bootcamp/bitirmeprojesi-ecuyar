using AutoMapper;
using Entity.Offer;
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
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.Offer
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser_DataModel> userManager;

        public OfferController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser_DataModel> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// Create an offer for a product. With auth.
        /// </summary>
        /// <param name="makeOfferDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApplicationResult<MakeOfferDto>> CreateOffer([FromBody] MakeOfferDto makeOfferDto)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<MakeOfferDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //check if product is offerable. Normally this will be controlled on the frontend but I add another check layer.
                var product = await unitOfWork.Products.GetById(makeOfferDto.ProductId, userDataModel.Result);

                //do not make offer for deleted, sold or non-offerable product
                if (!product.Result.IsOfferable || product.Result.IsDeleted || product.Result.IsSold)
                {
                    return new ApplicationResult<MakeOfferDto>
                    {
                        ErrorMessage = ErrorCodes.ProductIsNotOfferable,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //map objects
                var offerDataModel = mapper.Map<Offer_DataModel>(makeOfferDto);

                //if user use percentage for calculation, offered price will be calculated always using percentage
                if (makeOfferDto.PercentageOffered != null && makeOfferDto.PercentageOffered > 0)
                {
                    offerDataModel.OfferedPrice = product.Result.Price - (product.Result.Price * (decimal)makeOfferDto.PercentageOffered / 100);
                }
                else
                {
                    offerDataModel.OfferedPrice = makeOfferDto.OfferedPrice;
                }

                //other infos
                offerDataModel.BuyerId = userDataModel.Result.Id;
                offerDataModel.SellerId = product.Result.SellerId;
                offerDataModel.ProductId = product.Result.Id;
                offerDataModel.CreatedDate = DateTime.UtcNow;
                offerDataModel.CreatedBy = userDataModel.Result.Id;
                offerDataModel.CreatedById = userDataModel.Result.Id;

                await unitOfWork.Offers.Add(offerDataModel, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<MakeOfferDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = makeOfferDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<MakeOfferDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Update an offer's price or active status. With auth.
        /// </summary>
        /// <param name="updateOfferDto"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize]
        public async Task<ApplicationResult<UpdateOfferDto>> UpdateOffer([FromBody] UpdateOfferDto updateOfferDto)
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<UpdateOfferDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //get offer
                var offerDataModel = await unitOfWork.Offers.GetByIdWithoutUser(updateOfferDto.Id);
                //get product
                var productDataModel = await unitOfWork.Products.GetByIdWithoutUser(offerDataModel.Result.ProductId);

                //do not update offer that is deleted or succeccfull
                if (offerDataModel.Result.IsSuccessfullOffer || offerDataModel.Result.IsDeleted)
                {
                    return new ApplicationResult<UpdateOfferDto>
                    {
                        ErrorMessage = ErrorCodes.OfferCanNotBeUpdated,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //check if offer is user's offer and not deleted
                if (offerDataModel.Result.BuyerId != userDataModel.Result.Id)
                {
                    return new ApplicationResult<UpdateOfferDto>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //user can stop offering
                offerDataModel.Result.IsActiveOffer = updateOfferDto.IsActiveOffer;

                //if user use percentage for calculation, offered price will be calculated always using percentage
                if (updateOfferDto.PercentageOffered != null && updateOfferDto.PercentageOffered > 0)
                {
                    offerDataModel.Result.OfferedPrice = productDataModel.Result.Price - (productDataModel.Result.Price * (decimal)updateOfferDto.PercentageOffered / 100);
                }
                else
                {
                    offerDataModel.Result.OfferedPrice = updateOfferDto.OfferedPrice;
                }

                //other infos
                offerDataModel.Result.ModifiedDate = DateTime.UtcNow;
                offerDataModel.Result.ModifiedBy = userDataModel.Result.Id;
                offerDataModel.Result.ModifiedById = userDataModel.Result.Id;

                unitOfWork.Offers.Update(offerDataModel.Result, userDataModel.Result);
                unitOfWork.Complete();

                return new ApplicationResult<UpdateOfferDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = updateOfferDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<UpdateOfferDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Get user's offers that he made. With auth.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost("getFromUser")]
        [Authorize]
        public async Task<ApplicationResult<IEnumerable<ShowOfferDto>>> GetOffersFromUser()
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<IEnumerable<ShowOfferDto>>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //get offers of user
                var offerDataModel = await unitOfWork.Offers.WhereWithoutUser(x => x.IsDeleted == false && x.BuyerId == userDataModel.Result.Id);

                var offersDto = mapper.Map<IEnumerable<ShowOfferDto>>(offerDataModel.Result);

                //get products of offers
                foreach (var item in offersDto)
                {
                    var productDataModel = await unitOfWork.Products.GetByIdWithoutUser(item.ProductId);

                    //if product is sold or deleted dont add
                    if (productDataModel.Result.IsSold || productDataModel.Result.IsDeleted)
                    {
                        continue;
                    }
                    var productDto = mapper.Map<ShowProductDto>(productDataModel);

                    item.Product = productDto;
                }

                return new ApplicationResult<IEnumerable<ShowOfferDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = offersDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<IEnumerable<ShowOfferDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        /// <summary>
        /// Get user's offers that are made to him. With auth.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost("getToUser")]
        [Authorize]
        public async Task<ApplicationResult<IEnumerable<ShowOfferDto>>> GetOffersToUser()
        {
            try
            {
                //get current useer
                var userDataModel = GetCurrentUserAsync();

                if (userDataModel.Result == null)
                {
                    return new ApplicationResult<IEnumerable<ShowOfferDto>>
                    {
                        ErrorMessage = ErrorCodes.UnauthorizedAccess,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                //get offers of user
                var offerDataModel = await unitOfWork.Offers.WhereWithoutUser(x => x.IsDeleted == false && x.SellerId == userDataModel.Result.Id);

                var offersDto = mapper.Map<IEnumerable<ShowOfferDto>>(offerDataModel.Result);

                //get products of offers
                foreach (var item in offersDto)
                {
                    var productDataModel = await unitOfWork.Products.GetByIdWithoutUser(item.ProductId);

                    //if product is sold or deleted dont add
                    if (productDataModel.Result.IsSold || productDataModel.Result.IsDeleted)
                    {
                        continue;
                    }
                    var productDto = mapper.Map<ShowProductDto>(productDataModel.Result);

                    item.Product = productDto;
                }

                return new ApplicationResult<IEnumerable<ShowOfferDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = offersDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<IEnumerable<ShowOfferDto>>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.GeneralError
                };
            }
        }

        [HttpPut("offerResult")]
        [Authorize]
        public async Task<ApplicationResult<UpdateOfferDto>> AcceptOrDeclineOffer([FromQuery][Required] int offerId, [Required] bool decision)
        {
            //get current useer
            var userDataModel = GetCurrentUserAsync();

            if (userDataModel.Result == null)
            {
                return new ApplicationResult<UpdateOfferDto>
                {
                    ErrorMessage = ErrorCodes.UnauthorizedAccess,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }

            //check if offer's seller is user
            var offerDataModel = await unitOfWork.Offers.GetByIdWithoutUser(offerId);

            //check if offer id is exists
            if (offerDataModel.Result == null)
            {
                return new ApplicationResult<UpdateOfferDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false,
                    ErrorMessage = ErrorCodes.RecordNotFound
                };
            }

            if (offerDataModel.Result.SellerId != userDataModel.Result.Id)
            {
                return new ApplicationResult<UpdateOfferDto>
                {
                    ErrorMessage = ErrorCodes.UnauthorizedAccess,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }

            if (decision)
            {
                offerDataModel.Result.IsSuccessfullOffer = true;
                var productDM = unitOfWork.Products.GetByIdWithoutUser(offerDataModel.Result.ProductId);
                productDM.Result.Result.IsSold = true;
                //TODO: add to sales table
            }
            else if (!decision)
            {
                //offer will be unseccessfull and it will be unwanted but will not be deleted. seller can not delete others' offers
                offerDataModel.Result.IsSuccessfullOffer = false;
                offerDataModel.Result.IsActiveOffer = false;
            }

            unitOfWork.Complete();

            return new ApplicationResult<UpdateOfferDto>
            {
                ResponseTime = DateTime.UtcNow,
                Succeeded = true,
                Result = null
            };

        }


        private async Task<ApplicationUser_DataModel> GetCurrentUserAsync()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}
