using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTools.Common.Utils;

namespace ConsoleTools.Common.UserInterface.Arguments
{
    public abstract class SupportedArgument
    {
        public abstract string Key { get; }
        public virtual string ShortKey => this.Key;

        public abstract string Description { get; }
        public virtual string DefaultValue => null;

        public virtual bool Required => false;

        public virtual ArgumentInfo? Find(ArgumentParser argumentParser)
        {
            if (argumentParser == null) throw new ArgumentNullException(nameof(argumentParser));

            if (this.Key != null)
            {
                if (argumentParser.TryGetValue(this.Key, out var rslt)) return rslt;
                if (this.ShortKey != this.Key && argumentParser.TryGetValue(this.ShortKey, out rslt)) return rslt;
                return null;
            }

            if (this is IPositional positional)
            {
                return argumentParser.GetValueOrNull(positional.Position);
            }

            return null;
        }

        public static bool RequirementsMet(ArgumentParser argumentParser, params SupportedArgument[] supportedArguments)
        {
            return supportedArguments.Where(arg => arg.Required).All(arg => arg.Find(argumentParser) != null);
        }

        public static class Factory
        {
            public static IEnumerable<SupportedArgument> GetAssemblySupportedArguments(System.Reflection.Assembly assembly = null)
            {
                if (assembly == null) assembly = System.Reflection.Assembly.GetEntryAssembly();

                var baseType = typeof(SupportedArgument);
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract || !baseType.IsAssignableFrom(type)) continue;
                    if (!(Try.Ignore(() => Activator.CreateInstance(type)) is SupportedArgument instance)) continue;
                    yield return instance;
                }
            }

            public static IDictionary<SupportedArgument, ArgumentInfo?> GetAssemblySupportedArguments(ArgumentParser argumentParser, System.Reflection.Assembly assembly = null)
            {
                if (assembly == null) assembly = System.Reflection.Assembly.GetEntryAssembly();
                var supportedArgs = GetAssemblySupportedArguments(assembly);
                return supportedArgs.ToDictionary(arg => arg, arg => arg.Find(argumentParser));
            }
        }
    }

    public abstract class SupportedOnableArgument : SupportedArgument, IOnable
    {
        #region Implementation of IOnable
        public abstract bool DefaultState { get; }
        public bool IsOn(ArgumentParser argumentParser)
        {
            var arg = this.Find(argumentParser);
            return arg != null && arg.Value.HasFlag 
                ? arg.Value.IsOn 
                : this.DefaultState;
        }
        #endregion Implementation of IOnable
    }

    public abstract class SupportedPositionalArgument : SupportedArgument, IPositional
    {
        #region Implementation of IPositional
        public abstract int Position { get; }
        #endregion Implementation of IPositional
    }

    public abstract class SupportedPositionalOnableArgument : SupportedArgument, IPositional, IOnable
    {
        #region Implementation of IPositional
        public abstract int Position { get; }
        #endregion Implementation of IPositional

        #region Implementation of IOnable
        public abstract bool DefaultState { get; }
        public bool IsOn(ArgumentParser argumentParser) => this.Find(argumentParser)?.IsOn == true;
        #endregion Implementation of IOnable
    }
}
