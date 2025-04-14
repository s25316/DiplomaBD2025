namespace Domain.Features.People.ValueObjects.Info
{
    public class PersonUrlInfo
    {
        public Guid? Id { get; init; }

        public int UrlId { get; init; }

        public DateTime? Created { get; init; }

        public DateTime? Removed { get; init; }
    }
}
