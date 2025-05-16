using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.BaseEFRepository;
using DatabaseContractAttribute = UseCase.RelationalDatabase.Models.ContractAttribute;
using DatabaseContractCondition = UseCase.RelationalDatabase.Models.ContractCondition;
using DomainContractCondition = Domain.Features.ContractConditions.Aggregates.ContractCondition;
using DomainContractConditionId = Domain.Features.ContractConditions.ValueObjects.Ids.ContractConditionId;

namespace UseCase.Roles.CompanyUser.Repositories.ContractConditions
{
    public class ContractConditionRepository : IContractConditionRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private static readonly IEnumerable<CompanyUserRoles> _roles = [
            CompanyUserRoles.CompanyOwner];


        // Constructor
        public ContractConditionRepository(
            DiplomaBdContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Methods       
        public async Task<RepositoryCreateResponse<DomainContractCondition>> CreateAsync(
            PersonId personId,
            IEnumerable<DomainContractCondition> items,
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

        public async Task<RepositorySelectResponse<DomainContractCondition>> GetAsync(
            PersonId personId,
            DomainContractConditionId id,
            CancellationToken cancellationToken)
        {
            var personIdValue = personId.Value;
            var roleIds = _roles.Select(r => (int)r);
            var idValue = id.Value;

            var selectResult = await _context.ContractConditions
                .Include(cc => cc.ContractAttributes)
                .ThenInclude(cc => cc.ContractParameter)
                .Include(cc => cc.Company)
                .Where(cc => cc.ContractConditionId == idValue)
                .Select(cc => new
                {
                    ContractCondition = cc,
                    RolesCount = _context.CompanyPeople
                        .Count(role => roleIds.Any(roleId =>
                            role.PersonId == personIdValue &&
                            role.Deny == null &&
                            role.RoleId == roleId &&
                            role.CompanyId == cc.CompanyId
                        )),
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
            var item = selectResult.ContractCondition;

            if (item.Company.Removed != null)
            {
                return InvalidSelect(HttpCode.Gone);
            }
            if (item.Company.Blocked != null)
            {
                return InvalidSelect(
                    HttpCode.Forbidden,
                    Messages.Entity_Company_Status_Blocked);
            }

            return ValidSelect(_mapper.Map<DomainContractCondition>(item));
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
           PersonId personId,
           DomainContractCondition item,
           CancellationToken cancellationToken)
        {
            // Prepare Data
            var now = CustomTimeProvider.Now;
            var idValue = item.Id?.Value
                ?? throw new UseCaseLayerException();

            // Select From DB
            var selectResult = await _context.ContractConditions
                .Include(cc => cc.ContractAttributes)
                .ThenInclude(cc => cc.ContractParameter)
                .Include(cc => cc.Company)
                .Include(cc => cc.OfferConditions)
                .Where(cc => cc.ContractConditionId == idValue)
                .Select(cc => new
                {
                    ContractCondition = cc,
                    NotFutureOfferCount = _context.OfferConditions
                        .Include(oc => oc.Offer)
                        .Where(oc =>
                            oc.ContractConditionId == cc.ContractConditionId &&
                            oc.Removed == null &&
                            oc.Offer.PublicationStart <= now
                        ).Count()
                })
                .FirstOrDefaultAsync(cancellationToken);


            if (selectResult == null)
            {
                return InvalidUpdate(HttpCode.NotFound);
            }
            var dbItem = selectResult.ContractCondition;

            // Updating Part
            if (selectResult.NotFutureOfferCount == 0)
            {
                dbItem.SalaryMin = item.SalaryRange.Min;
                dbItem.SalaryMax = item.SalaryRange.Max;
                dbItem.HoursPerTerm = item.HoursPerTerm;
                dbItem.IsNegotiable = item.IsNegotiable;
                dbItem.Removed = item.Removed;

                var dbAttributes = dbItem.ContractAttributes
                    .Where(ca => ca.Removed == null)
                    .ToDictionary(ca => ca.ContractParameterId);

                var newAttributes = new List<DatabaseContractAttribute>();

                foreach (var i in item.Currencies.Values)
                {
                    if (dbAttributes.TryGetValue(i.ContractParameterId, out var dbAttribute))
                    {
                        dbAttribute.Removed = i.Removed;
                    }
                    else
                    {
                        newAttributes.Add(new DatabaseContractAttribute
                        {
                            ContractCondition = dbItem,
                            Created = i.Created,
                            ContractParameterId = i.ContractParameterId,
                            Removed = i.Removed,
                        });
                    }
                }

                foreach (var i in item.EmploymentTypes.Values)
                {
                    if (dbAttributes.TryGetValue(i.ContractParameterId, out var dbAttribute))
                    {
                        dbAttribute.Removed = i.Removed;
                    }
                    else
                    {
                        newAttributes.Add(new DatabaseContractAttribute
                        {
                            ContractCondition = dbItem,
                            Created = i.Created,
                            ContractParameterId = i.ContractParameterId,
                            Removed = i.Removed,
                        });
                    }
                }

                foreach (var i in item.SalaryTerms.Values)
                {
                    if (dbAttributes.TryGetValue(i.ContractParameterId, out var dbAttribute))
                    {
                        dbAttribute.Removed = i.Removed;
                    }
                    else
                    {
                        newAttributes.Add(new DatabaseContractAttribute
                        {
                            ContractCondition = dbItem,
                            Created = i.Created,
                            ContractParameterId = i.ContractParameterId,
                            Removed = i.Removed,
                        });
                    }
                }

                foreach (var i in item.WorkModes.Values)
                {
                    if (dbAttributes.TryGetValue(i.ContractParameterId, out var dbAttribute))
                    {
                        dbAttribute.Removed = i.Removed;
                    }
                    else
                    {
                        newAttributes.Add(new DatabaseContractAttribute
                        {
                            ContractCondition = dbItem,
                            Created = i.Created,
                            ContractParameterId = i.ContractParameterId,
                            Removed = i.Removed,
                        });
                    }
                }

                await _context.ContractAttributes.AddRangeAsync(newAttributes, cancellationToken);
            }
            else
            {

                var newDbItem = _mapper.Map<DatabaseContractCondition>(item);
                newDbItem.Removed = null;
                newDbItem.Created = now;

                if (item.Removed == null)
                {
                    dbItem.Removed = now;
                }

                var newAttributes = new List<DatabaseContractAttribute>();

                foreach (var i in item.EmploymentTypes.Values)
                {
                    newAttributes.Add(new DatabaseContractAttribute
                    {
                        ContractCondition = newDbItem,
                        Created = i.Created,
                        ContractParameterId = i.ContractParameterId,
                        Removed = i.Removed,
                    });
                }
                foreach (var i in item.WorkModes.Values)
                {
                    newAttributes.Add(new DatabaseContractAttribute
                    {
                        ContractCondition = newDbItem,
                        Created = i.Created,
                        ContractParameterId = i.ContractParameterId,
                        Removed = i.Removed,
                    });
                }
                foreach (var i in item.SalaryTerms.Values)
                {
                    newAttributes.Add(new DatabaseContractAttribute
                    {
                        ContractCondition = newDbItem,
                        Created = i.Created,
                        ContractParameterId = i.ContractParameterId,
                        Removed = i.Removed,
                    });
                }
                foreach (var i in item.Currencies.Values)
                {
                    newAttributes.Add(new DatabaseContractAttribute
                    {
                        ContractCondition = newDbItem,
                        Created = i.Created,
                        ContractParameterId = i.ContractParameterId,
                        Removed = i.Removed,
                    });
                }
                await _context.ContractConditions.AddAsync(newDbItem, cancellationToken);
                await _context.ContractAttributes.AddRangeAsync(newAttributes, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return ValidUpdate();
        }

        public async Task<RepositoryRemoveResponse> RemoveAsync(
            PersonId personId,
            DomainContractCondition item,
            CancellationToken cancellationToken)
        {
            var itemId = item.Id?.Value
                ?? throw new UseCaseLayerException();

            var dbContractCondition = await _context.ContractConditions
                .Where(cc => cc.ContractConditionId == itemId)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbContractCondition == null)
            {
                return RepositoryRemoveResponse.PrepareResponse(HttpCode.NotFound);
            }

            dbContractCondition.Removed = item.Removed;
            await _context.SaveChangesAsync(cancellationToken);
            return RepositoryRemoveResponse.ValidResponse();
        }

        // Private Static Methods
        // Create Part
        private static RepositoryCreateResponse<DomainContractCondition> ValidCreate(
            IEnumerable<DomainContractCondition> items)
        {
            return RepositoryCreateResponse<DomainContractCondition>.ValidResponse(items);
        }

        private static RepositoryCreateResponse<DomainContractCondition> PrepareCreateResponse(
            HttpCode code,
            IEnumerable<DomainContractCondition> items,
            string? description = null)
        {
            return RepositoryCreateResponse<DomainContractCondition>.PrepareResponse(
                code,
                description,
                items);
        }

        // Select Part 
        private static RepositorySelectResponse<DomainContractCondition> InvalidSelect(
            HttpCode code,
            string? message = null)
        {
            return RepositorySelectResponse<DomainContractCondition>.InvalidResponse(
                code,
                message);
        }

        private static RepositorySelectResponse<DomainContractCondition> ValidSelect(
            DomainContractCondition item)
        {
            return RepositorySelectResponse<DomainContractCondition>.ValidResponse(item);

        }

        // Update Part 
        private static RepositoryUpdateResponse InvalidUpdate(
            HttpCode code,
            string? message = null)
        {
            return RepositoryUpdateResponse.InvalidResponse(
                code,
                message);
        }

        private static RepositoryUpdateResponse ValidUpdate()
        {
            return RepositoryUpdateResponse.ValidResponse();
        }

        // Private Non Static Methods
        private async Task<RepositoryCreateResponse<DomainContractCondition>> CheckCompanyAsync(
           PersonId personId,
           IEnumerable<DomainContractCondition> items,
           CancellationToken cancellationToken)
        {
            var companyId = items.First().CompanyId.Value;
            var roleIds = _roles.Select(r => (int)r);

            var selectResult = await _context.Companies
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

            if (selectResult == null)
            {
                return PrepareCreateResponse(HttpCode.NotFound, items);
            }
            if (selectResult.RolesCount == 0)
            {
                return PrepareCreateResponse(HttpCode.Forbidden, items);
            }

            var dbCompany = selectResult.Company;
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
           IEnumerable<DomainContractCondition> items,
           CancellationToken cancellationToken)
        {

            var contractConditions = new List<DatabaseContractCondition>();
            var contractAttributes = new List<DatabaseContractAttribute>();
            foreach (var domain in items)
            {
                var database = _mapper.Map<DatabaseContractCondition>(domain);

                var paramiterIds = new List<int>();
                paramiterIds.AddRange(domain.Currencies.Keys);
                paramiterIds.AddRange(domain.SalaryTerms.Keys);
                paramiterIds.AddRange(domain.WorkModes.Keys);
                paramiterIds.AddRange(domain.EmploymentTypes.Keys);

                var attributes = paramiterIds.Select(id => new DatabaseContractAttribute
                {
                    ContractParameterId = id,
                    ContractCondition = database,
                    Created = database.Created,
                });
                contractConditions.Add(database);
                contractAttributes.AddRange(attributes);
            }

            await _context.ContractAttributes.AddRangeAsync(contractAttributes, cancellationToken);
            await _context.ContractConditions.AddRangeAsync(contractConditions, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

        }

    }
}
