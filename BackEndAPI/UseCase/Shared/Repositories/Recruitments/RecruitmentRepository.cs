using AutoMapper;
using Domain.Features.Offers.Enums;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.Recruitments.ValueObjects.Ids;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Shared.Repositories.BaseEFRepository;
using DatabaseRecruitment = UseCase.RelationalDatabase.Models.HrProcess;
using DomainOffer = Domain.Features.Offers.Aggregates.Offer;
using DomainPerson = Domain.Features.People.Aggregates.Person;
using DomainRecruitment = Domain.Features.Recruitments.Entities.Recruitment;

namespace UseCase.Shared.Repositories.Recruitments
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;

        // Constructor 
        public RecruitmentRepository(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<(HttpCode Code, string? Message)> IsValidCreateAsync(
            DomainRecruitment item,
            CancellationToken cancellationToken)
        {
            var offerIdValue = item.OfferId.Value;
            var personIdValue = item.PersonId.Value;

            var data = await _context.Offers
                .Include(offer => offer.OfferConnections)
                .Where(offer =>
                    offer.OfferId == offerIdValue &&
                    offer.OfferConnections.Any(oc => oc.Removed == null))
                .Select(offer => new
                {
                    Offer = offer,
                    Person = _context.People
                        .Where(person => person.PersonId == personIdValue)
                        .FirstOrDefault(),
                    Recruitment = _context.HrProcesses
                        .Where(recruitment =>
                            recruitment.PersonId == personIdValue &&
                            recruitment.OfferId == offerIdValue)
                        .FirstOrDefault(),
                    IsConflict = _context.OfferConnections
                        .Include(oc => oc.OfferTemplate)
                        .ThenInclude(oc => oc.Company)
                        .ThenInclude(oc => oc.CompanyPeople)
                        .Where(oc =>
                            oc.OfferId == offer.OfferId &&
                            oc.Removed == null)
                        .Any(oc =>
                            oc.OfferTemplate.Company.CompanyPeople.Any(role =>
                                role.PersonId == personIdValue &&
                                role.Deny == null
                            ))
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                return (HttpCode.BadRequest, "Offer: not Exist");
            }
            if (data.Person == null)
            {
                // Impossible
                return (HttpCode.BadRequest, "Person: not Exist");
            }
            if (data.Recruitment != null)
            {
                return (HttpCode.BadRequest, "Recruitment: Exist");
            }
            if (data.IsConflict)
            {
                return (HttpCode.Conflict, "Conflict: cannot recruit on offers bounded with your companies");
            }

            var domainPerson = _mapper.Map<DomainPerson>(data.Person);
            if (domainPerson.IsNotCompleteProfile)
            {
                return (HttpCode.BadRequest, "Person Profile not completed");
            }

            var domainOffer = _mapper.Map<DomainOffer>(data.Offer);
            if (domainOffer.Status != OfferStatus.Active)
            {
                return (HttpCode.BadRequest, $"Offer status: {domainOffer.Status.Description()}");
            }

            return (HttpCode.Ok, null);
        }

        public async Task<RepositoryCreateResponse<DomainRecruitment>> CreateAsync(
            PersonId personId,
            IEnumerable<DomainRecruitment> items,
            CancellationToken cancellationToken)
        {
            var item = items.FirstOrDefault() ?? throw new Exception();

            var recruitment = new DatabaseRecruitment
            {
                PersonId = item.PersonId.Value,
                OfferId = item.OfferId.Value,
                Message = item.Message,
                File = item.File,
                Created = item.Created,
                ProcessTypeId = 1,
            };

            await _context.HrProcesses.AddAsync(recruitment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return RepositoryCreateResponse<DomainRecruitment>.PrepareResponse(HttpCode.Created, null, items);
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
            PersonId personId,
            DomainRecruitment item,
            CancellationToken cancellationToken)
        {
            var recruitmentId = item.Id?.Value ?? throw new KeyNotFoundException();
            var database = await _context.HrProcesses
                .Where(recruitment => recruitment.ProcessId == recruitmentId)
                .FirstOrDefaultAsync(cancellationToken);
            if (database == null)
            {
                return RepositoryUpdateResponse.InvalidResponse(HttpCode.NotFound);
            }

            database.ProcessTypeId = (int)item.ProcessType;
            await _context.SaveChangesAsync(cancellationToken);
            return RepositoryUpdateResponse.ValidResponse();
        }

        public async Task<RepositorySelectResponse<DomainRecruitment>> GetAsync(
            PersonId personId,
            RecruitmentId id,
            CancellationToken cancellationToken)
        {
            var recruitmentId = id.Value;
            var personIdValue = personId.Value;

            var selectResult = await _context.HrProcesses
                .Where(recruitment => recruitment.ProcessId == recruitmentId)
                .Select(recruitment => new
                {
                    Recruitment = recruitment,
                    RoleCount = _context.CompanyPeople
                        .Where(role =>
                            role.Deny == null &&
                            role.PersonId == personIdValue)
                        .Count(role =>
                            _context.OfferConnections
                            .Include(x => x.OfferTemplate)
                            .Any(oc =>
                                oc.Removed == null &&
                                oc.OfferId == recruitment.OfferId &&
                                role.CompanyId == oc.OfferTemplate.CompanyId
                            )
                        ),
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null)
            {
                return RepositorySelectResponse<DomainRecruitment>.InvalidResponse(HttpCode.NotFound);
            }
            if (selectResult.RoleCount == 0)
            {
                return RepositorySelectResponse<DomainRecruitment>.InvalidResponse(HttpCode.Forbidden);
            }

            return RepositorySelectResponse<DomainRecruitment>
                .ValidResponse(_mapper.Map<DomainRecruitment>(selectResult.Recruitment));
        }

        public Task<RepositoryRemoveResponse> RemoveAsync(PersonId personId, DomainRecruitment item, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
