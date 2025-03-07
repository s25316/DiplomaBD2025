using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate.Response
{
    public class BranchCreateResponse : ResponseMetaData
    {
        public IEnumerable<ResponseCommandTemplate<BranchCreateCommand>> Commands { get; init; } = [];
    }
}
