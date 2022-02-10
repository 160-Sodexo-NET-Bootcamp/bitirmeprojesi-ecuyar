using AutoMapper;
using Entity.Category;
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
            CreateMap<User_DataModel, RegisterUserDto>();
            CreateMap<RegisterUserDto, User_DataModel>();

            //category mapping
            CreateMap<Category_DataModel, CategoryDto>();
            CreateMap<CategoryDto, Category_DataModel>();

            //product mapping
            CreateMap<Product_DataModel, RegisterProductDto>();
            CreateMap<RegisterProductDto, Product_DataModel>();
        }
    }
}
