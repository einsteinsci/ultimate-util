using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Logging
{
	public enum LogLevel
	{
		Debug = 0,
		Info,
		Success,
		Warning,
		Error,
		Fatal,
		Interface,
		BlockAllLogging
	}

	public class Logger : IDisposable
	{
		public static Logger Instance
		{ get; private set; }

		public string OutputFile
		{ get; private set; }

		public bool IncludeTimeStamps
		{ get; private set; }

		public LogLevel MinLogging
		{ get; private set; }

		public LogLevel MinFileLogging
		{ get; private set; }

		private StreamWriter _fileSteam;

		private bool _disposed = false;

		public event LogEvent OnLog;
		public event LogEvent OnLogPart;

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
		
		public static void Initialize(string fileOutput = null, bool doTimeStamps = true,
			LogLevel minLogLevel = LogLevel.Info, LogLevel minFileLevel = LogLevel.Debug)
		{
			Instance = new Logger(fileOutput, doTimeStamps, minLogLevel, minFileLevel);
		}

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

		public void LogLine(LogLevel level, string text, params object[] formatArgs)
		{
			if (level == LogLevel.BlockAllLogging)
			{
				throw new ArgumentException("Cannot use LogLevel {0} for actual logging."
					.Fmt(nameof(LogLevel.BlockAllLogging)));
			}

			string line = text.Fmt(formatArgs);

			if (level != LogLevel.Interface)
			{
				line = _getTimeStamp() + "[" + level.ToString().ToUpper() + "] " + line;
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

		private string _getTimeStamp()
		{
			if (!IncludeTimeStamps)
			{
				return "";
			}

			return "[" + DateTime.Now.ToString("hh:mm:ss") + "] ";
		}

		#region level log methods
		public void Debug(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Debug, text, formatArgs);
		}
		public static void LogDebug(string text, params object[] formatArgs)
		{
			Instance.Debug(text, formatArgs);
		}

		public void Info(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Info, text, formatArgs);
		}
		public static void LogInfo(string text, params object[] formatArgs)
		{
			Instance.Info(text, formatArgs);
		}

		public void Interface(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Interface, text, formatArgs);
		}
		public static void LogInterface(string text, params object[] formatArgs)
		{
			Instance.Interface(text, formatArgs);
		}

		public void Success(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Success, text, formatArgs);
		}
		public static void LogSuccess(string text, params object[] formatArgs)
		{
			Instance.Success(text, formatArgs);
		}

		public void Warning(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Warning, text, formatArgs);
		}
		public static void LogWarning(string text, params object[] formatArgs)
		{
			Instance.Warning(text, formatArgs);
		}

		public void Error(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Error, text, formatArgs);
		}
		public static void LogError(string text, params object[] formatArgs)
		{
			Instance.Error(text, formatArgs);
		}

		public void Fatal(string text, params object[] formatArgs)
		{
			LogLine(LogLevel.Fatal, text, formatArgs);
		}
		public static void LogFatal(string text, params object[] formatArgs)
		{
			Instance.Fatal(text, formatArgs);
		}
		#endregion level log methods

		public void Error(Exception e)
		{
			Error("Exception caught! {0}: {1}", e.GetType().Name, e.ToString());
		}
		public void LogError(Exception e)
		{
			Instance.Error(e);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
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
