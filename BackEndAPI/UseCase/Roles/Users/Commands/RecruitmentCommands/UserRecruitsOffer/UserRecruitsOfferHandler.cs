using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitsOffer.Request;
using UseCase.Shared.Repositories.Recruitments;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainRecruitment = Domain.Features.Recruitments.Entities.Recruitment;

namespace UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitsOffer
{
    public class UserRecruitsOfferHandler : IRequestHandler<UserRecruitsOfferRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IMongoDbFileService _mongoDbFileService;


        // Constructor
        public UserRecruitsOfferHandler(
            IMongoDbFileService mongoDbFileService,
            IRecruitmentRepository recruitmentRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mongoDbFileService = mongoDbFileService;
            _recruitmentRepository = recruitmentRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserRecruitsOfferRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var builder = PrepareBuilder(personId, request);
            if (builder.HasErrors())
            {
                return PrepareResponse(HttpCode.BadRequest, builder.GetErrors());
            }

            var domain = builder.Build();
            var validationData = await _recruitmentRepository.IsValidCreateAsync(domain, cancellationToken);
            if (validationData.Code != HttpCode.Ok)
            {
                return PrepareResponse(validationData.Code, validationData.Message);
            }
            var fileId = await _mongoDbFileService.SaveAsync(request.Command.File, cancellationToken);
            domain = new DomainRecruitment.Updater(domain)
                .SetFile(fileId)
                .Build();

            await _recruitmentRepository.CreateAsync(domain, cancellationToken);

            return PrepareResponse(HttpCode.Ok);
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
