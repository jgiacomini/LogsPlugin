using Plugin.Logs.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Plugin.Logs.Writer
{
    /// <summary>
    /// Base log writer
    /// </summary>
    /// <seealso cref="Plugin.Logs.Writer.ILogWriterService" />
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

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseLogWriterService"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="logDirectoryPath">The log directory path.</param>
		public BaseLogWriterService(
			string fileName,
			string logDirectoryPath)
		{
			_fileName = fileName;
			_logDirectoryPath = logDirectoryPath;
		}

        /// <inheritdoc />
        public async Task WriteLogAsync(DataToLog dataToLog)
        {
            DateTime today = DateTime.Today;
            string logType = "log";

            var logFilePath = Path.Combine($"{today.ToString("yyyy-MM")}",$"{_fileName}_{logType}_{today.ToString("yyyy-MM-dd")}.csv");

            var filePath = Path.Combine(_logDirectoryPath, logFilePath);
            await WriteInFileAsync(filePath, dataToLog);

            if (dataToLog.Level == LogLevel.Critical || dataToLog.Level == LogLevel.Error)
            {
                logType = "error";
                logFilePath = Path.Combine($"{today.ToString("yyyy-MM")}",$"{_fileName}_{logType}_{today.ToString("yyyy-MM-dd")}.csv");
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
            string toWrite = $"{dateStr};{level};\"{dataToLog.Data}\"";
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

            Debug.WriteLine("Purge done");
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
        /// Rights the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>return the right of the string</returns>
        public string Right(string value, int maxLength)
		{
			//Check if the value is valid
			if (string.IsNullOrEmpty(value))
			{
                //Set valid empty string as string could be null
                value = string.Empty;
			}
			else if (value.Length > maxLength)
			{
                //Make the string no longer than the max length
                value = value.Substring(value.Length - maxLength, maxLength);
			}

			//Return the string
			return value;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			// Nothing to release
		}
	}
}
