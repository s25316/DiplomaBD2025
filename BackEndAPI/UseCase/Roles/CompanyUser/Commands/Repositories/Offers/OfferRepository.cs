using AutoMapper;
using Domain.Features.Branches.ValueObjects;
using Domain.Features.Companies.ValueObjects;
using Domain.Features.OfferTemplates.ValueObjects;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using DatabaseOffer = UseCase.RelationalDatabase.Models.Offer;
using DomainOffer = Domain.Features.Offers.Entities.Offer;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.Offers
{
    public class OfferRepository : IOfferRepository
    {
        // Properties 
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public OfferRepository(IMapper mapper, DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task CreateAsync(
            IEnumerable<DomainOffer> items,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(items.Count());
            var databaseOffers = new List<DatabaseOffer>();
            var databaseOfferWorkModes = new List<OfferWorkMode>();
            var databaseOfferEmploymentTypes = new List<OfferEmploymentType>();
            foreach (var item in items)
            {
                var offer = _mapper.Map<DatabaseOffer>(item);
                var workModes = item.WorkModeIds.Select(wmId => new OfferWorkMode
                {
                    Offer = offer,
                    WorkModeId = wmId,
                });
                var employmentTypes = item.EmploymentTypeIds.Select(etId => new OfferEmploymentType
                {
                    Offer = offer,
                    EmploymentTypeId = etId,
                });
                databaseOfferEmploymentTypes.AddRange(employmentTypes);
                databaseOfferWorkModes.AddRange(workModes);
                databaseOffers.Add(offer);
            }

            await _context.Offers.AddRangeAsync(databaseOffers, cancellationToken);
            await _context.OfferWorkModes.AddRangeAsync(databaseOfferWorkModes, cancellationToken);
            await _context.OfferEmploymentTypes.AddRangeAsync(databaseOfferEmploymentTypes, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<HttpCode> IsAllowedOperationAsync(
            PersonId personId,
            CompanyId companyId,
            OfferTemplateId offerTemplateId,
            CancellationToken cancellationToken)
        {
            var result = await _context.Companies
                .Where(company => company.CompanyId == companyId.Value)
                .Select(company => new
                {
                    company,
                    OfferTemplateCount = _context.OfferTemplates
                        .Where(of => of.OfferTemplateId == offerTemplateId.Value)
                        .Count(),
                    OfferTemplateAndCompanyCount = _context.OfferTemplates
                        .Where(of =>
                            of.OfferTemplateId == offerTemplateId.Value &&
                            of.CompanyId == company.CompanyId)
                        .Count(),
                    Roles = _context.CompanyPeople
                        .Where(cp =>
                            cp.CompanyId == company.CompanyId &&
                            cp.PersonId == personId.Value &&
                            cp.Deny == null)
                        .Select(cp => cp.RoleId)
                        .ToList(),
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (result == null)
            {
                return HttpCode.NotFound;
            }
            if (result.OfferTemplateCount == 0 || result.OfferTemplateAndCompanyCount == 0)
            {
                return HttpCode.NotFound;
            }
            if (!result.Roles.Contains(1))
            {
                return HttpCode.Forbidden;
            }
            return HttpCode.Ok;
        }

        public async Task<IEnumerable<Guid>> GetNotFoundedBranchIdsAsync(
            CompanyId companyId,
            IEnumerable<BranchId> branchIds,
            CancellationToken cancellationToken)
        {
            var branchIdsSet = branchIds.Select(id => id.Value).ToHashSet();
            var ids = await _context.Branches.Where(branch =>
                branch.CompanyId == companyId.Value &&
                branchIdsSet.Contains(branch.BranchId))
                .Select(branch => branch.BranchId)
                .ToListAsync(cancellationToken);

            if (branchIdsSet.Count == ids.Count)
            {
                return [];
            }
            return branchIdsSet.Except(ids);
        }
    }
}
