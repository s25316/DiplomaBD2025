using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Response;
using UseCase.Roles.CompanyUser.Repositories.OfferTemplates;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove
{
    class OfferTemplateRemoveHandler : IRequestHandler<OfferTemplateRemoveRequest, OfferTemplateRemoveResponse>
    {
        // Property
        private readonly IAuthenticationInspectorService _inspectorService;
        private readonly IOfferTemplateRepository _offerTemplateRepository;


        // Constructor
        public OfferTemplateRemoveHandler(
            IAuthenticationInspectorService inspectorService,
            IOfferTemplateRepository offerTemplateRepository)
        {
            _inspectorService = inspectorService;
            _offerTemplateRepository = offerTemplateRepository;
        }


        // Methods
        public async Task<OfferTemplateRemoveResponse> Handle(OfferTemplateRemoveRequest request, CancellationToken cancellationToken)
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
        private static OfferTemplateRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return OfferTemplateRemoveResponse.PrepareResponse(code, message);
        }

        // Private Non Static Methods
        private PersonId GetPersonId(OfferTemplateRemoveRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }
    }
}
