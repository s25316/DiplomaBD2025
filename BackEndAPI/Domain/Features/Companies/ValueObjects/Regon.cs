// Ignore Spelling: Regon

using Domain.Features.Companies.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.Features.Companies.ValueObjects
{
    public record Regon
    {
        // Properties
        private static Regex _regex = new Regex(pattern: @"^[\d]{10}$");
        public string Value { get; init; }


        // Constructor
        public Regon(string value)
        {
            var modifiedValue = value
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
            if (!_regex.IsMatch(modifiedValue))
            {
                throw new RegonException($"{Messages.Record_Regon_Invalid}: {value}");
            }
            Value = modifiedValue;
        }


        // Methods
        public static implicit operator Regon(string value)
        {
            return new Regon(value);
        }

        public static implicit operator string(Regon value)
        {
            return value.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
