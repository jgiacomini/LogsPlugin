using Plugin.Logs.Model;
using Plugin.Logs.Writer;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugin.Logs
{
    /// <summary>
    /// Manage the main loop to log data
    /// </summary>
    internal class BackgroundWorker
    {
        #region Fields

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile BackgroundWorker _instance;

        /// <summary>
        /// The is alive
        /// </summary>
        private readonly bool _isAlive = true;

        /// <summary>
        /// The _queued
        /// </summary>
        private ConcurrentQueue<DataToLog> _queued = new ConcurrentQueue<DataToLog>();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        private BackgroundWorker()
        {
            Task.Run(() =>
            {
                MainLoop();
            });
        }

        public static BackgroundWorker Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new BackgroundWorker();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Launches this instance.
        /// </summary>
        private void Start()
        {
            Task.Run(() =>
            {
                MainLoop();
            });
        }

        /// <summary>
        /// Threads the loop.
        /// </summary>
        private async void MainLoop()
        {
            while (true)
            {
                await Task.Delay(200);
                await FlushAsync();
            }
        }

        /// <summary>
        /// Flushes the asynchronous.
        /// </summary>
        /// <returns>return a task</returns>
        internal async Task FlushAsync()
        {
            while (!_queued.IsEmpty)
            {
                if (_queued.TryDequeue(out DataToLog dataToLog))
                {
                    try
                    {
                        await dataToLog.Listener.WriteLogAsync(dataToLog);
                    }
                    catch (Exception ex)
                    {
                        _queued.Enqueue(dataToLog);
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the data to log.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="logListener">The listener.</param>
        internal void AddDataToLog(string data, LogLevel logLevel, ILogListener logListener)
        {
            var dataToLog = new DataToLog(data, logLevel, logListener);
            _queued.Enqueue(dataToLog);
        }
    }
}
