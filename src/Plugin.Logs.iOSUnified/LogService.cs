using System;
using Plugin.Logs;
using Plugin.Logs.Writer;

namespace Plugin.Logs
{
	/// <summary>
	/// Service to log anything you want. There is some magic in it.
	/// </summary>
	public class LogService : BaseLogService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogService"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public LogService(string fileName, string logDirectoryPath, uint nbDaysToKeep = 60)
            : base(new LogWriterService(fileName, logDirectoryPath), nbDaysToKeep)
		{
		}
	}
}
