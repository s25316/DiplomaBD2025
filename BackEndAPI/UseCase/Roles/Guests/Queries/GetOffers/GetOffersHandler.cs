using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Guests.Queries.GetOffers.Request;
using UseCase.Roles.Guests.Queries.GetOffers.Response;
using UseCase.Shared.DTOs.Responses;
using UseCase.Shared.Services.Time;

namespace UseCase.Roles.Guests.Queries.GetOffers
{
    public class GetOffersHandler : IRequestHandler<GetOffersRequest, GetOffersResponse>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly ITimeService _timeService;
        private readonly IMapper _mapper;


        // Constructor
        public GetOffersHandler(
            DiplomaBdContext context,
            ITimeService timeService,
            IMapper mapper)
        {
            _context = context;
            _timeService = timeService;
            _mapper = mapper;
        }


        // Methods
        public async Task<GetOffersResponse> Handle(GetOffersRequest request, CancellationToken cancellationToken)
        {
            var now = _timeService.GetNow();
            var list = await _context.Offers
                .Include(o => o.SalaryTerm)
                .Include(o => o.Currency)
                .Include(o => o.OfferTemplate)
                .ThenInclude(ot => ot.Company)
                .Include(o => o.OfferTemplate)
                .ThenInclude(ot => ot.OfferSkills)
                .ThenInclude(os => os.Skill)
                .ThenInclude(s => s.SkillType)
                .Include(o => o.Branch)
                .ThenInclude(b => b.Address)
                .ThenInclude(a => a.Street)
                .Include(o => o.Branch)
                .ThenInclude(b => b.Address)
                .ThenInclude(a => a.City)
                .ThenInclude(c => c.State)
                .ThenInclude(s => s.Country)
                .Include(o => o.OfferEmploymentTypes)
                .ThenInclude(oe => oe.EmploymentType)
                .Include(o => o.OfferWorkModes)
                .ThenInclude(ow => ow.WorkMode)
                .Where(offer =>
                    offer.PublicationStart < now &&
                    offer.PublicationEnd > now
                )
                .Select(o => new OfferDto
                {
                    OfferId = o.OfferId,
                    PublicationStart = o.PublicationStart,
                    PublicationEnd = o.PublicationEnd,
                    WorkBeginDate = o.WorkBeginDate == null ? null : _timeService.FromDateOnly(o.WorkBeginDate.Value),
                    WorkEndDate = o.WorkEndDate == null ? null : _timeService.FromDateOnly(o.WorkEndDate.Value),
                    SalaryRangeMin = o.SalaryRangeMin,
                    SalaryRangeMax = o.SalaryRangeMax,
                    IsNegotiated = o.IsNegotiated,
                    WebsiteUrl = o.WebsiteUrl,
                    Currency = _mapper.Map<CurrencyDto>(o.Currency),
                    SalaryTerm = _mapper.Map<SalaryTermDto>(o.SalaryTerm),
                    Skills = o.OfferTemplate.OfferSkills
                    .Where(x => x.Removed == null)
                    .Select(x => new OfferSkillResponseDto
                    {
                        Skill = _mapper.Map<SkillResponseDto>(x.Skill),
                        IsRequired = x.IsRequired,
                    }),
                    EmploymentTypes = o.OfferEmploymentTypes
                    .Where(x => x.Removed == null)
                    .Select(x => _mapper.Map<EmploymentTypeDto>(x.EmploymentType)),
                    WorkModes = o.OfferWorkModes
                    .Where(x => x.Removed == null)
                    .Select(x => _mapper.Map<WorkModeDto>(x.WorkMode))
                })
                .ToListAsync(cancellationToken);

            return new GetOffersResponse
            {
                Offers = list,
            };
        }
    }
}
