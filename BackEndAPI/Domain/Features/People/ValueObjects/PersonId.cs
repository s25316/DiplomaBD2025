using Domain.Shared.Templates.Identificators;

namespace Domain.Features.People.ValueObjects
{
    public record PersonId : GuidId
    {
        public PersonId(Guid original) : base(original)
        {
        }
    }
}
