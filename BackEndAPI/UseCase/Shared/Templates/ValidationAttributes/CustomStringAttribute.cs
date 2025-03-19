using System.ComponentModel.DataAnnotations;
namespace UseCase.Shared.Templates.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomStringAttribute : TemplateAttribute
    {
        /*protected static Func<object?, ValidationContext, ValidationResult?> PrepareIsValid<T>()
            where T : new()
        {
            return (value, validationContext) =>
            {
                if (IsNullOrWhiteSpace(value, validationContext, out var stringValue))
                {
                    return ValidationResult.Success;
                }

                try
                {
                    var item = new T(stringValue);
                    SetValue(validationContext, item.Value);
                }
                catch (Exception ex)
                {
                    return new ValidationResult(ex.Message);
                }
                return ValidationResult.Success;
            };
        }*/

        protected static bool IsNullOrWhiteSpace(
            object? value,
            ValidationContext validationContext,
            out string stringValue)
        {
            if (value == null)
            {
                stringValue = string.Empty;
                return true;
            }

            var param = value.ToString();
            if (string.IsNullOrWhiteSpace(param))
            {
                SetValue(validationContext, null);
                stringValue = string.Empty;
                return true;
            }

            stringValue = param;
            return false;
        }
    }
}
