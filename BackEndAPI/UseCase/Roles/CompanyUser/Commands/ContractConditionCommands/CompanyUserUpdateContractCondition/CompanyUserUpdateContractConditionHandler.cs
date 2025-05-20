using Domain.Features.ContractConditions.ValueObjects.Info;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserUpdateContractCondition.Request;
using UseCase.Roles.CompanyUser.Repositories.ContractConditions;
using UseCase.Shared.Dictionaries.Repositories;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainContractCondition = Domain.Features.ContractConditions.Aggregates.ContractCondition;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserUpdateContractCondition
{
    public class CompanyUserUpdateContractConditionHandler : IRequestHandler<CompanyUserUpdateContractConditionRequest, CommandResponse<CompanyUserUpdateContractConditionCommand>>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IDictionariesRepository _dictionariesRepository;
        private readonly IContractConditionRepository _conditionRepository;


        // Constructor
        public CompanyUserUpdateContractConditionHandler(
            IAuthenticationInspectorService authenticationInspector,
            IDictionariesRepository dictionariesRepository,
            IContractConditionRepository conditionRepository)
        {
            _authenticationInspector = authenticationInspector;
            _dictionariesRepository = dictionariesRepository;
            _conditionRepository = conditionRepository;
        }


        // Methods
        public async Task<CommandResponse<CompanyUserUpdateContractConditionCommand>> Handle(CompanyUserUpdateContractConditionRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);

            // Get From DB
            var selectResult = await _conditionRepository
                .GetAsync(personId, request.ContractConditionId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    selectResult.Metadata.Message,
                    request);
            }

            // Build Updater
            var updater = Update(selectResult.Item, request);
            if (updater.HasErrors())
            {
                return PrepareResponse(
                        HttpCode.BadRequest,
                        updater.GetErrors(),
                        request);
            }
            var contractCondition = updater.Build();


            // Check is valid Ids
            var notFondIds = await CheckContractPropertiesAsync(contractCondition);
            if (notFondIds.Any())
            {
                return PrepareResponse(
                            HttpCode.NotFound,
                            $"Contract Parameters not found: {string.Join(", ", notFondIds)}",
                            request);
            }

            // Update in DB
            var updateResult = await _conditionRepository.UpdateAsync(
                personId,
                contractCondition,
                cancellationToken);
            return PrepareResponse(
                        updateResult.Code,
                        updateResult.Metadata.Message,
                        request);
        }

        // Private Static Methods
        private static CommandResponse<CompanyUserUpdateContractConditionCommand> PrepareResponse(
            HttpCode code,
            string? message,
            CompanyUserUpdateContractConditionRequest request)
        {
            return CommandResponse<CompanyUserUpdateContractConditionCommand>.PrepareResponse(
                code,
                request.Command,
                message);
        }

        private static DomainContractCondition.Updater Update(
            DomainContractCondition item,
            CompanyUserUpdateContractConditionRequest request)
        {
            var updater = new DomainContractCondition.Updater(item);
            var command = request.Command;

            if (command.SalaryMin.HasValue && command.SalaryMax.HasValue)
            {
                updater.SetSalaryRange(
                    command.SalaryMin.Value,
                    command.SalaryMax.Value);
            }
            if (command.HoursPerTerm.HasValue)
            {
                updater.SetHoursPerTerm(command.HoursPerTerm.Value);
            }
            if (command.IsNegotiable.HasValue)
            {
                updater.SetIsNegotiable(command.IsNegotiable.Value);
            }

            updater.SetContractParameters(
                command.SalaryTermId,
                command.CurrencyId,
                command.WorkModeIds.Select(id => (ContractAttributeInfo)id),
                command.EmploymentTypeIds.Select(id => (ContractAttributeInfo)id));

            return updater;
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserUpdateContractConditionRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private async Task<IEnumerable<int>> CheckContractPropertiesAsync(DomainContractCondition item)
        {
            var dictionary = await _dictionariesRepository.GetContractParametersAsync();
            var notFoundIds = new List<int>();

            foreach (var key in item.Currencies.Keys)
            {
                if (!dictionary.ContainsKey(key))
                {
                    notFoundIds.Add(key);
                }
            }
            foreach (var key in item.SalaryTerms.Keys)
            {
                if (!dictionary.ContainsKey(key))
                {
                    notFoundIds.Add(key);
                }
            }
            foreach (var key in item.WorkModes.Keys)
            {
                if (!dictionary.ContainsKey(key))
                {
                    notFoundIds.Add(key);
                }
            }
            foreach (var key in item.EmploymentTypes.Keys)
            {
                if (!dictionary.ContainsKey(key))
                {
                    notFoundIds.Add(key);
                }
            }

            return notFoundIds;
        }
    }
}
