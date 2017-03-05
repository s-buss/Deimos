namespace Deimos.Logging
{
    public interface ILogger
    {
        LogLevel Level { get; set; }

        void Debug(string msg, params object[] args);

        void Info(string msg, params object[] args);

        void Warning(string msg, params object[] args);

        void Error(string msg, params object[] args);

        void Log(LogLevel lvl, string msg, params object[] args);
    }
}
