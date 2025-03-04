using AutoMapper;
using Domain.Features.Companies.ValueObjects;
using Domain.Features.People.ValueObjects;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using DatabaseOfferSkill = UseCase.RelationalDatabase.Models.OfferSkill;
using DatabaseOfferTemplate = UseCase.RelationalDatabase.Models.OfferTemplate;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Entities.OfferTemplate;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.OfferTemplates
{
    public class OfferTemplateRepository : IOfferTemplateRepository
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public OfferTemplateRepository(IMapper mapper, DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Public Methods
        public async Task CreateAsync(IEnumerable<DomainOfferTemplate> items, CancellationToken cancellationToken)
        {
            var listDatabaseOfferTemplates = new List<DatabaseOfferTemplate>();
            var listDatabaseOfferSkills = new List<DatabaseOfferSkill>();

            foreach (var domain in items)
            {
                var database = _mapper.Map<DatabaseOfferTemplate>(domain);
                var skills = domain.Skills.Select(skill => new DatabaseOfferSkill
                {
                    SkillId = skill.SkillId,
                    IsRequired = skill.IsRequired,
                    OfferTemplate = database,
                    Created = database.Created,
                });
                listDatabaseOfferTemplates.Add(database);
                listDatabaseOfferSkills.AddRange(skills);
            }

            await _context.OfferTemplates.AddRangeAsync(listDatabaseOfferTemplates, cancellationToken);
            await _context.OfferSkills.AddRangeAsync(listDatabaseOfferSkills, cancellationToken);
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
