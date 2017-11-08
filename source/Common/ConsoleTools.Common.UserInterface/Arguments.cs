using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleTools.Common.Extensions;

namespace ConsoleTools.Common.UserInterface
{
    public class Arguments
    {
        public static readonly Arguments Empty = new Arguments(null);

        public Arguments(string[] args)
        {
            this.Source = args ?? new string[0];
            this.Properties = ArgumentInfo.Factory.ParseArgsDistinct(this.Source).AsReadOnly();
        }

        public ArgumentInfo this[string key] => this.Properties[key?.ToLowerInvariant()];

        public ArgumentInfo this[int index] => this.Properties.Values.ElementAt(index);

        public string[] Source { get; }

        public IReadOnlyDictionary<string, ArgumentInfo> Properties { get; }

        public bool Any => this.Properties.Any();

        public int Count => this.Properties.Count;

        public bool TryGetValue(string key, out ArgumentInfo argument) => this.Properties.TryGetValue(key.ToLowerInvariant(), out argument);

        public bool TryGetValue(int index, out ArgumentInfo argument)
        {
            if (this.Count > index)
            {
                argument = this.Properties.Values.ElementAt(index);
                return true;
            }
            argument = ArgumentInfo.Empty;
            return false;
        }

        public ArgumentInfo? GetValueOrNull(string key) => this.TryGetValue(key, out var argument) ? argument : (ArgumentInfo?) null;
        public ArgumentInfo? GetValueOrNull(int index) => this.TryGetValue(index, out var argument) ? argument : (ArgumentInfo?) null;

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
}
