using System.ComponentModel.DataAnnotations;

namespace UseCase.Shared.Templates.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TemplateAttribute : ValidationAttribute
    {
        protected static void SetStringValue(ValidationContext validationContext, string? value)
        {
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                if (property != null && property.CanWrite && property.PropertyType == typeof(string))
                {
                    property.SetValue(validationContext.ObjectInstance, value);
                }
            }
        }

        protected static void SetIntValue(ValidationContext validationContext, int? value)
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

        protected static void SetDecimalValue(ValidationContext validationContext, decimal? value)
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
