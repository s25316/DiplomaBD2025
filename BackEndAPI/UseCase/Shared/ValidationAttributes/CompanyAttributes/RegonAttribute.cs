// Ignore Spelling: Regon

using Domain.Features.Companies.ValueObjects.Regons;
using System.ComponentModel.DataAnnotations;
using UseCase.Shared.ValidationAttributes.BaseAttributes;

namespace UseCase.Shared.ValidationAttributes.CompanyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RegonAttribute : BaseStringAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var func = BuildIsValid(regonString => new Regon(regonString).Value);
            return func(value, validationContext);
        }
    }
}
