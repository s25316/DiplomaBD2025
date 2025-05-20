using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserRemoveOfferTemplate.Request;
using UseCase.Roles.CompanyUser.Repositories.OfferTemplates;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserRemoveOfferTemplate
{
    class CompanyUserRemoveOfferTemplateHandler : IRequestHandler<CompanyUserRemoveOfferTemplateRequest, ResultMetadataResponse>
    {
        // Property
        private readonly IAuthenticationInspectorService _inspectorService;
        private readonly IOfferTemplateRepository _offerTemplateRepository;


        // Constructor
        public CompanyUserRemoveOfferTemplateHandler(
            IAuthenticationInspectorService inspectorService,
            IOfferTemplateRepository offerTemplateRepository)
        {
            _inspectorService = inspectorService;
            _offerTemplateRepository = offerTemplateRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(CompanyUserRemoveOfferTemplateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _offerTemplateRepository
                .GetAsync(personId, request.OfferTemplateId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    selectResult.Metadata.Message);
            }

            var item = selectResult.Item;
            item.Remove();

            var removeResponse = await _offerTemplateRepository
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
        private PersonId GetPersonId(CompanyUserRemoveOfferTemplateRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
