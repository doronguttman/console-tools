using ConsoleTools.Common.UserInterface.Arguments;
using System;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
            try
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
            catch (DirectoryNotFoundException)
            {
                // ignored
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing '{line}': {ex.Message}");
            }
        }

        private static void ProcessDirectory(string directory)
        {
            if (!Directory.Exists(directory)) return;

            if (!_filesOnly)
            {
                DeleteDirectory(directory);
                return;
            }

            if (!_removeEmptyDirs) return;

            var entries = Directory.GetFileSystemEntries(directory, "*.*", SearchOption.TopDirectoryOnly);
            if (entries.Length == 0) DeleteDirectory(directory);
        }

        private static void DeleteDirectory(string directory)
        {
            Console.Write($"Removing directory{(_recursive ? " recursively" : "")}: {directory}");
            if (!IsConfirmed) return;
            try
            {
                Directory.Delete(directory, _recursive);
            }
            catch (DirectoryNotFoundException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ProcessFile(string file)
        {
            if (!File.Exists(file)) return;

            Console.WriteLine($"Deleting file: {file}");
            if (!IsConfirmed) return;
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
