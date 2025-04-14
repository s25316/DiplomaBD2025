using System.Text.RegularExpressions;

namespace Domain.Features.People.ValueObjects.PhoneNumbers
{
    public record PhoneNumber
    {
        // Properties
        private static Regex _regex = new Regex(@"^+[0-9]{7,15}");
        public string Value { get; private set; }


        // Constructor
        public PhoneNumber(string value)
        {
            var normalizeValue = value
                .Replace("-", "")
                .Replace("\n", "")
                .Replace("\t", "")
                .Replace(" ", "");
            if (!_regex.IsMatch(normalizeValue))
            {
                throw new PhoneNumberException(
                    $"{Messages.Record_PhoneNumber_Invalid}: {value}");
            }
            Value = value;
        }

        // Methods

        public static implicit operator PhoneNumber?(string? value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? new PhoneNumber(value)
                : null;
        }

        public static implicit operator string?(PhoneNumber? value)
        {
            return value?.Value;
        }
    }
}
