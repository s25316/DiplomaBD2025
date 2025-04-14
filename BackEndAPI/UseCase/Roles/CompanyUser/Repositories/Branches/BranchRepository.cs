using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Templates.Repositories;
using DatabaseBranch = UseCase.RelationalDatabase.Models.Branch;
using DomainBranch = Domain.Features.Branches.Entities.Branch;
using DomainBranchId = Domain.Features.Branches.ValueObjects.BranchId;

namespace UseCase.Roles.CompanyUser.Repositories.Branches
{
    public class BranchRepository : IBranchRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private static readonly IEnumerable<CompanyUserRoles> _roles = [
            CompanyUserRoles.CompanyOwner];


        // Constructor
        public BranchRepository(
            DiplomaBdContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Methods
        public async Task<RepositoryCreateResponse<DomainBranch>> CreateAsync(
            PersonId personId,
            IEnumerable<DomainBranch> items,
            CancellationToken cancellationToken)
        {
            if (!items.Any())
            {
                return ValidCreate(items);
            }

            var validateResult = await CompanyCheckAsync(personId, items, cancellationToken);
            if (validateResult.Code != HttpCode.Created)
            {
                return validateResult;
            }

            await CreateAsync(items, cancellationToken);
            return validateResult; // HttpCode.Created
        }

        public async Task<RepositorySelectResponse<DomainBranch>> GetAsync(
           PersonId personId,
           DomainBranchId id,
           CancellationToken cancellationToken)
        {
            // Adapt Data
            var personIdValue = personId.Value;
            var branchId = id.Value;
            var roleIds = _roles.Select(r => (int)r);

            // Select from DB
            var selectResult = await _context.Branches
                .Include(b => b.Company)
                .Where(branch => branch.BranchId == branchId)
                .Select(branch => new
                {
                    Branch = branch,
                    RolesCount = _context.CompanyPeople
                        .Count(role => roleIds.Any(roleId =>
                            role.CompanyId == branch.CompanyId &&
                            role.PersonId == personIdValue &&
                            role.RoleId == roleId &&
                            role.Deny == null
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

            var dbBranch = selectResult.Branch;
            if (dbBranch.Company.Removed != null)
            {
                return InvalidSelect(HttpCode.Gone);
            }
            if (dbBranch.Company.Blocked != null)
            {
                return InvalidSelect(
                    HttpCode.Forbidden,
                    Messages.Entity_Company_Status_Blocked);
            }

            return ValidSelect(_mapper.Map<DomainBranch>(dbBranch));
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
            PersonId personId,
            DomainBranch item,
            CancellationToken cancellationToken)
        {
            // Adapt Data
            var branchId = item.Id?.Value ?? throw new KeyNotFoundException();

            // Select from DB
            var dbBranch = await _context.Branches
                .Include(b => b.Company)
                .Where(branch => branch.BranchId == branchId)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbBranch == null)
            {
                return InvalidUpdate(HttpCode.NotFound);
            }

            // Update in DB
            dbBranch.AddressId = item.AddressId.Value;
            dbBranch.Name = item.Name;
            dbBranch.Description = item.Description;
            dbBranch.Removed = item.Removed;
            await _context.SaveChangesAsync(cancellationToken);

            return ValidUpdate();
        }

        public async Task<RepositoryRemoveResponse> RemoveAsync(
            PersonId personId,
            DomainBranch item,
            CancellationToken cancellationToken)
        {
            var updateResut = await UpdateAsync(personId, item, cancellationToken);
            if (updateResut.Code != HttpCode.Ok)
            {
                return RepositoryRemoveResponse.PrepareResponse(
                    updateResut.Code,
                    updateResut.Metadata.Message);
            }
            return RepositoryRemoveResponse.ValidResponse();
        }

        // Private Static Methods
        // Create Part
        private static RepositoryCreateResponse<DomainBranch> ValidCreate(
            IEnumerable<DomainBranch> items)
        {
            return RepositoryCreateResponse<DomainBranch>.ValidResponse(items);
        }

        private static RepositoryCreateResponse<DomainBranch> InvalidCreate(
            HttpCode code,
            IEnumerable<DomainBranch> items,
            string? description = null)
        {
            return RepositoryCreateResponse<DomainBranch>.PrepareResponse(
                code,
                description,
                items);
        }

        // Select Part
        private static RepositorySelectResponse<DomainBranch> InvalidSelect(
            HttpCode code,
            string? message = null)
        {
            return RepositorySelectResponse<DomainBranch>.InvalidResponse(
                code,
                message);
        }

        private static RepositorySelectResponse<DomainBranch> ValidSelect(
            DomainBranch item)
        {
            return RepositorySelectResponse<DomainBranch>.ValidResponse(item);
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
        private async Task<RepositoryCreateResponse<DomainBranch>> CompanyCheckAsync(
            PersonId personId,
            IEnumerable<DomainBranch> items,
            CancellationToken cancellationToken)
        {
            var companyId = items.First().CompanyId.Value;
            var roleIds = _roles.Select(@enum => (int)@enum);

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
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (authorizationResult == null)
            {
                return InvalidCreate(HttpCode.NotFound, items);
            }
            if (authorizationResult.RolesCount == 0)
            {
                return InvalidCreate(HttpCode.Forbidden, items);
            }
            if (authorizationResult.Company.Removed != null)
            {
                return InvalidCreate(HttpCode.Gone, items);
            }
            if (authorizationResult.Company.Blocked != null)
            {
                return InvalidCreate(
                    HttpCode.Forbidden,
                    items,
                    Messages.Entity_Company_Status_Blocked);
            }

            return ValidCreate(items);
        }

        private async Task CreateAsync(
            IEnumerable<DomainBranch> items,
            CancellationToken cancellationToken)
        {
            var dbItems = _mapper.Map<IEnumerable<DatabaseBranch>>(items);
            await _context.Branches.AddRangeAsync(dbItems, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
