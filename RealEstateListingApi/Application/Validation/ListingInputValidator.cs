using FluentValidation;
using RealEstateListingApi.Application.DTOs;

namespace RealEstateListingApi.Application.Validation
{
    public class ListingInputValidator<T> : AbstractValidator<T> where T : ListingBaseDto
    {
        public ListingInputValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(50).WithMessage("Title must be 50 characters or less.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must be 1000 characters or less.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address must be 200 characters or less.")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}
