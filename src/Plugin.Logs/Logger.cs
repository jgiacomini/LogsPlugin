using Plugin.Logs.Extension;
using Plugin.Logs.Model;
using Plugin.Logs.Writer;
using System;

namespace Plugin.Logs
{
    /// <summary>
    /// Logger to log anything you want. There is some magic in it.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly ILogListener _logListener;
        private readonly uint _nbDaysToKeep;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="logDirectoryPath">The log directory path.</param>
        internal Logger(string fileName, string logDirectoryPath, uint nbDaysToKeep = 60)
            : this(new CsvListener(fileName, logDirectoryPath), nbDaysToKeep)
        {
        }

        internal Logger(ILogListener logListener, uint nbDaysToKeep = 60)
        {
            _nbDaysToKeep = nbDaysToKeep;
            _logListener = logListener ?? throw new ArgumentNullException("logWriter");
            logListener.PurgeOldDaysAsync(nbDaysToKeep).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void Log(string message, LogLevel logLevel = LogLevel.Information)
        {
            BackgroundWorker.Instance.AddDataToLog(message, logLevel, _logListener);
        }

        /// <inheritdoc />
        public void Log(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            BackgroundWorker.Instance.AddDataToLog(exception.ToFormattedString(), logLevel, _logListener);
        }

        /// <inheritdoc />
        public void Log(string message, Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            BackgroundWorker.Instance.AddDataToLog($"{message} {Environment.NewLine}{exception.ToFormattedString()}", logLevel, _logListener);
        }

        /// <inheritdoc />
        public System.Threading.Tasks.Task FlushAsync()
        {
            return BackgroundWorker.Instance.FlushAsync();
        }

        /// <inheritdoc />
        public System.Threading.Tasks.Task PurgeOldDaysAsync()
        {
            return _logListener.PurgeOldDaysAsync(_nbDaysToKeep);
        }
    }
}
