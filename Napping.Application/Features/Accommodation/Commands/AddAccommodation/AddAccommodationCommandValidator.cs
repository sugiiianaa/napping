using FluentValidation;

namespace Napping.Application.Features.Accommodation.Commands.AddAccommodation
{
    public class AddAccommodationCommandValidator : AbstractValidator<AddAccommodationCommand>
    {
        public AddAccommodationCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Rating).InclusiveBetween(0, 5);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        }
    }
}
