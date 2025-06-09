using Domain.Features.Companies.Entities;
using Domain.Features.Companies.Exceptions;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Companies.DomainEvents
{
    public record CompanyUnBlockedEvent : DomainEvent
    {
        // Properties
        public required Guid CompanyId { get; init; }


        // Methods
        public static CompanyUnBlockedEvent Prepare(
            Company company)
        {
            return new CompanyUnBlockedEvent
            {
                CompanyId = company.Id?.Value ?? throw new CompanyException(
                    Messages.Enitity_Company_NotInitializedId,
                    HttpCode.InternalServerError),
            };
        }
    }
}
