// Ignore Spelling: Regon

using Domain.Features.Companies.ValueObjects;
using System.ComponentModel.DataAnnotations;
using UseCase.Shared.Templates.ValidationAttributes;

namespace UseCase.Shared.ValidationAttributes
{
    public class RegonAttribute : CustomStringAttribute
    {
        // Protected Methods
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (IsNullOrWhiteSpace(value, validationContext, out var stringValue))
            {
                return ValidationResult.Success;
            }

            try
            {
                var item = new Regon(stringValue);
                SetValue(validationContext, item.Value);
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
            return ValidationResult.Success;
        }
    }
}
