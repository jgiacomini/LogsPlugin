using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Logs
{
    public class LoggerFactoryNotInitializedException : Exception
    {
        public LoggerFactoryNotInitializedException()
            : base($"{nameof(LoggerFactory)} not initialized")
        {
        }
    }
}
