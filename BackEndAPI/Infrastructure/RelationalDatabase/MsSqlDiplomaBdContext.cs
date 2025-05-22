// Ignore Spelling: Sql, Bd, newid, getdate, datetime, Faq, Parentld, Childld
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;

namespace Infrastructure.RelationalDatabase
{
    class MsSqlDiplomaBdContext : DiplomaBdContext
    {
        // Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                UseCase.Configuration.RelationalDatabaseConnectionString,
                x => x.UseNetTopologySuite());
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("Addresses_pk");

                entity.Property(e => e.AddressId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ApartmentNumber).HasMaxLength(25);
                entity.Property(e => e.HouseNumber).HasMaxLength(25);
                entity.Property(e => e.PostCode).HasMaxLength(25);

                entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Addresses_Cities");

                entity.HasOne(d => d.Street).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.StreetId)
                    .HasConstraintName("Addresses_Streets");
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.BranchId).HasName("Branches_pk");

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
                    .HasConstraintName("Branches_Addresses");

                entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Branches_Companies");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId).HasName("Cities_pk");

                entity.Property(e => e.CityId).HasDefaultValueSql("(NEXT VALUE FOR [CityId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.State).WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cities_States");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId).HasName("Companies_pk");

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
                entity.HasKey(e => e.CompanyPersonId).HasName("CompanyPeople_pk");

                entity.Property(e => e.CompanyPersonId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Deny).HasColumnType("datetime");
                entity.Property(e => e.Grant)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Company).WithMany(p => p.CompanyPeople)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CompanyPeople_Companies");

                entity.HasOne(d => d.Person).WithMany(p => p.CompanyPeople)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CompanyPeople_People");

                entity.HasOne(d => d.Role).WithMany(p => p.CompanyPeople)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CompanyPeople_Roles");
            });

            modelBuilder.Entity<ContractAttribute>(entity =>
            {
                entity.HasKey(e => e.ContractAttributeId).HasName("ContractAttributes_pk");

                entity.Property(e => e.ContractAttributeId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.ContractCondition).WithMany(p => p.ContractAttributes)
                    .HasForeignKey(d => d.ContractConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractAttributes_ContractConditions");

                entity.HasOne(d => d.ContractParameter).WithMany(p => p.ContractAttributes)
                    .HasForeignKey(d => d.ContractParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractAttributes_ContractParameters");
            });

            modelBuilder.Entity<ContractCondition>(entity =>
            {
                entity.HasKey(e => e.ContractConditionId).HasName("ContractConditions_pk");

                entity.Property(e => e.ContractConditionId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.HoursPerTerm).HasDefaultValue(1);
                entity.Property(e => e.IsNegotiable).HasDefaultValue(true);
                entity.Property(e => e.Removed).HasColumnType("datetime");
                entity.Property(e => e.SalaryMax).HasColumnType("money");
                entity.Property(e => e.SalaryMin).HasColumnType("money");

                entity.HasOne(d => d.Company).WithMany(p => p.ContractConditions)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractConditions_Companies");
            });

            modelBuilder.Entity<ContractParameter>(entity =>
            {
                entity.HasKey(e => e.ContractParameterId).HasName("ContractParameters_pk");

                entity.Property(e => e.ContractParameterId).HasDefaultValueSql("(NEXT VALUE FOR [ContractParameterId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.ContractParameterType).WithMany(p => p.ContractParameters)
                    .HasForeignKey(d => d.ContractParameterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractParameters_ContractParameterTypes");
            });

            modelBuilder.Entity<ContractParameterType>(entity =>
            {
                entity.HasKey(e => e.ContractParameterTypeId).HasName("ContractParameterTypes_pk");

                entity.Property(e => e.ContractParameterTypeId).HasDefaultValueSql("(NEXT VALUE FOR [ContractParameterTypeId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId).HasName("Countries_pk");

                entity.Property(e => e.CountryId).HasDefaultValueSql("(NEXT VALUE FOR [CountryId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Ex>(entity =>
            {
                entity.HasKey(e => e.ExceptionId).HasName("Exs_pk");

                entity.Property(e => e.ExceptionId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ExceptionType).HasMaxLength(100);
                entity.Property(e => e.Handled).HasColumnType("datetime");
                entity.Property(e => e.Method).HasMaxLength(100);
                entity.Property(e => e.StackTrace).HasMaxLength(800);
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.HasKey(e => e.FaqId).HasName("Faqs_pk");

                entity.Property(e => e.FaqId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Answer).HasMaxLength(800);
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Question).HasMaxLength(800);
                entity.Property(e => e.Removed).HasColumnType("datetime");
            });

            modelBuilder.Entity<HrProcess>(entity =>
            {
                entity.HasKey(e => e.ProcessId).HasName("HrProcess_pk");

                entity.ToTable("HrProcess");

                entity.Property(e => e.ProcessId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created).HasColumnType("datetime");
                entity.Property(e => e.File)
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.HasOne(d => d.Offer).WithMany(p => p.HrProcesses)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HrProcess_Offers");

                entity.HasOne(d => d.Person).WithMany(p => p.HrProcesses)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HrProcess_People");

                entity.HasOne(d => d.ProcessType).WithMany(p => p.HrProcesses)
                    .HasForeignKey(d => d.ProcessTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HrProcess_ProcessType");
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
                entity.Property(e => e.Read).HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Process).WithMany(p => p.Hrchats)
                    .HasForeignKey(d => d.ProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HRChat_HRProcess");
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
                entity.Property(e => e.Read).HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Notification).WithMany(p => p.Nchats)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NChat_Notifications");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId).HasName("Notifications_pk");

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
                    .HasConstraintName("Notifications_NotificationTypes");

                entity.HasOne(d => d.Person).WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("Notifications_People");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.HasKey(e => e.NotificationTypeId).HasName("NotificationTypes_pk");

                entity.Property(e => e.NotificationTypeId).HasDefaultValueSql("(NEXT VALUE FOR [NotificationTypeId_SEQUENCE])");
                entity.Property(e => e.Description).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasKey(e => e.OfferId).HasName("Offers_pk");

                entity.Property(e => e.OfferId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.PublicationEnd).HasColumnType("datetime");
                entity.Property(e => e.PublicationStart).HasColumnType("datetime");
                entity.Property(e => e.WebsiteUrl).HasMaxLength(800);

                entity.HasOne(d => d.Branch).WithMany(p => p.Offers)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("Offers_Branches");
            });

            modelBuilder.Entity<OfferCondition>(entity =>
            {
                entity.HasKey(e => e.OfferConditionId).HasName("OfferConditions_pk");

                entity.Property(e => e.OfferConditionId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.ContractCondition).WithMany(p => p.OfferConditions)
                    .HasForeignKey(d => d.ContractConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferConditions_ContractConditions");

                entity.HasOne(d => d.Offer).WithMany(p => p.OfferConditions)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferConditions_Offers");
            });

            modelBuilder.Entity<OfferConnection>(entity =>
            {
                entity.HasKey(e => e.OfferConnectionId).HasName("OfferConnections_pk");

                entity.Property(e => e.OfferConnectionId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Offer).WithMany(p => p.OfferConnections)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferConnections_Offers");

                entity.HasOne(d => d.OfferTemplate).WithMany(p => p.OfferConnections)
                    .HasForeignKey(d => d.OfferTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferConnections_OfferTemplates");
            });

            modelBuilder.Entity<OfferSkill>(entity =>
            {
                entity.HasKey(e => e.OfferSkillId).HasName("OfferSkills_pk");

                entity.Property(e => e.OfferSkillId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.OfferTemplate).WithMany(p => p.OfferSkills)
                    .HasForeignKey(d => d.OfferTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferSkills_OfferTemplates");

                entity.HasOne(d => d.Skill).WithMany(p => p.OfferSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferSkills_Skills");
            });

            modelBuilder.Entity<OfferTemplate>(entity =>
            {
                entity.HasKey(e => e.OfferTemplateId).HasName("OfferTemplates_pk");

                entity.Property(e => e.OfferTemplateId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Company).WithMany(p => p.OfferTemplates)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OfferTemplates_Companies");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.PersonId).HasName("People_pk");

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
                    .HasConstraintName("People_Addresses");
            });

            modelBuilder.Entity<PersonSkill>(entity =>
            {
                entity.HasKey(e => e.PersonSkillId).HasName("PersonSkills_pk");

                entity.Property(e => e.PersonSkillId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.HasOne(d => d.Person).WithMany(p => p.PersonSkills)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PersonSkills_People");

                entity.HasOne(d => d.Skill).WithMany(p => p.PersonSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PersonSkills_Skills");
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
                entity.HasKey(e => e.RoleId).HasName("Roles_pk");

                entity.Property(e => e.RoleId).HasDefaultValueSql("(NEXT VALUE FOR [RoleId_SEQUENCE])");
                entity.Property(e => e.Description).HasMaxLength(800);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.SkillId).HasName("Skills_pk");

                entity.Property(e => e.SkillId).HasDefaultValueSql("(NEXT VALUE FOR [SkillId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.SkillType).WithMany(p => p.Skills)
                    .HasForeignKey(d => d.SkillTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Skills_SkillTypes");

                entity.HasMany(d => d.ChildSkills).WithMany(p => p.ParentSkills)
                    .UsingEntity<Dictionary<string, object>>(
                        "SkillConnection",
                        r => r.HasOne<Skill>().WithMany()
                            .HasForeignKey("ChildSkillId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skills1"),
                        l => l.HasOne<Skill>().WithMany()
                            .HasForeignKey("ParentSkillId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skills2"),
                        j =>
                        {
                            j.HasKey("ParentSkillId", "ChildSkillId").HasName("SkillConnections_pk");
                            j.ToTable("SkillConnections");
                        });

                entity.HasMany(d => d.ParentSkills).WithMany(p => p.ChildSkills)
                    .UsingEntity<Dictionary<string, object>>(
                        "SkillConnection",
                        r => r.HasOne<Skill>().WithMany()
                            .HasForeignKey("ParentSkillId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skills2"),
                        l => l.HasOne<Skill>().WithMany()
                            .HasForeignKey("ChildSkillId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("SkillConnections_Skills1"),
                        j =>
                        {
                            j.HasKey("ParentSkillId", "ChildSkillId").HasName("SkillConnections_pk");
                            j.ToTable("SkillConnections");
                        });
            });

            modelBuilder.Entity<SkillType>(entity =>
            {
                entity.HasKey(e => e.SkillTypeId).HasName("SkillTypes_pk");

                entity.Property(e => e.SkillTypeId).HasDefaultValueSql("(NEXT VALUE FOR [SkillTypeId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.StateId).HasName("States_pk");

                entity.Property(e => e.StateId).HasDefaultValueSql("(NEXT VALUE FOR [StateId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Country).WithMany(p => p.States)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("States_Countries");
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.HasKey(e => e.StreetId).HasName("Streets_pk");

                entity.Property(e => e.StreetId).HasDefaultValueSql("(NEXT VALUE FOR [StreetId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Url>(entity =>
            {
                entity.HasKey(e => e.UrlId).HasName("Urls_pk");

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
                    .HasConstraintName("Urls_People");

                entity.HasOne(d => d.UrlType).WithMany(p => p.Urls)
                    .HasForeignKey(d => d.UrlTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Urls_UrlTypes");
            });

            modelBuilder.Entity<UrlType>(entity =>
            {
                entity.HasKey(e => e.UrlTypeId).HasName("UrlTypes_pk");

                entity.Property(e => e.UrlTypeId).HasDefaultValueSql("(NEXT VALUE FOR [UrlTypeId_SEQUENCE])");
                entity.Property(e => e.Name).HasMaxLength(100);
            });
            modelBuilder.HasSequence("CityId_SEQUENCE");
            modelBuilder.HasSequence("ContractParameterId_SEQUENCE");
            modelBuilder.HasSequence("ContractParameterTypeId_SEQUENCE");
            modelBuilder.HasSequence("CountryId_SEQUENCE");
            modelBuilder.HasSequence("NotificationTypeId_SEQUENCE");
            modelBuilder.HasSequence("ProcessTypeId_SEQUENCE");
            modelBuilder.HasSequence("RoleId_SEQUENCE");
            modelBuilder.HasSequence("SkillId_SEQUENCE");
            modelBuilder.HasSequence("SkillTypeId_SEQUENCE");
            modelBuilder.HasSequence("StateId_SEQUENCE");
            modelBuilder.HasSequence("StreetId_SEQUENCE");
            modelBuilder.HasSequence("UrlTypeId_SEQUENCE");
        }
    }
}
