using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.AdministrationEvents
{
    public record PersonAdministrationRevokeEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }


        // Methods
        public static PersonAdministrationRevokeEvent Prepare(
            Person person)
        {
            return new PersonAdministrationRevokeEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
            };
        }
    }
}
