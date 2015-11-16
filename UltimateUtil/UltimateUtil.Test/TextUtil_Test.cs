using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class TextUtil_Test
	{
		[TestMethod, TestCategory("text")]
		public void IsNullOrEmpty()
		{
			string n = null;
			string emp = "";
			string space = " ";

			Assert.IsTrue(emp.IsNullOrEmpty(), "Empty string failed.");
			Assert.IsTrue(n.IsNullOrEmpty(), "Null string failed.");
			Assert.IsFalse(space.IsNullOrEmpty(), "Space string failed.");
		}
	}
}
