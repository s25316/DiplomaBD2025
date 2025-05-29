using Domain.Features.People.ValueObjects.Ids;
using MediatR;
using UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitmentSetMessage.Request;
using UseCase.Shared.Repositories.RecruitmentMessages;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitmentSetMessage
{
    public class UserRecruitmentSetMessageHandler : IRequestHandler<UserRecruitmentSetMessageRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IRecruitmentMessagesRepository _recruitmentMessagesRepository;


        // Constructor
        public UserRecruitmentSetMessageHandler(
            IAuthenticationInspectorService authenticationInspector,
            IRecruitmentMessagesRepository recruitmentMessagesRepository)
        {
            _authenticationInspector = authenticationInspector;
            _recruitmentMessagesRepository = recruitmentMessagesRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserRecruitmentSetMessageRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var creationResult = await _recruitmentMessagesRepository.CreateAsync(
                personId,
                request.RecruitmentId,
                request.Command.Message,
                true,
                cancellationToken);
            return creationResult;
        }

        // Non Static Methods
        private PersonId GetPersonId(UserRecruitmentSetMessageRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
