namespace Napping.Domain.Entities
{
    public abstract class AuditableEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; private set; }
        public Guid UpdateByUserWithId { get; private set; }

        protected void UpdateLastModified(Guid updateByUserWithId)
        {
            LastUpdatedAt = DateTime.Now;
            UpdateByUserWithId = updateByUserWithId;
        }

        protected void UpdateLastModified()
        {
            LastUpdatedAt = DateTime.Now;
        }
    }


}
