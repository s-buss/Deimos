namespace Deimos.Logging
{
    public class Logger : ILogger
    {
        private readonly string _name;

        private LogLevel _level = LogLevel.Debug;

        public Logger(string name)
        {
            _name = name;
        }

        public LogLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public void Debug(string msg, params object[] args)
        {
            Log(LogLevel.Debug, msg, args);
        }

        public void Info(string msg, params object[] args)
        {
            Log(LogLevel.Info, msg, args);
        }

        public void Warning(string msg, params object[] args)
        {
            Log(LogLevel.Warning, msg, args);
        }

        public void Error(string msg, params object[] args)
        {
            Log(LogLevel.Warning, msg, args);
        }

        public void Log(LogLevel lvl, string msg, params object[] args)
        {
            if (lvl >= _level)
            {
                Logging.Log.Append(_name, lvl, msg, args);
            }
        }
    }
}
