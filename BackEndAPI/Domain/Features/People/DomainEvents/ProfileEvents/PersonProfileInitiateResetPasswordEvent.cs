using Domain.Features.People.Aggregates;
using Domain.Features.People.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.People.DomainEvents.ProfileEvents
{
    public record PersonProfileInitiateResetPasswordEvent : DomainEvent
    {
        // Properties
        public required Guid UserId { get; init; }
        public required string Email { get; init; }
        private string _urlSegment = null!;
        public string UrlSegment
        {
            get => _urlSegment;
            set => _urlSegment ??= value;
        }

        private DateTime _validTo;
        public DateTime ValidTo
        {
            get => _validTo;
            set => _validTo = _validTo == DateTime.MinValue ? value : _validTo;
        }


        // Methods
        public static PersonProfileInitiateResetPasswordEvent Prepare(Person person)
        {
            return new PersonProfileInitiateResetPasswordEvent
            {
                UserId = person.Id?.Value ?? throw new PersonException(
                    Messages.Enitity_Person_NotInitializedId,
                    HttpCode.InternalServerError),
                Email = person.Login.Value,
            };
        }
    }
}
