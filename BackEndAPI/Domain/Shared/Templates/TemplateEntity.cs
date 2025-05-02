namespace Domain.Shared.Templates
{
    public class TemplateEntity<TId>
    {
        // Properties
        private TId? _id;
        protected bool AllowedRegistrationEvents { get; set; } = false;
        private List<DomainEvent> _domainEvents { get; set; } = new List<DomainEvent>();
        public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;


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

        public void AllowRegistrationEvents()
        {
            if (!AllowedRegistrationEvents)
            {
                AllowedRegistrationEvents = true;
            }
        }

        public void AddDomainEvent(DomainEvent @event)
        {
            if (AllowedRegistrationEvents)
            {
                _domainEvents.Add(@event);
            }
        }
    }
}
