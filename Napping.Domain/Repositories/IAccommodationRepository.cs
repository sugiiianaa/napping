using Napping.Domain.Entities;

namespace Napping.Domain.Repositories
{
    public interface IAccommodationRepository
    {
        Task AddAsync(AccommodationEntity accommodationEntity);
        Task UpdateAsyn(AccommodationEntity accommodationEntity);
        Task<IList<AccommodationEntity>> GetByIds(IList<Guid> guids);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
