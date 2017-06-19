using Plugin.Logs.Writer;
using System;
namespace Plugin.Logs.Model
{
	/// <summary>
	/// The data to log
	/// </summary>
    public class DataToLog
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="DataToLog"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="logWritterService">The log writter service.</param>
        public DataToLog(string data, LogLevel logLevel, ILogWriterService logWritterService)
		{
			Data = data;
			When = DateTime.Now;
			Level = logLevel;
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
        /// Gets the log writter service.
        /// </summary>
        /// <value>
        /// The log writter service.
        /// </value>
        public ILogWriterService LogWritterService { get; private set; }
    }
}
