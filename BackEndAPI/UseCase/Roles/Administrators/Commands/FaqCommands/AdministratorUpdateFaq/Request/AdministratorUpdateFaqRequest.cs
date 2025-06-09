using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorUpdateFaq.Request
{
    public class AdministratorUpdateFaqRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid FaqId { get; init; }
        public required AdministratorUpdateFaqCommand Command { get; init; }
    }
}
