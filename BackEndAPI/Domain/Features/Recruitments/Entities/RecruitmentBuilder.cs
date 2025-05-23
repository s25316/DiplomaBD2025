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
        public class Builder : TemplateBuilder<Recruitment, RecruitmentId>
        {
            // Public Methods
            public Builder SetId(Guid id)
            {
                SetProperty(recruitment => recruitment.Id = id);
                return this;
            }

            public Builder SetPersonId(Guid id)
            {
                SetProperty(recruitment => recruitment.PersonId = id);
                return this;
            }

            public Builder SetOfferId(Guid id)
            {
                SetProperty(recruitment => recruitment.OfferId = id);
                return this;
            }

            public Builder SetMessage(string? message)
            {
                SetProperty(recruitment => recruitment.SetMessage(message));
                return this;
            }

            public Builder SetFile(string file)
            {
                SetProperty(recruitment => recruitment.SetFile(file));
                return this;
            }

            public Builder SetCreated(DateTime created)
            {
                SetProperty(recruitment => recruitment.Created = created);
                return this;
            }

            public Builder SetProcessType(ProcessType type)
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
