using Domain.Features.Offers.Exceptions;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.OfferRemove.Request;
using UseCase.Roles.CompanyUser.Commands.OfferRemove.Response;
using UseCase.Roles.CompanyUser.Repositories.Offers;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.OfferRemove
{
    public class OfferRemoveHandler : IRequestHandler<OfferRemoveRequest, OfferRemoveResponse>
    {
        // Property
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IOfferRepository _offerRepository;


        // Constructor
        public OfferRemoveHandler(
            IAuthenticationInspectorService authenticationInspector,
            IOfferRepository offerRepository)
        {
            _authenticationInspector = authenticationInspector;
            _offerRepository = offerRepository;
        }


        // Methods
        public async Task<OfferRemoveResponse> Handle(OfferRemoveRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var selectResult = await _offerRepository
                .GetAsync(personId, request.OfferId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    selectResult.Metadata.Message);
            }

            var item = selectResult.Item;

            try
            {
                item.Remove();
            }
            catch (OfferException ex)
            {
                return PrepareResponse(
                    HttpCode.BadRequest,
                    ex.Message);
            }

            var removeResponse = await _offerRepository
                .RemoveAsync(personId, item, cancellationToken);

            return PrepareResponse(
                removeResponse.Code,
                removeResponse.Metadata.Message);
        }

        // Private Static Methods
        private static OfferRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return OfferRemoveResponse.PrepareResponse(code, message);
        }

        // Private Non Static Methods
        private PersonId GetPersonId(OfferRemoveRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
