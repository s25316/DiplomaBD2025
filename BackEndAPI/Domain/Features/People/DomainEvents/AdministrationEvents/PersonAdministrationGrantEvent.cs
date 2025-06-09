using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.AdministrationEvents
{
    public record PersonAdministrationGrantEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }


        // Methods
        public static PersonAdministrationGrantEvent Prepare(
            Person person)
        {
            return new PersonAdministrationGrantEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
            };
        }
    }
}
