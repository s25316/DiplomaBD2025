namespace Domain.Shared.Templates
{
    public class TemplateEntity<TId>
    {
        public TId? Id { get; protected set; }
        public IEnumerable<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
