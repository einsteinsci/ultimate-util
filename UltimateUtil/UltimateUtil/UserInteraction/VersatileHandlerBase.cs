using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.UserInteraction
{
	/// <summary>
	/// Base type for handlers set in <see cref="VersatileIO.SetHandler(VersatileHandlerBase, bool)"/>.
	/// Contains all methods to be subscribed to <see cref="VersatileIO"/>.
	/// </summary>
	public abstract class VersatileHandlerBase
	{
		/// <summary>
		/// Handler method for <see cref="VersatileIO.OnLogPart"/>
		/// </summary>
		/// <param name="text">Text to write</param>
		/// <param name="color">Color of text, <c>null</c> to remain the same as previous output</param>
		public abstract void LogPart(string text, ConsoleColor? color);

		/// <summary>
		/// Handler method for <see cref="VersatileIO.OnLogLine"/>
		/// </summary>
		/// <param name="line">Text to write</param>
		/// <param name="color">Color of text</param>
		public abstract void LogLine(string line, ConsoleColor color);

		/// <summary>
		/// Handler method for <see cref="VersatileIO.OnGetString"/>
		/// </summary>
		/// <param name="prompt">Text to write as a user prompt</param>
		/// <returns>A <see cref="string"/> from the user</returns>
		public abstract string GetString(string prompt);

		/// <summary>
		/// Handler method for <see cref="VersatileIO.OnGetNumber"/>
		/// </summary>
		/// <param name="prompt">Text to write as a user prompt</param>
		/// <returns>A <see cref="double"/> from the user</returns>
		public abstract double GetDouble(string prompt);

		/// <summary>
		/// Handler method for <see cref="VersatileIO.OnGetSelection"/>
		/// </summary>
		/// <param name="prompt">Text to write as a user prompt after options are listed</param>
		/// <param name="options">Various options to list, with input codes as keys</param>
		/// <returns>The key of the selected option</returns>
		public abstract string GetSelection(string prompt, IDictionary<string, object> options);

		/// <summary>
		/// Handler method for <see cref="VersatileIO.OnGetIgnorableSelection"/>
		/// </summary>
		/// <param name="prompt">Text to write as a user prompt after options are listed</param>
		/// <param name="options">Various options to list, with input codes as keys</param>
		/// <returns>The key of the selected option, or <c>null</c> if ignored</returns>
		public abstract string GetSelectionIgnorable(string prompt, IDictionary<string, object> options);

		/// <summary>
		/// Initializes the essential parts of <see cref="VersatileIO"/>. Called by
		/// <see cref="VersatileIO.SetHandler(VersatileHandlerBase, bool)"/>.
		/// </summary>
		public virtual void InitializeIO()
		{
			VersatileIO.InitializeLevels();

			VersatileIO.OnLogPart = LogPart;
			VersatileIO.OnLogLine = LogLine;
			VersatileIO.OnGetString = GetString;
			VersatileIO.OnGetNumber = GetDouble;
			VersatileIO.OnGetSelection = GetSelection;
			VersatileIO.OnGetIgnorableSelection = GetSelectionIgnorable;
		}
	}
}
