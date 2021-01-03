using System;

namespace WebScraperConsoleClient.Commands
{
    public class ExitCommand : ConsoleCommandBase, IConsoleCommand
    {
        public string[] Args { get; }

        public ExitCommand(string[] args)
        {
            Args = args;
        }

        public ExitCode Execute()
        {
            Environment.Exit((int)ExitCode.NoError);
            return ExitCode.NoError;
        }
    }
}
