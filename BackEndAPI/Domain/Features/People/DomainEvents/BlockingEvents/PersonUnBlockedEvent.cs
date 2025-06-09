using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.BlockingEvents
{
    public record PersonUnBlockedEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }


        // Methods
        public static PersonUnBlockedEvent Prepare(
            Person person)
        {
            return new PersonUnBlockedEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
            };
        }
    }
}
