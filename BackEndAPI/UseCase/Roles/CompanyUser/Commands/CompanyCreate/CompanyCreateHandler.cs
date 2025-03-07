using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Response;
using UseCase.Roles.CompanyUser.Commands.Repositories.Companies;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCreate
{
    public class CompanyCreateHandler : IRequestHandler<CompanyCreateRequest, CompanyCreateResponse>
    {
        // Properties
        private readonly ICompanyRepository _companyRepository;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public CompanyCreateHandler(
            ICompanyRepository companyRepository,
            IAuthenticationInspectorService authenticationInspector)
        {
            _companyRepository = companyRepository;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        async Task<CompanyCreateResponse> IRequestHandler<CompanyCreateRequest, CompanyCreateResponse>.Handle(CompanyCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var builder = PrepareBuilder(request.Command);
            var domain = builder.Build();

            if (builder.HasErrors())
            {
                return new CompanyCreateResponse
                {
                    Command = new Shared.Templates.Response.ResponseCommandTemplate<CompanyCreateCommand>
                    {
                        Item = request.Command,
                        IsCorrect = false,
                        Message = builder.GetErrors(),
                    },
                    IsCorrect = false,
                    HttpCode = HttpCode.BadRequest,
                };
            }

            var duplicates = await _companyRepository
                .FindDuplicatesAsync(domain, cancellationToken);

            if (!string.IsNullOrWhiteSpace(duplicates))
            {
                return new CompanyCreateResponse
                {
                    Command = new Shared.Templates.Response.ResponseCommandTemplate<CompanyCreateCommand>
                    {
                        Item = request.Command,
                        IsCorrect = false,
                        Message = duplicates,
                    },
                    IsCorrect = false,
                    HttpCode = HttpCode.BadRequest,
                };
            }
            await _companyRepository.CreateAsync(
                personId,
                domain,
                cancellationToken);
            return new CompanyCreateResponse
            {
                Command = new Shared.Templates.Response.ResponseCommandTemplate<CompanyCreateCommand>
                {
                    Item = request.Command,
                    IsCorrect = true,
                    Message = HttpCode.Created.Description(),
                },
                IsCorrect = true,
                HttpCode = HttpCode.Created,
            };
        }

        // Private Methods
        private PersonId GetPersonId(CompanyCreateRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private static DomainCompany.Builder PrepareBuilder(CompanyCreateCommand command)
        {
            return new DomainCompany.Builder()
                .SetName(command.Name)
                .SetDescription(command.Description)
                .SetRegon(command.Regon)
                .SetNip(command.Nip)
                .SetKrs(command.Krs)
                .SetWebsiteUrl(command.WebsiteUrl);
        }
    }
}
