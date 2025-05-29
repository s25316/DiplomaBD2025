using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.Recruitments.Enums;
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitmentFile.Request;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.Recruitments;
using UseCase.Shared.Responses.ItemResponse.FileResponses;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainRecruitment = Domain.Features.Recruitments.Entities.Recruitment;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitmentFile
{
    public class CompanyUserGetRecruitmentFileHandler : IRequestHandler<CompanyUserGetRecruitmentFileRequest, FileResponse>
    {
        // Properties 
        private readonly IMongoDbFileService _mongoDbFileService;
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor 
        public CompanyUserGetRecruitmentFileHandler(
            IMongoDbFileService mongoDbFileService,
            IRecruitmentRepository recruitmentRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mongoDbFileService = mongoDbFileService;
            _recruitmentRepository = recruitmentRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<FileResponse> Handle(CompanyUserGetRecruitmentFileRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _recruitmentRepository.GetAsync(personId, request.RecruitmentId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(selectResult.Code);
            }
            var domainRecruitment = selectResult.Item;


            if (domainRecruitment.ProcessType == ProcessType.Recruit)
            {
                domainRecruitment = new DomainRecruitment.Updater(domainRecruitment)
                    .SetProcessType(ProcessType.Watched)
                    .Build();

                var updateResult = await _recruitmentRepository.UpdateAsync(personId, domainRecruitment, cancellationToken);
                if (updateResult.Code != HttpCode.Ok)
                {
                    // Impossible
                    throw new UseCaseLayerException($"{updateResult.Metadata.Message}: {request.RecruitmentId}, {personId.Value}");
                }
            }

            var fileDto = await _mongoDbFileService.GetAsync(domainRecruitment.File, cancellationToken);
            return PrepareResponse(HttpCode.Ok, fileDto);
        }

        // Static Methods
        private static FileResponse PrepareResponse(HttpCode code, FileDto? item = null)
        {
            return FileResponse.PrepareResponse(code, item);
        }

        // Non Static Methods
        private PersonId GetPersonId(CompanyUserGetRecruitmentFileRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
