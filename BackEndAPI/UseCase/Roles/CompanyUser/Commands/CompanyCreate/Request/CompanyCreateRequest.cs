// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request
{
    public class CompanyCreateRequest : RequestTemplate<CompanyCreateResponse>
    {
        public required CompanyCreateCommand Command { get; set; } = null!;
    }
}
