using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Logging
{
	public delegate void LogEvent(object sender, LogEventArgs e);

	public class LogEventArgs : EventArgs
	{
		public LogLevel Level
		{ get; private set; }

		public string Message
		{ get; private set; }

		public LogEventArgs(LogLevel level, string message) : base()
		{
			Level = level;
			Message = message;
		}
	}
}
