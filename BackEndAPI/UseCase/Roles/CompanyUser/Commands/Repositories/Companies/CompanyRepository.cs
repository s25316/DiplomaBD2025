using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.CustomProviders;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using DatabaseCompany = UseCase.RelationalDatabase.Models.Company;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.Companies
{
    public class CompanyRepository : ICompanyRepository
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        //Constructor
        public CompanyRepository(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task CreateAsync(
            PersonId personId,
            DomainCompany item,
            CancellationToken cancellationToken)
        {
            var company = Map(item);
            var role = new CompanyPerson
            {
                Company = company,
                PersonId = personId,
                RoleId = 1, // 1 - Company owner
                Grant = CustomTimeProvider.GetDateTimeNow(),
            };

            await _context.Companies.AddAsync(company, cancellationToken);
            await _context.CompanyPeople.AddAsync(role, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<string> FindDuplicatesAsync(
            DomainCompany domain,
            CancellationToken cancellationToken)
        {
            Expression<Func<DatabaseCompany, bool>> filter = databse =>
                (databse.Name == domain.Name ||
                 databse.Nip == (string)domain.Nip ||
                 databse.Regon == (string)domain.Regon) &&
                (domain.Krs == null || databse.Krs == domain.Krs.Value) &&
                (domain.Id == null || databse.CompanyId != domain.Id.Value);

            var duplicates = await _context.Companies
                .Where(filter)
                .Select(company => new
                {
                    company.Name,
                    company.Nip,
                    company.Regon,
                    company.Krs
                })
                .ToListAsync(cancellationToken);

            if (!duplicates.Any())
            {
                return string.Empty;
            }
            bool dupliacteName = false;
            bool dupliacteNip = false;
            bool dupliacteRegon = false;
            bool dupliacteKrs = domain.Krs == null;

            int counter = 0;
            while (
                counter < duplicates.Count &&
                (!dupliacteName || !dupliacteNip || !dupliacteRegon || !dupliacteKrs)
                )
            {
                var company = duplicates[counter++];
                if (!dupliacteName && company.Name == domain.Name)
                {
                    dupliacteName = true;
                }
                if (!dupliacteNip && company.Nip == domain.Nip.Value)
                {
                    dupliacteNip = true;
                }
                if (!dupliacteRegon && company.Regon == domain.Regon.Value)
                {
                    dupliacteRegon = true;
                }
                if (!dupliacteKrs && domain.Krs != null && company.Krs == domain.Krs.Value)
                {
                    dupliacteKrs = true;
                }
            }
            var builder = new StringBuilder();
            if (dupliacteName)
            {
                builder.AppendLine(Messages.Entity_Company_NameDuplicate);
            }
            if (dupliacteNip)
            {
                builder.AppendLine(Messages.Entity_Company_NipDuplicate);
            }
            if (dupliacteRegon)
            {
                builder.AppendLine(Messages.Entity_Company_RegonDuplicate);
            }
            if (dupliacteKrs && domain.Krs != null)
            {
                builder.AppendLine(Messages.Entity_Company_KrsDuplicate);
            }
            return builder.ToString();
        }

        // Private Methods
        private DomainCompany Map(DatabaseCompany database)
        {
            return _mapper.Map<DatabaseCompany, DomainCompany>(database);
        }

        private DatabaseCompany Map(DomainCompany domain)
        {
            var database = _mapper.Map<DomainCompany, DatabaseCompany>(domain);
            if (domain.Id != null)
            {
                database.CompanyId = domain.Id;
            }
            return database;
        }

    }
}
