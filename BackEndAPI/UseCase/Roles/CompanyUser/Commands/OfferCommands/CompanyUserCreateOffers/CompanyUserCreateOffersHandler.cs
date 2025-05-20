using Domain.Features.Offers.Enums;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using System.Text;
using UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserCreateOffers.Request;
using UseCase.Roles.CompanyUser.Repositories.Offers;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainOffer = Domain.Features.Offers.Aggregates.Offer;

namespace UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserCreateOffers
{
    public class CompanyUserCreateOffersHandler : IRequestHandler<CompanyUserCreateOffersRequest, CommandsResponse<CompanyUserCreateOffersCommand>>
    {
        // Property
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IOfferRepository _offerRepository;


        // Constructor
        public CompanyUserCreateOffersHandler(
            IAuthenticationInspectorService authenticationInspector,
            IOfferRepository offerRepository)
        {
            _authenticationInspector = authenticationInspector;
            _offerRepository = offerRepository;
        }


        // Methods
        public async Task<CommandsResponse<CompanyUserCreateOffersCommand>> Handle(CompanyUserCreateOffersRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var buildResult = Build(request);

            if (!buildResult.IsValid)
            {
                return new CommandsResponse<CompanyUserCreateOffersCommand>
                {
                    Result = buildResult.Dictionary.Select(pair => new BaseCommandResult<CompanyUserCreateOffersCommand>
                    {
                        Item = pair.Key,
                        IsCorrect = string.IsNullOrWhiteSpace(pair.Value.Error),
                        Message = pair.Value.Error ?? string.Empty,
                    }),
                    HttpCode = HttpCode.BadRequest,
                };
            }

            var dictionary = buildResult.Dictionary.ToDictionary(
                val => val.Key,
                val => val.Value.Offer);
            var repositoryResult = await _offerRepository.CreateAsync(
                personId,
                dictionary.Values,
                cancellationToken);

            return new CommandsResponse<CompanyUserCreateOffersCommand>
            {
                Result = dictionary.Select(pair => new BaseCommandResult<CompanyUserCreateOffersCommand>
                {
                    Item = pair.Key,
                    IsCorrect = repositoryResult.Dictionary[pair.Value].IsCorrect,
                    Message = repositoryResult.Dictionary[pair.Value].Message,
                }),
                HttpCode = repositoryResult.Code,
            };
        }

        // Private Static Methods
        private static DomainOffer.Builder PrepareBuilder(CompanyUserCreateOffersCommand command)
        {
            return new DomainOffer.Builder()
                .SetBranchId(command.BranchId)
                .SetPublicationRange(command.PublicationStart, command.PublicationEnd)
                .SetEmploymentLength(command.EmploymentLength)
                .SetWebsiteUrl(command.WebsiteUrl)
                .SetOfferTemplate((TemplateInfo)command.OfferTemplateId)
                .SetContractConditions(
                    command.ConditionIds
                    .Select(id => (ContractInfo)id));
        }

        private sealed class BuildResult
        {
            public required Dictionary<CompanyUserCreateOffersCommand, OfferAndError> Dictionary { get; init; }
            public required bool IsValid { get; init; }
        }

        private sealed class OfferAndError
        {
            public required DomainOffer Offer { get; init; }
            public required string? Error { get; init; }
        }

        private static BuildResult Build(CompanyUserCreateOffersRequest request)
        {
            var isValid = true;
            var dictionary = new Dictionary<CompanyUserCreateOffersCommand, OfferAndError>();
            var stringBuilder = new StringBuilder();

            foreach (var command in request.Commands)
            {
                stringBuilder.Clear();

                var builder = PrepareBuilder(command);
                var item = builder.Build();
                if (builder.HasErrors())
                {
                    stringBuilder.AppendLine(builder.GetErrors());
                }
                if (item.Status == OfferStatus.Expired ||
                    item.Status == OfferStatus.Active)
                {
                    stringBuilder.AppendLine(Messages.Entity_Offers_Status_Started);
                }

                if (isValid && stringBuilder.Length > 0)
                {
                    isValid = false;
                }

                dictionary[command] = new OfferAndError
                {
                    Offer = item,
                    Error = stringBuilder.ToString(),
                };
            }
            return new BuildResult
            {
                Dictionary = dictionary,
                IsValid = isValid,
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserCreateOffersRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
