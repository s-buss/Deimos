using System;

namespace Deimos.Logging.Implementations
{
    internal class ConsoleLog : LogBase
    {
        private class ConsoleLogger : ILogger
        {
            private readonly ConsoleLog _log;
            private readonly string _name;

            public ConsoleLogger(ConsoleLog log, string name)
            {
                _log = log;
                _name = name;
            }

            public void Log(LogLevel lvl, string msg, params object[] args)
            {
                Console.Write(DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
                Console.Write("{0} ", lvl.ToString()[0]);
                Console.Write("{0} ", _name);
                Console.WriteLine(string.Format(msg, args));
            }
        }

        protected override ILogger Create(string name)
        {
            return new ConsoleLogger(this, name);
        }

        private void Log(string name, LogLevel level, string msg, object[] args)
        {
            lock (this)
            {
                Console.Write(DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
                Console.Write("{0} ", level.ToString()[0]);
                Console.Write("{0} ", name);
                Console.WriteLine(string.Format(msg, args));
            }
        }
    }
}