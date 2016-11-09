namespace Deimos.Logging
{
    interface ILog
    {
        ILogger Root { get; }

        ILogger CreateLogger(string name);
    }
}
