using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.ProfileEvents
{
    public record PersonProfileActivatedEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string Login { get; init; } = null!;


        // Methods
        public static implicit operator PersonProfileActivatedEvent(Person person)
        {
            return new PersonProfileActivatedEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Login = person.Login.Value,
            };
        }
    }
}
