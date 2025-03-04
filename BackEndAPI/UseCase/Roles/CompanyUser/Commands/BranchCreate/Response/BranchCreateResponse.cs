using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate.Response
{
    public class BranchCreateResponse : ResponseTemplate
    {
        public IEnumerable<ResponseItemTemplate<BranchCreateCommand>> Commands { get; init; } = [];
    }
}
