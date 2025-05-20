using Domain.Features.Offers.Exceptions;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserRemoveOffer.Request;
using UseCase.Roles.CompanyUser.Repositories.Offers;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserRemoveOffer
{
    public class CompanyUserRemoveOfferHandler : IRequestHandler<CompanyUserRemoveOfferRequest, ResultMetadataResponse>
    {
        // Property
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IOfferRepository _offerRepository;


        // Constructor
        public CompanyUserRemoveOfferHandler(
            IAuthenticationInspectorService authenticationInspector,
            IOfferRepository offerRepository)
        {
            _authenticationInspector = authenticationInspector;
            _offerRepository = offerRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(CompanyUserRemoveOfferRequest request, CancellationToken cancellationToken)
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
        private static ResultMetadataResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(code, message);
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserRemoveOfferRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
