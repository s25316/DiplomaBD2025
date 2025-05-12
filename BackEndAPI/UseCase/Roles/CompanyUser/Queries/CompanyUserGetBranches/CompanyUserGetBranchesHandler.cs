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
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches
{
    public class CompanyUserGetBranchesHandler : IRequestHandler<CompanyUserGetBranchesRequest, ItemsResponse<CompanyUserGetBranchAndCompanyDto>>
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
        public Task<ItemsResponse<CompanyUserGetBranchAndCompanyDto>> Handle(CompanyUserGetBranchesRequest request, CancellationToken cancellationToken)
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

            throw new NotImplementedException();
        }

        // Static Methods
        private static ItemsResponse<CompanyUserGetBranchAndCompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<CompanyUserGetBranchAndCompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<CompanyUserGetBranchAndCompanyDto>.PrepareResponse(code, items, totalCount);
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
