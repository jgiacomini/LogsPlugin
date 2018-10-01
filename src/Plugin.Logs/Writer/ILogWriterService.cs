using System;
using System.Threading.Tasks;
using Plugin.Logs.Model;

namespace Plugin.Logs.Writer
{
    /// <summary>
    /// Represent the class which will write the logs
    /// </summary>
    public interface ILogWriterService
    {
        /// <summary>
        /// Purges the old days.
        /// </summary>
        /// <param name="nbDaysToKeep">The nb days to keep.</param>
        /// <returns>A <see cref="Task"/></returns>
        Task PurgeOldDaysAsync(uint nbDaysToKeep);

        /// <summary>
        /// Writes the log asynchronous.
        /// </summary>
        /// <param name="dataToLog">The data to log.</param>
        /// <returns>return a task</returns>
        Task WriteLogAsync(DataToLog dataToLog);
    }
}
