using System.Collections.Generic;

namespace Deimos.Logging
{
    public abstract class LogBase : ILog
    {
        private ILogger _root;
        private Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        protected LogBase()
        {
            _root = Create("");
        }

        public ILogger Root
        {
            get
            {
                return _root;
            }
        }

        public ILogger CreateLogger(string name)
        {
            ILogger logger;

            lock (this)
            {
                if (!_loggers.TryGetValue(name, out logger))
                {
                    logger = Create(name);
                    _loggers.Add(name, logger);
                }
            }

            return logger;
        }

        protected abstract ILogger Create(string name);
    }
}
