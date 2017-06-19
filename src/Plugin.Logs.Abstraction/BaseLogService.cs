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
		/// The logger
		/// </summary>
		private readonly ThreadLogger _threadLogger;

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

			_threadLogger = new ThreadLogger(logWriter);
			_threadLogger.Start();

			logWriter.PurgeOldDaysAsync(NbDaysToKeep);
		}

		/// <inheritdoc />
		public void Log(string message, LogLevel logLevel = LogLevel.Information)
		{
			_threadLogger.AddDataToLog(message, logLevel);
		}

		/// <inheritdoc />
		public void Log(Exception exception, LogLevel logLevel = LogLevel.Error)
		{
			_threadLogger.AddDataToLog(exception.CreateExceptionString(), logLevel);
		}

		/// <inheritdoc />
		public System.Threading.Tasks.Task FlushAsync()
		{
			return _threadLogger.FlushAsync();
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
			_threadLogger.Dispose();
		}
	}
}
