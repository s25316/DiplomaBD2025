using Domain.Features.People.Entities;
using Domain.Features.People.ValueObjects.BirthDates;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.People.ValueObjects.PhoneNumbers;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Emails;
using Domain.Shared.ValueObjects.Ids;

namespace Domain.Features.People.Aggregates
{
    public partial class Person : TemplateEntity<PersonId>
    {
        // Properties
        public AddressId? AddressId { get; private set; } = null;
        // Main Part
        public Email Login { get; private set; } = null!;
        public string Salt { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public bool HasTwoFactorAuthentication { get; private set; } = false;
        // Additional Part
        public string? Logo { get; private set; } = null;
        public string Name { get; private set; } = null!;
        public string Surname { get; private set; } = null!;
        public string? Description { get; private set; } = null!;
        public Email ContactEmail { get; private set; } = null!;
        public PhoneNumber? ContactPhoneNumber { get; private set; } = null!;
        public bool IsStudent { get; private set; } = false;
        public bool IsAdministrator { get; private set; } = false;
        public BirthDate? BirthDate { get; private set; } = null;
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; } = null;
        public DateTime? Blocked { get; private set; } = null;
        // Collections
        private readonly Dictionary<int, PersonUrl> _urls = [];
        public IReadOnlyDictionary<int, PersonUrl> Urls => _urls;

        private readonly Dictionary<SkillId, PersonSkill> _skills = [];
        public IReadOnlyDictionary<SkillId, PersonSkill> Skills => _skills;


        // Methods

    }
}
