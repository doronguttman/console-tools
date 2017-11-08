using ConsoleTools.Common.UserInterface.Arguments;
using System;
using System.IO;
using System.Linq;
using ConsoleTools.IO.Delete.Arguments;
using ConsoleTools.Common.Extensions;
using ConsoleTools.Common.UserInterface;

namespace ConsoleTools.IO.Delete
{
    class Program
    {
        private static bool _quiet;
        private static bool _filesOnly;
        private static bool _removeEmptyDirs;
        private static bool _recursive;

        static void Main(string[] args)
        {
            try
            {
                ProcessArguments(args);

//                ConsoleHelper.ProcessDirectedInput(ProcessLine);
                ConsoleHelper.ReadAllLines().ForEach(ProcessLine);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ProcessArguments(string[] args)
        {
            var arguments = new ArgumentParser(args);
            Common.Utils.Diagnostics.Debugger.BreakOnDebug(() => new DebugSupportedArgument().IsOn(arguments));

            var parsedArgs = SupportedArgument.Factory.GetAssemblySupportedArguments().ToList();
            _quiet = parsedArgs.FirstOfType<QuietArgument>().IsOn(arguments);
            _filesOnly = parsedArgs.FirstOfType<FilesOnlyArgument>().IsOn(arguments);
            _removeEmptyDirs = _filesOnly && parsedArgs.FirstOfType<RemoveEmptyDirsArgument>().IsOn(arguments);
            _recursive = parsedArgs.FirstOfType<RecursiveArgument>().IsOn(arguments);
        }

        private static void ProcessLine(string line)
        {
            var isDir = File.GetAttributes(line).HasFlag(FileAttributes.Directory);
            if (isDir)
            {
                ProcessDirectory(line);
            }
            else
            {
                ProcessFile(line);
            }
        }

        private static void ProcessDirectory(string directory)
        {
            if (!_filesOnly)
            {
                DeleteDirectory(directory);
            }
            else if (!_removeEmptyDirs) return;

            var entries = Directory.GetFileSystemEntries(directory, "*.*", SearchOption.TopDirectoryOnly);
            if (entries.Length == 0) DeleteDirectory(directory);

        }

        private static void DeleteDirectory(string directory)
        {
            Console.WriteLine($"Removing directory{(_recursive ? " recursively" : "")}: {directory}");
            if (!IsConfirmed) return;
            Directory.Delete(directory, _recursive);
        }

        private static void ProcessFile(string file)
        {
            Console.WriteLine($"Deleting file: {file}");
            if (!IsConfirmed) return;
            File.Delete(file);
        }

        private static bool IsConfirmed
        {
            get
            {
                if (_quiet) return true;

                Console.Write("Are you sure? (N)o/(Y)es/(A)all [No]: ");
                var line = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (line == "a")
                {
                    _quiet = true;
                    return true;
                }
                return line == "y";
            }
        }
    }
}
