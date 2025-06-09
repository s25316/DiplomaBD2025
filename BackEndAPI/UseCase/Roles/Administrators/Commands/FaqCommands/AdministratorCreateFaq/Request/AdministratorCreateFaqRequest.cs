using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorCreateFaq.Request
{
    public class AdministratorCreateFaqRequest : BaseRequest<ResultMetadataResponse>
    {
        public required AdministratorCreateFaqCommand Command { get; init; }
    }
}
