using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.Recruitments.Enums;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserUpdateRecruitment.Request;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.Recruitments;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainRecruitment = Domain.Features.Recruitments.Entities.Recruitment;

namespace UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserUpdateRecruitment
{
    public class CompanyUserUpdateRecruitmentHandler : IRequestHandler<CompanyUserUpdateRecruitmentRequest, ResultMetadataResponse>
    {
        // Properties 
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor 
        public CompanyUserUpdateRecruitmentHandler(
            IRecruitmentRepository recruitmentRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _recruitmentRepository = recruitmentRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(CompanyUserUpdateRecruitmentRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _recruitmentRepository.GetAsync(personId, request.RecruitmentId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(selectResult.Code);
            }
            var domainRecruitment = selectResult.Item;

            var updater = new DomainRecruitment.Updater(domainRecruitment)
                    .SetProcessType(request.Command.Accepted
                        ? ProcessType.Passed
                        : ProcessType.Rejected);
            if (updater.HasErrors())
            {
                return PrepareResponse(HttpCode.BadRequest, updater.GetErrors());
            }

            var updateResult = await _recruitmentRepository.UpdateAsync(personId, domainRecruitment, cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                // Impossible
                throw new UseCaseLayerException($"{updateResult.Metadata.Message}: {request.RecruitmentId}, {personId.Value}");
            }
            return PrepareResponse(HttpCode.Ok);
        }

        // Static Methods
        private static ResultMetadataResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(code, message);
        }

        // Non Static Methods
        private PersonId GetPersonId(CompanyUserUpdateRecruitmentRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
