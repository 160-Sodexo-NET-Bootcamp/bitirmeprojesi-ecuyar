using Entity.Product;
using FluentValidation;

namespace Entity.Offer
{
    public class MakeOfferDto
    {
        public int ProductId { get; set; }
        public decimal OfferedPrice { get; set; } = 0;
        public decimal? PercentageOffered { get; set; } = null;
        public bool IsActiveOffer { get; private set; } = true;
        public bool IsSuccessfullOffer { get; private set; } = false;
    }

    public class MakeOfferDtoValidator : AbstractValidator<MakeOfferDto>
    {
        public MakeOfferDtoValidator()
        {
            RuleFor(x => x.ProductId).NotNull();
        }
    }
}
