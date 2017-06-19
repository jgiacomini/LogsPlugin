using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Logs.Extension;
using Plugin.Logs.Model;
using Plugin.Logs.Writer;

namespace Plugin.Logs
{
	/// <summary>
	/// Manage the main loop to log data
	/// </summary>
	/// <seealso cref="System.IDisposable" />
    internal class ThreadLogger : IDisposable
	{
		#region Fields
		/// <summary>
		/// The log writer
		/// </summary>
		private readonly ILogWriterService _logWriter;

		/// <summary>
		/// The is alive
		/// </summary>
		private bool _isAlive = true;

		/// <summary>
		/// The _queued
		/// </summary>
		protected ConcurrentQueue<DataToLog> _queued = new ConcurrentQueue<DataToLog>();
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="ThreadLogger"/> class.
		/// </summary>
		/// <param name="logWriter">The log writer.</param>
		public ThreadLogger(ILogWriterService logWriter)
		{
			if (logWriter == null)
			{
				throw new ArgumentNullException("logWriter");
			}

			_logWriter = logWriter;
			Start();
		}

		/// <summary>
		/// Launches this instance.
		/// </summary>
		public void Start()
		{
			Task.Run(() =>
			{
				MainLoop();
			});
		}

		/// <summary>
		/// Gets a value indicating whether this instance is alive.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
		/// </value>
		protected bool IsAlive
		{
			get
			{
				return _isAlive;
			}
		}

		/// <summary>
		/// Threads the loop.
		/// </summary>
		protected async void MainLoop()
		{
			while (IsAlive)
			{
				await Task.Delay(200);
				await FlushAsync();
			}
		}

		/// <summary>
		/// Flushes the asynchronous.
		/// </summary>
		/// <returns>return a task</returns>
		public async Task FlushAsync()
		{
			try
			{
				while (!_queued.IsEmpty)
				{
					DataToLog dataToLog;
					if (_queued.TryDequeue(out dataToLog))
					{
						await _logWriter.WriteLogAsync(dataToLog);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Adds the data to log.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="logLevel">level of the log</param>
		public void AddDataToLog(string data, LogLevel logLevel)
		{
			var dataToLog = new DataToLog(data, logLevel);
			_queued.Enqueue(dataToLog);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_isAlive = false;
			_logWriter.Dispose();
		}
	}
}
