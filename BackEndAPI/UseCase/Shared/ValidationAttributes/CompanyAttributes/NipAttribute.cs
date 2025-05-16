using Domain.Features.Companies.ValueObjects.Nips;
using System.ComponentModel.DataAnnotations;
using UseCase.Shared.ValidationAttributes.BaseAttributes;

namespace UseCase.Shared.ValidationAttributes.CompanyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NipAttribute : BaseStringAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var func = BuildIsValid(nipString => new Nip(nipString).Value);
            return func(value, validationContext);
        }
    }
}
