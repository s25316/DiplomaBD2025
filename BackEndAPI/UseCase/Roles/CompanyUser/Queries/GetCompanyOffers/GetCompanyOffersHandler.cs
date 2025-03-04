using AutoMapper;
using Domain.Features.People.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Response;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOffers
{
    public class GetCompanyOffersHandler : IRequestHandler<GetCompanyOffersRequest, GetCompanyOffersResponse>
    {
        //Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        //Constructor
        public GetCompanyOffersHandler(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<GetCompanyOffersResponse> Handle(GetCompanyOffersRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            Expression<Func<Offer, bool>> filter = offer =>
                offer.OfferTemplate.Company.CompanyPeople.Any(x =>
                    x.Deny == null &&
                    x.PersonId == personId.Value) &&
                offer.OfferTemplate.OfferTemplateId == request.OfferTemplateId &&
                offer.OfferTemplate.Company.CompanyId == request.CompanyId //&&
            //offer.OfferWorkModes.Any(x => x.Removed == null) &&
            //offer.OfferEmploymentTypes.Any(x => x.Removed == null)
            ;
            var offers = await _context.Offers
                .Include(o => o.OfferTemplate)
                .ThenInclude(ot => ot.Company)
                .ThenInclude(c => c.CompanyPeople)

                .Include(o => o.Currency)
                .Include(o => o.SalaryTerm)

                .Include(o => o.OfferWorkModes)
                .ThenInclude(owm => owm.WorkMode)

                .Include(o => o.OfferEmploymentTypes)
                .ThenInclude(owm => owm.EmploymentType)

                .Where(filter)
                .ToListAsync(cancellationToken);

            return new GetCompanyOffersResponse
            {
                Offers = _mapper.Map<IEnumerable<OfferDto>>(offers)
            };
        }


        private PersonId GetPersonId(GetCompanyOffersRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
