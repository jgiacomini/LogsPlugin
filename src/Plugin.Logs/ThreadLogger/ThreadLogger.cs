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
    /// <seealso cref="System.IDisposable" />
    internal class ThreadLogger : IDisposable
    {
        #region Fields

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile ThreadLogger _instance;

        /// <summary>
        /// The is alive
        /// </summary>
        private bool _isAlive = true;

        /// <summary>
        /// The _queued
        /// </summary>
        private ConcurrentQueue<DataToLog> _queued = new ConcurrentQueue<DataToLog>();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadLogger"/> class.
        /// </summary>
        private ThreadLogger()
        {
            Start();
        }

        public static ThreadLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ThreadLogger();
                        }
                    }
                }

                return _instance;
            }
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
            while (!_queued.IsEmpty)
            {
                if (_queued.TryDequeue(out DataToLog dataToLog))
                {
                    try
                    {
                        await dataToLog.LogWritterService.WriteLogAsync(dataToLog);
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
        /// <param name="logWritterService">The log writter service.</param>
        public void AddDataToLog(string data, LogLevel logLevel, ILogWriterService logWritterService)
        {
            var dataToLog = new DataToLog(data, logLevel, logWritterService);
            _queued.Enqueue(dataToLog);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            FlushAsync().Wait();

            _isAlive = false;
        }
    }
}
