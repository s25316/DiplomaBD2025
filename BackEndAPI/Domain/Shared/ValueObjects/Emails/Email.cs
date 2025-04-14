using System.Text.RegularExpressions;

namespace Domain.Shared.ValueObjects.Emails
{
    public record Email
    {
        // Properties
        private static Regex _regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        public string Value { get; private set; }


        // Constructor
        public Email(string value)
        {
            if (!_regex.IsMatch(value))
            {
                throw new EmailException($"{Messages.Record_Email_Invalid}: {value}");
            }
            Value = value;
        }


        // Methods
        public static implicit operator Email?(string? value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? new Email(value)
                : null;
        }

        public static implicit operator string?(Email? value)
        {
            return value?.Value;
        }
    }
}
