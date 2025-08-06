namespace Common.Abstractions
{
    public abstract class AuditableEntity<T> : BaseEntity<T>
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public abstract class AuditableEntity : AuditableEntity<Guid>
    {
        public override Guid Id { get; set; } = Guid.CreateVersion7();
    }
}
