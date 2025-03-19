using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Templates.Repositories;
using UseCase.Shared.Templates.Response.Commands;
using DatabaseOffer = UseCase.RelationalDatabase.Models.Offer;
using DatabaseOfferCondition = UseCase.RelationalDatabase.Models.OfferCondition;
using DatabaseOfferConnection = UseCase.RelationalDatabase.Models.OfferConnection;
using DomainOffer = Domain.Features.Offers.Entities.Offer;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Repositories
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
            if (result.Code != HttpCode.Created)
            {
                return result;
            }

            await SaveAsync(items, cancellationToken);
            return Created(items);
        }

        private async Task SaveAsync(
            IEnumerable<DomainOffer> items,
            CancellationToken cancellationToken)
        {
            var now = CustomTimeProvider.GetDateTimeNow();
            var databaseOffers = new List<DatabaseOffer>();
            var connections = new List<DatabaseOfferConnection>();
            var offerConditions = new List<DatabaseOfferCondition>();

            foreach (var domain in items)
            {
                var databaseOffer = _mapper.Map<DatabaseOffer>(domain);
                var connection = new DatabaseOfferConnection
                {
                    Offer = databaseOffer,
                    OfferTemplateId = domain.OfferTemplateId.Value,
                    Created = now,
                };
                var conditions = domain.ContractConditionIds
                    .Select(id => new DatabaseOfferCondition
                    {
                        Offer = databaseOffer,
                        ContractConditionId = id,
                        Created = now,
                    });

                databaseOffers.Add(databaseOffer);
                connections.Add(connection);
                offerConditions.AddRange(conditions);
            }

            await _context.Offers.AddRangeAsync(databaseOffers, cancellationToken);
            await _context.OfferConnections.AddRangeAsync(connections, cancellationToken);
            await _context.OfferConditions.AddRangeAsync(offerConditions, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<RepositoryCreateResponse<DomainOffer>> CheckAsync(
            PersonId personId,
            IEnumerable<DomainOffer> items,
            CancellationToken cancellationToken)
        {
            var roleIds = _roles.Select(r => (int)r);

            var offerTemplateIds = items
                .Select(i => i.OfferTemplateId.Value)
                .ToHashSet();
            var branchIds = items
                .Where(i => i.BranchId != null)
                .Select(i => i.BranchId.Value)
                .ToHashSet();
            var contractConditionIds = items
                .SelectMany(i => i.ContractConditionIds)
                .Select(i => (Guid)i)
                .ToHashSet();

            var templates = await _context.OfferTemplates
                .Include(ot => ot.Company)
                .ThenInclude(c => c.CompanyPeople)
                .Where(ot => offerTemplateIds.Any(id => ot.OfferTemplateId == id))
            .Select(ot => new
            {
                OfferTemplateId = ot.OfferTemplateId,
                CompanyId = ot.CompanyId,
                RoleCount = ot.Company.CompanyPeople.Count(role => roleIds.Any(roleId =>
                    role.RoleId == roleId &&
                    role.PersonId == personId.Value &&
                    role.Deny == null
                )),
            })
                .ToDictionaryAsync(
                val => val.OfferTemplateId,
                val => new { val.RoleCount, val.CompanyId },
                cancellationToken);

            var branches = await _context.Branches
               .Include(ot => ot.Company)
               .ThenInclude(c => c.CompanyPeople)
               .Where(ot => branchIds.Any(id => ot.BranchId == id))
           .Select(ot => new
           {
               BranchId = ot.BranchId,
               CompanyId = ot.CompanyId,
               RoleCount = ot.Company.CompanyPeople.Count(role => roleIds.Any(roleId =>
                   role.RoleId == roleId &&
                   role.PersonId == personId.Value &&
                   role.Deny == null
               )),
           })
               .ToDictionaryAsync(
               val => val.BranchId,
               val => new { val.RoleCount, val.CompanyId },
               cancellationToken);

            var conditions = await _context.ContractConditions
               .Include(ot => ot.Company)
               .ThenInclude(c => c.CompanyPeople)
               .Where(ot => contractConditionIds.Any(id => ot.ContractConditionId == id))
           .Select(ot => new
           {
               ContractConditionId = ot.ContractConditionId,
               CompanyId = ot.CompanyId,
               RoleCount = ot.Company.CompanyPeople.Count(role => roleIds.Any(roleId =>
                   role.RoleId == roleId &&
                   role.PersonId == personId.Value &&
                   role.Deny == null
               )),
           })
               .ToDictionaryAsync(
               val => val.ContractConditionId,
               val => new { val.RoleCount, val.CompanyId },
               cancellationToken);

            var code = HttpCode.Created;
            var dictionary = new Dictionary<DomainOffer, ResponseCommandMetadata>();
            var stringBuilder = new StringBuilder();

            foreach (var item in items)
            {
                stringBuilder.Clear();
                var isNotFound = false;
                var isForbidden = false;

                // Company Part
                if (!templates.TryGetValue(item.OfferTemplateId.Value, out var template)
                    || template == null)
                {
                    isNotFound = true;
                    stringBuilder.AppendLine($"OfferTemplate {HttpCode.NotFound.Description()}");
                }
                if (template != null && template.RoleCount == 0)
                {
                    isForbidden = true;
                    stringBuilder.AppendLine($"OfferTemplate {HttpCode.Forbidden.Description()}");
                }

                // Branch Part
                if (item.BranchId != null)
                {
                    if (!branches.TryGetValue(item.BranchId.Value, out var branch)
                        || branch == null)
                    {
                        isNotFound = true;
                        stringBuilder.AppendLine($"Branch {HttpCode.NotFound.Description()}");
                    }

                    if (branch != null && branch.RoleCount == 0)
                    {
                        isForbidden = true;
                        stringBuilder.AppendLine($"Branch {HttpCode.Forbidden.Description()}");
                    }

                    if (
                        template != null &&
                        branch != null &&
                        template.CompanyId != branch.CompanyId)
                    {
                        isForbidden = true;
                        stringBuilder.AppendLine("Branch Company is not same as OfferTemplate Company");
                    }
                }

                // ContractConditionIds Part
                foreach (var ccId in item.ContractConditionIds)
                {
                    if (!conditions.TryGetValue(ccId.Value, out var contractCondition)
                        || contractCondition == null)
                    {
                        isNotFound = true;
                        stringBuilder.AppendLine($"ContractCondition {HttpCode.NotFound.Description()}: {ccId.Value}");
                    }

                    if (contractCondition != null && contractCondition.RoleCount == 0)
                    {
                        isForbidden = true;
                        stringBuilder.AppendLine($"ContractCondition {HttpCode.Forbidden.Description()}: {ccId.Value}");
                    }

                    if (template != null &&
                        contractCondition != null &&
                        template.CompanyId != contractCondition.CompanyId
                        )
                    {
                        isForbidden = true;
                        stringBuilder.AppendLine($"ContractCondition Company is not same as OfferTemplate Company");
                    }
                }

                if (code == HttpCode.Created && isForbidden)
                {
                    code = HttpCode.Forbidden;
                }
                if (code == HttpCode.Created && isNotFound)
                {
                    code = HttpCode.NotFound;
                }

                dictionary[item] = new ResponseCommandMetadata
                {
                    IsCorrect = stringBuilder.Length == 0,
                    Message = stringBuilder.ToString().Trim(),
                };
            }

            return new RepositoryCreateResponse<DomainOffer>
            {
                Dictionary = dictionary,
                Code = code
            };
        }

        protected static RepositoryCreateResponse<DomainOffer> Created(
            IEnumerable<DomainOffer> items)
        {
            return GenerateResponse(true, HttpCode.Created, items);
        }

        protected static RepositoryCreateResponse<DomainOffer> GenerateResponse(
            bool isCorrect,
            HttpCode code,
            IEnumerable<DomainOffer> items)
        {
            return new RepositoryCreateResponse<DomainOffer>
            {
                Dictionary = items.ToDictionary(
                        val => val,
                        val => new ResponseCommandMetadata
                        {
                            IsCorrect = isCorrect,
                            Message = code.Description(),
                        }),
                Code = code,
            };
        }
    }
}
