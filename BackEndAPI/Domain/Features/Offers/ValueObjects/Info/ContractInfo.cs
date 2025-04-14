namespace Domain.Features.Offers.ValueObjects.Info
{
    public class ContractInfo
    {
        public required Guid? Id { get; init; }
        public required Guid ContractConditionId { get; init; }
        public required DateTime? Created { get; init; }
        public required DateTime? Removed { get; set; }

        // Methods
        public static implicit operator ContractInfo(Guid offerTemplateId)
        {
            return new ContractInfo
            {
                Id = null,
                ContractConditionId = offerTemplateId,
                Created = null,
                Removed = null,
            };
        }
    }
}
