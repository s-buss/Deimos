using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Deimos.Logging.Implementations;
using System.Threading;

namespace Deimos.Logging
{
    public static class Log
    {
        private static ILog Instance = new NullLog();

        private static Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        private static BlockingCollection<LogRecord> _appendQueue = new BlockingCollection<LogRecord>();
        private static Thread _appendThread;

        public static LogLevel DefaultLevel { get; set; }

        static Log()
        {
            DefaultLevel = LogLevel.Info;

            _appendThread = new Thread(AppendRecords);
            _appendThread.IsBackground = true;
            _appendThread.Priority = ThreadPriority.BelowNormal;
            _appendThread.Name = "LogAppender";
            _appendThread.Start();
        }

        public static void ToFile(string path)
        {
            Instance = new FileLog(path);
        }

        public static void ToConsole()
        {
            Instance = new ConsoleLog();
        }

        public static ILogger CreateLogger(string name, LogLevel? level = null)
        {
            ILogger logger;

            lock (_loggers)
            {
                if (!_loggers.TryGetValue(name, out logger))
                {
                    logger = new Logger(name);
                    _loggers.Add(name, logger);

                    logger.Level = level.HasValue ? level.Value : DefaultLevel;
                }
            }

            return logger;
        }

        internal static void Append(string name, LogLevel level, string msg, object[] args)
        {
            _appendQueue.Add(new LogRecord
            {
                TimeStamp = DateTime.UtcNow,
                Logger = name,
                Level = level,
                Message = msg,
                Args = args
            });
        }

        private static void AppendRecords()
        {
            while (true)
            {
                LogRecord rec = _appendQueue.Take();

                Instance.AppendRecord(rec);
            }
        }
    }
}
