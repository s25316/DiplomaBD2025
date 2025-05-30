using Microsoft.AspNetCore.Http;
using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserUpdateCompanyLogo.Request
{
    public class CompanyUserUpdateCompanyLogoRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid CompanyId { get; set; }
        public required IFormFile File { get; set; }
    }
}
