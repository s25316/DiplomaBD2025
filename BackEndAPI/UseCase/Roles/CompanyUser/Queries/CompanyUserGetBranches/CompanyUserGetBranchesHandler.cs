using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches
{
    public class CompanyUserGetBranchesHandler : IRequestHandler<CompanyUserGetBranchesRequest, ItemsResponse<CompanyUserBranchAndCompanyDto>>
    {
        // Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public CompanyUserGetBranchesHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ItemsResponse<CompanyUserBranchAndCompanyDto>> Handle(CompanyUserGetBranchesRequest request, CancellationToken cancellationToken)
        {
            // Prepare Data
            var personId = GetPersonId(request.Metadata.Claims);
            var personIdValue = personId.Value;
            var roleIds = _authorizedRoles.Select(r => (int)r);

            // Prepare Query
            var baseQuery = PrepareQuery(request, personId);
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
                return PrepareResponse(HttpCode.Ok, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<CompanyUserBranchAndCompanyDto>();
            foreach (var item in selectResult)
            {
                if (item.Item.Company.Removed != null)
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }
                if (item.RolesCount == 0)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
                }
                items.Add(new CompanyUserBranchAndCompanyDto
                {
                    Company = _mapper.Map<CompanyDto>(item.Item.Company),
                    Branch = _mapper.Map<CompanyUserBranchDto>(item.Item),
                });
            }
            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<CompanyUserBranchAndCompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<CompanyUserBranchAndCompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<CompanyUserBranchAndCompanyDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
        private PersonId GetPersonId(IEnumerable<Claim> claims)
        {
            return _authenticationInspector.GetPersonId(claims);
        }

        private IQueryable<Branch> PrepareBaseQuery()
        {
            return _context.Branches
                .Include(branch => branch.Company)

                .Include(b => b.Address)
                .ThenInclude(a => a.Street)

                .Include(b => b.Address)
                .ThenInclude(a => a.City)
                .ThenInclude(a => a.State)
                .ThenInclude(a => a.Country)
                .AsNoTracking();
        }

        private IQueryable<Branch> PrepareQuery(
            CompanyUserGetBranchesRequest request,
            PersonId personId)
        {
            var query = PrepareBaseQuery();

            // Branch Query Parameters
            if (request.BranchId.HasValue)
            {
                return query.Where(branch =>
                    branch.BranchId == request.BranchId.Value);
            }
            // Company Query Parameters
            if (request.CompanyId.HasValue ||
                request.CompanyQueryParameters.HasValue)
            {
                query = query.Where(branch => _context.Companies
                    .WhereIdentificationData(
                    request.CompanyId,
                    request.CompanyQueryParameters)
                    .Any(company => company.CompanyId == branch.CompanyId)
                );
            }
            else
            {
                query = query
                    .Where(branch => branch.Company.Removed == null)
                    .Where(branch => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(cp => cp.CompanyId == branch.CompanyId));
            }

            query = query.Where(branch => request.ShowRemoved
                        ? branch.Removed != null
                        : branch.Removed == null);

            query = query.WhereText(request.SearchText);
            return query.OrderBy(
                request.GeographyPoint,
                request.ShowRemoved,
                request.OrderBy,
                request.Ascending);
        }
    }
}
