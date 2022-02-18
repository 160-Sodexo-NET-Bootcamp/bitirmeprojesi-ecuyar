using AutoMapper;
using Entity.Category;
using Entity.Identity;
using Entity.Offer;
using Entity.Product;
using Entity.User;
using MLS_Data.DataModels;

namespace Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //user mapping
            CreateMap<ApplicationUser_DataModel, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUser_DataModel>();

            CreateMap<LogInUserDto, ApplicationUser_DataModel>();

            CreateMap<ApplicationUser_DataModel, RegisterUserDto>();
            CreateMap<RegisterUserDto, ApplicationUser_DataModel>();

            CreateMap<ApplicationUser_DataModel, GetUserDto>();


            //category mapping
            CreateMap<Category_DataModel, CreateCategoryDto>();
            CreateMap<CreateCategoryDto, Category_DataModel>();

            CreateMap<GetCategoryDto, Category_DataModel>();
            CreateMap<Category_DataModel, GetCategoryDto>();

            CreateMap<Category_DataModel, GetCategoryWithProductsDto>();

            //product mapping
            CreateMap<Product_DataModel, RegisterProductDto>();
            CreateMap<RegisterProductDto, Product_DataModel>();

            CreateMap<Product_DataModel, ShowProductDto>();
            CreateMap<ShowProductDto, Product_DataModel>();

            //offer mapping
            CreateMap<MakeOfferDto, Offer_DataModel>();
            CreateMap<Offer_DataModel, MakeOfferDto>();

            CreateMap<UpdateOfferDto, Offer_DataModel>();
            CreateMap<Offer_DataModel, UpdateOfferDto>();

            CreateMap<ShowOfferDto, Offer_DataModel>();
            CreateMap<Offer_DataModel, ShowOfferDto>();
        }
    }
}
