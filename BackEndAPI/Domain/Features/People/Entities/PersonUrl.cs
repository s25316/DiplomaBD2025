using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.People.ValueObjects.Info;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;

namespace Domain.Features.People.Entities
{
    public class PersonUrl : TemplateEntity<PersonUrlId>
    {
        public int UrlTypeId { get; init; }

        public required DateTime Created { get; init; }

        public DateTime? Removed { get; private set; } = null;


        // Methods
        public void Remove()
        {
            if (!Removed.HasValue)
            {
                Removed = CustomTimeProvider.Now;
            }
        }

        public static implicit operator PersonUrl(PersonUrlInfo item)
        {
            return new PersonUrl
            {
                Id = item.Id,
                UrlTypeId = item.UrlTypeId,
                Created = item.Created ?? CustomTimeProvider.Now,
                Removed = item.Removed,
            };
        }
    }
}
