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
		[TestMethod]
		public void IsNullOrEmpty()
		{
			string n = null;
			string emp = string.Empty;
			string space = " ";

			Assert.IsTrue(n.IsNullOrEmpty() && emp.IsNullOrEmpty());
			Assert.IsFalse(space.IsNullOrEmpty());
		}
	}
}
