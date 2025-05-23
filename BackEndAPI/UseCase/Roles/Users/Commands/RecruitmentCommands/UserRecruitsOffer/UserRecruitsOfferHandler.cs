using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitsOffer.Request;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainRecruitment = Domain.Features.Recruitments.Entities.Recruitment;

namespace UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitsOffer
{
    public class UserRecruitsOfferHandler : IRequestHandler<UserRecruitsOfferRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public UserRecruitsOfferHandler(
            IAuthenticationInspectorService authenticationInspector)
        {
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserRecruitsOfferRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var builder = PrepareBuilder(personId, request);

            throw new NotImplementedException();
        }

        // Static Methods
        private static ResultMetadataResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(code, message);
        }

        private static DomainRecruitment.Builder PrepareBuilder(
            PersonId personId,
            UserRecruitsOfferRequest request)
        {
            return new DomainRecruitment.Builder()
                .SetPersonId(personId)
                .SetOfferId(request.OfferId)
                .SetMessage(request.Command.Description);
        }

        // Non Static Methods
        private PersonId GetPersonId(UserRecruitsOfferRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
