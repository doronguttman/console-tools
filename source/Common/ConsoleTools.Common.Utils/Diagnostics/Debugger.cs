using System.Diagnostics;

namespace ConsoleTools.Common.Utils.Diagnostics
{
    public static class Debugger
    {
        [Conditional("DEBUG")]
        public static void BreakOnDebug(bool condition = true)
        {
            if (condition) System.Diagnostics.Debugger.Break();
        }

        public static void Break(bool condition = true)
        {
            if (condition) System.Diagnostics.Debugger.Break();
        }
    }
}
