namespace WebScraperConsoleClient
{
    /// <summary>
    /// Exit codes. Note that errors must be logged, but should not showstop the program
    /// so no need more exit codes except "no error" and "critical error" cases for this program
    /// </summary>
    public enum ExitCode
    {
        /// <summary>
        /// No error
        /// </summary>
        NoError = 0,
        /// <summary>
        /// Critical error that showstops the program 
        /// </summary>
        CriticalError = 1,
    }
}
