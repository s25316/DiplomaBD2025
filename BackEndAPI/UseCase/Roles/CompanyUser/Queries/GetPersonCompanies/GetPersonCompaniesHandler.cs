using AutoMapper;
using Domain.Features.People.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Request;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetPersonCompanies
{
    public class GetPersonCompaniesHandler : IRequestHandler<GetPersonCompaniesRequest, GetPersonCompaniesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        //Constructor
        public GetPersonCompaniesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetPersonCompaniesResponse> Handle(GetPersonCompaniesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            Expression<Func<Company, bool>> filter = company =>
                company.CompanyPeople.Any(x =>
                    x.Deny == null &&
                    x.PersonId == personId.Value);

            var companies = await _context.Companies
                .Include(c => c.CompanyPeople)
                .Where(filter)
                .ToListAsync(cancellationToken);

            return new GetPersonCompaniesResponse
            {
                Companies = _mapper.Map<IEnumerable<CompanyDto>>(companies)
            };
        }

        private PersonId GetPersonId(GetPersonCompaniesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
