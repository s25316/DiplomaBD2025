using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCreate.Response
{
    public class CompanyCreateResponse : BaseResponse
    {
        public required CompanyCreateCommand Company { get; init; }
    }
}
