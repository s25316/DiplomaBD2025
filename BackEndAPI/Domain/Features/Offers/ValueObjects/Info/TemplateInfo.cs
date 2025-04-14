namespace Domain.Features.Offers.ValueObjects.Info
{
    public class TemplateInfo
    {
        public required Guid? Id { get; init; }
        public required Guid OfferTemplateId { get; init; }
        public required DateTime? Created { get; init; }
        public required DateTime? Removed { get; set; }

        // Methods
        public static implicit operator TemplateInfo(Guid offerTemplateId)
        {
            return new TemplateInfo
            {
                Id = null,
                OfferTemplateId = offerTemplateId,
                Created = null,
                Removed = null,
            };
        }
    }
}
