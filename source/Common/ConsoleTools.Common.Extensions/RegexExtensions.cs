using System.Text.RegularExpressions;

namespace ConsoleTools.Common.Extensions
{
    public static class RegexExtensions
    {
        public static Group SuccessfulOrNull(this Group regexGroup) => regexGroup?.Success == true ? regexGroup : null;
    }
}
