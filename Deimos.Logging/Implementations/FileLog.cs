using System;
using System.IO;

namespace Deimos.Logging.Implementations
{
    internal class FileLog : ILog
    {
        private const int Keep = 7;

        private readonly string _fileName;
        
        private TextWriter _writer;
        private DateTime _nextRotation;

        public FileLog(string filePath)
        {
            _fileName = filePath;
            _writer = new StreamWriter(_fileName, true);
            _nextRotation = DateTime.UtcNow.AddHours(24);
        }

        public void AppendRecord(LogRecord rec)
        {
            if (DateTime.UtcNow >= _nextRotation)
            {
                // Close the current log file.
                _writer.Close();

                RotateLogFiles();
                _nextRotation = DateTime.UtcNow.AddHours(24);

                // Open the file again
                _writer = new StreamWriter(_fileName, true);
            }

            lock (_writer)
            {
                _writer.Write(rec.TimeStamp.ToString("yyyy-MM-dd-HH:mm:ss.fff "));
                _writer.Write("{0} ", rec.Level.ToString()[0]);
                _writer.Write("{0} ", rec.Logger);
                _writer.WriteLine(rec.Message, rec.Args);
                _writer.Flush();
            }
        }

        private void RotateLogFiles()
        {
            // Delete the oldest file to keep
            File.Delete(GenerateFileName(Keep));

            // Rename other backups
            for (int n = Keep - 1; n > 0; n--)
            {
                string oldName = GenerateFileName(n);
                string newName = GenerateFileName(n + 1);
                if (File.Exists(oldName))
                {
                    File.Move(oldName, newName);
                }
            }

            // Backup the current file
            File.Move(_fileName, GenerateFileName(1));
        }


        private string GenerateFileName(int number)
        {
            return _fileName + "." + number;
        }
    }
}
