namespace Domain.Shared.CustomProviders.StringProvider
{
    public static class CustomStringProvider
    {
        // Public Methods
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

        public static string NormalizeWhitespace(
            string? text,
            WhiteSpace whitespaceRemovalStrategy)
        {
            switch (whitespaceRemovalStrategy)
            {
                case WhiteSpace.All:
                    return NormalizeWhitespaceRemoveAll(text);
                case WhiteSpace.AllExceptNewLine:
                    return NormalizeWhitespaceKeepNewLines(text);
                default:
                    throw new NotImplementedException();
            }
        }

        // Private Methods
        private static string NormalizeWhitespaceRemoveAll(string? text)
        {
            var array = Split(text);
            if (!array.Any())
            {
                return string.Empty;
            }
            return string.Join(" ", array);
        }

        private static string NormalizeWhitespaceKeepNewLines(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in text)
            {
                if (c == '\n' || !char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
