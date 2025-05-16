using System.ComponentModel.DataAnnotations;
using UseCase.Shared.ValidationAttributes.BaseAttributes;

namespace UseCase.Shared.ValidationAttributes.PaginationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PageAttribute : BaseAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var intiger = (int?)value;
            if (intiger == null || intiger < 1)
            {
                SetIntValue(validationContext, 1);
            }
            return ValidationResult.Success;
        }
    }
}
