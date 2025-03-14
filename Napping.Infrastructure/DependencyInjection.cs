using Microsoft.Extensions.DependencyInjection;
using Napping.Domain.Repositories;
using Napping.Infrastructure.Repositories;

namespace Napping.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccommodationRepository, AccommodationRepository>();

            return services;
        }
    }
}
