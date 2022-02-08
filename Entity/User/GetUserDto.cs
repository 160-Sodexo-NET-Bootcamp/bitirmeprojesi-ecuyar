using System;
using System.Collections.Generic;

namespace Entity.User
{
    public class GetUserDto
    {
        public Guid UserId { get; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public short TryCount { get; set; }
        public Guid Token { get; }
    }
}
