using FluentValidation;
using System;

namespace Entity.User
{
    public class RegisterUserDto
    {
        private Guid _UserId;
        private Guid _Token;

        public RegisterUserDto()
        {
            _UserId = Guid.NewGuid();
            _Token = Guid.NewGuid();
        }

        public Guid UserId { get => _UserId; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid Token { get => _Token; }
    }

    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Length(8, 20);
        }
    }
}
