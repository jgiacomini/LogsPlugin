using System;

namespace Plugin.Logs
{
    public class LoggerFactory : ILoggerFactory
    {
        private string _logDirectoryPath;
        private uint _nbDaysToKeep = 60;

        /// <inheritdoc />
        public uint NbDaysToKeep
        {
            get
            {
                return _nbDaysToKeep;
            }

            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("NbDaysToKeep", value, "Must be superior to 1");
                }

                _nbDaysToKeep = value;
            }
        }

        /// <inheritdoc />
        public string LogDirectoryPath
        {
            get
            {
                return _logDirectoryPath;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(LogDirectoryPath));
                }

                _logDirectoryPath = value;
            }
        }

        /// <inheritdoc />
        public ILogService GetLogger(string name)
        {
            if (string.IsNullOrWhiteSpace(_logDirectoryPath))
            {
                throw new 
            }

            return new LogService(name, LogDirectoryPath, NbDaysToKeep);
        }
    }
}
