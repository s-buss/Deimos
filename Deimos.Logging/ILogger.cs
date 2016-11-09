namespace Deimos.Logging
{
    public interface ILogger
    {
        void Log(LogLevel lvl, string msg, params object[] args);
    }
}
