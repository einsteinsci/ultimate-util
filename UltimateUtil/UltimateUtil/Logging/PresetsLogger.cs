using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateUtil.UserInteraction;

namespace UltimateUtil.Logging
{
	public static class PresetsLogger
	{
		public enum LoggerPresetType
		{
			Console,
			FileOnly,
			Debugger,
		}

		public static void Initialize(LoggerPresetType preset = LoggerPresetType.Console, string filePath = null,
			LogLevel minOutputLogging = LogLevel.Info, LogLevel minFileLogging = LogLevel.Debug)
		{
			Logger.Initialize(filePath, true, minOutputLogging, minFileLogging);

			switch (preset)
			{
				case LoggerPresetType.Console:
					PresetVersatileConsoleIO.Initialize(LogLevel.Interface.GetLevelColor());
					Logger.Logging += (s, e) => VersatileIO.WriteLine(e.Message, e.Level.GetLevelColor());
					Logger.LoggingPart += (s, e) => VersatileIO.Write(e.Message, e.Level.GetLevelColor());
					break;
				case LoggerPresetType.FileOnly:
					// already handled in Logger.
					break;
				case LoggerPresetType.Debugger:
					Logger.Logging += DebuggerLog;
					Logger.LoggingPart += DebuggerLogPart;
					break;
				default:
					break;
			}
		}

		public static ConsoleColor GetLevelColor(this LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Debug:
					return ConsoleColor.DarkGray;
				case LogLevel.Info:
					return ConsoleColor.Gray;
				case LogLevel.Interface:
					return ConsoleColor.White;
				case LogLevel.Success:
					return ConsoleColor.Green;
				case LogLevel.Warning:
					return ConsoleColor.Yellow;
				case LogLevel.Error:
					return ConsoleColor.Red;
				case LogLevel.Fatal:
					return ConsoleColor.DarkRed;
				case LogLevel.BlockAllLogging:
					return ConsoleColor.DarkMagenta;
				default:
					throw new ArgumentOutOfRangeException(nameof(level));
			}
		}
		
		public static void DebuggerLog(object sender, LogEventArgs e)
		{
			if (e.Level.IsAnyOf(LogLevel.Error, LogLevel.Fatal))
			{
				Debug.Fail(e.Message);
				Debug.WriteLine("");
			}
			else
			{
				Debug.WriteLine(e.Message);
			}
		}
		public static void DebuggerLogPart(object sender, LogEventArgs e)
		{
			if (e.Level.IsAnyOf(LogLevel.Error, LogLevel.Fatal))
			{
				Debug.Fail(e.Message);
			}
			else
			{
				Debug.Write(e.Message);
			}
		}
	}
}
