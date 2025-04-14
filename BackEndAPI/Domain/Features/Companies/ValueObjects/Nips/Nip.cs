using System.Text.RegularExpressions;

namespace Domain.Features.Companies.ValueObjects.Nips
{
    public record Nip
    {
        // Properties
        private static Regex _regex = new Regex(@"^([\d]{9}|[\d]{14})$");
        public string Value { get; init; }


        // Constructor
        public Nip(string value)
        {
            var modifiedValue = value
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
            if (!_regex.IsMatch(modifiedValue))
            {
                throw new NipException($"{Messages.Record_Nip_Invalid}: {value}");
            }
            Value = modifiedValue;
        }


        // Methods
        public static implicit operator Nip(string value)
        {
            return new Nip(value);
        }

        public static implicit operator string(Nip value)
        {
            return value.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
