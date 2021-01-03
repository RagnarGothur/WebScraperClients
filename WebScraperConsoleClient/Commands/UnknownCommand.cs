using Serilog;

using System;

namespace WebScraperConsoleClient.Commands
{
    internal class UnknownCommand : ConsoleCommandBase, IConsoleCommand
    {
        public string Name { get; }
        public string[] Args { get; }

        public UnknownCommand(string[] args, string name)
        {
            Args = args;
            Name = name;
        }

        public ExitCode Execute()
        {
            var errMsg = $"Unknown command \"{Name}\"";
            Log.Logger.Warning(errMsg);
            Console.Error.WriteLine(errMsg);

            return ExitCode.NoError;
        }
    }
}