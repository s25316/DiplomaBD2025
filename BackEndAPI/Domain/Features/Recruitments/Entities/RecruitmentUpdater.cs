using Domain.Features.Recruitments.Enums;
using Domain.Features.Recruitments.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;
using Domain.Shared.Templates.Builders;
using System.Text;

namespace Domain.Features.Recruitments.Entities
{
    public partial class Recruitment : TemplateEntity<RecruitmentId>
    {
        public class Updater : TemplateUpdater<Recruitment, RecruitmentId>
        {
            // Constructor
            public Updater(Recruitment value) : base(value) { }

            // Public Methods
            public Updater SetId(Guid id)
            {
                SetProperty(recruitment => recruitment.Id = id);
                return this;
            }

            public Updater SetFile(string file)
            {
                SetProperty(recruitment => recruitment.SetFile(file));
                return this;
            }

            public Updater SetProcessType(ProcessType type)
            {
                SetProperty(recruitment => recruitment.SetProcessType(type));
                return this;
            }

            protected override Action<Recruitment> SetDefaultValues()
            {
                return recrutment =>
                {
                    if (recrutment.Created == DateTime.MinValue)
                    {
                        recrutment.Created = CustomTimeProvider.Now;
                    }
                };
            }

            protected override Func<Recruitment, string> CheckIsObjectComplete()
            {
                return recrutment =>
                {
                    var stringBuilder = new StringBuilder();
                    if (recrutment.PersonId == null)
                    {
                        stringBuilder.AppendLine($"Empty:{nameof(Recruitment.PersonId)}");
                    }
                    if (recrutment.OfferId == null)
                    {
                        stringBuilder.AppendLine($"Empty:{nameof(Recruitment.OfferId)}");
                    }
                    return stringBuilder.ToString();
                };
            }
        }
    }
}
