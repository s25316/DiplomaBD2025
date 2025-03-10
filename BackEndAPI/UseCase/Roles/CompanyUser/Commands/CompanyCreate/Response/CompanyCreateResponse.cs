using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCreate.Response
{
    public class CompanyCreateResponse : ResponseMetaData
    {
        public required ResponseCommandTemplate<CompanyCreateCommand> Command { get; init; }
    }
}
