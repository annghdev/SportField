using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Abstractions
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
        private readonly List<IDomainEvent> _domainEvents = [];
        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvent()
        {
            _domainEvents.Clear();
        }
    }
}
