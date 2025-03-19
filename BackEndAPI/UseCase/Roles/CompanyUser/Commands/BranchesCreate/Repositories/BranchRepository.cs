using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Templates.Repositories;
using UseCase.Shared.Templates.Response.Commands;
using DatabaseBranch = UseCase.RelationalDatabase.Models.Branch;
using DomainBranch = Domain.Features.Branches.Entities.Branch;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate.Repositories
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
                return Created(items);
            }

            var validateResult = await ValidateAsync(personId, items, cancellationToken);
            if (validateResult.Code != HttpCode.Created)
            {
                return validateResult;
            }

            await EFCreateAsync(items, cancellationToken);
            return validateResult; // HttpCode.Created
        }


        // Private Static Methods
        private static RepositoryCreateResponse<DomainBranch> NotFound(
            IEnumerable<DomainBranch> items)
        {
            return GenerateResponse(false, HttpCode.NotFound, items);
        }

        private static RepositoryCreateResponse<DomainBranch> Forbidden(
            IEnumerable<DomainBranch> items)
        {
            return GenerateResponse(false, HttpCode.Forbidden, items);
        }

        private static RepositoryCreateResponse<DomainBranch> Created(
            IEnumerable<DomainBranch> items)
        {
            return GenerateResponse(true, HttpCode.Created, items);
        }

        private static RepositoryCreateResponse<DomainBranch> GenerateResponse(
            bool isCorrect,
            HttpCode code,
            IEnumerable<DomainBranch> items)
        {
            return new RepositoryCreateResponse<DomainBranch>
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
        /// <summary>
        /// Check is Exist and have access to Company 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="items"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<RepositoryCreateResponse<DomainBranch>> ValidateAsync(
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
            IEnumerable<DomainBranch> items,
            CancellationToken cancellationToken)
        {
            var dbItems = _mapper.Map<IEnumerable<DatabaseBranch>>(items);
            await _context.Branches.AddRangeAsync(dbItems, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
