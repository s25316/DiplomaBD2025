using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Templates.Repositories;
using UseCase.Shared.Templates.Response.Commands;
using DatabaseContractAttribute = UseCase.RelationalDatabase.Models.ContractAttribute;
using DatabaseContractCondition = UseCase.RelationalDatabase.Models.ContractCondition;
using DomainContractCondition = Domain.Features.ContractConditions.Entities.ContractCondition;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Repositories
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
                return Created(items);
            }

            var validationResult = await ValidateAsync(personId, items, cancellationToken);
            if (validationResult.Code != HttpCode.Created)
            {
                return validationResult;
            }

            await EFCreateAsync(items, cancellationToken);
            return validationResult;
        }

        // Private Static Methods
        private static RepositoryCreateResponse<DomainContractCondition> NotFound(
           IEnumerable<DomainContractCondition> items)
        {
            return GenerateResponse(false, HttpCode.NotFound, items);
        }

        private static RepositoryCreateResponse<DomainContractCondition> Forbidden(
            IEnumerable<DomainContractCondition> items)
        {
            return GenerateResponse(false, HttpCode.Forbidden, items);
        }

        private static RepositoryCreateResponse<DomainContractCondition> Created(
            IEnumerable<DomainContractCondition> items)
        {
            return GenerateResponse(true, HttpCode.Created, items);
        }

        private static RepositoryCreateResponse<DomainContractCondition> GenerateResponse(
            bool isCorrect,
            HttpCode code,
            IEnumerable<DomainContractCondition> items)
        {
            return new RepositoryCreateResponse<DomainContractCondition>
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


        // Private Non Static Methods
        private async Task<RepositoryCreateResponse<DomainContractCondition>> ValidateAsync(
           PersonId personId,
           IEnumerable<DomainContractCondition> items,
           CancellationToken cancellationToken)
        {
            var companyId = items.First().CompanyId.Value;
            var roleIds = _roles.Select(x => (int)x);
            var authorizationResult = await _context.Companies
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
                .FirstOrDefaultAsync(cancellationToken);

            if (authorizationResult == null)
            {
                return NotFound(items);
            }

            if (authorizationResult.RolesCount == 0)
            {
                return Forbidden(items);
            }
            return Created(items);
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
                if (domain.CurrencyId != null)
                {
                    paramiterIds.Add(domain.CurrencyId.Value);
                }
                if (domain.SalaryTermId != null)
                {
                    paramiterIds.Add(domain.SalaryTermId.Value);
                }
                paramiterIds.AddRange(domain.WorkModeIds);
                paramiterIds.AddRange(domain.EmploymentTypeIds);

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
