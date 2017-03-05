using System.IO;

namespace Deimos.Logging.Implementations
{
    internal class FileLog : ILog
    {
        private TextWriter _writer;

        public FileLog(string filePath)
        {
            _writer = new StreamWriter(filePath, true);
        }

        public void AppendRecord(LogRecord rec)
        {
            lock (_writer)
            {
                _writer.Write(rec.TimeStamp.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
                _writer.Write("{0} ", rec.Level.ToString()[0]);
                _writer.Write("{0} ", rec.Logger);
                _writer.WriteLine(rec.Message, rec.Args);
                _writer.Flush();
            }
        }
    }
}
