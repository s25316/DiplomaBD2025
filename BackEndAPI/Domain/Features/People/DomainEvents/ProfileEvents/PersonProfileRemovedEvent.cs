using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.ProfileEvents
{
    public record PersonProfileRemovedEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }

        public required DateTime Created { get; init; }
        private string _urlSegment = null!;
        public string UrlSegment
        {
            get => _urlSegment;
            set => _urlSegment ??= value;
        }

        private DateTime _validTo;
        public DateTime ValidTo
        {
            get => _validTo;
            set => _validTo = _validTo == DateTime.MinValue ? value : _validTo;
        }


        // Methods
        public static PersonProfileRemovedEvent Prepare(Person person, DateTime removed)
        {
            return new PersonProfileRemovedEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Created = removed,
            };
        }
    }
}
