using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.Companies.ValueObjects.Ids
{
    public record CompanyId : GuidProperty
    {
        // Constructor
        public CompanyId(Guid value) : base(value)
        {
        }


        public static implicit operator CompanyId?(Guid? guid)
        {
            return guid.HasValue ? new CompanyId(guid.Value) : null;
        }

        public static implicit operator Guid?(CompanyId? id)
        {
            return id != null ? id.Value : null;
        }

        public static implicit operator CompanyId(Guid guid)
        {
            return new CompanyId(guid);
        }

        public static implicit operator Guid(CompanyId id)
        {
            return id.Value;
        }

    }
}
