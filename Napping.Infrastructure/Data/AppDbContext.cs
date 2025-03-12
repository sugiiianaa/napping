using Microsoft.EntityFrameworkCore;
using Napping.Domain.Entities;

namespace Napping.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Parameterless constructor for migrations
        public AppDbContext() : base(new DbContextOptions<AppDbContext>())
        {
        }

        // Primary constructor for dependency injection
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AccommodationEntity> Accommodations { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RoomFacilityEntity> RoomsFacility { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // one-to-many between AccommodationEntity and RoomEntity
            modelBuilder.Entity<AccommodationEntity>()
                .HasMany(a => a.Rooms)
                .WithOne(r => r.Accommodation)
                .HasForeignKey(r => r.AccommodationId)
                .OnDelete(DeleteBehavior.Cascade);

            // many-to-many between RommEntity and RoomFacilityEntity
            modelBuilder.Entity<RoomEntity>()
                .HasMany(r => r.Facilities)
                .WithMany(f => f.Rooms)
                .UsingEntity(j => j.ToTable("RoomFacilities"));
        }
    }
}
