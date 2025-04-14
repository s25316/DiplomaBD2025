// Ignore Spelling: dtos

using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders.StringProvider;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Request;
using UseCase.Roles.CompanyUser.Queries.Template;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanies
{
    public class GetCompanyUserCompaniesHandler :
        GetCompanyUserGenericsBase<Company, CompanyDto>,
        IRequestHandler<GetCompanyUserCompaniesRequest, GetCompanyUserGenericItemsResponse<CompanyDto>>
    {
        //Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];


        //Constructor
        public GetCompanyUserCompaniesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector
            ) : base(context, mapper, authenticationInspector)
        { }


        // Methods    
        public async Task<GetCompanyUserGenericItemsResponse<CompanyDto>> Handle(GetCompanyUserCompaniesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request.Metadata.Claims);
            var query = BuildQuery(request, personId);
            var selector = BuildSelector(
                personId,
                _authorizedRoles,
                query,
                company => company.CompanyPeople);

            var selectedValues = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(
                selectedValues,
                company => company.Removed != null,
                company => _mapper.Map<CompanyDto>(company));
        }

        // Private Static Methods
        private static IQueryable<Company> ApplyOrderBy(
            IQueryable<Company> query,
            CompanyUserCompaniesOrderBy orderBy,
            bool ascending)
        {
            switch (orderBy)
            {
                case CompanyUserCompaniesOrderBy.Name:
                    return ascending ?
                        query.OrderBy(company => company.Name) :
                        query.OrderByDescending(company => company.Name);
                default:
                    return ascending ?
                        query.OrderBy(company => company.Created) :
                        query.OrderByDescending(company => company.Created);
            }
        }

        // Private Non Static Methods
        private IQueryable<Company> BuildBaseQuery()
        {
            return _context.Companies
                .Include(c => c.CompanyPeople.Where(x => x.Deny == null))
                .AsNoTracking();
        }

        private IQueryable<Company> BuildQuery(
            GetCompanyUserCompaniesRequest request,
            PersonId personId)
        {
            var query = BuildBaseQuery();

            // Single Company
            if (request.CompanyId.HasValue ||
                request.CompanyParameters.ContainsAny())
            {
                return query
                    .IdentificationFilter(request.CompanyId, request.CompanyParameters);
            }

            // Companies
            var searchWords = CustomStringProvider
                .Split(request.SearchText, WhiteSpace.All);

            query = query.SearchTextFilter(searchWords);

            query = query
                .Where(company => company.Removed == null)
                .Where(company => _context.CompanyPeople
                    .WhereAuthorize(personId, _authorizedRoles)
                    .Any(role => role.CompanyId == company.CompanyId));

            query = ApplyOrderBy(
                query,
                request.OrderBy,
                request.Ascending);

            return query;
        }
    }
}
