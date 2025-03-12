namespace Napping.Domain.Entities
{
    public class AccommodationEntity : AuditableEntity
    {
        // Property
        public string Name { get; private set; } = string.Empty;
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Address { get; private set; } = string.Empty;
        public decimal Rating { get; private set; }

        // Navigation Property
        public ICollection<RoomEntity> Rooms { get; private set; } = [];

        // Private Constructor
        private AccommodationEntity() { }

        // Public Methods
        public static AccommodationEntity Create(
            string name,
            double latitude,
            double longitude,
            string address,
            decimal rating)
        {
            Validate(name, address, rating);

            return new AccommodationEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Latitude = latitude,
                Longitude = longitude,
                Address = address,
                Rating = rating
            };
        }

        public void UpdateRating(decimal newRating)
        {
            if (newRating < 0 || newRating > 5)
            {
                throw new ArgumentException("Rating must be between 0 and 5.", nameof(newRating));
            }

            Rating = newRating;
            UpdateLastModified();
        }

        // Private Methods
        private static void Validate(
            string name,
            string address,
            decimal rating)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Address cannot be null or empty.", nameof(address));
            }

            if (rating < 0 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 0 and 5.", nameof(rating));
            }
        }
    }
}
