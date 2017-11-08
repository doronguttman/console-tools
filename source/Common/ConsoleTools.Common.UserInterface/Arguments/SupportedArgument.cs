using System;
using System.Linq;

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
    }

    public abstract class SupportedOnableArgument : SupportedArgument, IOnable
    {
        #region Implementation of IOnable
        public abstract bool DefaultState { get; }
        public bool IsOn(ArgumentParser argumentParser) => this.Find(argumentParser)?.IsOff != true && this.DefaultState;
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
