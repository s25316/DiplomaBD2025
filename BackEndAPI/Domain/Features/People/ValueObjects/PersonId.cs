using Domain.Shared.ValueObjects.Guids;

namespace Domain.Features.People.ValueObjects
{
    public record PersonId : GuidProperty
    {
        public PersonId(Guid original) : base(original)
        {
        }
    }
}
