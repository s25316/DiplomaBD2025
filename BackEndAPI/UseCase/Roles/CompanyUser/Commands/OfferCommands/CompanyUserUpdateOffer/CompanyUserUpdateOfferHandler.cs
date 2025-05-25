using Domain.Features.Offers.Enums;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserUpdateOffer.Request;
using UseCase.Roles.CompanyUser.Repositories.Offers;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainOffer = Domain.Features.Offers.Aggregates.Offer;
using DomainOfferException = Domain.Features.Offers.Exceptions.OfferException;

namespace UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserUpdateOffer
{
    public class CompanyUserUpdateOfferHandler : IRequestHandler<CompanyUserUpdateOfferRequest, CommandResponse<CompanyUserUpdateOfferCommand>>
    {
        // Property
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IOfferRepository _offerRepository;


        // Constructor
        public CompanyUserUpdateOfferHandler(
            IAuthenticationInspectorService authenticationInspector,
            IOfferRepository offerRepository)
        {
            _authenticationInspector = authenticationInspector;
            _offerRepository = offerRepository;
        }


        // Methods
        public async Task<CommandResponse<CompanyUserUpdateOfferCommand>> Handle(CompanyUserUpdateOfferRequest request, CancellationToken cancellationToken)
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
        private static CommandResponse<CompanyUserUpdateOfferCommand> PrepareResponse(
            HttpCode code,
            CompanyUserUpdateOfferRequest request,
            string? message = null)
        {
            return CommandResponse<CompanyUserUpdateOfferCommand>.PrepareResponse(
                code,
                request.Command,
                message);
        }

        private static DomainOffer.Updater Update(
            DomainOffer item,
            CompanyUserUpdateOfferRequest request)
        {
            var command = request.Command;
            var updater = new DomainOffer.Updater(item)
                        .SetEmploymentLength(command.EmploymentLength)
                        .SetPublicationRange(
                            command.PublicationStart,
                            command.PublicationEnd)
                        .SetWebsiteUrl(command.WebsiteUrl);

            if (item.Status == OfferStatus.Scheduled)
            {
                updater
                    .SetBranchId(command.BranchId)
                    .SetContractConditions(
                        command.ConditionIds
                        .Select(cc => (ContractInfo)cc));

                if (command.OfferTemplateId.HasValue)
                {
                    updater.SetOfferTemplate(command.OfferTemplateId);
                }
            }
            return updater;
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserUpdateOfferRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
