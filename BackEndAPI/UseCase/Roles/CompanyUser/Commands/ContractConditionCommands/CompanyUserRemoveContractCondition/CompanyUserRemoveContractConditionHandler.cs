using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserRemoveContractCondition.Request;
using UseCase.Roles.CompanyUser.Repositories.ContractConditions;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserRemoveContractCondition
{
    public class CompanyUserRemoveContractConditionHandler : IRequestHandler<CompanyUserRemoveContractConditionRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IContractConditionRepository _conditionRepository;


        // Constructor
        public CompanyUserRemoveContractConditionHandler(
            IAuthenticationInspectorService authenticationInspector,
            IContractConditionRepository conditionRepository)
        {
            _authenticationInspector = authenticationInspector;
            _conditionRepository = conditionRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(CompanyUserRemoveContractConditionRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _conditionRepository
                .GetAsync(personId, request.ContractConditionId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    selectResult.Metadata.Message);
            }

            var item = selectResult.Item;
            item.Remove();

            var removeResponse = await _conditionRepository
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
        private PersonId GetPersonId(CompanyUserRemoveContractConditionRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
