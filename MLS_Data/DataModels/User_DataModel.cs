using System;

namespace MLS_Data.DataModels
{
    public class User_DataModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
        public short TryCount { get; set; }
    }
}
