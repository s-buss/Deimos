namespace Deimos.Logging
{
    public static class ILoggerExtensions
    {
        public static void Debug(this ILogger logger, string msg, params object[] args)
        {
            logger.Log(LogLevel.Debug, msg, args);
        }

        public static void Info(this ILogger logger, string msg, params object[] args)
        {
            logger.Log(LogLevel.Info, msg, args);
        }

        public static void Warning(this ILogger logger, string msg, params object[] args)
        {
            logger.Log(LogLevel.Warning, msg, args);
        }

        public static void Error(this ILogger logger, string msg, params object[] args)
        {
            logger.Log(LogLevel.Warning, msg, args);
        }

    }
}
