using FluentValidation;
using System;

namespace Entity.Product
{
    public class RegisterProductDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public string PicturePath { get; set; }
        public bool IsSold { get; set; }

        //dafeult values if they are not given
        public bool IsOfferable { get; set; } = false;
        public bool UsageStatus { get; set; } = false;
    }

    public class ProductDtoValidator : AbstractValidator<RegisterProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.UsageStatus).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
