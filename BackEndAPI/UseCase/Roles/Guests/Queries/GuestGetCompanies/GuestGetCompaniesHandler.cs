using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Guests.Queries.GuestGetCompanies.Request;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Guests.Queries.GuestGetCompanies
{
    public class GuestGetCompaniesHandler : IRequestHandler<GuestGetCompaniesRequest, ItemsResponse<CompanyDto>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public GuestGetCompaniesHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<ItemsResponse<CompanyDto>> Handle(GuestGetCompaniesRequest request, CancellationToken cancellationToken)
        {
            // Prepare Query
            var baseQuery = PrepareBaseQuery(request);
            var paginatedQuery = baseQuery.Paginate(
                request.Pagination.Page,
                request.Pagination.ItemsPerPage);

            // Execute Query
            var selectResult = await paginatedQuery.Select(item => new
            {
                Item = item,
                TotalCount = baseQuery.Count(),
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
                if (item.Item.Removed != null)
                {
                    return PrepareResponse(HttpCode.Gone, [], 0);
                }
                if (item.Item.Blocked != null)
                {
                    return PrepareResponse(HttpCode.Forbidden, [], 0);
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
        private IQueryable<Company> PrepareBaseQuery()
        {
            return _context.Companies.AsNoTracking();
        }

        private IQueryable<Company> PrepareBaseQuery(GuestGetCompaniesRequest request)
        {
            var query = PrepareBaseQuery();

            if (request.CompanyQueryParameters.HasValue || request.CompanyId.HasValue)
            {
                return query.WhereIdentificationData(
                    request.CompanyId,
                    request.CompanyQueryParameters);
            }
            query = query.Where(company =>
                    company.Removed == null &&
                    company.Blocked == null);
            query = query.WhereText(request.SearchText);

            return query.OrderBy(
                request.OrderBy,
                request.Ascending);
        }
    }
}
