using Plugin.Logs.Extension;
using Plugin.Logs.Model;
using Plugin.Logs.Writer;
using System;

namespace Plugin.Logs
{
    /// <summary>
    /// Service to log anything you want. There is some magic in it.
    /// </summary>
    public class LogService : ILogService
    {
        /// <summary>
        /// The log writer
        /// </summary>
        private readonly ILogWriterService _logWriter;

        /// <summary>
        /// The nb month to keep
        /// </summary>
        private uint _nbDaysToKeep;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="logDirectoryPath">The log directory path.</param>
        public LogService(string fileName, string logDirectoryPath, uint nbDaysToKeep = 60)
            : this(new LogWriterService(fileName, logDirectoryPath), nbDaysToKeep)
        {
            NbDaysToKeep = nbDaysToKeep;
        }

        public LogService(ILogWriterService logWriter, uint nbDaysToKeep = 60)
        {
            NbDaysToKeep = nbDaysToKeep;
            _logWriter = logWriter ?? throw new ArgumentNullException("logWriter");

            logWriter.PurgeOldDaysAsync(NbDaysToKeep).ConfigureAwait(false);
        }

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
        public void Log(string message, LogLevel logLevel = LogLevel.Information)
        {
            ThreadLogger.Instance.AddDataToLog(message, logLevel, _logWriter);
        }

        /// <inheritdoc />
        public void Log(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            ThreadLogger.Instance.AddDataToLog(exception.CreateExceptionString(), logLevel, _logWriter);
        }

        /// <inheritdoc />
        public void Log(string message, Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            ThreadLogger.Instance.AddDataToLog($"{message} {Environment.NewLine}{exception.CreateExceptionString()}", logLevel, _logWriter);
        }

        /// <inheritdoc />
        public System.Threading.Tasks.Task FlushAsync()
        {
            return ThreadLogger.Instance.FlushAsync();
        }

        /// <inheritdoc />
        public System.Threading.Tasks.Task PurgeOldDaysAsync()
        {
            return _logWriter.PurgeOldDaysAsync(NbDaysToKeep);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _logWriter.Dispose();
            ThreadLogger.Instance.Dispose();
        }
    }
}
