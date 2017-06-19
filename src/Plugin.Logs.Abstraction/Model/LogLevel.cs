using System;
namespace Plugin.Logs.Model
{
	/// <summary>
	/// Log level
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// The trace level
		/// </summary>
		Trace = 0,
		/// <summary>
		/// The debug level
		/// </summary>
		Debug = 1,
		/// <summary>
		/// The information level
		/// </summary>
		Information = 2,
		/// <summary>
		/// The warning level
		/// </summary>
		Warning = 3,
		/// <summary>
		/// The error level
		/// </summary>
		Error = 4,
		/// <summary>
		/// The critical level
		/// </summary>
		Critical = 5,
		/// <summary>
		/// The none level
		/// </summary>
		None = 6
	}
}
