// Ignore Spelling: Regon Krs

using System.Text.RegularExpressions;

namespace Domain.Features.Companies.ValueObjects.Krss
{
    public record Krs
    {
        // Properties
        private static Regex _regex = new Regex(pattern: @"^[\d]{10}$");
        public string Value { get; init; }


        // Constructor
        public Krs(string value)
        {
            var modifiedValue = value
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
            if (!_regex.IsMatch(modifiedValue))
            {
                throw new KrsException($"{Messages.Record_Krs_Invalid}: {value}");
            }
            Value = modifiedValue;
        }


        // Methods
        public static implicit operator Krs?(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : new Krs(value);
        }

        public static implicit operator string?(Krs? value)
        {
            return value?.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
