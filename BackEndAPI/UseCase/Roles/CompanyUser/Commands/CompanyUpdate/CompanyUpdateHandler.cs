using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Response;
using UseCase.Roles.CompanyUser.Repositories.Companies;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.CompanyUpdate
{
    public class CompanyUpdateHandler : IRequestHandler<CompanyUpdateRequest, CompanyUpdateResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly ICompanyRepository _companyRepository;


        // Constructor
        public CompanyUpdateHandler(
            IAuthenticationInspectorService authenticationInspector,
            ICompanyRepository companyRepository)
        {
            _authenticationInspector = authenticationInspector;
            _companyRepository = companyRepository;
        }


        // Methods
        public async Task<CompanyUpdateResponse> Handle(CompanyUpdateRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Command.Krs) &&
                string.IsNullOrWhiteSpace(request.Command.Description) &&
                string.IsNullOrWhiteSpace(request.Command.WebsiteUrl))
            {
                return PrepareResponse(
                    HttpCode.Ok,
                    HttpCode.Ok.Description(),
                    request);
            }

            // Get from DB
            var personId = GetPersonId(request);
            var getResult = await _companyRepository.GetAsync(
                personId,
                request.CompanyId,
                cancellationToken);

            if (getResult.Code != HttpCode.Ok)
            {
                // If Invalid
                return PrepareResponse(
                    getResult.Code,
                    getResult.Metadata.Message,
                    request);
            }

            // Update in Domain
            var domainCompany = getResult.Item;
            var updater = PrepareUpdater(domainCompany, request.Command);

            if (updater.HasErrors())
            {
                return PrepareResponse(
                    HttpCode.BadRequest,
                    updater.GetErrors(),
                    request);
            }

            // Update in DB
            var updateResult = await _companyRepository.UpdateAsync(
                personId,
                updater.Build(),
                cancellationToken);

            // If is OK on NOT
            return PrepareResponse(
                    updateResult.Code,
                    updateResult.Metadata.Message,
                    request);
        }

        // Private Static Methods 
        private static CompanyUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            CompanyUpdateRequest request)
        {
            return CompanyUpdateResponse.PrepareResponse(
                code,
                message,
                request.Command);
        }

        private static DomainCompany.Updater PrepareUpdater(
            DomainCompany domain,
            CompanyUpdateCommand command)
        {
            var updater = new DomainCompany.Updater(domain);
            if (!string.IsNullOrWhiteSpace(command.Krs))
            {
                updater.SetKrs(command.Krs);
            }
            if (!string.IsNullOrWhiteSpace(command.Description))
            {
                updater.SetDescription(command.Description);
            }
            if (!string.IsNullOrWhiteSpace(command.WebsiteUrl))
            {
                updater.SetWebsiteUrl(command.WebsiteUrl);
            }
            return updater;
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUpdateRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
