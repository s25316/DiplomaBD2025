using Domain.Features.ContractConditions.ValueObjects.Ids;
using Domain.Features.Offers.ValueObjects.Ids;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;

namespace Domain.Features.Offers.Entities
{
    public class Contract : TemplateEntity<ContractId>
    {
        // Properties
        public required ContractConditionId ContractConditionId { get; init; }
        public required DateTime Created { get; init; }
        public required DateTime? Removed { get; set; }

        // Methods
        public void Remove()
        {
            if (!Removed.HasValue)
            {
                Removed = CustomTimeProvider.Now;
            }
        }

        public static implicit operator Contract(ContractInfo item)
        {
            return new Contract
            {
                Id = item.Id,
                ContractConditionId = item.ContractConditionId,
                Created = item.Created ?? CustomTimeProvider.Now,
                Removed = item.Removed,
            };
        }
    }
}
