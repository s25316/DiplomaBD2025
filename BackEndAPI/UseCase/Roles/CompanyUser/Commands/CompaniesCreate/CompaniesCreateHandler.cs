using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Repositories;
using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Response;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.Commands;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate
{
    public class CompaniesCreateHandler : IRequestHandler<CompaniesCreateRequest, CompaniesCreateResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly ICompanyRepository _companyRepository;


        // Constructor
        public CompaniesCreateHandler(
            IAuthenticationInspectorService authenticationInspector,
            ICompanyRepository companyRepository)
        {
            _authenticationInspector = authenticationInspector;
            _companyRepository = companyRepository;
        }


        // Methods
        public async Task<CompaniesCreateResponse> Handle(CompaniesCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var domainData = Build(request);

            if (!domainData.IsValid)
            {
                return new CompaniesCreateResponse
                {
                    Result = domainData.Dictionary
                    .Select(pair => new ResponseCommandTemplate<CompanyCreateCommand>
                    {
                        Item = pair.Key,
                        IsCorrect = string.IsNullOrWhiteSpace(pair.Value.Error),
                        Message = pair.Value.Error ?? string.Empty,
                    }),
                    HttpCode = HttpCode.BadRequest,
                };
            }

            var dictionary = domainData.Dictionary.ToDictionary(
                val => val.Key,
                val => val.Value.Item);
            var createResult = await _companyRepository.CreateAsync(
                personId,
                dictionary.Values,
                cancellationToken);

            return new CompaniesCreateResponse
            {
                Result = dictionary.Select(pair => new ResponseCommandTemplate<CompanyCreateCommand>
                {
                    Item = pair.Key,
                    IsCorrect = createResult.Dictionary[pair.Value].IsCorrect,
                    Message = createResult.Dictionary[pair.Value].Message,
                }),
                HttpCode = createResult.Code,
            };
        }

        // Private Static Methods
        private static DomainCompany.Builder PrepareBuilder(CompanyCreateCommand command)
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
            public required Dictionary<CompanyCreateCommand, BuilderErrorMessage> Dictionary;
            public required bool IsValid;
        }

        private sealed class BuilderErrorMessage
        {
            public required DomainCompany Item { get; init; }
            public required string? Error { get; set; }
        }

        private static BuildersResult Build(CompaniesCreateRequest request)
        {
            var isValid = true;
            var dictionary = new Dictionary<CompanyCreateCommand, BuilderErrorMessage>();
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

        // Private Non Static Methods
        private PersonId GetPersonId(CompaniesCreateRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
