using AutoMapper;
using Domain.Features.People.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches
{
    public class GetCompanyBranchesHandler : IRequestHandler<GetCompanyBranchesRequest, GetCompanyBranchesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        //Constructor
        public GetCompanyBranchesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyBranchesResponse> Handle(GetCompanyBranchesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            Expression<Func<Branch, bool>> filter = branch =>
                branch.Company.CompanyPeople.Any(x =>
                    x.Deny == null &&
                    x.PersonId == personId.Value) &&
                branch.CompanyId == request.CompanyId;

            var branches = await _context.Branches
                .Include(b => b.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(b => b.Address)
                .ThenInclude(a => a.Street)

                .Include(b => b.Address)
                .ThenInclude(a => a.City)
                .ThenInclude(a => a.State)
                .ThenInclude(a => a.Country)

                .Where(filter)
                .ToListAsync(cancellationToken);

            return new GetCompanyBranchesResponse
            {
                Branches = _mapper.Map<IEnumerable<BranchDto>>(branches),
            };
        }

        private PersonId GetPersonId(GetCompanyBranchesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
