using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserCreateCompanies.Request;
using UseCase.Shared.Repositories.Companies;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserCreateCompanies
{
    public class CompanyUserCreateCompaniesHandler : IRequestHandler<CompanyUserCreateCompaniesRequest, CommandsResponse<CompanyUserCreateCompaniesCommand>>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly ICompanyRepository _companyRepository;


        // Constructor
        public CompanyUserCreateCompaniesHandler(
            IAuthenticationInspectorService authenticationInspector,
            ICompanyRepository companyRepository)
        {
            _authenticationInspector = authenticationInspector;
            _companyRepository = companyRepository;
        }


        // Methods
        public async Task<CommandsResponse<CompanyUserCreateCompaniesCommand>> Handle(CompanyUserCreateCompaniesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var buildersResult = Build(request);

            // Handle Domain Invalid results
            if (!buildersResult.IsValid)
            {
                return InvalidResponse(buildersResult);
            }

            var dictionary = buildersResult.Dictionary.ToDictionary(
                val => val.Key,
                val => val.Value.Item);

            var createResult = await _companyRepository.CreateAsync(
                personId,
                dictionary.Values,
                cancellationToken);

            // Handle Repository Result Valid or Not 
            return new CommandsResponse<CompanyUserCreateCompaniesCommand>
            {
                Result = dictionary.Select(pair => new BaseCommandResult<CompanyUserCreateCompaniesCommand>
                {
                    Item = pair.Key,
                    IsCorrect = createResult.Dictionary[pair.Value].IsCorrect,
                    Message = createResult.Dictionary[pair.Value].Message,
                }),
                HttpCode = createResult.Code,
            };
        }

        // Private Static Methods
        private static DomainCompany.Builder PrepareBuilder(CompanyUserCreateCompaniesCommand command)
        {
            return new DomainCompany.Builder()
                .SetName(command.Name)
                .SetDescription(command.Description)
                .SetNip(command.Nip)
                .SetKrs(command.Krs)
                .SetRegon(command.Regon)
                .SetWebsiteUrl(command.WebsiteUrl);
        }

        private sealed class BuildersResult
        {
            public required Dictionary<CompanyUserCreateCompaniesCommand, BuilderErrorMessage> Dictionary;
            public required bool IsValid;
        }

        private sealed class BuilderErrorMessage
        {
            public required DomainCompany Item { get; init; }
            public required string? Error { get; set; }
        }

        private static BuildersResult Build(CompanyUserCreateCompaniesRequest request)
        {
            var isValid = true;
            var dictionary = new Dictionary<CompanyUserCreateCompaniesCommand, BuilderErrorMessage>();
            foreach (var command in request.Commands)
            {
                var builder = PrepareBuilder(command);
                if (isValid && builder.HasErrors())
                {
                    isValid = false;
                }
                dictionary[command] = new BuilderErrorMessage
                {
                    Item = builder.Build(),
                    Error = builder.GetErrors(),
                };
            }
            return new BuildersResult
            {
                Dictionary = dictionary,
                IsValid = isValid,
            };
        }

        private static CommandsResponse<CompanyUserCreateCompaniesCommand> InvalidResponse(
            BuildersResult builders)
        {
            return new CommandsResponse<CompanyUserCreateCompaniesCommand>
            {
                Result = builders.Dictionary
                    .Select(pair => new BaseCommandResult<CompanyUserCreateCompaniesCommand>
                    {
                        Item = pair.Key,
                        IsCorrect = string.IsNullOrWhiteSpace(pair.Value.Error),
                        Message = pair.Value.Error ?? string.Empty,
                    }),
                HttpCode = HttpCode.BadRequest,
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserCreateCompaniesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
