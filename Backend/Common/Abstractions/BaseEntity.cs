using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Abstractions
{
    public abstract class BaseEntity<T>
    {
        public virtual T Id { get; set; }
        private readonly List<BaseDomainEvent> _domainEvents = [];
        [NotMapped]
        public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvent()
        {
            _domainEvents.Clear();
        }
    }
    public abstract class BaseEntity : BaseEntity<Guid>
    {
        public override Guid Id { get; set; } = Guid.CreateVersion7();
    }
}
