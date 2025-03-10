using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate.Response
{
    public class BranchCreateResponse : ResponseMetaData
    {
        public IEnumerable<ResponseCommandTemplate<BranchCreateCommand>> Commands { get; init; } = [];
    }
}
