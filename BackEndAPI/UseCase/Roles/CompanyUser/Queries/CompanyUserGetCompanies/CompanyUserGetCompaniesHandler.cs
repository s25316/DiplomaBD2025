using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetCompanies.Request;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetCompanies
{

    public class CompanyUserGetCompaniesHandler : IRequestHandler<CompanyUserGetCompaniesRequest, ItemsResponse<CompanyDto>>
    {
        // Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public CompanyUserGetCompaniesHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ItemsResponse<CompanyDto>> Handle(CompanyUserGetCompaniesRequest request, CancellationToken cancellationToken)
        {
            // Prepare Data
            var personId = GetPersonId(request.Metadata.Claims);
            var personIdValue = personId.Value;
            var roleIds = _authorizedRoles.Select(r => (int)r);

            // Prepare Query
            var baseQuery = PrepareBaseQuery(request, personId);
            var paginatedQuery = baseQuery.Paginate(
                request.Pagination.Page,
                request.Pagination.ItemsPerPage);

            // Execute Query
            var selectResult = await paginatedQuery.Select(item => new
            {
                Item = item,
                TotalCount = baseQuery.Count(),
                RolesCount = _context.CompanyPeople.Count(role => roleIds.Any(roleId =>
                    role.CompanyId == item.CompanyId &&
                    role.PersonId == personIdValue &&
                    role.RoleId == roleId &&
                    role.Deny == null
                )),
            }).ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.NotFound, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<CompanyDto>();
            foreach (var item in selectResult)
            {
                if (item.RolesCount == 0)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
                }
                if (item.Item.Removed != null)
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }
                items.Add(_mapper.Map<CompanyDto>(item.Item));
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<CompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<CompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<CompanyDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private PersonId GetPersonId(IEnumerable<Claim> claims)
        {
            return _authenticationInspector.GetPersonId(claims);
        }

        private IQueryable<Company> PrepareBaseQuery()
        {
            return _context.Companies.AsNoTracking();
        }

        private IQueryable<Company> PrepareBaseQuery(
            CompanyUserGetCompaniesRequest request,
            PersonId personId)
        {
            var query = PrepareBaseQuery();

            if (request.CompanyQueryParameters.HasValue || request.CompanyId.HasValue)
            {
                return query.WhereIdentificationData(
                    request.CompanyId,
                    request.CompanyQueryParameters);
            }
            query = query.WhereText(request.SearchText);

            query = query
                .Where(company => company.Removed == null)
                .Where(company => _context.CompanyPeople
                    .WhereAuthorize(personId, _authorizedRoles)
                    .Any(role => role.CompanyId == company.CompanyId));

            return query.OrderBy(
                request.OrderBy,
                request.Ascending);
        }
    }
}
