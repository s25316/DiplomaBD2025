using System.ComponentModel.DataAnnotations;
using UseCase.Shared.Templates.ValidationAttributes;

namespace UseCase.Shared.ValidationAttributes.ContractConditionAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class HourAttribute : TemplateAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var intiger = (int?)value;
            if (intiger == null)
            {
                return ValidationResult.Success;
            }

            if (intiger < 1)
            {
                SetIntValue(validationContext, 1);
            }

            return ValidationResult.Success;
        }
    }
}
