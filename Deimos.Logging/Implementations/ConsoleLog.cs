using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Deimos.Logging.Implementations
{
    internal class ConsoleLog : LogBase
    {
        private struct LogRecord
        {
            public DateTime TimeStamp;
            public string Logger;
            public LogLevel Level;
            public string Message;
        }

        private BlockingCollection<LogRecord> _queue = new BlockingCollection<LogRecord>();
        private Thread _writerThread;

        private class ConsoleLogger : ILogger
        {
            private readonly ConsoleLog _log;
            private readonly string _name;

            private LogLevel _level = LogLevel.Debug;

            public ConsoleLogger(ConsoleLog log, string name)
            {
                _log = log;
                _name = name;
            }

            public LogLevel Level { get { return _level; } set { _level = value; } }

            public void Log(LogLevel lvl, string msg, params object[] args)
            {
                if (lvl >= _level)
                {
                    _log.Log(_name, lvl, msg, args);
                }
            }
        }

        public ConsoleLog()
        {
            _writerThread = new Thread(AppendRecords) { IsBackground = true };
            _writerThread.Start();
        }

        protected override ILogger Create(string name)
        {
            return new ConsoleLogger(this, name);
        }

        private void Log(string name, LogLevel level, string msg, object[] args)
        {
            _queue.Add(new LogRecord
            {
                TimeStamp = DateTime.UtcNow,
                Logger = name,
                Level = level,
                Message = string.Format(msg, args)
            });
        }

        private void AppendRecords()
        {
            while (true)
            {
                LogRecord rec = _queue.Take();

                Console.Write(rec.TimeStamp.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
                Console.Write("{0} ", rec.Level.ToString()[0]);
                Console.Write("{0} ", rec.Logger);
                Console.WriteLine(rec.Message);
            }
        }
    }
}