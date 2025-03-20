using Domain.Features.Companies.ValueObjects;
using System.ComponentModel.DataAnnotations;
using UseCase.Shared.Templates.ValidationAttributes;

namespace UseCase.Shared.ValidationAttributes.CompanyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NipAttribute : CustomStringAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var func = BuildIsValid(nipString => new Nip(nipString).Value);
            return func(value, validationContext);
        }
    }
}
