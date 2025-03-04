using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.Branches.ValueObjects
{
    public record BranchId : GuidProperty
    {
        // Constructor
        public BranchId(Guid value) : base(value)
        {
        }


        // Public Methods
        public static implicit operator BranchId?(Guid? guid)
        {
            return guid.HasValue ? new BranchId(guid.Value) : null;
        }

        public static implicit operator Guid?(BranchId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator BranchId(Guid guid)
        {
            return new BranchId(guid);
        }

        public static implicit operator Guid(BranchId id)
        {
            return id.Value;
        }
    }
}
