using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MLS_Data.DataModels
{
    public class User_DataModel : IdentityUser
    {
        //[Key]
        //public Guid UserId { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        //public Guid Token { get; set; }
        //public short TryCount { get; set; }
    }
}
