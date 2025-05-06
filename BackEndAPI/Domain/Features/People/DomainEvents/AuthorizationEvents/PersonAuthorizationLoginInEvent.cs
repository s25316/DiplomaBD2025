// Ignore Spelling: Jwt
using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.AuthorizationEvents
{
    public record PersonAuthorizationLoginInEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string Jwt { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime RefreshTokenValidTo { get; init; }


        // Constructor
        public static PersonAuthorizationLoginInEvent Prepare(
            Person person,
            string jwt,
            string refreshToken,
            DateTime refreshTokenValidTo)
        {
            return new PersonAuthorizationLoginInEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Jwt = jwt,
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            };
        }
    }
}
