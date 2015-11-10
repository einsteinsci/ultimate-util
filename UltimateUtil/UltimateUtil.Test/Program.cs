using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("String: ");
			string input = Console.ReadLine();

			Console.Write("Format: ");
			string format = Console.ReadLine();

			string[] split = input.FormatSplit(format);
			foreach (string s in split)
			{
				Console.WriteLine(s);
			}

			Console.ReadKey();
		}
	}
}
