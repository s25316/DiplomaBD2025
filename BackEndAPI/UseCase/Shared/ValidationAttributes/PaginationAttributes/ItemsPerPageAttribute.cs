using System.ComponentModel.DataAnnotations;
using UseCase.Shared.ValidationAttributes.BaseAttributes;

namespace UseCase.Shared.ValidationAttributes.PaginationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ItemsPerPageAttribute : BaseAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var intiger = (int?)value;
            if (intiger == null || intiger < 10)
            {
                SetIntValue(validationContext, 10);
                return ValidationResult.Success;
            }
            if (intiger > 100)
            {
                SetIntValue(validationContext, 100);
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}
