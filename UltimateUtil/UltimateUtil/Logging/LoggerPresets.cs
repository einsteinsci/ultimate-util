﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Logging
{
	public static class LoggerPresets
	{
		public enum Preset
		{
			Console,
			FileOnly,
			Debugger,
		}

		public static void Initialize(Preset preset = Preset.Console, string filePath = null,
			LogLevel minOutputLogging = LogLevel.Info, LogLevel minFileLogging = LogLevel.Debug)
		{
			Logger.Initialize(filePath, true, minOutputLogging, minFileLogging);

			switch (preset)
			{
				case Preset.Console:
					Logger.Logging += ConsoleLog;
					Logger.LoggingPart += ConsoleLogPart;
					break;
				case Preset.FileOnly:
					// already handled in Logger.
					break;
				case Preset.Debugger:
					Logger.Logging += DebuggerLog;
					Logger.LoggingPart += DebuggerLogPart;
					break;
				default:
					break;
			}
		}

		#region Console
		public static void ConsoleLog(object sender, LogEventArgs e)
		{
			ConsoleColor buf = Console.ForegroundColor;

			Console.ForegroundColor = GetLevelColor(e.Level);
			Console.WriteLine(e.Message);
			Console.ForegroundColor = buf;
		}

		public static void ConsoleLogPart(object sender, LogEventArgs e)
		{
			ConsoleColor buf = Console.ForegroundColor;

			Console.ForegroundColor = GetLevelColor(e.Level);
			Console.Write(e.Message);
			Console.ForegroundColor = buf;
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
		#endregion Console

		#region Debugger
		public static void DebuggerLog(object sender, LogEventArgs e)
		{
			if (e.Level.IsOneOf(LogLevel.Error, LogLevel.Fatal))
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
			if (e.Level.IsOneOf(LogLevel.Error, LogLevel.Fatal))
			{
				Debug.Fail(e.Message);
			}
			else
			{
				Debug.Write(e.Message);
			}
		}
		#endregion Debugger
	}
}
