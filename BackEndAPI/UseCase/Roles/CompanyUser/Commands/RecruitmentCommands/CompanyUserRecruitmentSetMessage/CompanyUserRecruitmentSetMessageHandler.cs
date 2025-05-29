using Domain.Features.People.ValueObjects.Ids;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserRecruitmentSetMessage.Request;
using UseCase.Shared.Repositories.RecruitmentMessages;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserRecruitmentSetMessage
{
    public class CompanyUserRecruitmentSetMessageHandler : IRequestHandler<CompanyUserRecruitmentSetMessageRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IRecruitmentMessagesRepository _recruitmentMessagesRepository;


        // Constructor
        public CompanyUserRecruitmentSetMessageHandler(
            IAuthenticationInspectorService authenticationInspector,
            IRecruitmentMessagesRepository recruitmentMessagesRepository)
        {
            _authenticationInspector = authenticationInspector;
            _recruitmentMessagesRepository = recruitmentMessagesRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(CompanyUserRecruitmentSetMessageRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var creationResult = await _recruitmentMessagesRepository.CreateAsync(
                personId,
                request.RecruitmentId,
                request.Command.Message,
                false,
                cancellationToken);
            return creationResult;
        }

        // Non Static Methods
        private PersonId GetPersonId(CompanyUserRecruitmentSetMessageRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
