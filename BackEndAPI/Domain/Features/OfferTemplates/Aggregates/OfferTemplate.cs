using Domain.Features.Companies.ValueObjects.Ids;
using Domain.Features.OfferTemplates.Entities;
using Domain.Features.OfferTemplates.Exceptions;
using Domain.Features.OfferTemplates.ValueObjects.Ids;
using Domain.Features.OfferTemplates.ValueObjects.Info;
using Domain.Shared.CustomProviders;
using Domain.Shared.CustomProviders.StringProvider;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Ids;

namespace Domain.Features.OfferTemplates.Aggregates
{
    public partial class OfferTemplate : TemplateEntity<OfferTemplateId>
    {
        // Properties
        public CompanyId CompanyId { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; }
        // Collections
        private Dictionary<SkillId, OfferSkill> _skillsDictionary = [];
        public IReadOnlyDictionary<SkillId, OfferSkill> SkillsDictionary => _skillsDictionary;


        // Public Methods
        public void Remove()
        {
            Removed = Removed.HasValue
                ? null
                : CustomTimeProvider.Now;
        }

        // Private Methods
        private void SetName(string name)
        {
            name = CustomStringProvider.NormalizeWhitespace(
                name,
                WhiteSpace.All);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new OfferTemplateException(Messages.Entity_OfferTemplate_EmptyName);
            }
            Name = name;
        }

        private void SetDescription(string description)
        {
            description = CustomStringProvider.NormalizeWhitespace(
                description,
                WhiteSpace.AllExceptNewLine);

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new OfferTemplateException(Messages.Entity_OfferTemplate_EmptyDescription);
            }
            Description = description;
        }

        private void SetSkills(IEnumerable<OfferSkillInfo> inputs)
        {
            if (!_skillsDictionary.Any())
            {
                foreach (var input in inputs)
                {
                    var offferSkill = Map(input);
                    _skillsDictionary[offferSkill.SkillId] = offferSkill;
                }
            }
            else
            {
                var inputsDictionary = inputs.ToDictionary(item => (SkillId)item.SkillId);

                // ToHashSet() Make faster next operations 
                var inputsKeys = inputsDictionary.Keys.ToHashSet();
                var existingKeys = _skillsDictionary.Keys.ToHashSet();

                var intersectKeys = inputsKeys.Intersect(existingKeys);
                var newKeys = inputsKeys.Except(intersectKeys);
                var removedKeys = existingKeys.Except(intersectKeys);

                foreach (var key in intersectKeys)
                {
                    var core = _skillsDictionary[key];
                    var input = inputsDictionary[key];

                    core.IsRequired = input.IsRequired;
                }

                foreach (var key in removedKeys)
                {
                    _skillsDictionary[key].Remove();
                }

                foreach (var key in newKeys)
                {
                    var offferSkill = Map(inputsDictionary[key]);
                    _skillsDictionary[offferSkill.SkillId] = offferSkill;
                }
            }
        }

        private OfferSkill Map(OfferSkillInfo info)
        {
            return new OfferSkill
            {
                Id = info.Id,
                SkillId = info.SkillId,
                Created = info.Created ?? CustomTimeProvider.Now,
                IsRequired = info.IsRequired
            };
        }
    }
}
