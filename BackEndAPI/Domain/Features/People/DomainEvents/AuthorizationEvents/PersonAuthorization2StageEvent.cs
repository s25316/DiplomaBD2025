using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.AuthorizationEvents
{
    public record PersonAuthorization2StageEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string UrlSegment { get; init; }
        public required string Email { get; init; }
        public required string Code { get; init; }
        public required DateTime CodeValidTo { get; init; }


        // Constructor
        public static PersonAuthorization2StageEvent Prepare(
            Person person,
            string urlSegment,
            string code,
            DateTime codeValidTo)
        {
            return new PersonAuthorization2StageEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Email = person.Login.Value,
                UrlSegment = urlSegment,
                Code = code,
                CodeValidTo = codeValidTo,
            };
        }
    }
}
