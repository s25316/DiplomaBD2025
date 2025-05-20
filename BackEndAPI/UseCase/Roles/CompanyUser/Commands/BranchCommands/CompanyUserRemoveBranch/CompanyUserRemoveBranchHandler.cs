using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserRemoveBranch.Request;
using UseCase.Roles.CompanyUser.Repositories.Branches;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserRemoveBranch
{
    public class CompanyUserRemoveBranchHandler : IRequestHandler<CompanyUserRemoveBranchRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _inspectorService;
        private readonly IBranchRepository _branchRepository;


        // Constructor
        public CompanyUserRemoveBranchHandler(
            IAuthenticationInspectorService inspectorService,
            IBranchRepository branchRepository)
        {
            _inspectorService = inspectorService;
            _branchRepository = branchRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(CompanyUserRemoveBranchRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _branchRepository
                .GetAsync(personId, request.BranchId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    selectResult.Metadata.Message);
            }

            var item = selectResult.Item;
            item.Remove();

            var removeResponse = await _branchRepository
                .RemoveAsync(personId, item, cancellationToken);

            return PrepareResponse(
                removeResponse.Code,
                removeResponse.Metadata.Message);
        }

        // Private Static Methods
        private static ResultMetadataResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(code, message);
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserRemoveBranchRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
