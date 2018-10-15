using System;
using System.Threading.Tasks;
using Plugin.Logs.Model;

namespace Plugin.Logs
{
    /// <summary>
    /// Represent a Log service
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Trace(string message);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warning(string message);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Critical(string message);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);

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
