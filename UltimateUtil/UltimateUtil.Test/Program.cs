using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateUtil.Logging;

namespace UltimateUtil.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			PresetsLogger.Initialize();
			Logger log = Logger.Instance;

			log.Debug("Debug text");
			log.Info("Info text");
			log.Success("Success text");
			log.Warning("Warning text");
			log.Error("Error text");
			log.Fatal("Fatal text");
			log.Interface("Interface text");

			Console.ReadKey();
		}
	}
}
