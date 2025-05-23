using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.Recruitments.ValueObjects.Ids
{
    public record RecruitmentId : GuidProperty
    {
        // Constructor
        public RecruitmentId(Guid original) : base(original)
        {
        }


        // Public Methods
        public static implicit operator RecruitmentId?(Guid? guid)
        {
            return guid.HasValue ? new RecruitmentId(guid.Value) : null;
        }

        public static implicit operator Guid?(RecruitmentId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator RecruitmentId(Guid guid)
        {
            return new RecruitmentId(guid);
        }

        public static implicit operator Guid(RecruitmentId id)
        {
            return id.Value;
        }
    }
}
