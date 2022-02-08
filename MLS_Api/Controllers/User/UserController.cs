using AutoMapper;
using Core.Hash;
using Entity.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MLS_Data.DataModels;
using MLS_Data.UoW;
using System;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        //Register user
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
        {
            try
            {
                //check email and username if it is exists
                //if exists send and warning to user

                var newUser = userDto;

                var hashedPassword = HashUserPassword.DoHash(userDto.UserId.ToString(), userDto.Password, userDto.Token.ToString());
                newUser.Password = hashedPassword;

                User_DataModel user_DataModel = mapper.Map<User_DataModel>(newUser);

                //process will return true if there is no error
                var result = await unitOfWork.Users.Add(user_DataModel);
                unitOfWork.Complete();

                if (result != true)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception)
            {
                //logger
                return BadRequest();
            }
        }

        //User login check
        [HttpPost]
        [Route("LogIn")]
        public IActionResult CheckUserCredentials([FromBody] LogInUserDto user)
        {
            try
            {
                //find the corresponding user
                var existsUser = unitOfWork.Users.Where(x => x.Username == user.Username || x.Email == user.Email);

                if (existsUser == null)
                {
                    return BadRequest();
                }

                var localHashedPassword = HashUserPassword.DoHash(existsUser[0].UserId.ToString(), user.Password, existsUser[0].Token.ToString());

                //check hashed passwords are matched
                if (localHashedPassword != existsUser[0].Password)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception)
            {
                //logger
                return BadRequest();
            }
        }
    }
}
