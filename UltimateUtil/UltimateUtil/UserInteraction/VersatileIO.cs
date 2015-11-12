using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.UserInteraction
{
	public enum InteractionType
	{
		LogPart,
		LogLine,
		InputString,
		InputNumber,
		Selection,
		OptionalSelection,
	}

	/// <summary>
	/// Delegate for sending a line of information to the user.
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
	/// <param name="options">
	/// Dictionary of items to select from. Keys denote shortcut codes for the items, while values 
	/// are what is listed when getting the selection.
	/// </param>
	/// <returns>The key for one of the items from <paramref name="options"/>, <c>null</c> if ignored.</returns>
	public delegate string GetSelection(string prompt, IDictionary<string, object> options);

	/// <summary>
	/// Contains methods for interacting with the user dynamically and simply.
	/// </summary>
	public static class VersatileIO
	{
		/// <summary>
		/// Subscribe to allow full-line output. Be sure the newline comes after
		/// the message, not before.
		/// </summary>
		public static event SendLog OnLogLine;
		/// <summary>
		/// Subscribe to allow partial-line (and complex) output. A null color
		/// parameter implies to use the color of the last output.
		/// </summary>
		public static event SendLogPart OnLogPart;

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

		/*
		/// <summary>
		/// Attempts to retrieve an <c>object</c> from a list.
		/// </summary>
		/// <param name="options"><see cref="IList{object}"/> of options that the user can select.</param>
		/// <param name="ignorable">
		/// <c>true</c> if the user can choose to ignore the options, returning <c>null</c>,
		/// <c>false</c> if not.
		/// </param>
		/// <returns>Resulting entry from the list, <c>null</c> if ignored.</returns>
		public static object GetSelection(string prompt, IList<object> options, bool ignorable = false)
		{
			if (ignorable)
			{
				if (OnGetIgnorableSelection != null)
				{
					return OnGetIgnorableSelection(prompt, options);
				}

				return null;
			}
			else
			{
				if (OnGetSelection != null)
				{
					return OnGetSelection(prompt, options);
				}

				return null;
			}
		}
		/// <summary>
		/// Attempts to retrieve an item from a list of <c>class</c> objects.
		/// </summary>
		/// <typeparam name="T">Type of item provided.</typeparam>
		/// <param name="options"><see cref="IList{T}"/> of options the user can select.</param>
		/// <param name="ignorable">
		/// <c>true</c> if the user can choose to ignore the options, returning <c>null</c>,
		/// <c>false</c> if not.
		/// </param>
		/// <returns>Resulting entry from the list, <c>null</c> if ignored.</returns>
		public static T GetSelection<T>(string prompt, IList<T> options, bool ignorable = false) where T : class
		{
			IList<object> objForm = new List<object>();
			foreach (T t in options)
			{
				objForm.Add(t);
			}

			return (T)GetSelection(prompt, objForm, ignorable);
		}
		/// <summary>
		/// Retrieves an item from a list of <c>struct</c> objects.
		/// </summary>
		/// <typeparam name="T">Type of item provided.</typeparam>
		/// <param name="options"><see cref="IList{T}"/> of options the user can select.</param>
		/// <returns>Resulting entry from the list, <c>null</c> if ignored.</returns>
		public static T GetSelection<T>(string prompt, IList<T> options) where T : struct
		{
			IList<object> objForm = new List<object>();
			foreach (T t in options)
			{
				objForm.Add(t);
			}

			return (T)GetSelection(prompt, objForm, false);
		}
		/// <summary>
		/// Attempts to retrieve an item from a list of <c>struct</c> objects.
		/// </summary>
		/// <typeparam name="T">Type of item provided.</typeparam>
		/// <param name="options"><see cref="IList{T}"/> of options the user can select.</param>
		/// <param name="ignorable">
		/// <c>true</c> if the user can choose to ignore the options, returning <c>null</c>,
		/// <c>false</c> if not.
		/// </param>
		/// <returns>Resulting entry from the list, <c>null</c> if ignored.</returns>
		public static T? GetSelectionIgnorable<T>(string prompt, IList<T> options) where T : struct
		{
			IList<object> objForm = new List<object>();
			foreach (T t in options)
			{
				objForm.Add(t);
			}

			object res = GetSelection(prompt, objForm, true);

			if (res == null)
			{
				return null;
			}
			else
			{
				return (T)res;
			}
		}
		*/

		public static string GetSelection(string prompt, IDictionary<string, object> options, bool ignorable = false)
		{
			if (!ignorable)
			{
				if (OnGetSelection != null)
				{
					return OnGetSelection(prompt, options);
				}

				return null;
			}
			else
			{
				if (OnGetIgnorableSelection != null)
				{
					return OnGetIgnorableSelection(prompt, options);
				}

				return null;
			}
		}

		public static int GetSelection(string prompt, IList<object> options, bool ignorable = false)
		{
			IDictionary<string, object> dict = new Dictionary<string, object>();
			for (int i = 0; i < options.Count; i++)
			{
				dict.Add(i.ToString(), options[i]);
			}

			string key = GetSelection(prompt, dict, ignorable);
			return int.Parse(key);
		}

		public static string GetSelection(string prompt, bool ignorable, params object[] args)
		{
			return GetSelection(prompt, ignorable, new List<object>(), args);
		}

		public static string GetSelection(string prompt, bool ignorable, IList<object> options, params object[] args)
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
		/// <see cref="TryGetNumber(string)"/> will be called, and a <see cref="double?"/> will be
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
