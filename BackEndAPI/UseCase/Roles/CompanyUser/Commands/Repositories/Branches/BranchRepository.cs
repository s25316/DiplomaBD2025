using AutoMapper;
using Domain.Features.Companies.ValueObjects;
using Domain.Features.People.ValueObjects;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using DatabaseBranch = UseCase.RelationalDatabase.Models.Branch;
using DomainBranch = Domain.Features.Branches.Entities.Branch;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.Branches
{
    public class BranchRepository : IBranchRepository
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public BranchRepository(IMapper mapper, DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Public Methods
        public async Task CreateAsync(
            IEnumerable<DomainBranch> items,
            CancellationToken cancellationToken)
        {
            var list = new List<DatabaseBranch>();
            foreach (var item in items)
            {
                list.Add(_mapper.Map<DatabaseBranch>(item));
            }
            await _context.Branches.AddRangeAsync(list, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> HasAccessToCompany(
            PersonId personId,
            CompanyId companyId,
            CancellationToken cancellationToken)
        {
            var count = await _context.CompanyPeople
                .Where(cp =>
                    cp.PersonId == personId.Value &&
                    cp.CompanyId == companyId.Value)
                .CountAsync(cancellationToken);
            return count > 0;
        }

    }
}
