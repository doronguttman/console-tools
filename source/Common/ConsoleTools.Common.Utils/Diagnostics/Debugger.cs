using System;
using System.Diagnostics;

namespace ConsoleTools.Common.Utils.Diagnostics
{
    public static class Debugger
    {
        [Conditional("DEBUG")]
        public static void BreakOnDebug(bool condition = true)
        {
            BreakOnDebug(() => condition);
        }

        [Conditional("DEBUG")]
        public static void BreakOnDebug(Func<bool> predicate)
        {
            if (predicate()) System.Diagnostics.Debugger.Break();
        }

        public static void Break(bool condition = true)
        {
            Break(() => condition);
        }

        public static void Break(Func<bool> predicate)
        {
            if (predicate()) System.Diagnostics.Debugger.Break();
        }
    }
}
