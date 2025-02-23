using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;

namespace Infrastructure.RelationalDatabase
{
    class MsSqlDiplomaBdContext : DiplomaBdContext
    {
        // Properties
        private readonly IConfiguration _configuration;

        // Constructor
        public MsSqlDiplomaBdContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning Implement Ex
            var connectionString = _configuration
                .GetSection("ConnectionStrings")["RelationalDatabase"] ??
                throw new Exception();
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("Address_pk");

                entity.ToTable("Address");

                entity.Property(e => e.AddressId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ApartmentNumber).HasMaxLength(25);
                entity.Property(e => e.HouseNumber).HasMaxLength(25);
                entity.Property(e => e.PostCode).HasMaxLength(25);

                entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Address_City");

                entity.HasOne(d => d.Street).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.StreetId)
                    .HasConstraintName("Address_Street");
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.BranchId).HasName("Branch_pk");

                entity.ToTable("Branch");

                entity.Property(e => e.BranchId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Address).WithMany(p => p.Branches)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Branch_Address");

                entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Branch_Company");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId).HasName("City_pk");

                entity.ToTable("City");

                entity.Property(e => e.CityId).HasDefaultValueSql("(NEXT VALUE FOR [CityId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.State).WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("City_State");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId).HasName("Company_pk");

                entity.ToTable("Company");

                entity.Property(e => e.CompanyId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Blocked).HasColumnType("datetime");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Krs).HasMaxLength(25);
                entity.Property(e => e.Logo).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Nip).HasMaxLength(25);
                entity.Property(e => e.Regon).HasMaxLength(25);
                entity.Property(e => e.Removed).HasColumnType("datetime");
                entity.Property(e => e.WebsiteUrl).HasMaxLength(800);
            });

            modelBuilder.Entity<CompanyPerson>(entity =>
            {
                entity.HasKey(e => e.CompanyPersonId).HasName("CompanyPerson_pk");

                entity.ToTable("CompanyPerson");

                entity.Property(e => e.CompanyPersonId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Deny).HasColumnType("datetime");
                entity.Property(e => e.Grant)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Company).WithMany(p => p.CompanyPeople)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CompanyPerson_Company");

                entity.HasOne(d => d.Person).WithMany(p => p.CompanyPeople)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CompanyPerson_Person");

                entity.HasOne(d => d.Role).WithMany(p => p.CompanyPeople)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CompanyPerson_Role");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId).HasName("Country_pk");

                entity.ToTable("Country");

                entity.Property(e => e.CountryId).HasDefaultValueSql("(NEXT VALUE FOR [CountryId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.CurrencyId).HasName("Currency_pk");

                entity.ToTable("Currency");

                entity.Property(e => e.CurrencyId).HasDefaultValueSql("(NEXT VALUE FOR [CurrencyId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<EmploymentType>(entity =>
            {
                entity.HasKey(e => e.EmploymentTypeId).HasName("EmploymentType_pk");

                entity.ToTable("EmploymentType");

                entity.Property(e => e.EmploymentTypeId).HasDefaultValueSql("(NEXT VALUE FOR [EmploymentTypeId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Ex>(entity =>
            {
                entity.HasKey(e => e.ExceptionId).HasName("Ex_pk");

                entity.ToTable("Ex");

                entity.Property(e => e.ExceptionId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ExceptionType).HasMaxLength(100);
                entity.Property(e => e.Method).HasMaxLength(100);
                entity.Property(e => e.StackTrace).HasMaxLength(800);
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.HasKey(e => e.FaqId).HasName("Faq_pk");

                entity.ToTable("Faq");

                entity.Property(e => e.FaqId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Answer).HasMaxLength(800);
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Question).HasMaxLength(800);
                entity.Property(e => e.Removed).HasColumnType("datetime");
            });

            modelBuilder.Entity<Hrchat>(entity =>
            {
                entity.HasKey(e => e.MessageId).HasName("HRChat_pk");

                entity.ToTable("HRChat");

                entity.Property(e => e.MessageId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Message).HasMaxLength(800);
                entity.Property(e => e.MongoUrl).HasMaxLength(100);
                entity.Property(e => e.Read).HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Process).WithMany(p => p.Hrchats)
                    .HasForeignKey(d => d.ProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HRChat_HRProcess");

                entity.HasOne(d => d.ProcessType).WithMany(p => p.Hrchats)
                    .HasForeignKey(d => d.ProcessTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HRChat_ProcessType");
            });

            modelBuilder.Entity<Hrprocess>(entity =>
            {
                entity.HasKey(e => e.ProcessId).HasName("HRProcess_pk");

                entity.ToTable("HRProcess");

                entity.Property(e => e.ProcessId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Offer).WithMany(p => p.Hrprocesses)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HRProcess_Offer");

                entity.HasOne(d => d.Person).WithMany(p => p.Hrprocesses)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HRProcess_Person");
            });

            modelBuilder.Entity<Nchat>(entity =>
            {
                entity.HasKey(e => e.MessageId).HasName("NChat_pk");

                entity.ToTable("NChat");

                entity.Property(e => e.MessageId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Message).HasMaxLength(800);
                entity.Property(e => e.MongoUrl).HasMaxLength(100);
                entity.Property(e => e.Read).HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Notification).WithMany(p => p.Nchats)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NChat_Notification");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("Notification_pk");

                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Completed).HasColumnType("datetime");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Message).HasMaxLength(800);
                entity.Property(e => e.Read).HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.NotificationType).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Notification_NotificationType");

                entity.HasOne(d => d.Person).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("Notification_Person");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.HasKey(e => e.NotificationTypeId).HasName("NotificationType_pk");

                entity.ToTable("NotificationType");

                entity.Property(e => e.NotificationTypeId).HasDefaultValueSql("(NEXT VALUE FOR [FaqId_SEQUENCE])");
                entity.Property(e => e.Description).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasKey(e => e.OfferId).HasName("Offer_pk");

                entity.ToTable("Offer");

                entity.Property(e => e.OfferId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.IsNegotiated).HasDefaultValue(true);
                entity.Property(e => e.PublicationEnd).HasColumnType("datetime");
                entity.Property(e => e.PublicationStart)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.SalaryRangeMax).HasColumnType("money");
                entity.Property(e => e.SalaryRangeMin).HasColumnType("money");
                entity.Property(e => e.WebsiteUrl).HasMaxLength(800);

                entity.HasOne(d => d.Branch).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("Offer_Branch");

                entity.HasOne(d => d.Company).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Offer_Company");

                entity.HasOne(d => d.Currency).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("Offer_Currency");

                entity.HasOne(d => d.OfferTemplate).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.OfferTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Offer_OfferTemplate");

                entity.HasOne(d => d.SalaryTerm).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.SalaryTermId)
                    .HasConstraintName("Offer_SalaryTerm");
            });

            modelBuilder.Entity<OfferEmploymentType>(entity =>
            {
                entity.HasKey(e => e.OfferEmploymentTypeId).HasName("OfferEmploymentType_pk");

                entity.ToTable("OfferEmploymentType");

                entity.Property(e => e.OfferEmploymentTypeId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.EmploymentType).WithMany(p => p.OfferEmploymentTypes)
                    .HasForeignKey(d => d.EmploymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferEmploymentType_EmploymentType");

                entity.HasOne(d => d.Offer).WithMany(p => p.OfferEmploymentTypes)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferEmploymentType_Offer");
            });

            modelBuilder.Entity<OfferSkill>(entity =>
            {
                entity.HasKey(e => e.OfferSkillId).HasName("OfferSkill_pk");

                entity.ToTable("OfferSkill");

                entity.Property(e => e.OfferSkillId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.OfferTemplate).WithMany(p => p.OfferSkills)
                    .HasForeignKey(d => d.OfferTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferSkill_OfferTemplate");

                entity.HasOne(d => d.Skill).WithMany(p => p.OfferSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferSkill_Skill");
            });

            modelBuilder.Entity<OfferTemplate>(entity =>
            {
                entity.HasKey(e => e.OfferTemplateId).HasName("OfferTemplate_pk");

                entity.ToTable("OfferTemplate");

                entity.Property(e => e.OfferTemplateId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Removed).HasColumnType("datetime");
            });

            modelBuilder.Entity<OfferWorkMode>(entity =>
            {
                entity.HasKey(e => e.OfferWorkModeId).HasName("OfferWorkMode_pk");

                entity.ToTable("OfferWorkMode");

                entity.Property(e => e.OfferWorkModeId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Offer).WithMany(p => p.OfferWorkModes)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferWorkMode_Offer");

                entity.HasOne(d => d.WorkMode).WithMany(p => p.OfferWorkModes)
                    .HasForeignKey(d => d.WorkModeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferWorkMode_WorkMode");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.PersonId).HasName("Person_pk");

                entity.ToTable("Person");

                entity.Property(e => e.PersonId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Blocked).HasColumnType("datetime");
                entity.Property(e => e.ContactEmail).HasMaxLength(100);
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Login).HasMaxLength(100);
                entity.Property(e => e.Logo).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.PhoneNum).HasMaxLength(100);
                entity.Property(e => e.Removed).HasColumnType("datetime");
                entity.Property(e => e.Surname).HasMaxLength(100);

                entity.HasOne(d => d.Address).WithMany(p => p.People)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("Person_Address");
            });

            modelBuilder.Entity<PersonSkill>(entity =>
            {
                entity.HasKey(e => e.PersonSkillId).HasName("PersonSkill_pk");

                entity.ToTable("PersonSkill");

                entity.Property(e => e.PersonSkillId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Person).WithMany(p => p.PersonSkills)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PersonSkill_Person");

                entity.HasOne(d => d.Skill).WithMany(p => p.PersonSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PersonSkill_Skill");
            });

            modelBuilder.Entity<ProcessType>(entity =>
            {
                entity.HasKey(e => e.ProcessTypeId).HasName("ProcessType_pk");

                entity.ToTable("ProcessType");

                entity.Property(e => e.ProcessTypeId).HasDefaultValueSql("(NEXT VALUE FOR [ProcessTypeId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("Role_pk");

                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasDefaultValueSql("(NEXT VALUE FOR [RoleId_SEQUENCE])");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<SalaryTerm>(entity =>
            {
                entity.HasKey(e => e.SalaryTermId).HasName("SalaryTerm_pk");

                entity.ToTable("SalaryTerm");

                entity.Property(e => e.SalaryTermId).HasDefaultValueSql("(NEXT VALUE FOR [SalaryTermId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.SkillId).HasName("Skill_pk");

                entity.ToTable("Skill");

                entity.Property(e => e.SkillId).HasDefaultValueSql("(NEXT VALUE FOR [SkillId_SEQUENCE])");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.SkillType).WithMany(p => p.Skills)
                    .HasForeignKey(d => d.SkillTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Skill_SkillType");

                entity.HasMany(d => d.Childlds).WithMany(p => p.Parentlds)
                    .UsingEntity<Dictionary<string, object>>(
                        "SkillConnection",
                        r => r.HasOne<Skill>().WithMany()
                            .HasForeignKey("Childld")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skill2"),
                        l => l.HasOne<Skill>().WithMany()
                            .HasForeignKey("Parentld")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skill1"),
                        j =>
                        {
                            j.HasKey("Parentld", "Childld").HasName("SkillConnections_pk");
                            j.ToTable("SkillConnections");
                        });

                entity.HasMany(d => d.Parentlds).WithMany(p => p.Childlds)
                    .UsingEntity<Dictionary<string, object>>(
                        "SkillConnection",
                        r => r.HasOne<Skill>().WithMany()
                            .HasForeignKey("Parentld")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skill1"),
                        l => l.HasOne<Skill>().WithMany()
                            .HasForeignKey("Childld")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skill2"),
                        j =>
                        {
                            j.HasKey("Parentld", "Childld").HasName("SkillConnections_pk");
                            j.ToTable("SkillConnections");
                        });
            });

            modelBuilder.Entity<SkillType>(entity =>
            {
                entity.HasKey(e => e.SkillTypeId).HasName("SkillType_pk");

                entity.ToTable("SkillType");

                entity.Property(e => e.SkillTypeId).HasDefaultValueSql("(NEXT VALUE FOR [SkillTypeId_SEQUENCE])");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.StateId).HasName("State_pk");

                entity.ToTable("State");

                entity.Property(e => e.StateId).HasDefaultValueSql("(NEXT VALUE FOR [StateId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Country).WithMany(p => p.States)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("State_Country");
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.HasKey(e => e.StreetId).HasName("Street_pk");

                entity.ToTable("Street");

                entity.Property(e => e.StreetId).HasDefaultValueSql("(NEXT VALUE FOR [StreetId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Url>(entity =>
            {
                entity.HasKey(e => e.UrlId).HasName("Url_pk");

                entity.ToTable("Url");

                entity.Property(e => e.UrlId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Removed).HasColumnType("datetime");
                entity.Property(e => e.Value).HasMaxLength(800);

                entity.HasOne(d => d.Person).WithMany(p => p.Urls)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Url_Person");

                entity.HasOne(d => d.UrlType).WithMany(p => p.Urls)
                    .HasForeignKey(d => d.UrlTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Url_UrlType");
            });

            modelBuilder.Entity<UrlType>(entity =>
            {
                entity.HasKey(e => e.UrlTypeId).HasName("UrlType_pk");

                entity.ToTable("UrlType");

                entity.Property(e => e.UrlTypeId).HasDefaultValueSql("(NEXT VALUE FOR [UrlTypeId_SEQUENCE])");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<WorkMode>(entity =>
            {
                entity.HasKey(e => e.WorkModeId).HasName("WorkMode_pk");

                entity.ToTable("WorkMode");

                entity.Property(e => e.WorkModeId).HasDefaultValueSql("(NEXT VALUE FOR [WorkModeId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });
            modelBuilder.HasSequence("CityId_SEQUENCE");
            modelBuilder.HasSequence("CountryId_SEQUENCE");
            modelBuilder.HasSequence("CurrencyId_SEQUENCE");
            modelBuilder.HasSequence("EmploymentTypeId_SEQUENCE");
            modelBuilder.HasSequence("FaqId_SEQUENCE");
            modelBuilder.HasSequence("ProcessTypeId_SEQUENCE");
            modelBuilder.HasSequence("RoleId_SEQUENCE");
            modelBuilder.HasSequence("SalaryTermId_SEQUENCE");
            modelBuilder.HasSequence("SkillId_SEQUENCE");
            modelBuilder.HasSequence("SkillTypeId_SEQUENCE");
            modelBuilder.HasSequence("StateId_SEQUENCE");
            modelBuilder.HasSequence("StreetId_SEQUENCE");
            modelBuilder.HasSequence("UrlTypeId_SEQUENCE");
            modelBuilder.HasSequence("WorkModeId_SEQUENCE");
        }

    }
}
