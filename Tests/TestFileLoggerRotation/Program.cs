using Deimos.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestFileLoggerRotation
{
    class Program
    {
        private static ILogger Logger = Log.CreateLogger("Test");

        static void Main(string[] args)
        {
            Log.ToFile("dummy.log");

            while (true)
            {
                Logger.Info("I am alive.");
                Thread.Sleep(30000);
            }
        }
    }
}
