using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateUtil.Logging;

namespace UltimateUtil.Test
{
	[TestClass]
	public class Logger_Test
	{
		static string testResult;

		[TestMethod, TestCategory("logger")]
		public void LogTest()
		{
			Logger log = _setUpLogger();

			log.LogPart(LogLevel.Debug, "part{0};", 1);
			log.LogPart(LogLevel.Error, "part{0};", 2);

			Assert.AreEqual("part1;part2;", testResult);

			testResult = "";
			log.LogLine(LogLevel.Debug, "part{0}", 1);
			log.LogLine(LogLevel.Interface, "part{0}", 2);
			log.LogLine(LogLevel.Fatal, "part{0}", 3);

			Assert.AreEqual("<|>[DEBUG] part1<|>part2<|>[FATAL] part3", testResult);
		}

		[TestMethod, TestCategory("logger")]
		public void LogFileTest()
		{
			string tempFolder = Environment.GetEnvironmentVariable("temp");
			string filePath = Path.Combine(tempFolder, "stuff.txt");
			Logger log = new Logger(filePath, false);

			log.LogLine(LogLevel.Info, "hello world");
			log.Dispose();

			string contents = File.ReadAllText(filePath).Trim();
			Assert.AreEqual("[INFO] hello world", contents);
		}

		private Logger _setUpLogger()
		{
			testResult = "";

			Logger log = new Logger(null, false, LogLevel.Debug, LogLevel.Debug);

			log.OnLog += Log_OnLog;
			log.OnLogPart += Log_OnLogPart;
			return log;
		}

		private void Log_OnLogPart(object sender, LogEventArgs e)
		{
			testResult += e.Message;
		}

		private void Log_OnLog(object sender, LogEventArgs e)
		{
			testResult += "<|>" + e.Message;
		}
	}
}
