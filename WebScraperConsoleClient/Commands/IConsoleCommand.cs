namespace WebScraperConsoleClient.Commands
{
    public interface IConsoleCommand
    {
        public string[] Args { get; }

        public ExitCode Execute();
    }
}
