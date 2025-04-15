using Domain.Shared.CustomProviders;

namespace Domain.Features.People.ValueObjects.Info
{
    public class PersonUrlInfo
    {
        // Properties
        public required Guid? Id { get; init; }

        public required int UrlTypeId { get; init; }

        public required DateTime? Created { get; init; }

        public required DateTime? Removed { get; init; }


        // Methods
        public static implicit operator PersonUrlInfo(int urlId)
        {
            return new PersonUrlInfo
            {
                Id = null,
                UrlTypeId = urlId,
                Created = CustomTimeProvider.Now,
                Removed = null,
            };
        }
    }
}
