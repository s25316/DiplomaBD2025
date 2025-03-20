// Ignore Spelling: Krs

using Domain.Features.Companies.ValueObjects;
using System.ComponentModel.DataAnnotations;
using UseCase.Shared.Templates.ValidationAttributes;

namespace UseCase.Shared.ValidationAttributes.CompanyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class KrsAttribute : CustomStringAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var func = BuildIsValid(krsString => new Krs(krsString).Value);
            return func(value, validationContext);
        }
    }
}
