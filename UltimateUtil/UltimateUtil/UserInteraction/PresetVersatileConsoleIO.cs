using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.UserInteraction
{
	public static class PresetVersatileConsoleIO
	{
		public static ConsoleColor PromptColor
		{ get; set; }

		public static bool BePersistent
		{ get; set; }

		public static void Initialize(ConsoleColor promptColor = ConsoleColor.White, 
			bool bePersistent = true, bool initializedMessage = true)
		{
			PromptColor = promptColor;
			BePersistent = bePersistent;

			VersatileIO.OnLogPart += OnLogPart;
			VersatileIO.OnLogLine += OnLogLine;
			VersatileIO.OnGetString = GetString;
			VersatileIO.OnGetNumber = GetDouble;
			VersatileIO.OnGetSelection = GetSelection;
			VersatileIO.OnGetIgnorableSelection = GetSelectionIgnorable;

			VersatileIO.WriteLine("Logger initialized", ConsoleColor.Gray);
		}

		public static void OnLogPart(string text, ConsoleColor? color)
		{
			if (color != null)
			{
				Console.ForegroundColor = color.Value;
			}
			Console.Write(text);
		}
		public static void OnLogLine(string line, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(line);
		}

		public static string GetString(string prompt)
		{
			Console.ForegroundColor = PromptColor;
			Console.Write(prompt);
			return Console.ReadLine();
		}

		public static double GetDouble(string prompt)
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
						Console.WriteLine("Defaulting to 0.");
						d = 0;
						worked = true;
					}
				}
			}

			return d;
		}

		public static string GetSelection(string prompt, IDictionary<string, object> options)
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

		public static string GetSelectionIgnorable(string prompt, IDictionary<string, object> options)
		{
			foreach (KeyValuePair<string, object> kvp in options)
			{
				Console.ForegroundColor = PromptColor;
				Console.WriteLine("  [{0}]: {1}", kvp.Key, kvp.Value);
			}

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
