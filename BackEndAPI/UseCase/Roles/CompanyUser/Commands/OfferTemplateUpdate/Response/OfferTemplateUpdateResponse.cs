using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Response
{
    public class OfferTemplateUpdateResponse
         : ItemResponse<BaseCommandResult<OfferTemplateUpdateCommand>>
    {
        public static OfferTemplateUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            OfferTemplateUpdateCommand command)
        {
            return new OfferTemplateUpdateResponse
            {
                HttpCode = code,
                Result = new BaseCommandResult<OfferTemplateUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
