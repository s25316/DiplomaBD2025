using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.BaseEFRepository;
using UseCase.Shared.Responses.CommandResults;
using DatabaseCompany = UseCase.RelationalDatabase.Models.Company;
using DomainCompany = Domain.Features.Companies.Entities.Company;
using DomainCompanyId = Domain.Features.Companies.ValueObjects.Ids.CompanyId;

namespace UseCase.Roles.CompanyUser.Repositories.Companies
{
    public class CompanyRepository : ICompanyRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private static readonly IEnumerable<CompanyUserRoles> _roles = [
            CompanyUserRoles.CompanyOwner
            ];


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
                return ValidCreate(items);
            }

            var validationResult = await ValidateAsync(items, cancellationToken);
            if (validationResult.Code != HttpCode.Created)
            {
                return validationResult;
            }

            await EFCreateAsync(personId, items, cancellationToken);
            return ValidCreate(items);
        }

        public async Task<RepositorySelectResponse<DomainCompany>> GetAsync(
            PersonId personId,
            DomainCompanyId id,
            CancellationToken cancellationToken)
        {
            var roleIds = _roles.Select(role => (int)role);
            var personIdValue = personId.Value;

            var selectResult = await _context.Companies
                .Where(company => company.CompanyId == id.Value)
                .Select(company => new
                {
                    Company = company,
                    RolesCount = _context.CompanyPeople
                        .Where(role => roleIds.Any(roleId =>
                            role.RoleId == roleId &&
                            role.PersonId == personIdValue &&
                            role.Deny == null
                        )).Count(),
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


            var dbCompany = selectResult.Company;
            if (dbCompany.Removed != null)
            {
                return InvalidSelect(HttpCode.Gone);
            }
            if (dbCompany.Blocked != null)
            {
                return InvalidSelect(
                    HttpCode.Forbidden,
                    Messages.Entity_Company_Status_Blocked);
            }

            return ValidSelect(_mapper.Map<DomainCompany>(dbCompany));
        }

        public async Task<RepositoryUpdateResponse> UpdateAsync(
            PersonId personId,
            DomainCompany item,
            CancellationToken cancellationToken)
        {
            // Prepare Data
            var companyId = item.Id?.Value
                ?? throw new UseCaseLayerException();
            // Because Only KRS is unique and updating in Domain
            var krs = item.Krs?.Value ?? null;

            // select from DB data
            var dbCompanies = await _context.Companies
                .Where(company =>
                    (krs == null && company.CompanyId == companyId) ||
                    (krs != null && (company.CompanyId == companyId || company.Krs == krs))
                )
                .ToListAsync(cancellationToken);

            if (!dbCompanies.Any())
            {
                return InvalidUpdate(HttpCode.NotFound);
            }
            if (dbCompanies.Count != 1)
            {
                return InvalidUpdate(
                    HttpCode.Conflict,
                    Messages.Entity_Company_Krs_Conflict);
            }

            // Updating
            var dbCompany = dbCompanies[0];
            dbCompany.Krs = item.Krs?.Value;
            dbCompany.Description = item.Description;
            dbCompany.WebsiteUrl = item.WebsiteUrl;
            dbCompany.Logo = item.Logo;
            await _context.SaveChangesAsync(cancellationToken);

            return ValidUpdate();
        }

        public Task<RepositoryRemoveResponse> RemoveAsync(
            PersonId personId,
            DomainCompany item,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Private Static Methods
        // Create Part
        private static RepositoryCreateResponse<DomainCompany> ValidCreate(
            IEnumerable<DomainCompany> items)
        {
            return RepositoryCreateResponse<DomainCompany>.ValidResponse(items);
        }

        private static RepositoryCreateResponse<DomainCompany> PrepareCreateResponse(
            HttpCode code,
            Dictionary<DomainCompany, ResultMetadata> dictionary)
        {
            return RepositoryCreateResponse<DomainCompany>.PrepareResponse(code, dictionary);
        }

        // Select Part
        private static RepositorySelectResponse<DomainCompany> InvalidSelect(
            HttpCode code,
            string? message = null)
        {
            return RepositorySelectResponse<DomainCompany>.InvalidResponse(code, message);
        }

        private static RepositorySelectResponse<DomainCompany> ValidSelect(DomainCompany company)
        {
            return RepositorySelectResponse<DomainCompany>.ValidResponse(company);
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
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var nameSet = dbDuplicates.Select(x => x.Name).ToHashSet();
            var regonSet = dbDuplicates.Select(x => x.Regon).ToHashSet();
            var nipSet = dbDuplicates.Select(x => x.Nip).ToHashSet();
            var krsSet = dbDuplicates.Select(x => x.Krs).ToHashSet();


            var code = HttpCode.Created;
            var dictionary = new Dictionary<DomainCompany, ResultMetadata>();

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
                dictionary.Add(item, new ResultMetadata
                {
                    IsCorrect = stringBuilder.Length == 0,
                    Message = stringBuilder.ToString().Trim(),
                });
            }

            return PrepareCreateResponse(code, dictionary);
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
