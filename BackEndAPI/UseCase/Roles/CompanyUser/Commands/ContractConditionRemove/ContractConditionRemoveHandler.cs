using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Response;
using UseCase.Roles.CompanyUser.Repositories.ContractConditions;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionRemove
{
    public class ContractConditionRemoveHandler : IRequestHandler<ContractConditionRemoveRequest, ContractConditionRemoveResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IContractConditionRepository _conditionRepository;


        // Constructor
        public ContractConditionRemoveHandler(
            IAuthenticationInspectorService authenticationInspector,
            IContractConditionRepository conditionRepository)
        {
            _authenticationInspector = authenticationInspector;
            _conditionRepository = conditionRepository;
        }


        // Methods
        public async Task<ContractConditionRemoveResponse> Handle(ContractConditionRemoveRequest request, CancellationToken cancellationToken)
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
        private static ContractConditionRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return ContractConditionRemoveResponse.PrepareResponse(code, message);
        }

        // Private Non Static Methods
        private PersonId GetPersonId(ContractConditionRemoveRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
