using Domain.Features.ContractConditions.ValueObjects.Info;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using System.Text;
using UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserCreateContractConditions.Request;
using UseCase.Roles.CompanyUser.Repositories.ContractConditions;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Dictionaries.Repositories;
using UseCase.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainContractCondition = Domain.Features.ContractConditions.Aggregates.ContractCondition;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserCreateContractConditions
{
    public class CompanyUserCreateContractConditionsHandler : IRequestHandler<CompanyUserCreateContractConditionsRequest, CommandsResponse<CompanyUserCreateContractConditionsCommand>>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IDictionariesRepository _dictionariesRepository;
        private readonly IContractConditionRepository _conditionRepository;


        // Constructor
        public CompanyUserCreateContractConditionsHandler(
            IAuthenticationInspectorService authenticationInspector,
            IDictionariesRepository dictionariesRepository,
            IContractConditionRepository conditionRepository)
        {
            _authenticationInspector = authenticationInspector;
            _dictionariesRepository = dictionariesRepository;
            _conditionRepository = conditionRepository;
        }


        // Methods
        public async Task<CommandsResponse<CompanyUserCreateContractConditionsCommand>> Handle(CompanyUserCreateContractConditionsRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var contractParametersDictionary = await _dictionariesRepository.GetContractParametersAsync();
            var resultBuild = Build(request, contractParametersDictionary);

            if (!resultBuild.IsValid)
            {
                return new CommandsResponse<CompanyUserCreateContractConditionsCommand>
                {
                    Result = resultBuild.Dictionary.Select(pair => new BaseCommandResult<CompanyUserCreateContractConditionsCommand>
                    {
                        Item = pair.Key,
                        IsCorrect = string.IsNullOrWhiteSpace(pair.Value.Error),
                        Message = pair.Value.Error ?? string.Empty,
                    }),
                    HttpCode = HttpCode.BadRequest,
                };
            }

            var dictionary = resultBuild.Dictionary.ToDictionary(
                var => var.Key,
                var => var.Value.ContractCondition);
            var repositoryResult = await _conditionRepository.CreateAsync(
                personId,
                dictionary.Values,
                cancellationToken);

            return new CommandsResponse<CompanyUserCreateContractConditionsCommand>
            {
                Result = dictionary.Select(pair => new BaseCommandResult<CompanyUserCreateContractConditionsCommand>
                {
                    Item = pair.Key,
                    IsCorrect = repositoryResult.Dictionary[pair.Value].IsCorrect,
                    Message = repositoryResult.Dictionary[pair.Value].Message,
                }),
                HttpCode = repositoryResult.Code,
            };
        }

        // Private Static Methods
        private static DomainContractCondition.Builder PrepareBuilder(
            Guid companyId,
            CompanyUserCreateContractConditionsCommand command)
        {
            return new DomainContractCondition.Builder()
                .SetCompanyId(companyId)
                .SetIsNegotiable(command.IsNegotiable)
                .SetSalaryRange(command.SalaryMin, command.SalaryMax)
                .SetHoursPerTerm(command.HoursPerTerm)
                .SetContractParameters(
                    command.SalaryTermId,
                    command.CurrencyId,
                    command.WorkModeIds.Select(id => (ContractAttributeInfo)id),
                    command.EmploymentTypeIds.Select(id => (ContractAttributeInfo)id));
        }

        private static string CheckContractParamiters(
            IEnumerable<int> ids,
            ContractParameterTypes parameter,
            IReadOnlyDictionary<int, ContractParameterDto> contractParametersDictionary)
        {
            var invalid = new StringBuilder();
            var notFound = new StringBuilder();
            foreach (var id in ids)
            {
                if (!contractParametersDictionary.TryGetValue(id, out var param))
                {
                    notFound.Append($"{id} ");
                    continue;
                }

                if (param.ContractParameterType.ContractParameterTypeId != (int)parameter)
                {
                    invalid.Append($"{id} ");
                }
            }

            if (notFound.Length > 0)
            {
                notFound.Insert(0, "Not found Ids: ");
            }
            if (invalid.Length > 0)
            {
                invalid.Insert(0, $"For {parameter.Description()} invalid Ids: ");
            }
            return notFound.AppendLine(invalid.ToString()).ToString().Trim();
        }

        private static string CheckDomainFields(
            DomainContractCondition item,
            IReadOnlyDictionary<int, ContractParameterDto> contractParametersDictionary)
        {
            var stringBuilder = new StringBuilder();

            if (item.SalaryTerms.Any())
            {
                stringBuilder.AppendLine(CheckContractParamiters(
                    item.SalaryTerms.Values.Select(st => st.ContractParameterId),
                    ContractParameterTypes.SalaryTerm,
                    contractParametersDictionary));
            }
            if (item.Currencies.Any())
            {
                stringBuilder.AppendLine(CheckContractParamiters(
                    item.Currencies.Values.Select(st => st.ContractParameterId),
                    ContractParameterTypes.Currency,
                    contractParametersDictionary));
            }

            stringBuilder.AppendLine(CheckContractParamiters(
                    item.WorkModes.Values.Select(st => st.ContractParameterId),
                    ContractParameterTypes.WorkMode,
                    contractParametersDictionary));

            stringBuilder.AppendLine(CheckContractParamiters(
                    item.EmploymentTypes.Values.Select(st => st.ContractParameterId),
                    ContractParameterTypes.EmploymentType,
                    contractParametersDictionary));

            return stringBuilder.ToString().Trim();
        }

        private sealed class ResultBuild
        {
            public required Dictionary<CompanyUserCreateContractConditionsCommand, ContractConditionAndError> Dictionary { get; init; }
            public required bool IsValid { get; init; }
        }

        private sealed class ContractConditionAndError
        {
            public required DomainContractCondition ContractCondition { get; init; }
            public required string? Error { get; init; }
        }

        private static ResultBuild Build(
            CompanyUserCreateContractConditionsRequest request,
            IReadOnlyDictionary<int, ContractParameterDto> contractParametersDictionary)
        {
            var dictionary = new Dictionary<CompanyUserCreateContractConditionsCommand, ContractConditionAndError>();
            var isValid = true;

            var stringBuilder = new StringBuilder();

            foreach (var command in request.Commands)
            {
                stringBuilder.Clear();

                var builder = PrepareBuilder(request.CompanyId, command);
                var item = builder.Build();
                if (builder.HasErrors())
                {
                    stringBuilder.AppendLine(builder.GetErrors());
                }

                stringBuilder.AppendLine(
                    CheckDomainFields(item, contractParametersDictionary)
                    );


                var value = new ContractConditionAndError
                {
                    ContractCondition = item,
                    Error = stringBuilder.ToString().Trim(),
                };
                dictionary.Add(command, value);

                if (isValid && !string.IsNullOrWhiteSpace(value.Error))
                {
                    isValid = false;
                }
            }
            return new ResultBuild
            {
                Dictionary = dictionary,
                IsValid = isValid
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserCreateContractConditionsRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
