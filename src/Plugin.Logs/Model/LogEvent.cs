using Plugin.Logs.Writer;
using System;
namespace Plugin.Logs.Model
{
    /// <summary>
    /// The event to log
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEvent"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="listener">The log writter service.</param>
        internal LogEvent(string data, LogLevel logLevel, ILogListener listener)
        {
            Data = data;
            When = DateTime.Now;
            Level = logLevel;
            Listener = listener;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }

        /// <summary>
        /// When does it occured
        /// </summary>
        public DateTime When { get; private set; }

        /// <summary>
        /// Level of the log
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Gets the <see cref="ILogListener"/>.
        /// </summary>
        /// <value>
        /// The log <see cref="ILogListener"/>.
        /// </value>
        public ILogListener Listener { get; private set; }
    }
}
