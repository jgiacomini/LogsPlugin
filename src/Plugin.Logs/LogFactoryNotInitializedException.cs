using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Logs
{
    public class LogFactoryNotInitializedException : Exception
    {
        public LogFactoryNotInitializedException()
            : base($"{nameof(LoggerFactory)} not initialized")
        {
        }
    }
}
