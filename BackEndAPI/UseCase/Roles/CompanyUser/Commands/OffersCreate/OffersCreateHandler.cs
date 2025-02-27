using Domain.Features.People.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Response;
using UseCase.Roles.Guests.Queries.Dictionaries.Repositories;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Services.Time;
using UseCase.Shared.Templates.Requests;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate
{
    public class OffersCreateHandler : IRequestHandler<OffersCreateRequest, OffersCreateResponse>
    {
        // Properties 
        private readonly DiplomaBdContext _context;
        private readonly ITimeService _timeService;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IDictionariesRepository _dictionariesRepository;

        // Constructor
        public OffersCreateHandler(
            DiplomaBdContext context,
            ITimeService timeService,
            IAuthenticationInspectorService authenticationInspector,
            IDictionariesRepository dictionariesRepository)
        {
            _context = context;
            _timeService = timeService;
            _authenticationInspector = authenticationInspector;
            _dictionariesRepository = dictionariesRepository;
        }


        // Methods
        public async Task<OffersCreateResponse> Handle(OffersCreateRequest request, CancellationToken cancellationToken)
        {
            var commands = new List<BaseResponseGeneric<OfferCreateCommand>>();
            var offers = new List<Offer>();

            var workModesDictionary = await _dictionariesRepository.GetWorkModesAsync();
            var employmentTypesDictionary = await _dictionariesRepository.GetEmploymentTypesAsync();
            var salaryTermsDictionary = await _dictionariesRepository.GetSalaryTermsAsync();
            var currenciesDictionary = await _dictionariesRepository.GetSalaryTermsAsync();
            foreach (var command in request.Commands)
            {
                var builder = new StringBuilder();
                if (
                    command.SalaryTermId != null &&
                    !salaryTermsDictionary.ContainsKey(command.SalaryTermId.Value)
                    )
                {
                    builder.AppendLine($"SalaryTermId not found: {command.SalaryTermId.Value}");
                }
                if (
                    command.CurrencyId != null &&
                    !currenciesDictionary.ContainsKey(command.CurrencyId.Value)
                    )
                {
                    builder.AppendLine($"CurrencyId not found: {command.CurrencyId.Value}");
                }

                var notFoundIds = new List<int>();
                foreach (var workModeId in command.WorkModes)
                {
                    if (!workModesDictionary.ContainsKey(workModeId))
                    {
                        notFoundIds.Add(workModeId);
                    }
                }
                if (notFoundIds.Any())
                {
                    builder.AppendLine(
                        $"WorkModeId not found: {string.Join(", ", notFoundIds)}"
                        );
                }

                notFoundIds.Clear();
                foreach (var employmentTypeId in command.EmploymentTypes)
                {
                    if (!employmentTypesDictionary.ContainsKey(employmentTypeId))
                    {
                        notFoundIds.Add(employmentTypeId);
                    }
                }
                if (notFoundIds.Any())
                {
                    builder.AppendLine(
                    $"EmploymentTypeId not found: {string.Join(", ", notFoundIds)}"
                    );
                }

                if (builder.Length > 0)
                {
                    commands.Add(new BaseResponseGeneric<OfferCreateCommand>
                    {
                        Item = command,
                        IsSuccess = false,
                        Message = builder.ToString()
                    });
                    continue;
                }

                offers.Add(new Offer
                {
                    OfferTemplateId = request.OfferTemplateId,
                    BranchId = command.BranchId,
                    PublicationStart = command.PublicationStart,
                    PublicationEnd = command.PublicationEnd,
                    WorkBeginDate = command.WorkBeginDate == null ?
                        null :
                        DateOnly.FromDateTime(command.WorkBeginDate.Value),

                    WorkEndDate = command.WorkEndDate == null ?
                        null :
                        DateOnly.FromDateTime(command.WorkEndDate.Value),
                    SalaryRangeMin = command.SalaryRangeMin,
                    SalaryRangeMax = command.SalaryRangeMax,
                    SalaryTermId = command.SalaryTermId,
                    CurrencyId = command.CurrencyId,
                    IsNegotiated = command.IsNegotiated,
                    WebsiteUrl = command.WebsiteUrl,
                });
                commands.Add(new BaseResponseGeneric<OfferCreateCommand>
                {
                    Item = command,
                    IsSuccess = true,
                    Message = "Success"
                });
            }

            await _context.Offers.AddRangeAsync(offers, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new OffersCreateResponse
            {
                Commands = commands,
            };
        }

        private async Task<bool> HasAccessToCompanyAsync(
            Guid companyId,
            PersonId personId,
            CancellationToken cancellationToken)
        {
            var roleIds = await _context.CompanyPeople.Where(cp =>
                cp.PersonId == personId.Value &&
                cp.CompanyId == companyId &&
                cp.Deny == null)
                .Select(cp => cp.RoleId)
                .ToListAsync(cancellationToken);
            return roleIds.Any();
        }


        private PersonId GetPersonId(RequestMetadata metadata)
        {
            return _authenticationInspector.GetPersonId(metadata.Claims);
        }

        private DateTime Now()
        {
            return _timeService.GetNow();
        }
    }
}
