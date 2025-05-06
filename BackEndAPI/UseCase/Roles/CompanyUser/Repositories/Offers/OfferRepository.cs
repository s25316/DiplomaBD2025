using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Templates.Repositories;
using UseCase.Shared.Templates.Response.Commands;
using DatabaseOffer = UseCase.RelationalDatabase.Models.Offer;
using DatabaseOfferCondition = UseCase.RelationalDatabase.Models.OfferCondition;
using DatabaseOfferConnection = UseCase.RelationalDatabase.Models.OfferConnection;
using DomainOffer = Domain.Features.Offers.Aggregates.Offer;
using DomainOfferId = Domain.Features.Offers.ValueObjects.Ids.OfferId;

namespace UseCase.Roles.CompanyUser.Repositories.Offers
{
    public class OfferRepository : IOfferRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private readonly IEnumerable<CompanyUserRoles> _roles = [
            CompanyUserRoles.CompanyOwner];


        // Constructor
        public OfferRepository(
            DiplomaBdContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Methods
        public async Task<RepositoryCreateResponse<DomainOffer>> CreateAsync(
            PersonId personId,
            IEnumerable<DomainOffer> items,
            CancellationToken cancellationToken)
        {
            var result = await CheckAsync(personId, items, cancellationToken);
            if (result.Code != HttpCode.Ok)
            {
                return result;
            }

            await CreateAsync(items, cancellationToken);
            return ValidCreate(items);
        }

        public async Task<RepositorySelectResponse<DomainOffer>> GetAsync(
            PersonId personId,
            DomainOfferId id,
            CancellationToken cancellationToken)
        {
            var personIdValue = personId.Value;
            var rolesIds = _roles.Select(r => (int)r);
            var offerId = id.Value;

            var selectResult = await _context.Offers
                .Include(o => o.OfferConnections)
                .ThenInclude(o => o.OfferTemplate)
                .ThenInclude(o => o.Company)
                .Include(o => o.OfferConditions)
                .Where(o => o.OfferId == offerId)
                .Select(o => new
                {
                    Offer = o,
                    RoleCount = _context.CompanyPeople
                        .Count(role => rolesIds.Any(roleId =>
                                role.RoleId == roleId &&
                                role.PersonId == personIdValue &&
                                role.Deny == null &&
                                role.CompanyId == o.OfferConnections.First().OfferTemplate.CompanyId
                            )),
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null)
            {
                return InvalidSelect(HttpCode.NotFound);
            }
            if (selectResult.RoleCount == 0)
            {
                return InvalidSelect(HttpCode.Forbidden);
            }

            var dbOffer = selectResult.Offer;
            if (dbOffer.OfferConnections.First().OfferTemplate.Company.Removed != null)
            {
                return InvalidSelect(HttpCode.Gone);
            }
            if (dbOffer.OfferConnections.First().OfferTemplate.Company.Blocked != null)
            {
                return InvalidSelect(
                    HttpCode.Forbidden,
                    Messages.Entity_Company_Status_Blocked);
            }

            return ValidSelect(_mapper.Map<DomainOffer>(dbOffer));
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
            PersonId personId,
            DomainOffer item,
            CancellationToken cancellationToken)
        {
            var result = await CheckAsync(personId, [item], cancellationToken);
            if (result.Code != HttpCode.Ok)
            {
                return InvalidUpdate(result.Code, result.Dictionary[item].Message);
            }

            var offerId = item.Id?.Value
                ?? throw new UseCaseLayerException();
            var dbOffer = await _context.Offers
                .Include(o => o.OfferConnections)
                .Include(o => o.OfferConditions)
                .Where(o => o.OfferId == offerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbOffer == null)
            {
                return InvalidUpdate(HttpCode.NotFound);
            }

            dbOffer.BranchId = item.BranchId?.Value;
            dbOffer.PublicationStart = item.PublicationRange.Start;
            dbOffer.PublicationEnd = item.PublicationRange.End;
            dbOffer.EmploymentLength = item.EmploymentLength?.Value;
            dbOffer.WebsiteUrl = item.WebsiteUrl?.Value;

            var connections = new List<DatabaseOfferConnection>();
            var offerConditions = new List<DatabaseOfferCondition>();


            // OfferConnections
            var dbOfferDictionary = dbOffer.OfferConnections
                .Where(o => o.Removed == null)
                .ToDictionary(c => c.OfferTemplateId);

            foreach (var template in item.Templates.Values)
            {
                if (dbOfferDictionary.TryGetValue(template.OfferTemplateId.Value, out var dbValue))
                {
                    dbValue.Removed = template.Removed;
                }
                else
                {
                    connections.Add(new DatabaseOfferConnection
                    {
                        Offer = dbOffer,
                        OfferTemplateId = template.OfferTemplateId.Value,
                        Created = template.Created,
                        Removed = template.Removed,
                    });
                }
            }


            // OfferConditions
            var dbOfferConditions = dbOffer.OfferConditions
                .Where(o => o.Removed == null)
                .ToDictionary(c => c.ContractConditionId);

            foreach (var contract in item.Contracts.Values)
            {
                if (dbOfferConditions.TryGetValue(contract.ContractConditionId.Value, out var dbValue))
                {
                    dbValue.Removed = contract.Removed;
                }
                else
                {
                    offerConditions.Add(new DatabaseOfferCondition
                    {
                        Offer = dbOffer,
                        ContractConditionId = contract.ContractConditionId.Value,
                        Created = contract.Created,
                        Removed = contract.Removed,
                    });
                }
            }

            await _context.OfferConnections.AddRangeAsync(connections, cancellationToken);
            await _context.OfferConditions.AddRangeAsync(offerConditions, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ValidUpdate();
        }

        public async Task<RepositoryRemoveResponse> RemoveAsync(
            PersonId personId,
            DomainOffer item,
            CancellationToken cancellationToken)
        {
            var itemId = item.Id?.Value
                ?? throw new UseCaseLayerException();

            var dbOffer = await _context.Offers
                .Where(o => o.OfferId == itemId)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbOffer == null)
            {
                return RepositoryRemoveResponse.PrepareResponse(HttpCode.NotFound);
            }

            dbOffer.PublicationStart = item.PublicationRange.Start;
            dbOffer.PublicationEnd = item.PublicationRange.End;
            await _context.SaveChangesAsync(cancellationToken);

            return RepositoryRemoveResponse.ValidResponse();
        }

        // Private Static Methods
        // Create Part
        private static RepositoryCreateResponse<DomainOffer> ValidCreate(IEnumerable<DomainOffer> items)
        {
            return RepositoryCreateResponse<DomainOffer>.ValidResponse(items);
        }

        private static RepositoryCreateResponse<DomainOffer> CreateResponse(
            HttpCode code,
            Dictionary<DomainOffer, ResponseCommandMetadata> dictionary)
        {
            return RepositoryCreateResponse<DomainOffer>.PrepareResponse(
                code,
                dictionary);
        }

        // Select Part
        private static RepositorySelectResponse<DomainOffer> InvalidSelect(
            HttpCode code,
            string? message = null)
        {
            return RepositorySelectResponse<DomainOffer>.InvalidResponse(code, message);
        }

        private static RepositorySelectResponse<DomainOffer> ValidSelect(
           DomainOffer item)
        {
            return RepositorySelectResponse<DomainOffer>.ValidResponse(item);
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


        // Private  Non Static Methods
        private async Task<RepositoryCreateResponse<DomainOffer>> CheckAsync(
            PersonId personId,
            IEnumerable<DomainOffer> items,
            CancellationToken cancellationToken)
        {
            var personIdValue = personId.Value;
            var rolesIds = _roles.Select(r => (int)r);

            // Prepare Values
            var branchIds = new HashSet<Guid>();
            var templateIds = new HashSet<Guid>();
            var contractIds = new HashSet<Guid>();

            foreach (var item in items)
            {
                if (item.BranchId != null)
                {
                    branchIds.Add(item.BranchId.Value);
                }
                foreach (var offerTemplaeId in item.Templates.Keys)
                {
                    templateIds.Add(offerTemplaeId.Value);
                }
                foreach (var contractConditionId in item.Contracts.Keys)
                {
                    contractIds.Add(contractConditionId.Value);
                }
            }

            // Select Values
            var dbBranches = await _context.Branches
                .Include(b => b.Company)
                .Where(b => branchIds.Any(branchId => b.BranchId == branchId))
                .Select(b => new
                {
                    Branch = b,
                    RolesCount = _context.CompanyPeople
                        .Count(role => rolesIds.Any(roleId =>
                            role.CompanyId == b.CompanyId &&
                            role.Deny == null &&
                            role.RoleId == roleId &&
                            role.PersonId == personIdValue
                        )),
                })
                .AsNoTracking()
                .ToDictionaryAsync(
                    b => b.Branch.BranchId,
                    b => b,
                    cancellationToken);

            var dbOfferTemplates = await _context.OfferTemplates
                .Include(ot => ot.Company)
                .Where(ot => templateIds.Any(templateId => ot.OfferTemplateId == templateId))
                .Select(ot => new
                {
                    OfferTemplate = ot,
                    RolesCount = _context.CompanyPeople
                        .Count(role => rolesIds.Any(roleId =>
                            role.CompanyId == ot.CompanyId &&
                            role.Deny == null &&
                            role.RoleId == roleId &&
                            role.PersonId == personIdValue
                        )),
                })
                .AsNoTracking()
                .ToDictionaryAsync(
                    ot => ot.OfferTemplate.OfferTemplateId,
                    ot => ot,
                    cancellationToken);

            var dbContractConditions = await _context.ContractConditions
                .Include(cc => cc.Company)
                .Where(ot => contractIds.Any(contractId => ot.ContractConditionId == contractId))
                .Select(cc => new
                {
                    ContractCondition = cc,
                    RolesCount = _context.CompanyPeople
                        .Count(role => rolesIds.Any(roleId =>
                            role.CompanyId == cc.CompanyId &&
                            role.Deny == null &&
                            role.RoleId == roleId &&
                            role.PersonId == personIdValue
                        )),
                })
                .AsNoTracking()
                .ToDictionaryAsync(
                    cc => cc.ContractCondition.ContractConditionId,
                    cc => cc,
            cancellationToken);

            // Data checking
            var resonseCode = HttpCode.Ok;
            var responseDictionary = new Dictionary<DomainOffer, ResponseCommandMetadata>();
            var stringBuilder = new StringBuilder();
            var companyIds = new HashSet<Guid>();

            foreach (var item in items)
            {
                stringBuilder.Clear();
                companyIds.Clear();

                // Branch Part
                if (item.BranchId != null)
                {
                    if (dbBranches.TryGetValue(item.BranchId.Value, out var dbBranchData))
                    {
                        companyIds.Add(dbBranchData.Branch.CompanyId);

                        if (dbBranchData.RolesCount == 0)
                        {
                            stringBuilder.AppendLine($"Branch: {HttpCode.Forbidden.Description()}");
                        }
                        if (dbBranchData.Branch.Company.Removed != null)
                        {
                            stringBuilder.AppendLine($"Branch: Company {HttpCode.Gone.Description()}");
                        }
                        if (dbBranchData.Branch.Company.Blocked != null)
                        {
                            stringBuilder.AppendLine($"Branch: {Messages.Entity_Company_Status_Blocked}");
                        }
                    }
                    else
                    {
                        stringBuilder.AppendLine($"Branch: {HttpCode.NotFound.Description()}");
                    }
                }

                // OfferTemplates Part
                foreach (var template in item.Templates.Values)
                {
                    if (dbOfferTemplates.TryGetValue(template.OfferTemplateId.Value, out var dbOfferTemplateData))
                    {
                        companyIds.Add(dbOfferTemplateData.OfferTemplate.CompanyId);

                        if (dbOfferTemplateData.RolesCount == 0)
                        {
                            stringBuilder.AppendLine($"Offer Template: {HttpCode.Forbidden.Description()}");
                        }
                        if (dbOfferTemplateData.OfferTemplate.Company.Removed != null)
                        {
                            stringBuilder.AppendLine($"Offer Template: Company {HttpCode.Gone.Description()}");
                        }
                        if (dbOfferTemplateData.OfferTemplate.Company.Blocked != null)
                        {
                            stringBuilder.AppendLine($"Offer Template: {Messages.Entity_Company_Status_Blocked}");
                        }
                    }
                    else
                    {
                        stringBuilder.AppendLine($"Offer Template: {HttpCode.NotFound.Description()}");
                    }
                }

                // ContractConditions Part
                foreach (var contract in item.Contracts.Values)
                {
                    if (dbContractConditions.TryGetValue(contract.ContractConditionId.Value, out var dbContractConditionData))
                    {
                        companyIds.Add(dbContractConditionData.ContractCondition.CompanyId);

                        if (dbContractConditionData.RolesCount == 0)
                        {
                            stringBuilder.AppendLine($"Contract Condition: {HttpCode.Forbidden.Description()}");
                        }
                        if (dbContractConditionData.ContractCondition.Company.Removed != null)
                        {
                            stringBuilder.AppendLine($"Contract Condition: Company {HttpCode.Gone.Description()}");
                        }
                        if (dbContractConditionData.ContractCondition.Company.Blocked != null)
                        {
                            stringBuilder.AppendLine($"Contract Condition: {Messages.Entity_Company_Status_Blocked}");
                        }
                    }
                    else
                    {
                        stringBuilder.AppendLine($"Contract Condition: {HttpCode.NotFound.Description()}");
                    }
                }
                // Company not same

                if (companyIds.Count > 1)
                {
                    stringBuilder.AppendLine($"All elements should be from single company");
                }

                // Create Response Data
                if (resonseCode == HttpCode.Ok &&
                    stringBuilder.Length > 0)
                {
                    resonseCode = HttpCode.BadRequest;
                }

                responseDictionary[item] = new ResponseCommandMetadata
                {
                    IsCorrect = stringBuilder.Length > 0,
                    Message = stringBuilder.ToString(),
                };
            }
            return CreateResponse(resonseCode, responseDictionary);
        }

        private async Task CreateAsync(
           IEnumerable<DomainOffer> items,
           CancellationToken cancellationToken)
        {
            var databaseOffers = new List<DatabaseOffer>();
            var connections = new List<DatabaseOfferConnection>();
            var offerConditions = new List<DatabaseOfferCondition>();

            foreach (var item in items)
            {
                var newDbOffer = _mapper.Map<DatabaseOffer>(item);

                foreach (var template in item.Templates.Values.Where(t => t.Removed == null))
                {
                    connections.Add(new DatabaseOfferConnection
                    {
                        Offer = newDbOffer,
                        OfferTemplateId = template.OfferTemplateId.Value,
                        Created = template.Created,
                        Removed = template.Removed,
                    });
                }
                foreach (var contract in item.Contracts.Values.Where(t => t.Removed == null))
                {
                    offerConditions.Add(new DatabaseOfferCondition
                    {
                        Offer = newDbOffer,
                        OfferConditionId = contract.ContractConditionId.Value,
                        Created = contract.Created,
                        Removed = contract.Removed,
                    });
                }
                databaseOffers.Add(newDbOffer);
            }

            await _context.Offers.AddRangeAsync(databaseOffers, cancellationToken);
            await _context.OfferConnections.AddRangeAsync(connections, cancellationToken);
            await _context.OfferConditions.AddRangeAsync(offerConditions, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
