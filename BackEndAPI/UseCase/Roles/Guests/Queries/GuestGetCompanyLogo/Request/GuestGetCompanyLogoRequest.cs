using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse.FileResponses;

namespace UseCase.Roles.Guests.Queries.GuestGetCompanyLogo.Request
{
    public class GuestGetCompanyLogoRequest : BaseRequest<FileResponse>
    {
        public required Guid CompanyId { get; init; }
    }
}
