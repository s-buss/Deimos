using Deimos.Logging.Implementations;

namespace Deimos.Logging
{
    public static class Log
    {
        private static ILog Instance = new NullLog();

        public static void ToFile(string path)
        {
            Instance = new FileLog(path);
        }

        public static void ToConsole()
        {
            Instance = new ConsoleLog();
        }

        public static ILogger CreateLogger(string name)
        {
            return Instance.CreateLogger(name);
        }
    }
}
