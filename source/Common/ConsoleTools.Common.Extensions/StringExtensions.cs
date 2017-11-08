using System;

namespace ConsoleTools.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool EqualsIgnoreCase(this string a, string b) => string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
    }
}
