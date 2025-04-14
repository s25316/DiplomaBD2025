namespace Domain.Shared.Templates
{
    public class TemplateEntity<TId>
    {
        // Properties
        private TId? _id;
        public IEnumerable<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();


        // Methods
        public bool HasValue => _id is not null;

        public TId? Id
        {
            get { return _id; }
            set
            {
                if (_id is null)
                {
                    _id = value;
                }
            }
        }
    }
}
