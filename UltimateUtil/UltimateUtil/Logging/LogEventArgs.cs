using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Logging
{
	/// <summary>
	/// Delegate for sending log info
	/// </summary>
	/// <param name="sender">Object that fires the event, usually a logger.</param>
	/// <param name="e"><see cref="EventArgs"/> for storing event arguments</param>
	public delegate void LogEvent(object sender, LogEventArgs e);

	/// <summary>
	/// <see cref="EventArgs"/> class for log events
	/// </summary>
	public class LogEventArgs : EventArgs
	{
		/// <summary>
		/// Level of which the log is. Usually determines color and visibility.
		/// </summary>
		public LogLevel Level
		{ get; private set; }

		/// <summary>
		/// Message to be logged.
		/// </summary>
		public string Message
		{ get; private set; }

		/// <summary>
		/// Creates a new instance of <see cref="LogEventArgs"/>
		/// </summary>
		/// <param name="level">Level of log</param>
		/// <param name="message">Message to be logged</param>
		public LogEventArgs(LogLevel level, string message) : base()
		{
			Level = level;
			Message = message;
		}
	}
}
