using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateUtil.Logging;
using UltimateUtil.Universal;

namespace UltimateUtil.UserInteraction
{
	/// <summary>
	/// Type of interaction for <see cref="VersatileIO"/>. Mostly useless.
	/// </summary>
	public enum InteractionType
	{
		/// <summary>
		/// Interaction that outputs a <see cref="string"/> of text
		/// </summary>
		LogPart,
		/// <summary>
		/// Interaction that outputs a <see cref="string"/> with a newline following it
		/// </summary>
		LogLine,
		/// <summary>
		/// Interaction that gets a <see cref="string"/> from the user
		/// </summary>
		InputString,
		/// <summary>
		/// Interaction that gets a <see cref="double"/> from the user
		/// </summary>
		InputNumber,
		/// <summary>
		/// Interaction that gets a selection from a list of choices displayed to the user
		/// </summary>
		Selection,
		/// <summary>
		/// Interaction that gets a selection from a list of choices displayed to the user,
		/// with the option to skip (returning <c>null</c>)
		/// </summary>
		OptionalSelection,
	}

	/// <summary>
	/// Delegate for sending a line of information to the user. The newline comes after the text.
	/// </summary>
	/// <param name="text">Line of text to send</param>
	/// <param name="color">Color of line. Ignore if inapplicable.</param>
	public delegate void SendLog(string text, ConsoleColor color);
	/// <summary>
	/// Delegate for sending a string of text to the user.
	/// </summary>
	/// <param name="text"><see cref="string"/> to send</param>
	/// <param name="color">Color of line, <c>null</c> indicates to use the color from previous output.</param>
	public delegate void SendLogPart(string text, ConsoleColor? color);

	/// <summary>
	/// Delegate for retrieving a <see cref="string"/> from the user.
	/// </summary>
	/// <param name="prompt">Text to prompt the user.</param>
	/// <returns>Resulting <see cref="string"/> from the user</returns>
	public delegate string GetString(string prompt);
	/// <summary>
	/// Delegate for retrieving a number from the user.
	/// </summary>
	/// <param name="prompt">Text to prompt the user.</param>
	/// <returns>Resulting <see cref="double"/> from the user</returns>
	public delegate double GetNumber(string prompt);

	/// <summary>
	/// Delegate for retrieving a selection from a list of options from the user.
	/// </summary>
	/// <param name="prompt">
	/// Text to display to the user once all the choices are listed.
	/// </param>
	/// <param name="options">
	/// Dictionary of items to select from. Keys denote shortcut codes for the items, while values 
	/// are what is listed when getting the selection.
	/// </param>
	/// <returns>The key for one of the items from <paramref name="options"/>, <c>null</c> if ignored.</returns>
	public delegate string GetSelection(string prompt, IDictionary<string, object> options);

	/// <summary>
	/// Contains methods for interacting with the user dynamically and simply, while remaining open
	/// to the programmer. Designed to be used by class libraries.
	/// </summary>
	public static class VersatileIO
	{
		/// <summary>
		/// Set the values to set the default colors of VersatileIO logging by level.
		/// Remove a key to disable logging for that level.
		/// </summary>
		public static Dictionary<LogLevel, ConsoleColor> LevelColors
		{ get; private set; }

		/// <summary>
		/// Minimum level at which to output by level. Has no effect on output by color.
		/// Levels below this are not written.
		/// </summary>
		public static LogLevel MinLogLevel
		{ get; set; }

		/// <summary>
		/// Subscribe to allow full-line output. Be sure the newline comes after
		/// the message, not before.
		/// </summary>
		public static SendLog OnLogLine
		{ get; set; }
		/// <summary>
		/// Subscribe to allow partial-line (and complex) output. A null color
		/// parameter implies to use the color of the last output.
		/// </summary>
		public static SendLogPart OnLogPart
		{ get; set; }

		/// <summary>
		/// Set this to determine how string input is retrieved.
		/// </summary>
		public static GetString OnGetString
		{ get; set; }
		/// <summary>
		/// Set this to determine how numeric input is retrieved.
		/// </summary>
		public static GetNumber OnGetNumber
		{ get; set; }

		/// <summary>
		/// Set this to determine how enumerated selective input is retrieved.
		/// </summary>
		public static GetSelection OnGetSelection
		{ get; set; }
		/// <summary>
		/// Set this to determine how enumerated selective input, with the option to 
		/// skip, is retrieved. Return null if the user skips.
		/// </summary>
		public static GetSelection OnGetIgnorableSelection
		{ get; set; }

		/// <summary>
		/// The current handler if a handler is used. <c>null</c> if manual subscription
		/// is done. Set with <see cref="SetHandler(VersatileHandlerBase, bool)"/>.
		/// </summary>
		public static VersatileHandlerBase CurrentHandler
		{ get; private set; }

		/// <summary>
		/// Initializes output by level. Not necessary for output by color and input methods.
		/// </summary>
		/// <param name="alternateColors">Alternate color scheme to use instead of default</param>
		/// <remarks>
		/// Default color scheme:
		/// <c>
		/// new Dictionary&lt;LogLevel, ConsoleColor&gt;() {
		/// { LogLevel.Verbose, ConsoleColor.DarkGray },
		///	{ LogLevel.Debug, ConsoleColor.Gray },
		///	{ LogLevel.Info, ConsoleColor.White },
		///	{ LogLevel.Success, ConsoleColor.Green },
		///	{ LogLevel.Warning, ConsoleColor.Yellow },
		///	{ LogLevel.Interface, ConsoleColor.Blue },
		///	{ LogLevel.Error, ConsoleColor.Red },
		///	{ LogLevel.Fatal, ConsoleColor.DarkRed } }
		/// </c>
		/// </remarks>
		public static void InitializeLevels(Dictionary<LogLevel, ConsoleColor> alternateColors = null)
		{
			LevelColors = alternateColors ?? new Dictionary<LogLevel, ConsoleColor>()
			{
				{ LogLevel.Verbose, ConsoleColor.DarkGray },
				{ LogLevel.Debug, ConsoleColor.Gray },
				{ LogLevel.Info, ConsoleColor.White },
				{ LogLevel.Success, ConsoleColor.Green },
				{ LogLevel.Warning, ConsoleColor.Yellow },
				{ LogLevel.Interface, ConsoleColor.Blue },
				{ LogLevel.Error, ConsoleColor.Red },
				{ LogLevel.Fatal, ConsoleColor.DarkRed }
			};

			MinLogLevel = LogLevel.Info;
		}

		/// <summary>
		/// Sets the <see cref="CurrentHandler"/> by initializing it and pointing
		/// <see cref="VersatileIO"/> to its methods.
		/// </summary>
		/// <param name="handler">Handler to subscribe</param>
		/// <param name="message">Whether to display a message when initialization is complete.</param>
		public static void SetHandler(VersatileHandlerBase handler, bool message = true)
		{
			handler.InitializeIO();

			if (message)
			{
				Debug("VersatileIO handler set.");
			}
		}
		/// <summary>
		/// Sets the <see cref="CurrentHandler"/> by instantiating and initializing
		/// it, and pointing <see cref="VersatileIO"/> to its methods.
		/// </summary>
		/// <param name="handlerType">Type of handler to subscribe</param>
		/// <param name="message">Whether to display a message when initialization is complete.</param>
		public static void SetHandler(Type handlerType, bool message = true)
		{
			if (!handlerType.InheritsFrom<VersatileHandlerBase>())
			{
				throw new ArgumentException("{0} must inherit from {1}.".Fmt(
					nameof(handlerType), nameof(VersatileHandlerBase)));
			}

			try
			{
				object obj = Activator.CreateInstance(handlerType);
				VersatileHandlerBase h = obj as VersatileHandlerBase;
				SetHandler(h, message);
			}
			catch (MissingMethodException)
			{
				throw new ArgumentException("Handler does not have a parameterless constructor.");
			}
		}
		/// <summary>
		/// Sets the <see cref="CurrentHandler"/> by instantiating and initializing
		/// it, and pointing <see cref="VersatileIO"/> to its methods.
		/// </summary>
		/// <typeparam name="THandler">Type of handler to subscribe</typeparam>
		/// <param name="message">Whether to display a message when initialization is complete</param>
		public static void SetHandler<THandler>(bool message = true)
		{
			SetHandler(typeof(THandler));
		}

		/// <summary>
		/// Writes a line of text using the output event <see cref="OnLogLine"/>.
		/// </summary>
		/// <param name="text">Text to output, excluding the trailing newline</param>
		/// <param name="color">Color of output text</param>
		public static void WriteLine(string text = "", ConsoleColor color = ConsoleColor.White)
		{
			if (OnLogLine != null)
			{
				OnLogLine(text, color);
			}
		}
		/// <summary>
		/// Writes a string of text using the output event <see cref="OnLogPart"/>.
		/// </summary>
		/// <param name="text">Text to output</param>
		/// <param name="color">Color of output text. <c>null</c> denotes to use the previous color.</param>
		public static void Write(string text, ConsoleColor? color = null)
		{
			if (OnLogPart != null)
			{
				OnLogPart(text, color);
			}
		}

		/// <summary>
		/// Writes a line of text via <see cref="WriteLine(string, ConsoleColor)"/>, coloring by
		/// a given <see cref="LogLevel"/> as according to <see cref="LevelColors"/>.
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="level">Level at which to write</param>
		/// <param name="args">Arguments supplied for <see cref="string.Format(string, object[])"/></param>
		/// <remarks>
		/// If <paramref name="level"/> is a lower level than <see cref="MinLogLevel"/>, the method
		/// will return and nothing will be output.
		/// </remarks>
		public static void WriteLineLevel(string text, LogLevel level, params object[] args)
		{
			if (level < MinLogLevel)
			{
				return;
			}

			if (LevelColors.ContainsKey(level))
			{
				string outputLine = text;
				try
				{
					outputLine = text.Fmt(args);
				}
				catch (FormatException)
				{ }

				ConsoleColor color = LevelColors[level];
				WriteLine(outputLine, color);
			}
		}

		#region Output by Level
		/// <summary>
		/// Logs a line of text by the <see cref="F:UltimateUtil.Logging.LogLevel.Verbose">LogLevel.Verbose</see> level
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Formatting args</param>
		public static void Verbose(string text, params object[] args)
		{
			WriteLineLevel(text, LogLevel.Verbose, args);
		}
		/// <summary>
		/// Logs a line of text by the <see cref="F:UltimateUtil.Logging.LogLevel.Debug">LogLevel.Debug</see> level
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Formatting args</param>
		public static void Debug(string text, params object[] args)
		{
			WriteLineLevel(text, LogLevel.Debug, args);
		}
		/// <summary>
		/// Logs a line of text by the <see cref="F:UltimateUtil.Logging.LogLevel.Info">LogLevel.Info</see> level
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Formatting args</param>
		public static void Info(string text, params object[] args)
		{
			WriteLineLevel(text, LogLevel.Info, args);
		}
		/// <summary>
		/// Logs a line of text by the <see cref="F:UltimateUtil.Logging.LogLevel.Success">LogLevel.Success</see> level
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Formatting args</param>
		public static void Success(string text, params object[] args)
		{
			WriteLineLevel(text, LogLevel.Success, args);
		}
		/// <summary>
		/// Logs a line of text by the <see cref="F:UltimateUtil.Logging.LogLevel.Warning">LogLevel.Warning</see> level
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Formatting args</param>
		public static void Warning(string text, params object[] args)
		{
			WriteLineLevel(text, LogLevel.Warning, args);
		}
		/// <summary>
		/// Logs a line of text by the <see cref="F:UltimateUtil.Logging.LogLevel.Error">LogLevel.Error</see> level
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Formatting args</param>
		public static void Error(string text, params object[] args)
		{
			WriteLineLevel(text, LogLevel.Error, args);
		}
		/// <summary>
		/// Logs a line of text by the <see cref="F:UltimateUtil.Logging.LogLevel.Fatal">LogLevel.Fatal</see> level
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="args">Formatting args</param>
		public static void Fatal(string text, params object[] args)
		{
			WriteLineLevel(text, LogLevel.Fatal, args);
		}
		#endregion

		/// <summary>
		/// Writes a line of text with formatting codes for color, patterned after Minecraft chat
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="escape">Escape character that formatting codes are prefixed with</param>
		/// <example>
		/// <c>WriteComplex("\cRED \9BLUE \aGREEN");</c> prints "RED " in red text, followed by
		/// "BLUE " in blue text, followed by "GREEN" in green text.
		/// </example>
		public static void WriteComplex(string text, char escape = '&')
		{
			List<string> parts = new List<string>();
			string currentPart = "";
			bool nextCode = false;
			ConsoleColor? currentColor = null;
			foreach (char c in text)
			{
				if (c == escape)
				{
					nextCode = true;
					continue;
				}

				if (nextCode)
				{
					if (!currentPart.IsNullOrEmpty())
					{
						Write(currentPart, currentColor);
					}

					bool failedCode = false;
					currentPart = "";
					try
					{
						byte colorNum = Convert.ToByte(c.ToString(), 16);
						currentColor = (ConsoleColor)colorNum;
					}
					catch (FormatException)
					{
						failedCode = true;
					}
					catch (ArgumentException)
					{
						failedCode = true;
					}
					nextCode = false;

					if (failedCode)
					{
						currentPart += escape.ToString() + c.ToString();
					}

					continue;
				}

				currentPart += c;
			}

			if (!currentPart.IsNullOrEmpty())
			{
				Write(currentPart, currentColor);
			}

			WriteLine();
		}

		/// <summary>
		/// Writes a line of colored text similar to <see cref="string.Format(string, object[])"/>
		/// by replacing formatter parts (such as <c>{0}</c>) with codes for the given colors,
		/// transforming the text from then onward to the specified color.
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="escape">
		/// Escape character used internally. Do not use a character that appears in <paramref name="text"/>.
		/// </param>
		/// <param name="colors">Colors to format text into</param>
		/// <example>
		/// <c>WriteComplex("{0}RED {1}BLUE {2}GREEN", '\\', ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green);</c>
		/// prints "RED " in red text, followed by "BLUE " in blue text, followed by "GREEN" in green text.
		/// </example>
		public static void WriteComplex(string text, char escape, params ConsoleColor[] colors)
		{
			List<string> formatters = new List<string>();
			foreach (ConsoleColor col in colors)
			{
				byte b = (byte)col;
				formatters.Add(escape.ToString() + b.ToString("X"));
			}

			string withFormatters = text.Fmt(formatters.ToArray());
			WriteComplex(withFormatters, escape);
		}
		/// <summary>
		/// Writes a line of colored text similar to <see cref="string.Format(string, object[])"/>
		/// by replacing formatter parts (such as <c>{0}</c>) with codes for the given colors,
		/// transforming the text from then onward to the specified color.
		/// </summary>
		/// <param name="text">Text to write after formatting</param>
		/// <param name="colors">Colors to format text into</param>
		/// <example>
		/// <c>WriteComplex("{0}RED {1}BLUE {2}GREEN", ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green);</c>
		/// prints "RED " in red text, followed by "BLUE " in blue text, followed by "GREEN" in green text.
		/// </example>
		public static void WriteComplex(string text, params ConsoleColor[] colors)
		{
			WriteComplex(text, '\u00A7', colors);
		}

		/// <summary>
		/// Retrieves a string from the input method stored in <see cref="OnGetString"/>.
		/// </summary>
		/// <param name="prompt">Text to display as a prompt</param>
		/// <returns><c>string</c> retrieved from method, <c>null</c> if there was no method.</returns>
		public static string GetString(string prompt)
		{
			if (OnGetString != null)
			{
				return OnGetString(prompt);
			}

			return null;
		}

		/// <summary>
		/// Attempts to retrieve a number from the input method stored in
		/// <see cref="OnGetNumber"/>. Returns null if no method was found.
		/// </summary>
		/// <param name="prompt">Text to display as a prompt</param>
		/// <returns>
		/// <c>double?</c> object representing the result of the method, 
		/// <c>null</c> if there was no method.
		/// </returns>
		public static double? TryGetNumber(string prompt)
		{
			if (OnGetNumber != null)
			{
				return OnGetNumber(prompt);
			}

			return null;
		}
		/// <summary>
		/// Attempts to retrieve a number from the input method stored in
		/// <see cref="OnGetNumber"/>. Throws a <see cref="NullReferenceException"/> if
		/// no method was found.
		/// </summary>
		/// <param name="prompt">Text to display as a prompt</param>
		/// <returns><c>double</c> representing the result of the method</returns>
		/// <exception cref="NullReferenceException">
		/// Thrown if <see cref="OnGetNumber"/> was never set.
		/// </exception>
		public static double GetNumber(string prompt)
		{
			double? d = TryGetNumber(prompt);

			if (d == null)
			{
				throw new NullReferenceException("Delegate {0} was never set.".Fmt(nameof(OnGetNumber)));
			}

			return d.Value;
		}

		private static IDictionary<string, object> _toObjForm<T>(IDictionary<string, T> dict)
		{
			IDictionary<string, object> res = new Dictionary<string, object>();
			foreach (KeyValuePair<string, T> kvp in dict)
			{
				res.Add(kvp.Key, kvp.Value);
			}

			return res;
		}
		
		/// <summary>
		/// Retrieves an item from the selection process stored in <see cref="OnGetSelection"/>.
		/// </summary>
		/// <typeparam name="T">Value type of dictionary where options are stored</typeparam>
		/// <param name="prompt">Text to display as a prompt</param>
		/// <param name="options">Available options to display the user.</param>
		/// <param name="ignorable">Whether the selection can be ignored by the user</param>
		/// <returns>The key of the selected item in <paramref name="options"/></returns>
		public static string GetSelection<T>(string prompt, IDictionary<string, T> options, bool ignorable = false)
		{
			IDictionary<string, object> objDict = _toObjForm(options);

			if (!ignorable)
			{
				if (OnGetSelection != null)
				{
					return OnGetSelection(prompt, objDict);
				}

				return null;
			}
			else
			{
				if (OnGetIgnorableSelection != null)
				{
					return OnGetIgnorableSelection(prompt, objDict);
				}

				return null;
			}
		}

		/// <summary>
		/// Retrieves an item from the selection process stored in <see cref="OnGetSelection"/>.
		/// </summary>
		/// <typeparam name="T">Type within list</typeparam>
		/// <param name="prompt">Text to display as a prompt</param>
		/// <param name="options">Available options to display the user.</param>
		/// <param name="ignorable">Whether the selection can be ignored by the user</param>
		/// <returns>The index of the selected item in <paramref name="options"/>, or <c>-1</c> if ignored by user</returns>
		public static int GetSelection<T>(string prompt, IList<T> options, bool ignorable = false)
		{
			IDictionary<string, object> dict = new Dictionary<string, object>();
			for (int i = 0; i < options.Count; i++)
			{
				dict.Add(i.ToString(), options[i]);
			}

			string key = GetSelection(prompt, dict, ignorable);
			if (key == null)
			{
				return -1;
			}

			return int.Parse(key);
		}

		/// <summary>
		/// Retrieves an item from the selection process stored in <see cref="OnGetSelection"/>.
		/// </summary>
		/// <param name="prompt">Text to display as a prompt</param>
		/// <param name="ignorable">Whether the selection can be ignored by the user</param>
		/// <param name="args">Available options to display the user, alternating between key and value.</param>
		/// <returns>The key of the selected item in <paramref name="args"/>, or <c>null</c> if ignored by user.</returns>
		public static string GetSelection(string prompt, bool ignorable, params object[] args)
		{
			return GetSelection(prompt, ignorable, new List<object>(), args);
		}

		/// <summary>
		/// Retrieves an item from the selection process stored in <see cref="OnGetSelection"/>.
		/// </summary>
		/// <typeparam name="T">Type within list</typeparam>
		/// <param name="prompt">Text to display as a prompt</param>
		/// <param name="ignorable">Whether the selection can be ignored by the user</param>
		/// <param name="options">Available options to display the user, listed by number.</param>
		/// <param name="args">Additional options to display, alternating between key and value.</param>
		/// <returns>The key of the selected item, or <c>null</c> if ignored by user</returns>
		public static string GetSelection<T>(string prompt, bool ignorable, IList<T> options, params object[] args)
		{
			IDictionary<string, object> dict = new Dictionary<string, object>();
			for (int i = 0; i < options.Count; i++)
			{
				dict.Add(i.ToString(), options[i]);
			}

			bool atVal = false;
			string key = null;
			foreach (object o in args)
			{
				if (!atVal)
				{
					key = o as string;

					if (key == null)
					{
						throw new ArgumentException("Every odd-numbered item must be a string key.", nameof(args));
					}
				}
				else
				{
					dict.Add(key, o);
				}

				atVal = !atVal;
			}

			if (dict.Count() == 0)
			{
				throw new ArgumentException("No options were given to select from.", nameof(args));
			}

			return GetSelection(prompt, dict, ignorable);
		}

		/// <summary>
		/// Interacts with the user with one of the methods in this class.
		/// </summary>
		/// <param name="interactionType">Type of interaction to make</param>
		/// <param name="text">Text to display, prompt for input interactions</param>
		/// <param name="logColor">Secondary option (output color)</param>
		/// <param name="info">Other arguments, such as selection objects</param>
		/// <returns>Result of the interaction, null if interaction is not input.</returns>
		/// <remarks>
		/// If <paramref name="interactionType"/> is <see cref="InteractionType.InputNumber"/>, then
		/// <see cref="TryGetNumber(string)"/> will be called, and a <see cref="Nullable{Double}"/> will be
		/// returned.
		/// 
		/// If <paramref name="interactionType"/> is <see cref="InteractionType.Selection"/> or
		/// <see cref="InteractionType.OptionalSelection"/>, then <see cref="GetSelection(string, bool, object[])"/>
		/// will be called, and an <see cref="string"/> of the selected ID will be returned.
		/// 
		/// The color of the prompt on prompting interactions will not be affected <paramref name="logColor"/>; that
		/// parameter is meaningless in those circumstances.
		/// 
		/// I really have no idea why I have this method or the <see cref="InteractionType"/> enum.
		/// </remarks>
		/// <exception cref="NullReferenceException">
		/// Thrown if <paramref name="interactionType"/> is <see cref="InteractionType.LogLine"/> and
		/// <paramref name="logColor"/> is <c>null</c>.
		/// </exception>
		public static object Interact(InteractionType interactionType, string text, ConsoleColor? logColor, params object[] info)
		{
			if (logColor == null && interactionType == InteractionType.LogLine)
			{
				throw new NullReferenceException("Argument {0} cannot be null if {1} is {2}"
					.Fmt(nameof(logColor), nameof(interactionType), nameof(InteractionType.LogLine)));
			}

			switch (interactionType)
			{
				case InteractionType.LogPart:
					Write(text, logColor);
					return null;
				case InteractionType.LogLine:
					WriteLine(text, logColor.Value);
					return null;
				case InteractionType.InputString:
					return GetString(text);
				case InteractionType.InputNumber:
					return TryGetNumber(text);
				case InteractionType.Selection:
					return GetSelection(text, false, info);
				case InteractionType.OptionalSelection:
					return GetSelection(text, true, info);
				default:
					return null;
			}
		}
	}
}
