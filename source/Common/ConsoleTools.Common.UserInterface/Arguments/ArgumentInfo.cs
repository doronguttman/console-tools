using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleTools.Common.Extensions;

namespace ConsoleTools.Common.UserInterface.Arguments
{
    public struct ArgumentInfo
    {
        private static readonly Regex Matcher = new Regex(@"^(?<FLAG>--|\+\+|-|\+|\/)?(?<KEY>.+?)(((?<SEP>=)(?<OPEN>"")?(?<VALUE>.+?)(?<CLOSE>"")?)|(?<SEP>=))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public static readonly ArgumentInfo Empty = new ArgumentInfo();

        internal ArgumentInfo(string arg)
        {
            this.Source = arg;

            var match = arg.IsNullOrWhiteSpace() ? null : Matcher.Match(arg);
            if (match?.Success == true)
            {
                this.Flag = match.Groups["FLAG"].SuccessfulOrNull()?.Value;
                var key = match.Groups["KEY"].SuccessfulOrNull()?.Value;
                this.Key = key?.ToLowerInvariant();
                this.Value = match.Groups["VALUE"].SuccessfulOrNull()?.Value ?? key;
            }
            else
            {
                this.Flag = null;
                this.Key = arg.ToLowerInvariant();
                this.Value = arg;
            }
        }

        public string Source { get; }
        public string Key { get; }
        public string Flag { get; }
        public string Value { get; }

        public bool HasKey => !this.Key.IsNullOrWhiteSpace();
        public bool HasValue => !this.Value.IsNullOrWhiteSpace();
        public bool HasFlag => !this.Flag.IsNullOrWhiteSpace();

        public bool IsOn => this.Flag == "+" || this.Flag == "++";
        public bool IsOff => this.Flag == "-" || this.Flag == "--";

        #region Overrides of ValueType
        public override string ToString() => $"{this.Flag ?? string.Empty}{this.Key ?? "null"}:{this.Value ?? "null"}";

        #endregion Overrides of ValueType

        internal static class Factory
        {
            internal static IEnumerable<ArgumentInfo> ParseArgs(IEnumerable<string> args)
            {
                return args.Select(arg => new ArgumentInfo(arg)).Where(argInfo => argInfo.HasKey);
            }

            internal static IDictionary<string, ArgumentInfo> ParseArgsDistinct(IEnumerable<string> args)
            {
                return ParseArgs(args)
                    .Distinct(info => info.Key.GetHashCode())
                    .ToDictionary(arg => arg.Key);
            }
        }
    }
}