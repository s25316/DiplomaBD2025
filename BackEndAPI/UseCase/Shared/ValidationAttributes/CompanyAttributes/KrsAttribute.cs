// Ignore Spelling: Krs

using Domain.Features.Companies.ValueObjects.Krss;
using System.ComponentModel.DataAnnotations;
using UseCase.Shared.ValidationAttributes.BaseAttributes;

namespace UseCase.Shared.ValidationAttributes.CompanyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class KrsAttribute : BaseStringAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var func = BuildIsValid(krsString => new Krs(krsString).Value);
            return func(value, validationContext);
        }
    }
}
