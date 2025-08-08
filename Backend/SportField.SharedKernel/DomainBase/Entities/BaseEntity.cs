using System.ComponentModel.DataAnnotations.Schema;

namespace SportField.SharedKernel.DomainBase.Entities
{
    public abstract class BaseEntity<T>
    {
        public virtual T Id { get; set; } = default!;
        private readonly List<BaseDomainEvent> _domainEvents = [];
        [NotMapped]
        public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents;

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
