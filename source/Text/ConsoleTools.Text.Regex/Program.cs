using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ConsoleTools.Common.UserInterface.Arguments;

namespace ConsoleTools.Text.Regex
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var arguments = new ArgumentParser(args);

                Common.Utils.Diagnostics.Debugger.BreakOnDebug(() => new DebugSupportedArgument().IsOn(arguments));
                
                var stats = arguments.GetValueOrNull("stats")?.IsOn == true;

                var filter = GetFilter(arguments);
                if (filter == null)
                {
                    throw new ArgumentException("Missing arguments");
                }
                sw.Stop();
                if (stats) Console.WriteLine($"Init time: {sw.Elapsed:c}");

                sw = Stopwatch.StartNew();
                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (filter(line)) Console.WriteLine(line);
                }
                sw.Stop();
                if (stats) Console.WriteLine($"Processing time: {sw.Elapsed:c}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static Func<string, bool> GetFilter(ArgumentParser arguments)
        {
            var arg = arguments.GetValueOrNull("regex") ?? arguments.GetValueOrNull(0);
            if (arg == null) return null;

            var regexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;
            if (arguments.GetValueOrNull("case")?.IsOff == true) regexOptions = regexOptions | RegexOptions.IgnoreCase;
            if (arguments.GetValueOrNull("multiline")?.IsOn == true) regexOptions = regexOptions | RegexOptions.Multiline;
            if (arguments.GetValueOrNull("rtl").HasValue) regexOptions = regexOptions | RegexOptions.RightToLeft;
            if (arguments.GetValueOrNull("singleline")?.IsOn == true) regexOptions = regexOptions | RegexOptions.Singleline;

            var regex = new System.Text.RegularExpressions.Regex(arg.Value.Value, regexOptions);

            return (arguments.GetValueOrNull("neg") ?? arguments.GetValueOrNull("negative"))?.IsOn == true
                ? (Func<string, bool>) (str => !regex.IsMatch(str))
                : (str => regex.IsMatch(str));
        }
    }
}
