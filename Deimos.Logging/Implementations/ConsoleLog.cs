using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Deimos.Logging.Implementations
{
    internal class ConsoleLog : ILog
    {
        public ConsoleLog()
        {
        }

        public void AppendRecord(LogRecord rec)
        {
            Console.Write(rec.TimeStamp.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
            Console.Write("{0} ", rec.Level.ToString()[0]);
            Console.Write("{0} ", rec.Logger);
            Console.WriteLine(rec.Message, rec.Args);
        }
    }
}
