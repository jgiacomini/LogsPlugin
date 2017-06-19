using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Plugin.Logs.Model;

namespace Plugin.Logs.Writer
{
	public abstract class BaseLogWriterService : ILogWriterService
	{
		#region Fields

		/// <summary>
		/// The log directory path
		/// </summary>
		protected readonly string _logDirectoryPath;

		/// <summary>
		/// The file name
		/// </summary>
		protected readonly string _fileName;

		/// <summary>
		/// The application version
		/// </summary>
		protected readonly string _appVersion;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseLogWriterService"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="logDirectoryPath">The log directory path.</param>
		/// <param name="appVersion">The application version.</param>
		public BaseLogWriterService(
			string fileName,
			string logDirectoryPath,
			string appVersion)
		{
			_appVersion = appVersion;
			_fileName = fileName;
			_logDirectoryPath = logDirectoryPath;
		}

        /// <inheritdoc />
        public async Task WriteLogAsync(DataToLog dataToLog)
        {
            DateTime today = DateTime.Today;
            string logType = "log";
            var logFilePath = $"{today.ToString("yyyy-MM")}\\{_fileName}_{logType}_{today.ToString("yyyy-MM-dd")}.csv";

            await WriteInFileAsync(Path.Combine(_logDirectoryPath, logFilePath), dataToLog);

            if (dataToLog.Level == LogLevel.Critical || dataToLog.Level == LogLevel.Error)
            {
                logType = "error";
                logFilePath = $"{today.ToString("yyyy-MM")}\\{_fileName}_{logType}_{today.ToString("yyyy-MM-dd")}.csv";
                await WriteInFileAsync(Path.Combine(_logDirectoryPath, logFilePath), dataToLog);
            }
        }

        /// <summary>
        /// Writes the in file asynchronous.
        /// </summary>
        /// <param name="logFilePath">The log file path.</param>
        /// <param name="dataToLog">The data to log.</param>
        /// <returns></returns>
        protected abstract Task WriteInFileAsync(string logFilePath, DataToLog dataToLog);

        /// <summary>
        /// Generates the string to write.
        /// </summary>
        /// <param name="dataToLog">The data to log.</param>
        /// <returns>return a string</returns>
        protected string GenerateStringToWrite(DataToLog dataToLog)
        {
            string level = dataToLog.Level.ToString().ToUpperInvariant().PadRight(11);

            var dateStr = dataToLog.When.ToString("yyyy-MM-dd HH:mm:ss");
            string toWrite = $"{dateStr};{_appVersion};{level};\"{dataToLog.Data}\"";
            return toWrite;
        }

        /// <inheritdoc />
        public Task PurgeOldDaysAsync(uint nbDaysToKeep)
		{
			return Task.Run(() =>
			{
				try
				{
					InternalPurgeAllDays(nbDaysToKeep);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex);
				}
			});
		}

        /// <summary>
        /// Internals the purge all days.
        /// </summary>
        /// <param name="nbDaysToKeep">Nb days to keep.</param>
		void InternalPurgeAllDays(uint nbDaysToKeep)
		{
			var directories = GetDirectories(_logDirectoryPath);
			var minDate = DateTime.Today.AddDays(-1 * nbDaysToKeep);
			foreach (var directory in directories)
			{
				PurgeDirectory(directory, minDate);
			}
		}

        /// <summary>
        /// Purges the directory.
        /// </summary>
        /// <param name="directory">Directory.</param>
        /// <param name="minDate">Minimum date.</param>
		void PurgeDirectory(string directory, DateTime minDate)
		{
			var files = GetFiles(directory);

			foreach (var file in files)
			{
				var date = Right(file.Replace(".csv", ""), 10);
				DateTime currentDate;
				if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out currentDate))
				{
					if (currentDate < minDate)
					{
						try
						{
							DeleteFile(file);
						}
						catch (Exception ex)
						{
							Debug.WriteLine(ex);
						}
					}
				}
			}
		}

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <returns>The directories.</returns>
        /// <param name="directory">Directory.</param>
		protected abstract string[] GetDirectories(string directory);

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <returns>The files.</returns>
        /// <param name="directory">Directory.</param>
		protected abstract string[] GetFiles(string directory);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="file">File.</param>
		protected abstract void DeleteFile(string file);

        /// <summary>
        /// Right the specified sValue and iMaxLength.
        /// </summary>
        /// <returns>The right.</returns>
        /// <param name="sValue">S value.</param>
        /// <param name="iMaxLength">I max length.</param>
		public string Right(string sValue, int iMaxLength)
		{
			//Check if the value is valid
			if (string.IsNullOrEmpty(sValue))
			{
				//Set valid empty string as string could be null
				sValue = string.Empty;
			}
			else if (sValue.Length > iMaxLength)
			{
				//Make the string no longer than the max length
				sValue = sValue.Substring(sValue.Length - iMaxLength, iMaxLength);
			}

			//Return the string
			return sValue;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			// Nothing to release
		}
	}
}
