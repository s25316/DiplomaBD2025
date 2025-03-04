namespace Domain.Shared.ValueObjects.Urls
{
    public record UrlProperty
    {
        // Properties
        public string Value { get; init; }


        // Constructor
        public UrlProperty(string value)
        {
            try
            {
                Value = new Uri(value.Trim()).ToString();
            }
            catch
            {
                throw new UrlPropertyException($"{Messages.Record_Url_Invalid}: {value}");
            }
        }


        // Methods
        public static implicit operator UrlProperty?(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : new UrlProperty(value);
        }

        public static implicit operator string?(UrlProperty? value)
        {
            return value?.Value;
        }
    }
}
