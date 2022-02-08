using AutoMapper;
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
        }
    }
}
