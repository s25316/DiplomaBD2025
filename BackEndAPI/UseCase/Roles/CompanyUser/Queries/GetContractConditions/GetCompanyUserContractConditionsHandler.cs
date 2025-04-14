using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders.StringProvider;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Request;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Response;
using UseCase.Roles.CompanyUser.Queries.Template;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.ExtensionMethods.EF.ContractConditions;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetContractConditions
{
    public class GetCompanyUserContractConditionsHandler :
        GetCompanyUserGenericsBase<ContractCondition, CompanyAndContractConditionDto>,
        IRequestHandler<GetCompanyUserContractConditionsRequest, GetCompanyUserGenericItemsResponse<CompanyAndContractConditionDto>>
    {
        //Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserContractConditionsHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector
            ) : base(context, mapper, authenticationInspector)
        { }


        // Methods
        public async Task<GetCompanyUserGenericItemsResponse<CompanyAndContractConditionDto>> Handle(GetCompanyUserContractConditionsRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request.Metadata.Claims);
            var query = PrepareQuery(personId, request);
            var selector = BuildSelector(
                personId,
                _authorizedRoles,
                query,
                contractCondition => contractCondition.Company.CompanyPeople);

            var selectResult = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(
                selectResult,
                contractCondition => contractCondition.Company.Removed != null,
                contractCondition => new CompanyAndContractConditionDto
                {
                    Company = _mapper.Map<CompanyDto>(contractCondition.Company),
                    ContractCondition = _mapper.Map<ContractConditionDto>(contractCondition),
                });
        }

        private static IQueryable<ContractCondition> ApplyOrderBy(
            IQueryable<ContractCondition> query,
            CompanyUserContractConditionsOrderBy orderBy,
            bool ascending,
            bool showRemoved)
        {
            if (showRemoved && orderBy == CompanyUserContractConditionsOrderBy.ContractConditionRemoved)
            {
                return ascending
                        ? query.OrderBy(cc => cc.Removed)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.Removed)
                            .ThenByDescending(cc => cc.Created);
            }

            switch (orderBy)
            {
                case CompanyUserContractConditionsOrderBy.CompanyName:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Name);
                case CompanyUserContractConditionsOrderBy.CompanyCreated:
                    return ascending
                        ? query.OrderBy(cc => cc.Company.Created)
                            .ThenBy(cc => cc.Company.Name)
                        : query.OrderByDescending(cc => cc.Company.Created)
                            .ThenByDescending(cc => cc.Company.Name);
                case CompanyUserContractConditionsOrderBy.SalaryMin:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMin)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMin)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryMax:
                    return ascending
                        ? query.OrderBy(cc => cc.SalaryMax)
                            .ThenBy(cc => cc.Created)
                        : query.OrderByDescending(cc => cc.SalaryMax)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryAvg:
                    return ascending
                        ? query
                            .OrderBy(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => (cc.SalaryMin + cc.SalaryMax) / 2)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryPerHourMin:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMin / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryPerHourMax:
                    return ascending
                        ? query
                            .OrderBy(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => cc.SalaryMax / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                case CompanyUserContractConditionsOrderBy.SalaryPerHourAvg:
                    return ascending
                        ? query
                            .OrderBy(cc => ((cc.SalaryMax + cc.SalaryMin) / 2) / cc.HoursPerTerm)
                            .ThenBy(cc => cc.Created)
                        : query
                            .OrderByDescending(cc => ((cc.SalaryMax + cc.SalaryMin) / 2) / cc.HoursPerTerm)
                            .ThenByDescending(cc => cc.Created);
                default://case CompanyUserContractConditionsOrderBy.ContractConditionCreated:
                    return ascending
                        ? query.OrderBy(cc => cc.Created)
                            .ThenBy(cc => cc.SalaryMin)
                        : query.OrderByDescending(cc => cc.Created)
                            .ThenByDescending(cc => cc.SalaryMin);
            }
        }

        // Private Non Static Methods         
        private IQueryable<ContractCondition> BaseQuery()
        {
            return _context.ContractConditions
                .Include(cc => cc.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(cc => cc.ContractAttributes)
                .ThenInclude(c => c.ContractParameter)
                .ThenInclude(c => c.ContractParameterType)
                .AsNoTracking();
        }

        private IQueryable<ContractCondition> PrepareQuery(
            PersonId personId,
            GetCompanyUserContractConditionsRequest request)
        {
            // Base Configuration
            var query = BaseQuery();

            // Filter ContractCondition if have no access to it
            if (request.ContractConditionId.HasValue)
            {
                return query.Where(cc => cc.ContractConditionId == request.ContractConditionId);
            }
            // Filter Companies if have no access to it
            if (request.CompanyId.HasValue ||
                request.CompanyParameters.ContainsAny())
            {
                query = query.Where(cc => _context.Companies
                    .IdentificationFilter(request.CompanyId, request.CompanyParameters)
                    .Any(company => company.CompanyId == cc.CompanyId));
            }
            // Filter only to which have access
            else
            {
                query = query.Where(cc =>
                    cc.Company.Removed == null)
                    .Where(cc => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(role => role.CompanyId == cc.CompanyId));
            }

            // Show only removed, or only not removed
            query = query.Where(cc => request.ShowRemoved
                ? cc.Removed != null
                : cc.Removed == null);

            // Search Text Filter 
            var searchWords = CustomStringProvider
                .Split(request.SearchText, WhiteSpace.All);

            if (searchWords.Any())
            {
                query = query.Where(cc => _context.Companies
                    .SearchTextFilter(searchWords)
                    .Any(company => company.CompanyId == cc.CompanyId));
            }

            // Contract Parameters And Salary
            query = query.ContractParametersAndSalaryFilter(
                request.ContractParameterIds,
                request.SalaryParameters);

            query = ApplyOrderBy(
                query,
                request.OrderBy,
                request.Ascending,
                request.ShowRemoved);

            return query;
        }
    }
}
