namespace Deimos.Logging.Implementations
{
    public class NullLog : ILog
    {
        private class NullLogger : ILogger
        {
            public LogLevel Level { get; set; }

            public void Log(LogLevel lvl, string msg, params object[] args)
            { }
        }

        private ILogger _logger = new NullLogger();

        public ILogger Root
        {
            get { return _logger; }
        }

        public ILogger CreateLogger(string name)
        {
            return _logger;
        }
    }
}
