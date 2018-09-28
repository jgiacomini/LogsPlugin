using System;
using System.Threading.Tasks;
using Plugin.Logs.Model;

namespace Plugin.Logs
{
    /// <summary>
    /// Represent a Log service
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ILogService : IDisposable
    {
        /// <summary>
        /// Gets or sets the nb month to keep.
        /// </summary>
        /// <value>
        /// The nb month to keep.
        /// </value>
        uint NbDaysToKeep { get; }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        void Log(string message, LogLevel logLevel = LogLevel.Information);

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logLevel">The log level.</param>
        void Log(Exception exception, LogLevel logLevel = LogLevel.Error);

        /// <summary>
        /// Logs the specified exception and his custom message.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logLevel">The log level.</param>
        void Log(string message, Exception exception, LogLevel logLevel = LogLevel.Error);

        /// <summary>
        /// Purges the old days asynchronous.
        /// </summary>
        /// <returns>return a task</returns>
        Task PurgeOldDaysAsync();

        /// <summary>
        /// Flushes the asynchronous.
        /// </summary>
        /// <returns>return a task</returns>
        Task FlushAsync();
    }
}
