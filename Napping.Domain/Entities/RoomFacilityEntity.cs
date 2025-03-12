namespace Napping.Domain.Entities
{
    public class RoomFacilityEntity : AuditableEntity
    {
        // Property
        public string Name { get; private set; } = string.Empty;

        // Navigation Property
        public ICollection<RoomEntity> Rooms { get; private set; } = new List<RoomEntity>();

        // Private Constructor
        private RoomFacilityEntity() { }

        // Public Methods
        public static RoomFacilityEntity Create(string name)
        {
            Validate(name);

            return new RoomFacilityEntity
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        // Private Methods
        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
        }
    }
}

