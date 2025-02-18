namespace Domain.Shared.Templates
{
    class DomainEntity<TId>
    {
        public TId? Id { get; init; }
        public IEnumerable<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
