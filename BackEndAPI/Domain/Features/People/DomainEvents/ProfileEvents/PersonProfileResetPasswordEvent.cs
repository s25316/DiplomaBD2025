using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.ProfileEvents
{
    public record PersonProfileResetPasswordEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string Salt { get; init; }
        public required string Password { get; init; }


        // Methods
        public static PersonProfileResetPasswordEvent Prepare(
            Person person,
            string salt,
            string password)
        {
            return new PersonProfileResetPasswordEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Salt = salt,
                Password = password,
            };
        }
    }
}
