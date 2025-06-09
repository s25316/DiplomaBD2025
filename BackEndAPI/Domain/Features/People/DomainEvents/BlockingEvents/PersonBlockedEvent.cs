using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.BlockingEvents
{
    public record PersonBlockedEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string Message { get; init; }


        // Methods
        public static PersonBlockedEvent Prepare(
            Person person,
            string message)
        {
            return new PersonBlockedEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Message = message,
            };
        }
    }
}
