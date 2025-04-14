using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.BranchRemove.Response
{
    public class BranchRemoveResponse : ResponseTemplate<ResponseCommandMetadata>
    {
        public static BranchRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new BranchRemoveResponse
            {
                HttpCode = code,
                Result = new ResponseCommandMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? code.Description()
                }
            };
        }
    }
}
