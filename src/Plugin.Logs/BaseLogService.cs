using System;
using Plugin.Logs.Extension;
using Plugin.Logs.Model;
using Plugin.Logs.Writer;

namespace Plugin.Logs
{
    /// <summary>
    /// Base class  of log service
    /// </summary>
    public abstract class BaseLogService : ILogService
    {
        /// <summary>
        /// The log writer
        /// </summary>
        private readonly ILogWriterService _logWriter;

        /// <summary>
        /// The nb month to keep
        /// </summary>
        private uint _nbDaysToKeep;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLogService"/> class.
        /// </summary>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="nbDaysToKeep">The nb days to keep.</param>
        public BaseLogService(ILogWriterService logWriter, uint nbDaysToKeep)
        {
            NbDaysToKeep = nbDaysToKeep;

            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }

            _logWriter = logWriter;

            logWriter.PurgeOldDaysAsync(NbDaysToKeep).ConfigureAwait(false);
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
