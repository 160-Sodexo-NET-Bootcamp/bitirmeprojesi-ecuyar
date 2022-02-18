using Microsoft.AspNetCore.Identity;
using System;

namespace MLS_Data.DataModels
{
    public class ApplicationUser_DataModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
