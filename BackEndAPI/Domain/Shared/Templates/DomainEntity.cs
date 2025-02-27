namespace Domain.Shared.Templates
{
    public class DomainEntity<TId>
    {
        public TId? Id { get; init; }
        public IEnumerable<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
