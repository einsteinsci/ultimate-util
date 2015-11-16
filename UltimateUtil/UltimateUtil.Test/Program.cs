using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateUtil.Fluid;
using UltimateUtil.Logging;
using UltimateUtil.UserInteraction;

namespace UltimateUtil.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			PresetVersatileConsoleIO.Initialize();

			TestVersatileIO();

			Console.ReadKey();
		}

		private static void TestVersatileIO()
		{
			//double num = VersatileIO.GetNumber("number? ");
			//VersatileIO.WriteLine("num = {0}".Fmt(num));
			//
			//string str = VersatileIO.GetString("string? ");
			//VersatileIO.WriteLine("str = " + str);

			VersatileIO.WriteComplex("{0}RED {1}BLUE {2}GREEN", ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green);
			VersatileIO.WriteLine();

			//List<string> options = FluidUtils.List("AAAA", "BBBB", "CCCC");
			//string chose = VersatileIO.GetSelection("Select an option: ", true, options, "u", "UNUSUALS");
			//string res = "NOTHING";
			//if (chose == "u")
			//{
			//	res = "UNUSUALS";
			//}
			//if (chose != null)
			//{
			//	int n = chose.Parse(-1);
			//	if (n.IsBetweenExclusive(-1, options.Count))
			//	{
			//		res = options[n];
			//	}
			//}
			//VersatileIO.WriteLine("You chose {0}: {1}".Fmt(chose ?? "NULL", res));
		}
	}
}
