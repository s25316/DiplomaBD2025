using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.BranchUpdate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.BranchUpdate.Response
{
    public class BranchUpdateResponse
     : ResponseTemplate<ResponseCommandTemplate<BranchUpdateCommand>>
    {
        public static BranchUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            BranchUpdateCommand command)
        {
            return new BranchUpdateResponse
            {
                HttpCode = code,
                Result = new ResponseCommandTemplate<BranchUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
