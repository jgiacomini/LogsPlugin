using Plugin.Logs.Writer;
using Windows.Storage;

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
        public LogService(string fileName, uint nbDaysToKeep = 60)
            : base(new LogWriterService(fileName, ApplicationData.Current.TemporaryFolder.Path), nbDaysToKeep)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="logDirectoryPath">The log directory path.</param>
        public LogService(string fileName, string logDirectoryPath, uint nbDaysToKeep = 60)
            : base(new LogWriterService(fileName, logDirectoryPath), nbDaysToKeep)
        {
        }
    }
}
