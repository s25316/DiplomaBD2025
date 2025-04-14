using Domain.Features.Offers.ValueObjects.Enums;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.OfferUpdate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferUpdate.Response;
using UseCase.Roles.CompanyUser.Repositories.Offers;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainOffer = Domain.Features.Offers.Aggregates.Offer;
using DomainOfferException = Domain.Features.Offers.Exceptions.OfferException;

namespace UseCase.Roles.CompanyUser.Commands.OfferUpdate
{
    public class OfferUpdateHandler : IRequestHandler<OfferUpdateRequest, OfferUpdateResponse>
    {
        // Property
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IOfferRepository _offerRepository;


        // Constructor
        public OfferUpdateHandler(
            IAuthenticationInspectorService authenticationInspector,
            IOfferRepository offerRepository)
        {
            _authenticationInspector = authenticationInspector;
            _offerRepository = offerRepository;
        }


        // Methods
        public async Task<OfferUpdateResponse> Handle(OfferUpdateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);

            // Get from Db
            var selectResult = await _offerRepository.GetAsync(
                personId,
                request.OfferId,
                cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    request,
                    selectResult.Metadata.Message);
            }

            try
            {
                // Update in Domain
                var updater = Update(selectResult.Item, request);
                if (updater.HasErrors())
                {
                    return PrepareResponse(
                        HttpCode.BadRequest,
                        request,
                        updater.GetErrors());
                }

                var updateResult = await _offerRepository.UpdateAsync(
                    personId,
                    updater.Build(),
                    cancellationToken);

                return PrepareResponse(
                        updateResult.Code,
                        request,
                        updateResult.Metadata.Message);
            }
            catch (DomainOfferException ex)
            {
                return PrepareResponse(
                    HttpCode.BadRequest,
                    request,
                    ex.Message);
            }
        }

        // Private Non Static Methods
        private static OfferUpdateResponse PrepareResponse(
            HttpCode code,
            OfferUpdateRequest request,
            string? message = null)
        {
            return OfferUpdateResponse
                .PrepareResponse(code, message, request.Command);
        }

        private static DomainOffer.Updater Update(
            DomainOffer item,
            OfferUpdateRequest request)
        {
            var command = request.Command;
            var updater = new DomainOffer.Updater(item)
                        .SetPublicationRange(
                            command.PublicationStart,
                            command.PublicationEnd)
                        .SetWebsiteUrl(command.WebsiteUrl);

            if (item.Status == OfferStatus.Scheduled)
            {
                updater
                    .SetOfferTemplate(command.OfferTemplateId)
                    .SetBranchId(command.BranchId)
                    .SetEmploymentLength(command.EmploymentLength)
                    .SetContractConditions(
                        command.ConditionIds
                        .Select(cc => (ContractInfo)cc));
            }
            return updater;
        }

        // Private Non Static Methods
        private PersonId GetPersonId(OfferUpdateRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
