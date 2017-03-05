using System;
using System.IO;

using System.Collections.Concurrent;
using System.Threading;

namespace Deimos.Logging.Implementations
{
    internal class FileLog : LogBase
    {
        private struct LogRecord
        {
            public DateTime TimeStamp;
            public string Logger;
            public LogLevel Level;
            public string Message;
        }

        private class FileLogger : ILogger
        {
            private readonly FileLog _log;
            private readonly string _name;

            private LogLevel _level = LogLevel.Debug;

            public FileLogger(FileLog log, string name)
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

        private TextWriter _writer;
        private BlockingCollection<LogRecord> _queue = new BlockingCollection<LogRecord>();
        private Thread _writerThread;

        public FileLog(string filePath)
        {
            _writer = new StreamWriter(filePath, true);
            _writerThread = new Thread(AppendRecords) { IsBackground = true };
            _writerThread.Start();
        }

        private void AppendRecords()
        {
            while (true)
            {
                LogRecord rec = _queue.Take();

                lock (_writer)
                {
                    _writer.Write(rec.TimeStamp.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
                    _writer.Write("{0} ", rec.Level.ToString()[0]);
                    _writer.Write("{0} ", rec.Logger);
                    _writer.WriteLine(rec.Message);
                    _writer.Flush();
                }
            }
        }

        public void Log(string name, LogLevel level, string msg, object[] args)
        {
            _queue.Add(new LogRecord {
                TimeStamp = DateTime.UtcNow,
                Logger = name,
                Level = level,
                Message = string.Format(msg, args)
            });
        }

        protected override ILogger Create(string name)
        {
            return new FileLogger(this, name);
        }
    }
}
