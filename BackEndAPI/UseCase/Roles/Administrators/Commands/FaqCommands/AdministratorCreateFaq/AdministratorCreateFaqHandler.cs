using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorCreateFaq.Request;
using UseCase.Roles.Administrators.Commands.FaqCommands.Repositories;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorCreateFaq
{
    public class AdministratorCreateFaqHandler : IRequestHandler<AdministratorCreateFaqRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IFaqRepository _repository;


        // Constructor
        public AdministratorCreateFaqHandler(IFaqRepository repository) { _repository = repository; }


        // Methods
        public async Task<ResultMetadataResponse> Handle(AdministratorCreateFaqRequest request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(
                request.Command.Question,
                request.Command.Answer,
                cancellationToken);
            return ResultMetadataResponse.PrepareResponse(HttpCode.Created);
        }
    }
}
