using Domain.Features.Branches.ValueObjects;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using MediatR;
using System.Text;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Response;
using UseCase.Roles.CompanyUser.Commands.Repositories.Offers;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.DTOs.Responses.Dictionaries;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response;
using DomainOffer = Domain.Features.Offers.Entities.Offer;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate
{
    public class OffersCreateHandler : IRequestHandler<OffersCreateRequest, OffersCreateResponse>
    {
        // Properties 
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IDictionariesRepository _dictionariesRepository;
        private readonly IOfferRepository _offerRepository;


        // Constructor
        public OffersCreateHandler(
            IAuthenticationInspectorService authenticationInspector,
            IDictionariesRepository dictionariesRepository,
            IOfferRepository offerRepository)
        {
            _authenticationInspector = authenticationInspector;
            _dictionariesRepository = dictionariesRepository;
            _offerRepository = offerRepository;
        }


        // Methods
        public async Task<OffersCreateResponse> Handle(OffersCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var isAllowedOperation = await _offerRepository.IsAllowedOperationAsync(
                personId,
                request.CompanyId,
                request.OfferTemplateId,
                cancellationToken);

            if ((int)isAllowedOperation != 200)
            {
                return new OffersCreateResponse
                {
                    Commands = request.Commands.Select(cmd => new ResponseItemTemplate<OfferCreateCommand>
                    {
                        Item = cmd,
                        IsCorrect = false,
                        Message = isAllowedOperation.ToString(),
                    }),
                    HttpCode = isAllowedOperation,
                    IsCorrect = false,
                };
            }

            var validationResult = await CheckRequestAsync(request, cancellationToken);
            if (!validationResult.IsValidRequest)
            {
                return new OffersCreateResponse
                {
                    Commands = validationResult.Dictionary.Select(pair => new ResponseItemTemplate<OfferCreateCommand>
                    {
                        Item = pair.Key,
                        IsCorrect = string.IsNullOrWhiteSpace(pair.Value.ErrorMessage),
                        Message = pair.Value.ErrorMessage,
                    }),
                    HttpCode = HttpCode.BadRequest,
                    IsCorrect = false,
                };
            }

            var domainOffers = validationResult.Dictionary.Select(pair => pair.Value.Builder.Build());
            await _offerRepository.CreateAsync(domainOffers, cancellationToken);

            return new OffersCreateResponse
            {
                Commands = request.Commands.Select(cmd => new ResponseItemTemplate<OfferCreateCommand>
                {
                    Item = cmd,
                    IsCorrect = true,
                    Message = HttpCode.Created.Description(),
                }),
                HttpCode = HttpCode.Created,
                IsCorrect = true,
            };
        }

        // Private Methods
        private PersonId GetPersonId(OffersCreateRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        // Record for returning Complicated result for Method below
        private sealed record CheckRequestResult(
            bool IsValidRequest,
            Dictionary<OfferCreateCommand, (DomainOffer.Builder Builder, string ErrorMessage)> Dictionary);

        private async Task<CheckRequestResult> CheckRequestAsync(
            OffersCreateRequest request,
            CancellationToken cancellationToken)
        {
            var resultDictionary = new Dictionary<OfferCreateCommand, (DomainOffer.Builder Builder, string ErrorMessage)>();
            var currenciesDictionary = await _dictionariesRepository.GetCurrenciesAsync();
            var workModesDictionary = await _dictionariesRepository.GetWorkModesAsync();
            var salaryTermsDictionary = await _dictionariesRepository.GetSalaryTermsAsync();
            var employmentTypesDictionary = await _dictionariesRepository.GetEmploymentTypesAsync();

            var branchIds = request.Commands
                .Where(cmd => cmd.BranchId != null)
                .Select(cmd => new BranchId(cmd.BranchId.Value));
            var notFoundedBranchIds = await _offerRepository.GetNotFoundedBranchIdsAsync(
                request.CompanyId,
                branchIds,
                cancellationToken);
            var notFoundedBranchIdsHashSet = notFoundedBranchIds.ToHashSet();

            var isValidRequest = true;
            foreach (var command in request.Commands)
            {
                var builder = CreateDomainOfferBuilder(
                    request.OfferTemplateId,
                    command);

                var domain = builder.Build();
                var errors = new StringBuilder();
                if (builder.HasErrors())
                {
                    errors.AppendLine(builder.GetErrors());
                }

                var notFoundDictionaryValues = ReturnFoundDictionaryValues(
                   domain,
                   currenciesDictionary,
                   workModesDictionary,
                   salaryTermsDictionary,
                   employmentTypesDictionary);

                if (!string.IsNullOrWhiteSpace(notFoundDictionaryValues))
                {
                    errors.AppendLine(notFoundDictionaryValues);
                }

                if (
                    notFoundedBranchIdsHashSet.Any() &&
                    domain.BranchId != null &&
                    notFoundedBranchIdsHashSet.Contains(domain.BranchId))
                {
                    errors.AppendLine($"{nameof(OfferCreateCommand.BranchId)} not found: {domain.BranchId.Value}");
                }

                if (isValidRequest && errors.Length > 0)
                {
                    isValidRequest = false;
                }
                resultDictionary[command] = (builder, errors.ToString());
            }
            return new CheckRequestResult(isValidRequest, resultDictionary);
        }

        private DomainOffer.Builder CreateDomainOfferBuilder(
            Guid offerTemplateId,
            OfferCreateCommand command)
        {
            return new DomainOffer.Builder()
                    .SetOfferTemplateId(offerTemplateId)
                    .SetBranchId(command.BranchId)
                    .SetDatesRanges(
                    command.PublicationStart,
                    command.PublicationEnd,
                    command.WorkBeginDate,
                    command.WorkEndDate)
                    .SetSalaryData(
                    command.SalaryRangeMin,
                    command.SalaryRangeMax,
                    command.SalaryTermId,
                    command.CurrencyId,
                    command.IsNegotiated)
                    .SetWebsiteUrl(command.WebsiteUrl)
                    .SetEmploymentTypeIds(command.EmploymentTypeIds)
                    .SetWorkModeIds(command.WorkModeIds);
        }

        private string ReturnFoundDictionaryValues(
            DomainOffer domain,
            Dictionary<int, CurrencyDto> currenciesDictionary,
            Dictionary<int, WorkModeDto> workModesDictionary,
            Dictionary<int, SalaryTermDto> salaryTermsDictionary,
            Dictionary<int, EmploymentTypeDto> employmentTypesDictionary

            )
        {
            var errors = new StringBuilder();

            if (domain.SalaryTermId != null)
            {
                var notFoundSalaryTermId = ReturnNotFoundIds(
                [domain.SalaryTermId.Value],
                salaryTermsDictionary,
                nameof(OfferCreateCommand.SalaryTermId));

                if (!string.IsNullOrWhiteSpace(notFoundSalaryTermId))
                {
                    errors.AppendLine(notFoundSalaryTermId);
                }
            }
            if (domain.CurrencyId != null)
            {
                var notFoundCurrencyId = ReturnNotFoundIds(
                [domain.CurrencyId.Value],
                currenciesDictionary,
                nameof(OfferCreateCommand.CurrencyId));

                if (!string.IsNullOrWhiteSpace(notFoundCurrencyId))
                {
                    errors.AppendLine(notFoundCurrencyId);
                }
            }

            var notFoundEmploymentTypeIds = ReturnNotFoundIds(
                domain.EmploymentTypeIds,
                employmentTypesDictionary,
                nameof(OfferCreateCommand.EmploymentTypeIds));
            if (!string.IsNullOrWhiteSpace(notFoundEmploymentTypeIds))
            {
                errors.AppendLine(notFoundEmploymentTypeIds);
            }

            var notFoundWorkModeIds = ReturnNotFoundIds(
                domain.WorkModeIds,
                workModesDictionary,
                nameof(OfferCreateCommand.WorkModeIds));
            if (!string.IsNullOrWhiteSpace(notFoundWorkModeIds))
            {
                errors.AppendLine(notFoundWorkModeIds);
            }

            return errors.ToString().Trim();
        }

        private static string ReturnNotFoundIds<T>(
            IEnumerable<int> inputIds,
            Dictionary<int, T> dictionary,
            string propertyName)
        {
            var notFoundIds = new List<int>();

            foreach (var employmentTypeId in inputIds)
            {
                if (!dictionary.ContainsKey(employmentTypeId))
                {
                    notFoundIds.Add(employmentTypeId);
                }
            }
            if (notFoundIds.Any())
            {
                return $"Not found {propertyName}: {string.Join(", ", notFoundIds)}";
            }
            return string.Empty;
        }
    }
}
