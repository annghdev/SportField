namespace Common.Abstractions
{
    public class AggregateRoot<T> : BaseEntity<T>
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
