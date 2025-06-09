// Ignore Spelling: Regon Krs

using Domain.Features.Companies.DomainEvents;
using Domain.Features.Companies.Exceptions;
using Domain.Features.Companies.ValueObjects.Ids;
using Domain.Features.Companies.ValueObjects.Krss;
using Domain.Features.Companies.ValueObjects.Nips;
using Domain.Features.Companies.ValueObjects.Regons;
using Domain.Shared.CustomProviders;
using Domain.Shared.CustomProviders.StringProvider;
using Domain.Shared.Templates;
using Domain.Shared.ValueObjects.Urls;

namespace Domain.Features.Companies.Entities
{
    public partial class Company : TemplateEntity<CompanyId>
    {
        // Properties
        public string? Logo { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; } = null!;
        public Regon Regon { get; private set; } = null!;
        public Nip Nip { get; private set; } = null!;
        public Krs? Krs { get; private set; } = null;
        public UrlProperty? WebsiteUrl { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; }
        public DateTime? Blocked { get; private set; }

        // Computed Properties
        public bool IsBlocked => Blocked.HasValue;


        // Public Methods
        public void Block(string message)
        {
            if (!IsBlocked)
            {
                Blocked = CustomTimeProvider.Now;
                AddDomainEvent(CompanyBlockedEvent.Prepare(this, message));
            }
        }

        public void UnBlock()
        {
            if (IsBlocked)
            {
                Blocked = null;
                AddDomainEvent(CompanyUnBlockedEvent.Prepare(this));
            }
        }

        // Private Methods
        private void SetName(string? name)
        {
            name = CustomStringProvider.NormalizeWhitespace(
                name,
                WhiteSpace.All);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CompanyException(Messages.Entity_Company_EmptyName);
            }
            Name = name;
        }

        private void SetDescription(string? description)
        {
            description = CustomStringProvider.NormalizeWhitespace(
                description,
                WhiteSpace.AllExceptNewLine);

            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description;
        }

        private void SetLogo(string? logo)
        {
            Logo = string.IsNullOrWhiteSpace(logo) ? null : logo.Trim();
        }

        private void SetWebsiteUrl(string? websiteUrl)
        {
            WebsiteUrl = websiteUrl;
        }

        private void SetRegon(string? regon)
        {
            if (string.IsNullOrWhiteSpace(regon))
            {
                throw new CompanyException(Messages.Entity_Company_EmptyRegon);
            }
            Regon = regon;
        }

        private void SetNip(string? nip)
        {
            if (string.IsNullOrWhiteSpace(nip))
            {
                throw new CompanyException(Messages.Entity_Company_EmptyNip);
            }
            Nip = nip;
        }

        private void SetKrs(string? krs)
        {
            if (Krs != null)
            {
                throw new CompanyException(Messages.Entity_Company_ChangeExistingKrs);
            }
            Krs = krs;
        }
    }
}
