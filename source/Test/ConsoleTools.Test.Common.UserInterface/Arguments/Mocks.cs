using System.Linq;
using System.Reflection;
using ConsoleTools.Common.UserInterface.Arguments;

namespace ConsoleTools.Test.Common.UserInterface.Arguments
{
    internal static class Mocks
    {
        /// <summary>{}</summary>
        public static readonly string[] EmptySet = {};
        /// <summary>{ "" }</summary>
        public static readonly string[] WhitespacedItem = { "" };
        /// <summary>{ "Foo" }</summary>
        public static readonly string[] SingleItem = { "Foo" };
        /// <summary>{ "Foo", "Bar" }</summary>
        public static readonly string[] MultipleItems = { "Foo", "Bar" };
        /// <summary>{ "+Foo", "++Bar", "-alice", "--bob", "/john" }</summary>
        public static readonly string[] FlaggedItems = { "+Foo", "++Bar", "-alice", "--bob", "/john" };
        /// <summary>{ "+Foo", "++Bar" }</summary>
        public static readonly string[] OnFlaggedItems = { "+Foo", "++Bar" };
        /// <summary>{ "-Foo", "--Bar" }</summary>
        public static readonly string[] OffFlaggedItems = { "-Foo", "--Bar" };
        /// <summary>{ "Foo:true", "Bar=false", "-alice:\"one two\"", "bob=\"two one\"" }</summary>
        public static readonly string[] ValuedItems = { "Foo:true", "Bar=false", "-alice:\"one two\"", "bob=\"two one\"" };
        /// <summary>{ "+foO=", "Foo=bar", "foo", "-fOo=BAR" }</summary>
        public static readonly string[] DuplicateItems = { "+foO=", "Foo=bar", "foo", "-fOo=BAR" };

        public static readonly SupportedArgument PositionalArgument = new Positional();
        public static readonly SupportedArgument NamedArgument = new Named();
        public static readonly SupportedArgument OnableArgument = new Onable();
        public static readonly SupportedArgument ShortKeyedArgument = new ShortKeyed();
        public static readonly SupportedArgument RequiredArgument = new RequiredArg();

        public static readonly SupportedArgument[] AllArguments = typeof(Mocks)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(f => f.GetValue(null))
            .OfType<SupportedArgument>()
            .ToArray();
        
        private sealed class Positional : SupportedPositionalArgument
        {
            public override string Key => null;
            public override string Description => "Positional supported argument";
            public override int Position => 0;
            public override string DefaultValue => "Foo";
        }

        private sealed class Named : SupportedArgument
        {
            public override string Key => "Foo";
            public override string Description => "Onable supported argument";
            public override string DefaultValue => null;
        }

        private sealed class Onable : SupportedOnableArgument
        {
            public override string Key => "Foo";
            public override string Description => "Onable supported argument";
            public override bool DefaultState => true;
            public override string DefaultValue => null;
        }

        private sealed class ShortKeyed : SupportedArgument
        {
            public override string Key => "Foo";
            public override string ShortKey => "f";
            public override string Description => "ShortKeyed supported argument";
        }

        private sealed class RequiredArg : SupportedArgument
        {
            public override string Key => "Bar";
            public override string Description => "Required supported argument";
            public override bool Required => true;
        }
    }
}
