using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plugin.Logs.Model;

namespace Plugin.Logs.Writer
{
    public class LogWriterService : ILogWriterService
    {
        #region Fields

        /// <summary>
        /// The log directory path
        /// </summary>
        private readonly string _logDirectoryPath;

        /// <summary>
        /// The file name
        /// </summary>
        private readonly string _fileName;

        #endregion

        public LogWriterService(string fileName, string logDirectoryPath)
        {
            _fileName = fileName;
            _logDirectoryPath = logDirectoryPath;
        }

        /// <summary>
        /// Writes the in file async.
        /// </summary>
        /// <returns>The in file async.</returns>
        /// <param name="logFilePath">Log file path.</param>
        /// <param name="dataToLog">Data to log.</param>
        private async Task WriteInFileAsync(string logFilePath, DataToLog dataToLog)
        {
            var directory = new FileInfo(logFilePath).Directory;

            if (!directory.Exists)
            {
                directory.Create();
            }

            using (FileStream fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    await sw.WriteLineAsync(GenerateStringToWrite(dataToLog));
                }
            }
        }

        /// <inheritdoc />
        public async Task WriteLogAsync(DataToLog dataToLog)
        {
            DateTime today = DateTime.Today;
            string logType = "log";

            var logFilePath = Path.Combine($"{today.ToString("yyyy-MM")}", $"{_fileName}_{logType}_{today.ToString("yyyy-MM-dd")}.csv");

            var filePath = Path.Combine(_logDirectoryPath, logFilePath);
            await WriteInFileAsync(filePath, dataToLog);

            if (dataToLog.Level == LogLevel.Critical || dataToLog.Level == LogLevel.Error)
            {
                logType = "error";
                logFilePath = Path.Combine($"{today.ToString("yyyy-MM")}", $"{_fileName}_{logType}_{today.ToString("yyyy-MM-dd")}.csv");
                await WriteInFileAsync(Path.Combine(_logDirectoryPath, logFilePath), dataToLog);
            }
        }

        /// <summary>
        /// Generates the string to write.
        /// </summary>
        /// <param name="dataToLog">The data to log.</param>
        /// <returns>return a string</returns>
        private string GenerateStringToWrite(DataToLog dataToLog)
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
        private void InternalPurgeAllDays(uint nbDaysToKeep)
        {
            var directories = Directory.GetDirectories(_logDirectoryPath);
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
        private void PurgeDirectory(string directory, DateTime minDate)
        {
            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                var date = Right(file.Replace(".csv", string.Empty), 10);
                if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime currentDate))
                {
                    if (currentDate < minDate)
                    {
                        try
                        {
                            File.Delete(file);
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
        /// Rights the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>return the right of the string</returns>
        private string Right(string value, int maxLength)
        {
            // Check if the value is valid
            if (string.IsNullOrEmpty(value))
            {
                // Set valid empty string as string could be null
                value = string.Empty;
            }
            else if (value.Length > maxLength)
            {
                // Make the string no longer than the max length
                value = value.Substring(value.Length - maxLength, maxLength);
            }

            // Return the string
            return value;
        }
    }
}
