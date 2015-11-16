using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class ThreadUtil_Test
	{
		[TestMethod, TestCategory("threading")]
		public void RunDelayed()
		{
			int number = 5;

			Thread thread = ThreadUtil.RunDelayed(1000, () => number = 24);
			Assert.IsTrue(thread.IsAlive);
			Assert.AreEqual(5, number);

			Thread.Sleep(1200);
			Assert.AreEqual(24, number);
		}
	}
}
