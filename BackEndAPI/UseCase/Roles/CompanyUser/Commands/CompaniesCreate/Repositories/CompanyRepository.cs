using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Templates.Repositories;
using UseCase.Shared.Templates.Response.Commands;
using DatabaseCompany = UseCase.RelationalDatabase.Models.Company;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;


        // Constructor
        public CompanyRepository(
            DiplomaBdContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Methods

        public async Task<RepositoryCreateResponse<DomainCompany>> CreateAsync(
            PersonId personId,
            IEnumerable<DomainCompany> items,
            CancellationToken cancellationToken)
        {
            if (!items.Any())
            {
                return Created(items);
            }

            var validationResult = await ValidateAsync(items, cancellationToken);
            if (validationResult.Code != HttpCode.Created)
            {
                return validationResult;
            }

            await EFCreateAsync(personId, items, cancellationToken);
            return Created(items);
        }

        // Private Static Methods
        private static RepositoryCreateResponse<DomainCompany> Created(
            IEnumerable<DomainCompany> items)
        {
            return GenerateResponse(true, HttpCode.Created, items);
        }

        private static RepositoryCreateResponse<DomainCompany> GenerateResponse(
            bool isCorrect,
            HttpCode code,
            IEnumerable<DomainCompany> items)
        {
            return new RepositoryCreateResponse<DomainCompany>
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
        private async Task<RepositoryCreateResponse<DomainCompany>> ValidateAsync(
            IEnumerable<DomainCompany> items,
            CancellationToken cancellationToken)
        {
            var nipList = items.Select(i => i.Nip.Value).ToList();
            var regonList = items.Select(i => i.Regon.Value).ToList();
            var krsList = items.Where(i => i.Krs != null)
                .Select(i => i.Krs.Value).ToList();
            var nameList = items.Select(i => i.Name).ToList();

            Expression<Func<DatabaseCompany, bool>> filter = company =>
                nameList.Any(name => company.Name == name) ||
                regonList.Any(regon => company.Regon == regon) ||
                nipList.Any(nip => company.Nip == nip) ||
                krsList.Any(krs => company.Krs == krs);

            var dbDuplicates = await _context.Companies
                .Where(filter)
                .Select(db => new { db.Name, db.Nip, db.Regon, db.Krs })
                .ToListAsync(cancellationToken);

            var nameSet = dbDuplicates.Select(x => x.Name).ToHashSet();
            var regonSet = dbDuplicates.Select(x => x.Regon).ToHashSet();
            var nipSet = dbDuplicates.Select(x => x.Nip).ToHashSet();
            var krsSet = dbDuplicates.Select(x => x.Krs).ToHashSet();


            var code = HttpCode.Created;
            var dictionary = new Dictionary<DomainCompany, ResponseCommandMetadata>();

            var stringBuilder = new StringBuilder();

            foreach (var item in items)
            {
                stringBuilder.Clear();

                if (nameSet.Contains(item.Name))
                {
                    stringBuilder.AppendLine(Messages.Entity_Company_Name_Conflict);
                }
                if (regonSet.Contains(item.Regon.Value))
                {
                    stringBuilder.AppendLine(Messages.Entity_Company_Regon_Conflict);
                }
                if (nipSet.Contains(item.Nip.Value))
                {
                    stringBuilder.AppendLine(Messages.Entity_Company_Nip_Conflict);
                }
                if (item.Krs != null && krsSet.Contains(item.Regon.Value))
                {
                    stringBuilder.AppendLine(Messages.Entity_Company_Krs_Conflict);
                }

                if (code == HttpCode.Created && stringBuilder.Length > 0)
                {
                    code = HttpCode.Conflict;
                }
                dictionary.Add(item, new ResponseCommandMetadata
                {
                    IsCorrect = stringBuilder.Length == 0,
                    Message = stringBuilder.ToString().Trim(),
                });
            }
            return new RepositoryCreateResponse<DomainCompany>
            {
                Code = code,
                Dictionary = dictionary,
            };
        }

        private async Task EFCreateAsync(
            PersonId personId,
            IEnumerable<DomainCompany> items,
            CancellationToken cancellationToken)
        {
            var dbItems = _mapper.Map<IEnumerable<DatabaseCompany>>(items);
            var dbRoles = dbItems.Select(dbItem => new CompanyPerson
            {
                Company = dbItem,
                PersonId = personId.Value,
                RoleId = (int)CompanyUserRoles.CompanyOwner,
                Grant = dbItem.Created,
            });

            await _context.CompanyPeople.AddRangeAsync(dbRoles, cancellationToken);
            await _context.Companies.AddRangeAsync(dbItems, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
