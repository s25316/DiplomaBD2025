﻿using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.Administrators.Queries.AdministratorGetCompanies.Request;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetCompanies
{
    public class AdministratorGetCompaniesHandler : IRequestHandler<AdministratorGetCompaniesRequest, ItemsResponse<CompanyDto>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public AdministratorGetCompaniesHandler(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ItemsResponse<CompanyDto>> Handle(AdministratorGetCompaniesRequest request, CancellationToken cancellationToken)
        {
            // Prepare Query
            var baseQuery = PrepareBaseQuery(request);
            var paginatedQuery = baseQuery.Paginate(
                request.Pagination.Page,
                request.Pagination.ItemsPerPage);


            // Execute Query
            var selectResult = await paginatedQuery
                .Select(item => new
                {
                    Item = item,
                    TotalCount = baseQuery.Count(),
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Prepare Response
            if (!selectResult.Any())
            {
                return PrepareResponse(HttpCode.Ok, [], 0);
            }

            var totalCount = selectResult.FirstOrDefault()?.TotalCount ?? 0;
            var items = new List<CompanyDto>();
            foreach (var item in selectResult)
            {
                items.Add(_mapper.Map<CompanyDto>(item.Item));
            }

            return PrepareResponse(HttpCode.Ok, items, totalCount);
            throw new NotImplementedException();
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

        private IQueryable<Company> PrepareBaseQuery(AdministratorGetCompaniesRequest request)
        {
            var query = PrepareBaseQuery();

            if (request.CompanyQueryParameters.HasValue || request.CompanyId.HasValue)
            {
                return query.WhereIdentificationData(
                    request.CompanyId,
                    request.CompanyQueryParameters);
            }
            query = query
                .WhereText(request.SearchText)
                .ShowRemoved(request.ShowRemoved);

            return query.OrderBy(
                request.OrderBy,
                request.Ascending);
        }
    }
}
