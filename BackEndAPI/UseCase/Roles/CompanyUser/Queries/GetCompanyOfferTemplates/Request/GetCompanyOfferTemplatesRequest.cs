using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Request
{
    public class GetCompanyOfferTemplatesRequest : RequestTemplate<GetCompanyOfferTemplatesResponse>
    {
        public required Guid CompanyId { get; init; }
    }
}
