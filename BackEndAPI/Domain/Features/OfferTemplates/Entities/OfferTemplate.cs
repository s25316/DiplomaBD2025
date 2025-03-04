using Domain.Features.Companies.ValueObjects;
using Domain.Features.OfferTemplates.Exceptions;
using Domain.Features.OfferTemplates.ValueObjects;
using Domain.Shared.Templates;

namespace Domain.Features.OfferTemplates.Entities
{
    public partial class OfferTemplate : TemplateEntity<OfferTemplateId>
    {
        // Properties
        public CompanyId CompanyId { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; }
        public HashSet<OfferSkill> Skills { get; private set; } = [];


        //Methods
        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new OfferTemplateException(Messages.Entity_OfferTemplate_EmptyName);
            }
            Name = name.Trim();
        }

        private void SetDescription(string description)
        {

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new OfferTemplateException(Messages.Entity_OfferTemplate_EmptyDescription);
            }
            Description = description.Trim();
        }

        private void SetSkillsIds(IEnumerable<OfferSkill> skills)
        {
            Skills = new HashSet<OfferSkill>(skills);
        }
    }
}
