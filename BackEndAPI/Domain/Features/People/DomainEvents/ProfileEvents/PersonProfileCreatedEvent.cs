using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.ProfileEvents
{
    public record PersonProfileCreatedEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string Email { get; init; } = null!;
        public required string Salt { get; init; } = null!;
        public required string Password { get; init; } = null!;
        public required DateTime Created { get; init; }
        private string _urlSegment = null!;
        public string UrlSegment
        {
            get => _urlSegment;
            set => _urlSegment ??= value;
        }


        // Methods
        public static implicit operator PersonProfileCreatedEvent(Person person)
        {
            return new PersonProfileCreatedEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Email = person.Login.Value,
                Salt = person.Salt,
                Password = person.Password,
                Created = person.Created,
            };
        }
    }
}
