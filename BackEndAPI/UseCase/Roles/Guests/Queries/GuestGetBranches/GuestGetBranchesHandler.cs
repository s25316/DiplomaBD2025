using AutoMapper;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Guests.Queries.GuestGetBranches.Request;
using UseCase.Roles.Guests.Queries.GuestGetBranches.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetBranches
{
    public class GuestGetBranchesHandler : IRequestHandler<GuestGetBranchesRequest, ItemsResponse<GuestBranchAndCompanyDto>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        //private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public GuestGetBranchesHandler(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ItemsResponse<GuestBranchAndCompanyDto>> Handle(GuestGetBranchesRequest request, CancellationToken cancellationToken)
        {
            // Prepare Data
            var now = CustomTimeProvider.Now;
            Expression<Func<Offer, bool>> getActiveOffers = offer =>
                offer.PublicationStart < now &&
                (
                    offer.PublicationEnd == null ||
                    offer.PublicationEnd > now
                );

            // Prepare Query
            var baseQuery = PrepareQuery(request, getActiveOffers);
            var paginatedQuery = baseQuery.Paginate(
                request.Pagination.Page,
                request.Pagination.ItemsPerPage);

            // Execute Query 
            var selectResult = await paginatedQuery.Select(branch => new
            {
                Item = branch,
                TotalCount = baseQuery.Count(),
                OfferCount = _context.Offers
                    .Where(offer => offer.BranchId == branch.BranchId)
                    .Count(getActiveOffers),
            }).ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.NotFound, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<GuestBranchAndCompanyDto>();
            foreach (var item in selectResult)
            {
                if (item.Item.Company.Removed != null ||
                    (item.Item.Removed.HasValue && item.OfferCount == 0))
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }
                if (item.Item.Company.Blocked.HasValue)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
                }
                items.Add(new GuestBranchAndCompanyDto
                {
                    Company = _mapper.Map<CompanyDto>(item.Item.Company),
                    Branch = _mapper.Map<GuestBranchDto>(item.Item),
                    OfferCount = item.OfferCount,
                });
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
        }

        // Static Methods
        private static ItemsResponse<GuestBranchAndCompanyDto> PrepareResponse(
            HttpCode code,
            IEnumerable<GuestBranchAndCompanyDto> items,
            int totalCount)
        {
            return ItemsResponse<GuestBranchAndCompanyDto>.PrepareResponse(code, items, totalCount);
        }

        // Non Static Methods
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
            GuestGetBranchesRequest request,
            Expression<Func<Offer, bool>> getActiveOffers)
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
                query = query.Where(branch =>
                    branch.Company.Removed == null &&
                    branch.Company.Blocked == null)
                    .Where(branch =>
                    branch.Removed == null ||
                    (
                        branch.Removed != null &&
                        _context.Offers
                        .Where(offer => offer.BranchId == branch.BranchId)
                        .Any(getActiveOffers)
                    ));
            }

            query = query.WhereText(request.SearchText);
            return query.OrderBy(
                request.GeographyPoint,
                request.OrderBy,
                request.Ascending);
        }
    }
}
