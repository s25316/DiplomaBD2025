// Ignore Spelling: Enums

using Domain.Shared.Exceptions;
using System.ComponentModel;

namespace Domain.Shared.Enums
{
    public static class EnumExtensionMethods
    {
        public static string Description<TEnum>(this TEnum value) where TEnum : Enum
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return value.ToString();
            }

            DescriptionAttribute attribute = (DescriptionAttribute?)Attribute
                .GetCustomAttribute(field, typeof(DescriptionAttribute)) ??
                throw new EnumException($"In {value.GetType()} have not implemented Description to {value}");
            return attribute.Description;
        }

        public static IEnumerable<TEnum> GetList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
