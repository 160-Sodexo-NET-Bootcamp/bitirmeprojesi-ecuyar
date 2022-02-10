using FluentValidation;
using System;

namespace Entity.Product
{
    public class RegisterProductDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public short BrandId { get; set; }
        public short ColorId { get; set; }
        public int CategoryId { get; set; }
        public Guid SellerId { get; set; }
        public decimal Price { get; set; }
        public string PicturePath { get; set; }
        public bool IsSold { get; set; }
        public bool UsageStatus { get; set; }

        //dafeult values if they are not given
        public bool IsOfferable { get; set; } = false;
    }

    public class RegisterProductDtoValidator : AbstractValidator<RegisterProductDto>
    {
        public RegisterProductDtoValidator()
        {
            RuleFor(x => x.ProductName).NotNull().MaximumLength(100);
            RuleFor(x => x.Description).NotNull().MaximumLength(500);
            RuleFor(x => x.CategoryId).NotNull();
            RuleFor(x => x.UsageStatus).NotNull();
            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.SellerId).NotNull();
        }
    }
}
