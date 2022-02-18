using AutoMapper;
using BackgroundWorker.Jobs;
using Core.EmailService;
using Entity.Shared;
using Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MLS_Data.DataModels;
using MLS_Data.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MLS_Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public UserManager<ApplicationUser_DataModel> userManager;
        private readonly SignInManager<ApplicationUser_DataModel> signInManager;

        public UserController(UserManager<ApplicationUser_DataModel> userManager, SignInManager<ApplicationUser_DataModel> signInManager, IMapper mapper, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// User Sign Up
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<ApplicationResult<GetUserDto>> Register([FromBody] RegisterUserDto registerUserDto)
        {
            //TODO : Error handling
            try
            {
                //create new user and map to data model
                var applicationUser_model = mapper.Map<ApplicationUser_DataModel>(registerUserDto);
                applicationUser_model.EmailConfirmed = false;
                applicationUser_model.TwoFactorEnabled = true;

                var result = await userManager.CreateAsync(applicationUser_model, registerUserDto.Password);

                if (!result.Succeeded)
                {
                    return new ApplicationResult<GetUserDto>
                    {
                        ErrorMessage = ErrorCodes.GeneralError,
                        ResponseTime = DateTime.UtcNow,
                        Succeeded = false
                    };
                }

                // TODO: Add mail confirmation
                //email confirmation api is absent but due to Url.Action can not create link, I don't make confirmation
                //var code = userManager.GenerateEmailConfirmationTokenAsync(applicationUser_model);
                //var link = Url.Action("confirm", "User", new { userId = applicationUser_model.Id, token = code.Result });

                var dynamicData = new Dictionary<string, string>
                {
                    {"firstname", applicationUser_model.FirstName },
                    {"lastname", applicationUser_model.LastName }
                };

                //create email message class
                WelcomeEmailSender emailSender = new(applicationUser_model.Email, dynamicData);

                //use background job
                SendWelcomeEmail job = new();
                //send email by triggering job
                Hangfire.BackgroundJob.Enqueue(() => job.WelcomeEmailSendJob(emailSender));

                var user = await userManager.FindByIdAsync(applicationUser_model.Id);
                var userDto = mapper.Map<GetUserDto>(user);

                return new ApplicationResult<GetUserDto>
                {
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = true,
                    Result = userDto
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<GetUserDto>
                {
                    ErrorMessage = ErrorCodes.GeneralError,
                    ResponseTime = DateTime.UtcNow,
                    Succeeded = false
                };
            }
        }

        /// <summary>
        /// User Log In
        /// </summary>
        /// /// <remarks>
        /// You can use 
        /// {
        /// "email": "uyar.enescan@gmail.com",
        /// "password": "enescanuyar"
        /// }
        /// </remarks>
        /// <param name="logInUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInUserDto logInUserDto)
        {
            try
            {
                //find corresponding user
                var dataModel = await userManager.FindByEmailAsync(logInUserDto.Email);

                //user is not exists
                if (dataModel == null)
                {
                    return BadRequest(ErrorCodes.WrongCredentials);
                }

                //try to log in
                var result = await signInManager.PasswordSignInAsync(dataModel.UserName, logInUserDto.Password, true, true);

                //if user is locked
                if (result.IsLockedOut)
                {
                    var localTime = Convert.ToDateTime(dataModel.LockoutEnd.ToString()).ToLocalTime();

                    return BadRequest($"{ErrorCodes.UserBlocked} Please wait until {localTime}.");
                }

                //if wrong password, increase failed attempt count
                if (!result.Succeeded)
                {
                    await userManager.AccessFailedAsync(dataModel);

                    return BadRequest(ErrorCodes.WrongCredentials);
                }

                //log in is successfull
                return Ok(CreateTokenResponse(dataModel));
            }
            catch (Exception)
            {
                //logger
                return BadRequest(ErrorCodes.GeneralError);
            }
        }

        /// <summary>
        /// Confirm user's email. [Not in use now.]
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery][Required] string userId, [Required] string token)
        {
            try
            {
                //find user
                var user = userManager.FindByIdAsync(userId).Result;

                //check confirmation
                var confirmResult = await userManager.ConfirmEmailAsync(user, token);

                if (!confirmResult.Succeeded)
                {
                    return BadRequest("Error on confirmation.");
                }

                return Ok("Your email is confirmed.");
            }
            catch (Exception)
            {
                //logger
                return BadRequest("Error on confirmation.");
            }
        }

        private string GetToken(ApplicationUser_DataModel user)
        {
            var dateTimeUtc = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, dateTimeUtc.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: dateTimeUtc,
                expires: dateTimeUtc.AddSeconds(configuration.GetValue<int>("Tokens:Lifetime")),
                audience: configuration.GetValue<string>("Tokens:Audience"),
                issuer: configuration.GetValue<string>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private JwtTokenResult CreateTokenResponse(ApplicationUser_DataModel user)
        {
            var token = GetToken(user);
            JwtTokenResult result = new()
            {
                AccessToken = token,
                UserId = user.Id
            };
            return result;
        }
    }


}
