namespace Domain.Features.ContractConditions.ValueObjects.Info
{
    public record class ContractAttributeInfo
    {
        public required Guid? Id { get; init; }
        public required int ContractParameterId { get; init; }
        public required DateTime? Created { get; init; }
        public DateTime? Removed { get; set; } = null;

        // Methods

        public static implicit operator ContractAttributeInfo?(int? number)
        {
            if (number.HasValue)
            {
                return new ContractAttributeInfo
                {
                    Id = null,
                    ContractParameterId = number.Value,
                    Created = null,
                    Removed = null,
                };
            }
            else
            {
                return null;
            }
        }

        public static implicit operator ContractAttributeInfo(
            int number)
        {
            return new ContractAttributeInfo
            {
                Id = null,
                ContractParameterId = number,
                Created = null,
                Removed = null,
            };
        }
    }
}
