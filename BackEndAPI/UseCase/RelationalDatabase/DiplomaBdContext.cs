using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase.Models;

namespace UseCase.RelationalDatabase;

public partial class DiplomaBdContext : DbContext
{
    public DiplomaBdContext()
    {
    }

    public DiplomaBdContext(DbContextOptions<DiplomaBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyPerson> CompanyPeople { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }

    public virtual DbSet<Ex> Exes { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<Hrchat> Hrchats { get; set; }

    public virtual DbSet<Hrprocess> Hrprocesses { get; set; }

    public virtual DbSet<Nchat> Nchats { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<OfferEmploymentType> OfferEmploymentTypes { get; set; }

    public virtual DbSet<OfferSkill> OfferSkills { get; set; }

    public virtual DbSet<OfferTemplate> OfferTemplates { get; set; }

    public virtual DbSet<OfferWorkMode> OfferWorkModes { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonSkill> PersonSkills { get; set; }

    public virtual DbSet<ProcessType> ProcessTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalaryTerm> SalaryTerms { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<SkillType> SkillTypes { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<UrlType> UrlTypes { get; set; }

    public virtual DbSet<WorkMode> WorkModes { get; set; }
}
