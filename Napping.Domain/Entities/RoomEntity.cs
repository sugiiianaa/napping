namespace Napping.Domain.Entities
{
    public class RoomEntity : AuditableEntity
    {
        // Property
        public Guid AccommodationId { get; private set; }
        public string RoomType { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string Description { get; private set; } = string.Empty;

        // Navigation Property
        public AccommodationEntity Accommodation { get; private set; } = null!;
        public ICollection<RoomFacilityEntity> Facilities { get; private set; } = new List<RoomFacilityEntity>();

        // Private Constructor
        private RoomEntity() { }

        // Public Methods 
        public static RoomEntity Create(
            Guid accommodationId,
            string roomType,
            decimal price,
            string description,
            ICollection<RoomFacilityEntity> facilities)
        {
            Validate(accommodationId, roomType, price, description);

            return new RoomEntity
            {
                Id = Guid.NewGuid(),
                AccommodationId = accommodationId,
                RoomType = roomType,
                Price = price,
                Description = description,
                Facilities = facilities
            };
        }

        // Private Methods
        private static void Validate(
            Guid accommodationId,
            string roomType,
            decimal price,
            string description
            )
        {
            if (accommodationId == Guid.Empty)
            {
                throw new ArgumentException("AccommodationId cannot be null or empty", nameof(accommodationId));
            }

            if (string.IsNullOrEmpty(roomType))
            {
                throw new ArgumentException("RoomType cannot be null or empty", nameof(roomType));
            }

            if (price <= 0)
            {
                throw new ArgumentException("Price cannot be zero or negative", nameof(price));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Description cannot be null or empty", nameof(description));
            }

        }
    }
}
