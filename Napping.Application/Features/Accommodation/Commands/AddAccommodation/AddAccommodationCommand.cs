using MediatR;

namespace Napping.Application.Features.Accommodation.Commands.AddAccommodation
{
    public record AddAccommodationCommand(
        string Name,
        double Latitude,
        double Longitude,
        string Address,
        decimal Rating) : IRequest<Guid>;
}
