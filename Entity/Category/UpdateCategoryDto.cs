using FluentValidation;

namespace Entity.Category
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class UpdateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.CategoryName).NotNull();
        }
    }
}
