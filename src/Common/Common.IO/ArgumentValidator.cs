namespace Common.IO
{
    public static class ArgumentValidator
    {
        public static void ValidateNonEmptyStrings(params string?[] arguments)
        {
            foreach (string? argument in arguments)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(argument);
            }
        }

        public static void ValidateNotNullArguments<T>(params T?[] arguments) where T : class
        {
            foreach (var argument in arguments)
            {
                ArgumentNullException.ThrowIfNull(argument);
            }
        }
    }
}
