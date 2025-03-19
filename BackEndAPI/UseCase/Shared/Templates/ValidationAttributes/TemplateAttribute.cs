using System.ComponentModel.DataAnnotations;

namespace UseCase.Shared.Templates.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TemplateAttribute : ValidationAttribute
    {
        protected static void SetValue(ValidationContext validationContext, string? value)
        {
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(validationContext.ObjectInstance, value);
                }
            }
        }
    }
}
