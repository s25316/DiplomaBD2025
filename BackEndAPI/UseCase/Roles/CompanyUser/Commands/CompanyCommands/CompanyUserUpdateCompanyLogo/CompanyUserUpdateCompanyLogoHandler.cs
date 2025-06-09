using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.MongoDb.Enums;
using UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserUpdateCompanyLogo.Request;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.Companies;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserUpdateCompanyLogo
{
    public class CompanyUserUpdateCompanyLogoHandler : IRequestHandler<CompanyUserUpdateCompanyLogoRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly ICompanyRepository _companyRepository;
        private readonly IMongoDbFileService _mongoDbFileService;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public CompanyUserUpdateCompanyLogoHandler(
            ICompanyRepository companyRepository,
            IMongoDbFileService mongoDbFileService,
            IAuthenticationInspectorService authenticationInspector)
        {
            _companyRepository = companyRepository;
            _mongoDbFileService = mongoDbFileService;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(CompanyUserUpdateCompanyLogoRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _companyRepository.GetAsync(personId, request.CompanyId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(selectResult.Code);
            }
            var domainCompany = selectResult.Item;


            if (!string.IsNullOrWhiteSpace(domainCompany.Logo))
            {
                await _mongoDbFileService.DeleteFileAsync(
                    domainCompany.Logo,
                    MongoDbCollection.CompanyLogo,
                    cancellationToken);
            }
            var fileId = await _mongoDbFileService.SaveAsync(
                request.File,
                MongoDbCollection.CompanyLogo,
                cancellationToken);

            var updater = PrepareUpdater(domainCompany, fileId);
            if (updater.HasErrors())
            {
                throw new UseCaseLayerException(updater.GetErrors());
            }
            domainCompany = updater.Build();


            var updateResult = await _companyRepository.UpdateAsync(personId, domainCompany, cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException(updateResult.Metadata.Message);
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

        private static DomainCompany.Updater PrepareUpdater(DomainCompany item, string file)
        {
            return new DomainCompany.Updater(item)
                .SetLogo(file);

        }
        // Non Static Methods
        private PersonId GetPersonId(CompanyUserUpdateCompanyLogoRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
