using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Domain.Shared.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.BaseEFRepository;
using DatabaseOfferSkill = UseCase.RelationalDatabase.Models.OfferSkill;
using DatabaseOfferTemplate = UseCase.RelationalDatabase.Models.OfferTemplate;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Aggregates.OfferTemplate;
using DomainOfferTemplateId = Domain.Features.OfferTemplates.ValueObjects.Ids.OfferTemplateId;

namespace UseCase.Roles.CompanyUser.Repositories.OfferTemplates
{
    public class OfferTemplateRepository : IOfferTemplateRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private static readonly IEnumerable<CompanyUserRoles> _roles = [
            CompanyUserRoles.CompanyOwner];


        // Constructor
        public OfferTemplateRepository(
            DiplomaBdContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Methods


        public async Task<RepositoryCreateResponse<DomainOfferTemplate>> CreateAsync(
            PersonId personId,
            IEnumerable<DomainOfferTemplate> items,
            CancellationToken cancellationToken)
        {
            if (!items.Any())
            {
                return ValidCreate(items);
            }
            var validationResult = await CheckCompanyAsync(personId, items, cancellationToken);
            if (validationResult.Code != HttpCode.Created)
            {
                return validationResult;
            }

            await EFCreateAsync(items, cancellationToken);
            return validationResult;
        }

        public async Task<RepositorySelectResponse<DomainOfferTemplate>> GetAsync(
            PersonId personId,
            DomainOfferTemplateId id,
            CancellationToken cancellationToken)
        {
            var personIdValue = personId.Value;
            var idValue = id.Value;
            var roleIds = _roles.Select(r => (int)r);
            var selectResult = await _context.OfferTemplates
                .Include(ot => ot.Company)
                .Include(ot => ot.OfferSkills)
                .Where(ot => ot.OfferTemplateId == idValue)
                .Select(ot => new
                {
                    OfferTemplate = ot,
                    RolesCount = _context.CompanyPeople
                        .Count(role => roleIds.Any(roleId =>
                            role.PersonId == personIdValue &&
                            role.Deny == null &&
                            role.RoleId == roleId &&
                            role.CompanyId == ot.CompanyId
                        ))
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null)
            {
                return InvalidSelect(HttpCode.NotFound);
            }
            if (selectResult.RolesCount == 0)
            {
                return InvalidSelect(HttpCode.Forbidden);
            }

            var dbItem = selectResult.OfferTemplate;
            if (dbItem.Company.Removed != null)
            {
                return InvalidSelect(HttpCode.Gone);
            }
            if (dbItem.Company.Blocked != null)
            {
                return InvalidSelect(
                    HttpCode.Forbidden,
                    Messages.Entity_Company_Status_Blocked);
            }

            return ValidSelect(_mapper.Map<DomainOfferTemplate>(dbItem));
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
            PersonId personId,
            DomainOfferTemplate item,
            CancellationToken cancellationToken)
        {
            // Prepare data
            var now = CustomTimeProvider.Now;
            var idValue = item.Id?.Value
                ?? throw new UseCaseLayerException();

            var selectResult = await _context.OfferTemplates
                .Include(ot => ot.Company)
                .Include(ot => ot.OfferSkills)
                .Where(ot => ot.OfferTemplateId == idValue)
                .Select(ot => new
                {
                    OfferTemplate = ot,
                    CountNotFuture = _context.OfferConnections
                        .Include(o => o.Offer)
                        .Where(oc =>
                            oc.Removed == null &&
                            oc.OfferTemplateId == ot.OfferTemplateId &&
                            oc.Offer.PublicationStart < now)
                        .Count(),
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null)
            {
                return InvalidUpdate(HttpCode.NotFound);
            }

            var dbItem = selectResult.OfferTemplate;
            if (selectResult.CountNotFuture > 0)
            {
                // If offer has active or expired offers
                // Create New
                var newDbItem = _mapper.Map<DatabaseOfferTemplate>(item);
                newDbItem.Removed = null;
                newDbItem.Created = now;

                var dbSkills = item.SkillsDictionary.Values
                    .Where(s => s.Removed == null)
                    .Select(s => new DatabaseOfferSkill
                    {
                        OfferTemplate = newDbItem,
                        SkillId = s.SkillId,
                        Created = dbItem.Created,
                        IsRequired = s.IsRequired,
                    });

                await _context.OfferTemplates.AddAsync(newDbItem, cancellationToken);
                await _context.OfferSkills.AddRangeAsync(dbSkills, cancellationToken);

                if (dbItem.Removed == null)
                {
                    dbItem.Removed = now;
                }
            }
            else
            {
                // If Have only future offers
                dbItem.Name = item.Name;
                dbItem.Description = item.Description;
                dbItem.Removed = item.Removed;

                foreach (var dbSkill in dbItem.OfferSkills.Where(os => os.Removed == null))
                {
                    if (item.SkillsDictionary.TryGetValue((SkillId)dbSkill.SkillId, out var domainSkill))
                    {
                        dbSkill.IsRequired = domainSkill.IsRequired;
                        dbSkill.Removed = domainSkill.Removed;
                    }
                }

                var createSkillsList = item.SkillsDictionary.Values
                    .Where(s => s.Id == null)
                    .Select(s => new DatabaseOfferSkill
                    {
                        OfferTemplateId = dbItem.OfferTemplateId,
                        SkillId = s.SkillId,
                        IsRequired = s.IsRequired,
                        Created = s.Created,
                    });
                await _context.OfferSkills.AddRangeAsync(createSkillsList, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return ValidUpdate();
        }

        public async Task<RepositoryRemoveResponse> RemoveAsync(
            PersonId personId,
            DomainOfferTemplate item,
            CancellationToken cancellationToken)
        {
            // Prepare data
            var idValue = item.Id?.Value
                ?? throw new UseCaseLayerException();

            var dbOfferTemplate = await _context.OfferTemplates
                .Where(ot => ot.OfferTemplateId == idValue)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbOfferTemplate == null)
            {
                return RepositoryRemoveResponse.PrepareResponse(HttpCode.NotFound);
            }

            dbOfferTemplate.Removed = item.Removed;
            await _context.SaveChangesAsync(cancellationToken);

            return RepositoryRemoveResponse.ValidResponse();
        }

        // Private Static Methods
        // Create Part
        private static RepositoryCreateResponse<DomainOfferTemplate> ValidCreate(
            IEnumerable<DomainOfferTemplate> items)
        {
            return RepositoryCreateResponse<DomainOfferTemplate>.ValidResponse(items);
        }
        private static RepositoryCreateResponse<DomainOfferTemplate> PrepareCreateResponse(
            HttpCode code,
            IEnumerable<DomainOfferTemplate> items,
            string? message = null)
        {
            return RepositoryCreateResponse<DomainOfferTemplate>.PrepareResponse(
                code,
                message,
                items);
        }

        // Select Part
        private static RepositorySelectResponse<DomainOfferTemplate> InvalidSelect(
            HttpCode code,
            string? message = null)
        {
            return RepositorySelectResponse<DomainOfferTemplate>.InvalidResponse(
                code,
                message);
        }
        private static RepositorySelectResponse<DomainOfferTemplate> ValidSelect(
            DomainOfferTemplate item)
        {
            return RepositorySelectResponse<DomainOfferTemplate>.ValidResponse(item);
        }

        // Update Part
        private static RepositoryUpdateResponse InvalidUpdate(
            HttpCode code,
            string? message = null)
        {
            return RepositoryUpdateResponse.InvalidResponse(code, message);
        }

        private static RepositoryUpdateResponse ValidUpdate()
        {
            return RepositoryUpdateResponse.ValidResponse();
        }


        // Private Non Static Methods
        private async Task<RepositoryCreateResponse<DomainOfferTemplate>> CheckCompanyAsync(
            PersonId personId,
            IEnumerable<DomainOfferTemplate> items,
            CancellationToken cancellationToken)
        {
            var companyId = items.First().CompanyId.Value;
            var roleIds = _roles.Select(role => (int)role);

            var companySelect = await _context.Companies
                .Include(c => c.CompanyPeople)
                .Where(c => c.CompanyId == companyId)
                .Select(c => new
                {
                    Company = c,
                    RolesCount = c.CompanyPeople.Count(cp => roleIds.Any(roleId =>
                        cp.PersonId == personId.Value &&
                        cp.Deny == null &&
                        cp.RoleId == roleId
                    )),
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (companySelect == null)
            {
                return PrepareCreateResponse(HttpCode.NotFound, items);
            }
            if (companySelect.RolesCount == 0)
            {
                return PrepareCreateResponse(HttpCode.Forbidden, items);
            }

            var dbCompany = companySelect.Company;
            if (dbCompany.Removed != null)
            {
                return PrepareCreateResponse(HttpCode.Gone, items);
            }
            if (dbCompany.Blocked != null)
            {
                return PrepareCreateResponse(
                    HttpCode.Forbidden,
                    items,
                    Messages.Entity_Company_Status_Blocked);
            }

            return ValidCreate(items);
        }

        private async Task EFCreateAsync(
            IEnumerable<DomainOfferTemplate> items,
            CancellationToken cancellationToken)
        {
            var dbItems = new List<DatabaseOfferTemplate>();
            var dbOfferSkills = new List<DatabaseOfferSkill>();
            foreach (var item in items)
            {
                var dbItem = _mapper.Map<DatabaseOfferTemplate>(item);
                var skills = item.SkillsDictionary.Select(skillId => new DatabaseOfferSkill
                {
                    OfferTemplate = dbItem,
                    SkillId = skillId.Key,
                    Created = dbItem.Created,
                    IsRequired = skillId.Value.IsRequired,
                });
                dbItems.Add(dbItem);
                dbOfferSkills.AddRange(skills);
            }

            await _context.OfferTemplates.AddRangeAsync(dbItems, cancellationToken);
            await _context.OfferSkills.AddRangeAsync(dbOfferSkills, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
