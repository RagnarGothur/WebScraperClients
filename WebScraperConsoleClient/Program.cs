using Serilog;

using System;

namespace WebScraperConsoleClient
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger();

            try
            {
                foreach (string arg in args)
                    CommandParser.Parse(arg).Execute();

                var exitCode = ExitCode.NoError;
                do
                {
                    Console.Write(">");

                    var input = Console.ReadLine();
                    var command = CommandParser.Parse(input);
                    exitCode = command.Execute();
                }
                while (exitCode == ExitCode.NoError);

                return (int)exitCode;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"{e.Message} | see logs for more information");
                Log.Logger.Error(e.Message);
                Log.Logger.Error(e.StackTrace);

                return (int)ExitCode.CriticalError;
            }
        }

        private static ILogger CreateSerilogLogger()
        {
            const string path = @"..\..\..\..\..\log.txt";
            const string logTemplate = "[{Timestamp:yyyy:MM:dd:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}";

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(
                    path,
                    outputTemplate: logTemplate,
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();
        }
    }
}
