using Domain.Features.People.ValueObjects.Ids;
using MediatR;
using UseCase.Roles.Users.Queries.GetPersonRecruitmentMessages.Request;
using UseCase.Shared.Repositories.RecruitmentMessages;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitmentMessages
{
    public class GetPersonRecruitmentMessagesHandler : IRequestHandler<GetPersonRecruitmentMessagesRequest, ItemsResponse<MessageDto>>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IRecruitmentMessagesRepository _recruitmentMessagesRepository;


        // Constructor
        public GetPersonRecruitmentMessagesHandler(
            IAuthenticationInspectorService authenticationInspector,
            IRecruitmentMessagesRepository recruitmentMessagesRepository)
        {
            _authenticationInspector = authenticationInspector;
            _recruitmentMessagesRepository = recruitmentMessagesRepository;
        }


        // Methods
        public async Task<ItemsResponse<MessageDto>> Handle(GetPersonRecruitmentMessagesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _recruitmentMessagesRepository.GetAsync(
                personId,
                request.RecruitmentId,
                true,
                request.PaginationQueryParameters,
                request.Ascending,
                cancellationToken);
            return selectResult;
        }

        // Non Static Methods
        private PersonId GetPersonId(GetPersonRecruitmentMessagesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
