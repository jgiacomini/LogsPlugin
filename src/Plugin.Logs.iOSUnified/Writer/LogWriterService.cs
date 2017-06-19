using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plugin.Logs.Model;

namespace Plugin.Logs.Writer
{
    public class LogWriterService : BaseLogWriterService
	{
		public LogWriterService(string fileName, string logDirectoryPath) :
			base(fileName, logDirectoryPath)
		{
		}

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="file">File.</param>
		protected override void DeleteFile(string file)
		{
			File.Delete(file);
		}

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <returns>The directories.</returns>
        /// <param name="directory">Directory.</param>
		protected override string[] GetDirectories(string directory)
		{
			return Directory.GetDirectories(directory);
		}

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <returns>The files.</returns>
        /// <param name="directory">Directory.</param>
		protected override string[] GetFiles(string directory)
		{
			return Directory.GetFiles(directory);
		}

        /// <summary>
        /// Writes the in file async.
        /// </summary>
        /// <returns>The in file async.</returns>
        /// <param name="logFilePath">Log file path.</param>
        /// <param name="dataToLog">Data to log.</param>
		protected override async Task WriteInFileAsync(string logFilePath, DataToLog dataToLog)
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
	}
}
