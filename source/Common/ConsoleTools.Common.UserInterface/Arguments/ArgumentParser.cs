using System.Collections.Generic;
using System.Linq;
using ConsoleTools.Common.Extensions;

namespace ConsoleTools.Common.UserInterface.Arguments
{
    public sealed class ArgumentParser
    {
        public static readonly ArgumentParser Empty = new ArgumentParser(null);

        public ArgumentParser(string[] args)
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
    }
}
