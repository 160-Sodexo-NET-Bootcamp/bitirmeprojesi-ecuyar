using Entity.Shared;
using FluentValidation;

namespace Entity.Category
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
        public int ParentCategoryId { get; set; }
    }

    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.CategoryName).NotNull();
            RuleFor(x => x.ParentCategoryId).NotNull();
        }
    }
}
