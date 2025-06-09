using MediatR;
using UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorUpdateFaq.Request;
using UseCase.Roles.Administrators.Commands.FaqCommands.Repositories;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorUpdateFaq
{
    public class AdministratorUpdateFaqHandler : IRequestHandler<AdministratorUpdateFaqRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IFaqRepository _repository;


        // Constructor
        public AdministratorUpdateFaqHandler(IFaqRepository repository) { _repository = repository; }


        // Methods
        public async Task<ResultMetadataResponse> Handle(AdministratorUpdateFaqRequest request, CancellationToken cancellationToken)
        {
            var updateResult = await _repository
                .UpdateAsync(request.FaqId, request.Command.Answer, cancellationToken);
            return ResultMetadataResponse
                .PrepareResponse(updateResult.Code, updateResult.Metadata.Message);
        }
    }
}
