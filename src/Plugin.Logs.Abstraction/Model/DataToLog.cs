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
		public DataToLog(string data, LogLevel logLevel = LogLevel.Information)
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
	}
}
