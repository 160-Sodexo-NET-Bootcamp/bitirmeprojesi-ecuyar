using FluentValidation;

namespace Entity.User
{
    public class LogInUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LogInUserDtoValidator : AbstractValidator<LogInUserDto>
    {
        public LogInUserDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Length(8, 20);
        }
    }
}
