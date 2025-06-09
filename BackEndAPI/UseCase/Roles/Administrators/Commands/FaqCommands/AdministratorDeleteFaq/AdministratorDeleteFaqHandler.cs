using MediatR;
using UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorDeleteFaq.Request;
using UseCase.Roles.Administrators.Commands.FaqCommands.Repositories;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorDeleteFaq
{
    public class AdministratorDeleteFaqHandler : IRequestHandler<AdministratorDeleteFaqRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IFaqRepository _repository;


        // Constructor
        public AdministratorDeleteFaqHandler(IFaqRepository repository) { _repository = repository; }


        // Methods
        public async Task<ResultMetadataResponse> Handle(AdministratorDeleteFaqRequest request, CancellationToken cancellationToken)
        {
            var deleteResult = await _repository
                .RemoveAsync(request.FaqId, cancellationToken);
            Console.WriteLine(deleteResult.Code);
            return ResultMetadataResponse
                .PrepareResponse(deleteResult.Code, deleteResult.Metadata.Message);
        }
    }
}
