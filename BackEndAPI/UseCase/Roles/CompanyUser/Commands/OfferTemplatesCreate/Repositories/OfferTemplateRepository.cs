using AutoMapper;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Shared.Templates.Repositories;
using UseCase.Shared.Templates.Response.Commands;
using DatabaseOfferSkill = UseCase.RelationalDatabase.Models.OfferSkill;
using DatabaseOfferTemplate = UseCase.RelationalDatabase.Models.OfferTemplate;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Entities.OfferTemplate;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Repositories
{
    public class OfferTemplateRepository : IOfferTemplateRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IMapper _mapper;
        private static readonly IEnumerable<CompanyUserRoles> _roles = [
            CompanyUserRoles.CompanyOwner];


        // Constructor
        public OfferTemplateRepository(
            DiplomaBdContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Methods
        public async Task<RepositoryCreateResponse<DomainOfferTemplate>> CreateAsync(
            PersonId personId,
            IEnumerable<DomainOfferTemplate> items,
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
        private static RepositoryCreateResponse<DomainOfferTemplate> NotFound(
            IEnumerable<DomainOfferTemplate> items)
        {
            return GenerateResponse(false, HttpCode.NotFound, items);
        }

        private static RepositoryCreateResponse<DomainOfferTemplate> Forbidden(
            IEnumerable<DomainOfferTemplate> items)
        {
            return GenerateResponse(false, HttpCode.Forbidden, items);
        }

        private static RepositoryCreateResponse<DomainOfferTemplate> Created(
            IEnumerable<DomainOfferTemplate> items)
        {
            return GenerateResponse(true, HttpCode.Created, items);
        }

        private static RepositoryCreateResponse<DomainOfferTemplate> GenerateResponse(
            bool isCorrect,
            HttpCode code,
            IEnumerable<DomainOfferTemplate> items)
        {
            return new RepositoryCreateResponse<DomainOfferTemplate>
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
        private async Task<RepositoryCreateResponse<DomainOfferTemplate>> ValidateAsync(
            PersonId personId,
            IEnumerable<DomainOfferTemplate> items,
            CancellationToken cancellationToken)
        {
            var companyId = items.First().CompanyId.Value;
            var roleIds = _roles.Select(role => (int)role);

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
            IEnumerable<DomainOfferTemplate> items,
            CancellationToken cancellationToken)
        {
            var dbItems = new List<DatabaseOfferTemplate>();
            var dbOfferSkills = new List<DatabaseOfferSkill>();
            foreach (var item in items)
            {
                var dbItem = _mapper.Map<DatabaseOfferTemplate>(item);
                var skills = item.Skills.Select(skillId => new DatabaseOfferSkill
                {
                    OfferTemplate = dbItem,
                    SkillId = skillId.SkillId,
                    Created = dbItem.Created,
                });
                dbItems.Add(dbItem);
                dbOfferSkills.AddRange(skills);
            }

            await _context.OfferTemplates.AddRangeAsync(dbItems, cancellationToken);
            await _context.OfferSkills.AddRangeAsync(dbOfferSkills, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
