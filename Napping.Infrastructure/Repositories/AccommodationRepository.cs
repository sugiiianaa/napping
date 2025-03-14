using Napping.Domain.Entities;
using Napping.Domain.Repositories;
using Napping.Infrastructure.Data;

namespace Napping.Infrastructure.Repositories
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly AppDbContext _appDbContext;

        public AccommodationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(AccommodationEntity accommodationEntity)
        {
            await _appDbContext.Accommodations.AddAsync(accommodationEntity);
        }

        public Task<IList<AccommodationEntity>> GetByIds(IList<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsyn(AccommodationEntity accommodationEntity)
        {
            throw new NotImplementedException();
        }
    }
}
