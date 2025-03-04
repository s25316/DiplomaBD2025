using AutoMapper;
using Domain.Features.People.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Response;
using UseCase.Shared.DTOs.Responses.Companies.OfferTemplates;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates
{
    class GetCompanyOfferTemplatesHandler : IRequestHandler<GetCompanyOfferTemplatesRequest, GetCompanyOfferTemplatesResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        //Constructor
        public GetCompanyOfferTemplatesHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyOfferTemplatesResponse> Handle(GetCompanyOfferTemplatesRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            Expression<Func<OfferTemplate, bool>> filter = ot =>
                ot.Company.CompanyPeople.Any(x =>
                    x.Deny == null &&
                    x.PersonId == personId.Value) &&
                ot.CompanyId == request.CompanyId;/*&&
                ot.OfferSkills.Any(x => x.Removed == null);*/

            var offerTemplates = await _context.OfferTemplates

                .Include(ot => ot.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(ot => ot.OfferSkills)
                .ThenInclude(os => os.Skill)
                .ThenInclude(s => s.SkillType)

                .Where(filter)
                .ToListAsync(cancellationToken);

            return new GetCompanyOfferTemplatesResponse
            {
                OfferTemplates = _mapper.Map<IEnumerable<OfferTemplateDto>>(offerTemplates),
            };
        }

        private PersonId GetPersonId(GetCompanyOfferTemplatesRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
