using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.ProfileEvents
{
    public record PersonProfileRestoredEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }


        // Methods
        public static implicit operator PersonProfileRestoredEvent(Person person)
        {
            return new PersonProfileRestoredEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
            };
        }
    }
}
