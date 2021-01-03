using Serilog;

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebScraperConsoleClient.Commands
{
    public class GetImagesCommand : ConsoleCommandBase, IConsoleCommand
    {
        private const string WEB_SCRAPER_URL = "http://localhost:5000";
        private const string GET_IMAGES = "images";
        private const string FULLY_COMPLETED_HEADER = "Fully-Completed";

        public string[] Args { get; }

        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly string _webScraperUrl;
        private readonly string _url;
        private readonly int _imageCount;
        private readonly int _threadCount;

        public GetImagesCommand(string[] args)
        {
            Args = args;

            _webScraperUrl = $"{GetArgumentValue(Args, "-WebScraperUrl") ?? WEB_SCRAPER_URL}/{GET_IMAGES}";
            _url = GetArgumentValue(Args, "-Url") ?? throw new ArgumentException("-Url argument is required");

            _imageCount = Int32.Parse(
                GetArgumentValue(Args, "-ImageCount") ?? throw new ArgumentException("-ImageCount argument is required")
            );

            _threadCount = Int32.Parse(
                GetArgumentValue(Args, "-ThreadCount") ?? throw new ArgumentException("-ThreadCount argument is required")
            );
        }

        public ExitCode Execute()
        {
            try
            {
                var url = $"{_webScraperUrl}?url={_url}&imageCount={_imageCount}&threadCount={_threadCount}";

                var response = Task.Run(async () => await _httpClient.GetAsync(url)).Result;
                response.EnsureSuccessStatusCode();
                var isFullyCompleted = response.Headers.GetValues(FULLY_COMPLETED_HEADER).First();

                if (isFullyCompleted == "0")
                    Console.Out.WriteLine("partially completed");

                Console.Out.WriteLine(
                    Task.Run(async () => await response.Content.ReadAsStringAsync()).Result
                );
            }
            catch (Exception e) when
            (
                e is HttpRequestException
                ||
                (e is AggregateException ae && ae.InnerException is HttpRequestException)
            )
            {
                Console.Error.WriteLine($"{e.Message} | see logs for more information");
                Log.Logger.Error(e.Message);
                Log.Logger.Error(e.StackTrace);
                Log.Logger.Error(e.InnerException?.Message);
                Log.Logger.Error(e.InnerException?.StackTrace);
            }

            return ExitCode.NoError;
        }
    }
}
