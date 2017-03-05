namespace Deimos.Logging
{
    public interface ILogger
    {
        LogLevel Level { get; set; }

        void Log(LogLevel lvl, string msg, params object[] args);
    }
}
