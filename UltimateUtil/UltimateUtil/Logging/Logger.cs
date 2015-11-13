using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Logging
{
	/// <summary>
	/// Various levels of log importance. Often determines output color and visibility.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Lowest <see cref="LogLevel"/>. Used for complex debugging operations.
		/// Production level code should default to hide this level.
		/// </summary>
		Debug = 0,
		/// <summary>
		/// Denotes low-level information with minor importance and neutral tone.
		/// </summary>
		Info,
		/// <summary>
		/// Denotes information about code that has completed successfully.
		/// </summary>
		Success,
		/// <summary>
		/// Denotes information about code that may be a problem, but is not causing
		/// any current detriment to the program.
		/// </summary>
		Warning,
		/// <summary>
		/// Denotes information about code that has failed to work correctly or for
		/// logging an <see cref="Exception"/> that has been caught and dealt with.
		/// </summary>
		Error,
		/// <summary>
		/// Denotes information about code that has failed severely, and requires one or
		/// more parts of the program to be restarted or reset before the program can
		/// function correctly.
		/// </summary>
		Fatal,
		/// <summary>
		/// Denotes a prompt asking the user for information. Logging that uses this
		/// <see cref="LogLevel"/> excludes any timestamp or level tag.
		/// </summary>
		Interface,
		/// <summary>
		/// This is not used in actual logging. It is only used for blocking all of a type of
		/// log output for a <see cref="Logger"/> instance, via <see cref="Logger.MinLogging"/> 
		/// and <see cref="Logger.MinFileLogging"/>.
		/// </summary>
		BlockAllLogging
	}

	/// <summary>
	/// Logging class for various logging functions. Has a singleton instance included, which static
	/// logging methods use. Also can log to file as well. This class can be extended for further log
	/// customization.
	/// </summary>
	public class Logger : IDisposable
	{
		/// <summary>
		/// Singleton instance, used by static methods.
		/// </summary>
		public static Logger Instance
		{ get; private set; }

		/// <summary>
		/// Path of output file where log is mirrored. <c>null</c> if no file output.
		/// </summary>
		public string OutputFile
		{ get; private set; }

		/// <summary>
		/// Whether to include time stamps automatically (<c>"[HH:MM:SS] "</c>) before
		/// every log message.
		/// </summary>
		public bool IncludeTimeStamps
		{ get; private set; }

		/// <summary>
		/// Minimum log level for a log message to have if it is to be output into the main
		/// log events <see cref="OnLog"/> or <see cref="OnLogPart"/>.
		/// </summary>
		public LogLevel MinLogging
		{ get; private set; }

		/// <summary>
		/// Minimum log level for a log message to have if it is to be output into the log file.
		/// Has no meaning if <see cref="OutputFile"/> is <c>null</c>.
		/// </summary>
		public LogLevel MinFileLogging
		{ get; private set; }

		private StreamWriter _fileSteam;

		private bool _disposed = false;

		/// <summary>
		/// Subscribe to this event to add an output for logging that 
		/// calls <see cref="LogLine(LogLevel, string, object[])"/>.
		/// </summary>
		public event LogEvent OnLog;
		/// <summary>
		/// Subscribe to this event to add an output for logging that
		/// calls <see cref="LogPart(LogLevel, string, object[])"/>.
		/// </summary>
		public event LogEvent OnLogPart;

		/// <summary>
		/// Subscribe to this event to add an output for logging that calls
		/// static logging methods.
		/// </summary>
		public static event LogEvent Logging
		{
			add
			{
				Instance.OnLog += value;
			}
			remove
			{
				Instance.OnLog -= value;
			}
		}
		/// <summary>
		/// Subscribe to this event to ad an output for logging that calls
		/// <see cref="Instance"/>.<see cref="LogPart(LogLevel, string, object[])"/>.
		/// </summary>
		public static event LogEvent LoggingPart
		{
			add
			{
				Instance.OnLogPart += value;
			}
			remove
			{
				Instance.OnLogPart -= value;
			}
		}
		
		/// <summary>
		/// Initializes the singleton <see cref="Instance"/> for static logging
		/// </summary>
		/// <param name="fileOutput">File path to mirror log output. <c>null</c> means no file output.</param>
		/// <param name="doTimeStamps">Whether to include time stamps on log messages.</param>
		/// <param name="minLogLevel">Minimum log level for event output.</param>
		/// <param name="minFileLevel">Minimum log level for file output.</param>
		public static void Initialize(string fileOutput = null, bool doTimeStamps = true,
			LogLevel minLogLevel = LogLevel.Info, LogLevel minFileLevel = LogLevel.Debug)
		{
			Instance = new Logger(fileOutput, doTimeStamps, minLogLevel, minFileLevel);
		}

		/// <summary>
		/// Instantiates a new instance of <see cref="Logger"/>.
		/// </summary>
		/// <param name="fileOutput">File path to mirror log output. <c>null</c> means no file output.</param>
		/// <param name="doTimeStamps">Whether to include time stamps on log messages.</param>
		/// <param name="minLogLevel">Minimum log level for event output.</param>
		/// <param name="minFileLevel">Minimum log level for file output.</param>
		public Logger(string fileOutput, bool doTimeStamps, LogLevel minLogLevel, LogLevel minFileLevel)
		{
			if (fileOutput != null)
			{
				_fileSteam = new StreamWriter(fileOutput, true, Encoding.UTF8);
			}

			IncludeTimeStamps = doTimeStamps;
			MinLogging = minLogLevel;
			MinFileLogging = minFileLevel;
		}
		
		~Logger()
		{
			Dispose(false);
		}

		/// <summary>
		/// Logs a line of text through the log events, with a given Log level, text, 
		/// and string format parameters.
		/// </summary>
		/// <param name="level"><see cref="LogLevel"/> of logged text</param>
		/// <param name="text">Line of text to log</param>
		/// <param name="formatArgs">Arguments used for <see cref="string.Format(string, object[])"/></param>
		public virtual void LogLine(LogLevel level, string text, params object[] formatArgs)
		{
			if (level == LogLevel.BlockAllLogging)
			{
				throw new ArgumentException("Cannot use LogLevel {0} for actual logging."
					.Fmt(nameof(LogLevel.BlockAllLogging)));
			}

			string line = text.Fmt(formatArgs);

			if (level != LogLevel.Interface)
			{
				line = getTimeStamp() + "[" + level.ToString().ToUpper() + "] " + line;
			}

			if (OnLog != null && level >= MinLogging)
			{
				OnLog(this, new LogEventArgs(level, line));
			}

			if (_fileSteam != null && level >= MinFileLogging)
			{
				_fileSteam.WriteLine(line);
			}
		}

		/// <summary>
		/// Logs a piece of text (without a newline) through the log events, with a given Log level, text, 
		/// and string format parameters.
		/// </summary>
		/// <param name="level"><see cref="LogLevel"/> of logged text</param>
		/// <param name="text">Line of text to log</param>
		/// <param name="formatArgs">Arguments used for <see cref="string.Format(string, object[])"/></param>
		public void LogPart(LogLevel level, string text, params object[] formatArgs)
		{
			if (level == LogLevel.BlockAllLogging)
			{
				throw new ArgumentException("Cannot use LogLevel {0} for actual logging."
					.Fmt(nameof(LogLevel.BlockAllLogging)));
			}

			string part = text.Fmt(formatArgs);

			if (OnLogPart != null && level >= MinLogging)
			{
				OnLogPart(this, new LogEventArgs(level, part));
			}

			if (_fileSteam != null && level >= MinFileLogging)
			{
				_fileSteam.Write(part);
			}
		}
		
		/// <summary>
		/// Gets the timestamp of <see cref="DateTime.Now"/>, prepended to
		/// log messages if <see cref="IncludeTimeStamps"/> is <c>true.</c>
		/// </summary>
		/// <returns>The current timestamp in the format <c>"[{0:hh:mm:ss}] "</c></returns>
		protected virtual string getTimeStamp()
		{
			if (!IncludeTimeStamps)
			{
				return "";
			}

			return "[" + DateTime.Now.ToString("hh:mm:ss") + "] ";
		}

		#region level log methods
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Debug"/>.
		/// </summary>
		/// <param name="text">Debug text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public void Debug(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Debug, text, formatArgs);
		}
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Debug"/> through <see cref="Instance"/>.
		/// </summary>
		/// <param name="text">Debug text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public static void LogDebug(string text, params object[] formatArgs)
		{
			Instance.Debug(text, formatArgs);
		}

		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Info"/>.
		/// </summary>
		/// <param name="text">Info text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public void Info(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Info, text, formatArgs);
		}
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Info"/> through <see cref="Instance"/>.
		/// </summary>
		/// <param name="text">Info text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public static void LogInfo(string text, params object[] formatArgs)
		{
			Instance.Info(text, formatArgs);
		}

		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Interface"/>.
		/// </summary>
		/// <param name="text">Prompt text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public void Interface(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Interface, text, formatArgs);
		}
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Interface"/> through <see cref="Instance"/>.
		/// </summary>
		/// <param name="text">Prompt text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public static void LogInterface(string text, params object[] formatArgs)
		{
			Instance.Interface(text, formatArgs);
		}

		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Success"/>.
		/// </summary>
		/// <param name="text">Success text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public void Success(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Success, text, formatArgs);
		}
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Success"/> through <see cref="Instance"/>.
		/// </summary>
		/// <param name="text">Success text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public static void LogSuccess(string text, params object[] formatArgs)
		{
			Instance.Success(text, formatArgs);
		}

		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Warning"/>.
		/// </summary>
		/// <param name="text">Warning text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public void Warning(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Warning, text, formatArgs);
		}
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Warning"/> through <see cref="Instance"/>.
		/// </summary>
		/// <param name="text">Warning text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public static void LogWarning(string text, params object[] formatArgs)
		{
			Instance.Warning(text, formatArgs);
		}

		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Error"/>.
		/// </summary>
		/// <param name="text">Error text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public void Error(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Error, text, formatArgs);
		}
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Error"/> through <see cref="Instance"/>.
		/// </summary>
		/// <param name="text">Error text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public static void LogError(string text, params object[] formatArgs)
		{
			Instance.Error(text, formatArgs);
		}

		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Fatal"/>.
		/// </summary>
		/// <param name="text">Fatal error text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public void Fatal(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Fatal, text, formatArgs);
		}
		/// <summary>
		/// Logs a line of text at <see cref="LogLevel.Fatal"/> through <see cref="Instance"/>.
		/// </summary>
		/// <param name="text">Fatal error text to log</param>
		/// <param name="formatArgs">Format parameters for <see cref="string.Format(string, object[])"/></param>
		public static void LogFatal(string text, params object[] formatArgs)
		{
			Instance.Fatal(text, formatArgs);
		}
		#endregion level log methods

		/// <summary>
		/// Logs an <see cref="Exception"/>, its message, and its stacktrace using
		/// <see cref="Error(string, object[])"/>.
		/// </summary>
		/// <param name="e"><see cref="Exception"/> to log</param>
		public virtual void Error(Exception e)
		{
			Error("Exception! {0}: {1}", e.GetType().Name, e.ToString());
		}
		/// <summary>
		/// Logs an <see cref="Exception"/>, its message, and its stacktrace through 
		/// <see cref="Instance"/>.
		/// </summary>
		/// <param name="e"><see cref="Exception"/> to log</param>
		public void LogError(Exception e)
		{
			Instance.Error(e);
		}

		/// <summary>
		/// Disposes the current <see cref="IDisposable"/> recursively.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Disposes the current <see cref="IDisposable"/>, with the option to do so recursively.
		/// </summary>
		/// <param name="recursive">Whether to dispose recursively</param>
		public void Dispose(bool recursive)
		{
			if (_disposed)
				return;

			if (recursive)
			{
				_fileSteam.Close();
				_fileSteam.Dispose();
			}
			_disposed = true;
		}
	}
}
