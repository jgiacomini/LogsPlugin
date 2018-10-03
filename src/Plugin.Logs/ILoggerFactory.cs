namespace Plugin.Logs
{
    public interface ILoggerFactory
    {
        /// <summary>
        /// Gets or sets the the directory path of logs files
        /// </summary>
        /// <value>
        /// The nb month to keep.
        /// </value>
        string LogDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the nb month to keep.
        /// </summary>
        /// <value>
        /// The nb month to keep.
        /// </value>
        uint NbDaysToKeep { get; }

        /// <summary>
        /// Get a new logger
        /// </summary>
        /// <param name="name">logger name</param>
        /// <returns>return a new logger</returns>
        ILogService GetLogger(string name);
    }
}