using Domain.Features.Companies.Entities;
using Domain.Features.Companies.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Companies.DomainEvents
{
    public record CompanyBlockedEvent : DomainEvent
    {
        // Properties
        public required Guid CompanyId { get; init; }
        public required string Message { get; init; }


        // Methods
        public static CompanyBlockedEvent Prepare(
            Company company,
            string message)
        {
            return new CompanyBlockedEvent
            {
                CompanyId = company.Id?.Value ?? throw new CompanyException(
                    Messages.Enitity_Company_NotInitializedId,
                    HttpCode.InternalServerError),
                Message = message,
            };
        }
    }
}
