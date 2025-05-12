using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Response;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Request;
using UseCase.Roles.CompanyUser.Queries.Template;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Branches;
using UseCase.Shared.ExtensionMethods.EF.Companies;
using UseCase.Shared.ExtensionMethods.EF.CompanyPeople;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetBranches
{
    public class GetCompanyUserBranchesHandler :
        GetCompanyUserGenericsBase<Branch, CompanyUserGetBranchAndCompanyDto>,
        IRequestHandler<GetCompanyUserBranchesRequest, GetCompanyUserGenericItemsResponse<CompanyUserGetBranchAndCompanyDto>>
    {
        //Properties
        private static readonly IEnumerable<CompanyUserRoles> _authorizedRoles = [
            CompanyUserRoles.CompanyOwner];

        //Constructor
        public GetCompanyUserBranchesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
            : base(context, mapper, authenticationInspector)
        { }


        // Methods
        public async Task<GetCompanyUserGenericItemsResponse<CompanyUserGetBranchAndCompanyDto>> Handle(GetCompanyUserBranchesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request.Metadata.Claims);
            var query = BuildQuery(request, personId);
            var selector = BuildSelector(
                personId,
                _authorizedRoles,
                query,
                branch => branch.Company.CompanyPeople);

            var selectedValues = await query
                .Paginate(request.Pagination)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return PrepareResponse(
                selectedValues,
                branch => branch.Company.Removed != null,
                branch => new CompanyUserGetBranchAndCompanyDto
                {
                    Company = _mapper.Map<CompanyDto>(branch.Company),
                    Branch = _mapper.Map<CompanyUserBranchDto>(branch),
                });
        }



        private IQueryable<Branch> BuildBaseQuery()
        {
            return _context.Branches
                .Include(b => b.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(b => b.Address)
                .ThenInclude(a => a.Street)

                .Include(b => b.Address)
                .ThenInclude(a => a.City)
                .ThenInclude(a => a.State)
                .ThenInclude(a => a.Country)
                .AsNoTracking();
        }

        private IQueryable<Branch> BuildQuery(
            GetCompanyUserBranchesRequest request,
            PersonId personId)
        {
            var query = BuildBaseQuery();

            // Choose only One Branch
            if (request.BranchId.HasValue)
            {
                return query.Where(branch => branch.BranchId == request.BranchId.Value);
            }

            // Choose Branches by Company even we haven`t access 
            if (request.CompanyId.HasValue ||
                request.CompanyParameters.HasValue)
            {
                query = query.Where(branch => _context.Companies
                    .WhereIdentificationData(request.CompanyId, request.CompanyParameters)
                    .Any(company => company.CompanyId == branch.CompanyId)
                );
            }
            // Choose Branches where we have access and eliminate Removed Companies 
            else
            {
                query = query
                    .Where(branch => branch.Company.Removed == null)
                    .Where(branch => _context.CompanyPeople
                        .WhereAuthorize(personId, _authorizedRoles)
                        .Any(cp => cp.CompanyId == branch.CompanyId));
            }

            // Select Removed or not removed
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
