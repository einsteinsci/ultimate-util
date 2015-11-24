using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.UserInteraction
{
	/// <summary>
	/// Provides an example implementation of <see cref="VersatileIO"/> delegates
	/// for the <see cref="Console"/>.
	/// </summary>
	public class PresetVersatileConsoleIO : VersatileHandlerBase
	{
		/// <summary>
		/// Color to use in user prompts
		/// </summary>
		public ConsoleColor PromptColor
		{ get; set; }

		/// <summary>
		/// Whether to continue asking if the user enters invalid input. If set to
		/// <c>false</c>, the method will default to <see cref="double.NaN"/> or <c>null</c>.
		/// </summary>
		public bool BePersistent
		{ get; set; }

		/// <summary>
		/// Creates a new instance of <see cref="PresetVersatileConsoleIO"/> and prepares 
		/// <see cref="VersatileIO"/> for logging.
		/// </summary>
		/// <param name="promptColor">Color to use in prompts</param>
		/// <param name="bePersistent">Whether to be persistent in prompts</param>
		public PresetVersatileConsoleIO(ConsoleColor promptColor = ConsoleColor.White, 
			bool bePersistent = true)
		{
			PromptColor = promptColor;
			BePersistent = bePersistent;
		}

		/// <summary>
		/// Method supplied to the <see cref="VersatileIO.OnLogPart"/> event.
		/// </summary>
		/// <param name="text">Text to log</param>
		/// <param name="color">Color of text, <c>null</c> if not changed.</param>
		public override void LogPart(string text, ConsoleColor? color)
		{
			if (color != null)
			{
				Console.ForegroundColor = color.Value;
			}
			Console.Write(text);
		}
		/// <summary>
		/// Method supplied to the <see cref="VersatileIO.OnLogLine"/> event.
		/// </summary>
		/// <param name="line">Text to log</param>
		/// <param name="color">Color of text</param>
		public override void LogLine(string line, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(line);
		}

		/// <summary>
		/// Method supplied to the <see cref="VersatileIO.OnGetString"/> delegate.
		/// </summary>
		/// <param name="prompt">Text to prompt the user for input</param>
		/// <returns>The resulting <see cref="string"/>.</returns>
		public override string GetString(string prompt)
		{
			Console.ForegroundColor = PromptColor;
			Console.Write(prompt);
			return Console.ReadLine();
		}

		/// <summary>
		/// Method supplied to the <see cref="VersatileIO.OnGetNumber"/> delegate.
		/// </summary>
		/// <param name="prompt">Text to prompt the user for input</param>
		/// <returns>The resulting <see cref="double"/>.</returns>
		public override double GetDouble(string prompt)
		{
			double d = double.NaN;

			bool worked = false;
			while (!worked)
			{
				Console.ForegroundColor = PromptColor;
				Console.Write(prompt);
				string str = Console.ReadLine();
				
				if (double.TryParse(str, out d))
				{
					worked = true;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("'{0}' is not a valid number.", d);

					if (!BePersistent)
					{
						Console.WriteLine("Defaulting to NaN.");
						d = double.NaN;
						worked = true;
					}
				}
			}

			return d;
		}

		/// <summary>
		/// Method supplied to the <see cref="VersatileIO.OnGetSelection"/> delegate.
		/// </summary>
		/// <param name="prompt">Dictionary of options and their respective keys</param>
		/// <param name="options">Text given as a prompt after selections are listed.</param>
		/// <returns>The key of the resulting item.</returns>
		public override string GetSelection(string prompt, IDictionary<string, object> options)
		{
			foreach (KeyValuePair<string, object> kvp in options)
			{
				Console.ForegroundColor = PromptColor;
				Console.WriteLine("  [{0}]: {1}", kvp.Key, kvp.Value);
			}

			while (true)
			{
				Console.ForegroundColor = PromptColor;
				Console.Write(prompt);
				string input = Console.ReadLine();

				if (options.ContainsKeyIgnoreCase(input))
				{
					return input;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("'{0}' is not one of the options above.", input);

					if (!BePersistent)
					{
						Console.WriteLine("Defaulting to first option.");
						return options.Keys.FirstOrDefault();
					}
				}
			}
		}

		/// <summary>
		/// Method supplied to the <see cref="VersatileIO.OnGetIgnorableSelection"/> delegate.
		/// </summary>
		/// <param name="prompt">Dictionary of options and their respective keys</param>
		/// <param name="options">Text given as a prompt after selections are listed.</param>
		/// <returns>The key of the resulting item.</returns>
		public override string GetSelectionIgnorable(string prompt, IDictionary<string, object> options)
		{
			foreach (KeyValuePair<string, object> kvp in options)
			{
				Console.ForegroundColor = PromptColor;
				Console.WriteLine("  [{0}]: {1}", kvp.Key, kvp.Value);
			}

			Console.ForegroundColor = PromptColor;
			Console.Write(prompt);
			string input = Console.ReadLine();

			if (options.ContainsKeyIgnoreCase(input))
			{
				return input;
			}
			else
			{
				return null;
			}
		}
	}
}
