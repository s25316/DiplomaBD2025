using Domain.Features.ContractConditions.ValueObjects.Ids;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.Entities
{
    public class ContractAttribute : TemplateEntity<ContractAttributeId>
    {
        // Properties
        public required int ContractParameterId { get; init; }
        public required DateTime Created { get; init; }
        public DateTime? Removed { get; set; } = null;

        // Methods 
        public void Remove() => Removed = CustomTimeProvider.Now;
    }
}
