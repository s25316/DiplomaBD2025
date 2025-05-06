using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.AuthorizationEvents
{
    public record PersonAuthorizationInvalid : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string Reason { get; init; }


        // Constructor
        public static PersonAuthorizationInvalid Prepare(Person person, string reason)
        {
            return new PersonAuthorizationInvalid
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Reason = reason,
            };
        }
    }
}
