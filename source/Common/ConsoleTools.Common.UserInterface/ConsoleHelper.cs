using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ConsoleTools.Common.Utils;

namespace ConsoleTools.Common.UserInterface
{
    public static class ConsoleHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public delegate void LineProcessor(string line);

        public static void ProcessDirectedInput(LineProcessor lineProcessor, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.Register(() => throw new OperationCanceledException(cancellation));
                string line;
                while (!cancellation.IsCancellationRequested && (line = Console.ReadLine()) != null)
                {
                    lineProcessor(line);
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
        }

        public static IEnumerable<string> ReadAllLines(CancellationToken cancellation = default)
        {
            if (Console.IsInputRedirected)
            {
                var input = Console.In.ReadToEnd();
                var lines = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(line => line.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));

                ReattachConsole();

                foreach (var line in lines)
                {
                    yield return line;
                }
            }
            else
            {
                cancellation.Register(() => throw new OperationCanceledException(cancellation));
                string line;
                while ((line = Try.Ignore<OperationCanceledException, string>(Console.ReadLine)) != null)
                {
                    yield return line;
                }
            }
        }

        public static bool ReattachConsole()
        {
            try
            {
                Console.In.ReadToEnd();
                if (!FreeConsole() || !AttachConsole(-1)) return false;
                Console.SetIn(new StreamReader(Console.OpenStandardInput()));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
