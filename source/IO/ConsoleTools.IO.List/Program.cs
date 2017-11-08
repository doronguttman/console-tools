using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ConsoleTools.Common.UserInterface.Arguments;

namespace ConsoleTools.IO.List
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

                var path = GetPath(arguments);
                var sort = arguments.GetValueOrNull("sort");
                sw.Stop();
                if (stats) Console.WriteLine($"Init time: {sw.Elapsed:c}");

                sw = Stopwatch.StartNew();
                var entries = GetEntries(path, arguments);
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

        private static IEnumerable<string> GetEntries(string path, ArgumentParser arguments)
        {
            var recuresive = new Arguments.RecursiveArgument().IsOn(arguments);

            if (!new Arguments.DirectoriesArgument().IsOn(arguments))
            {
                return Directory.GetFiles(path, "*.*", recuresive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }

            if (!new Arguments.FilesArgument().IsOn(arguments))
            {
                return Directory.GetDirectories(path, "*.*", recuresive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }

            return Directory.GetFileSystemEntries(path, "*.*", recuresive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        private static string GetPath(ArgumentParser arguments)
        {
            return arguments.Count == 0
                ? Environment.CurrentDirectory
                : Path.GetFullPath(Environment.ExpandEnvironmentVariables(arguments[0].Key));
        }
    }
}
