using System;
using System.IO;

namespace Deimos.Logging.Implementations
{
    internal class FileLog : LogBase
    {
        private class FileLogger : ILogger
        {
            private readonly FileLog _log;
            private readonly string _name;

            public FileLogger(FileLog log, string name)
            {
                _log = log;
                _name = name;
            }

            public void Log(LogLevel lvl, string msg, params object[] args)
            {
                _log.Log(_name, lvl, msg, args);
            }
        }

        private TextWriter _writer;

        public FileLog(string filePath)
        {
            _writer = new StreamWriter(filePath, true);
        }

        public void Log(string name, LogLevel level, string msg, object[] args)
        {
            lock (_writer)
            {
                _writer.Write(DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
                _writer.Write("{0} ", level.ToString()[0]);
                _writer.Write("{0} ", name);
                _writer.WriteLine(string.Format(msg, args));
                _writer.Flush();
            }
        }

        protected override ILogger Create(string name)
        {
            return new FileLogger(this, name);
        }
    }
}
