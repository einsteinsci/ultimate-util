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

			TestRandom();

			Console.ReadKey();
		}

		private static void TestRandom()
		{
			long time = DateTime.Now.Ticks;
			Random rand = new Random();

			int t = 0, f = 0;
			for (int i = 0; i < 10000; i++)
			{
				bool b = rand.NextBool();

				if (b)
				{
					t++;
				}
				else
				{
					f++;
				}
			}

			VersatileIO.WriteComplex("&aTRUE: {0}\n&cFALSE: {1}".Fmt(t, f), '&');
			long elapsedTicks = DateTime.Now.Ticks - time;
			TimeSpan dtime = TimeSpan.FromTicks(elapsedTicks);
			VersatileIO.WriteLine("Elapsed time: {0} ms".Fmt(dtime.TotalMilliseconds));
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
