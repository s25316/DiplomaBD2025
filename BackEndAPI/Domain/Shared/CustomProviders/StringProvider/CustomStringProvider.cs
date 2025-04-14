namespace Domain.Shared.CustomProviders.StringProvider
{
    public static class CustomStringProvider
    {
        // Public Methods

        public static IEnumerable<string> Split(
            string? text,
            WhiteSpace splitStrategy = WhiteSpace.All)
        {
            char[] separators;
            switch (splitStrategy)
            {
                case WhiteSpace.AllExceptNewLine:
                    separators = GetAllSeparatorsExceptNewLine();
                    break;
                default:
                    separators = GetAllSeparators();
                    break;
            }

            return string.IsNullOrWhiteSpace(text)
                ? []
                : text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string NormalizeWhitespace(
            string? text,
            WhiteSpace whitespaceRemovalStrategy)
        {
            var array = Split(text, whitespaceRemovalStrategy);
            if (!array.Any())
            {
                return string.Empty;
            }
            return string.Join(" ", array);
        }

        // Private Methods
        private static char[] GetAllSeparators()
        {
            char[] separators = { ' ', ',', '\n', '\t' };
            return separators;
        }

        private static char[] GetAllSeparatorsExceptNewLine()
        {
            // !char.IsWhiteSpace(c)
            char[] separators = { ' ', ',', '\t' };
            return separators;
        }
    }
}
