using System;

namespace Deimos.Logging
{
    public struct LogRecord
    {
        public DateTime TimeStamp;
        public string Logger;
        public LogLevel Level;
        public string Message;
        public object[] Args;
    }
}
