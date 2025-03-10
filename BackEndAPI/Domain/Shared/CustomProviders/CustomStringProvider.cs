namespace Domain.Shared.CustomProviders
{
    public static class CustomStringProvider
    {
        public static char[] GetSeparators()
        {
            char[] separators = { ' ', ',', '\n', '\t' };
            return separators;
        }

        public static IEnumerable<string> Split(string? text)
        {
            var separators = GetSeparators();
            return string.IsNullOrWhiteSpace(text)
                ? []
                : text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
