using MediatR;
using Napping.Domain.Entities;
using Napping.Domain.Repositories;

namespace Napping.Application.Features.Accommodation.Commands.AddAccommodation
{
    public class AddAccommodationCommandHandler : IRequestHandler<AddAccommodationCommand, Guid>
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public AddAccommodationCommandHandler(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }

        public async Task<Guid> Handle(AddAccommodationCommand request, CancellationToken cancellationToken)
        {
            var accommodationEntity = AccommodationEntity.Create(
                name: request.Name,
                latitude: request.Latitude,
                longitude: request.Longitude,
                address: request.Address,
                rating: request.Rating);


            await _accommodationRepository.AddAsync(accommodationEntity);

            await _accommodationRepository.SaveChangesAsync(cancellationToken);

            return accommodationEntity.Id;

        }
    }
}
