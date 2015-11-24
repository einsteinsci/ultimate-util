using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateUtil.Fluid;
using UltimateUtil.UserInteraction;

namespace UltimateUtil.Logging
{
	/// <summary>
	/// Provides presets for initializing a <see cref="Logger"/> for common scenarios.
	/// </summary>
	public static class PresetsLogger
	{
		/// <summary>
		/// Type of presets available 
		/// </summary>
		public enum LoggerPresetType
		{
			/// <summary>
			/// Set up console logging
			/// </summary>
			Console,
			/// <summary>
			/// Set up file logging only
			/// </summary>
			FileOnly,
			/// <summary>
			/// Set up logging into the VS Debugger
			/// </summary>
			Debugger,
		}

		/// <summary>
		/// Initializes a <see cref="Logger"/> to a given <paramref name="preset"/> with various settings.
		/// </summary>
		/// <param name="preset">Determines which preset is set up</param>
		/// <param name="filePath">Path for file output. <c>null</c> indicates no file output.</param>
		/// <param name="minOutputLogging">Minimum output log level</param>
		/// <param name="minFileLogging">Minimum file output log level</param>
		public static void Initialize(LoggerPresetType preset = LoggerPresetType.Console, string filePath = null,
			LogLevel minOutputLogging = LogLevel.Info, LogLevel minFileLogging = LogLevel.Debug)
		{
			Logger.Initialize(filePath, true, minOutputLogging, minFileLogging);

			switch (preset)
			{
				case LoggerPresetType.Console:
					var versatile = new PresetVersatileConsoleIO(LogLevel.Interface.GetLevelColor());
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

		/// <summary>
		/// Converts a <see cref="LogLevel"/> into a <see cref="ConsoleColor"/>.
		/// </summary>
		/// <param name="level">Converted <see cref="LogLevel"/>.</param>
		/// <returns>The <see cref="ConsoleColor"/> most fitting for <paramref name="level"/>.</returns>
		public static ConsoleColor GetLevelColor(this LogLevel level)
		{
			switch (level)
			{
			case LogLevel.Verbose:
				return ConsoleColor.DarkGray;
			case LogLevel.Debug:
				return ConsoleColor.Gray;
			case LogLevel.Info:
				return ConsoleColor.White;
			case LogLevel.Interface:
				return ConsoleColor.Blue;
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
		
		/// <summary>
		/// Logs a line in the VS debugger.
		/// </summary>
		/// <param name="sender">Sending object (usually a <see cref="Logger"/>)</param>
		/// <param name="e"><see cref="LogEventArgs"/> containing message info</param>
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
		/// <summary>
		/// Logs a <see cref="string"/> into the VS debugger.
		/// </summary>
		/// <param name="sender">Sending object (usually a <see cref="Logger"/>)</param>
		/// <param name="e"><see cref="LogEventArgs"/> containing message info</param>
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
