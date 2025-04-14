using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Response
{
    public class CompanyUpdateResponse
        : ResponseTemplate<ResponseCommandTemplate<CompanyUpdateCommand>>
    {
        public static CompanyUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            CompanyUpdateCommand command)
        {
            return new CompanyUpdateResponse
            {
                HttpCode = code,
                Result = new ResponseCommandTemplate<CompanyUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
