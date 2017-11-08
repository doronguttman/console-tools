using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using ConsoleTools.Common.UserInterface;

namespace ConsoleTools.IO.List
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var arguments = new Arguments(args);
                
                ConnectDebugger(arguments);

                var stats = arguments.GetValueOrNull("stats")?.IsOn == true;

                var path = GetPath(arguments);
                var filter = GetFilter(arguments);
                var sort = arguments.GetValueOrNull("sort");
                sw.Stop();
                if (stats) Console.WriteLine($"Init time: {sw.Elapsed:c}");

                sw = Stopwatch.StartNew();
                var entries = Directory.GetFileSystemEntries(path, "*.*", SearchOption.AllDirectories).AsEnumerable();
                sw.Stop();
                if (stats)
                {
                    Console.WriteLine($"Enumeration time: {sw.Elapsed:c}");
                    using (var stream = new MemoryStream())
                    {
                        new BinaryFormatter().Serialize(stream, entries);
                        Console.WriteLine($"Enumeration size: {stream.Length:N1}B");
                    }
                }

                sw = Stopwatch.StartNew();
                if (filter != null) entries = entries.Where(entry => filter.IsMatch(entry));
                if (sort != null) entries = sort.Value.IsOff ? entries.OrderByDescending(e => e) : entries.OrderBy(e => e);

                foreach (var entry in entries)
                {
                    Console.WriteLine(entry);
                }
                sw.Stop();
                if (stats) Console.WriteLine($"Processing time: {sw.Elapsed:c}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static Regex GetFilter(Arguments arguments)
        {
            if (!arguments.TryGetValue("filter", out var filterArg) && !arguments.TryGetValue(1, out filterArg))
                return null;

            return new Regex(filterArg.Value);
        }

        private static string GetPath(Arguments arguments)
        {
            return arguments.Count == 0
                ? Environment.CurrentDirectory
                : Path.GetFullPath(Environment.ExpandEnvironmentVariables(arguments[0].Key));
        }

        [Conditional("DEBUG")]
        private static void ConnectDebugger(Arguments arguments)
        {
            if (arguments.TryGetValue("debug", out var arg) && arg.IsOn) Debugger.Break();
        }
    }
}
