using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class NumberUtils_Test
	{
		[TestMethod, TestCategory("number")]
		public void IsBetween()
		{
			Assert.IsTrue((5).IsBetween(1, 10));
			Assert.IsTrue((10).IsBetween(1, 10));
			Assert.IsFalse((10).IsBetween(3, 6));
		}

		[TestMethod, TestCategory("number")]
		public void IsBetweenExclusive()
		{
			Assert.IsTrue((5).IsBetweenExclusive(1, 10));
			Assert.IsFalse((10).IsBetweenExclusive(1, 10));
			Assert.IsFalse((10).IsBetweenExclusive(3, 6));
		}

		[TestMethod, TestCategory("number")]
		public void ToPercent()
		{
			Assert.AreEqual(35.0, 0.35.ToPercent());
			Assert.AreEqual(0.3, 0.003.ToPercent());
			Assert.AreEqual(136.0, 1.36.ToPercent());

			Assert.AreEqual(35, 0.35.ToPercentInt());
			Assert.AreEqual(0, 0.003.ToPercentInt());
			Assert.AreEqual(136, 1.36.ToPercentInt());
		}

		[TestMethod, TestCategory("number")]
		public void KMG()
		{
			Assert.AreEqual(2048L, 2L.K());
			Assert.AreEqual(2L.K().K(), 2L.M());
			Assert.AreEqual(2L.M().K(), 2L.G());
		}

		[TestMethod, TestCategory("number")]
		public void ToCurrency()
		{
			Assert.AreEqual("$49.95", (49.95).ToCurrency());
		}

		[TestMethod, TestCategory("number")]
		public void OddEven()
		{
			Assert.IsTrue(5.IsOdd());
			Assert.IsFalse(4.IsOdd());

			Assert.IsTrue(4.IsEven());
			Assert.IsFalse(5.IsEven());
		}

		[TestMethod, TestCategory("number")]
		public void Multiple()
		{
			Assert.IsTrue(459.IsMultipleOf(9));
			Assert.IsFalse(38.IsMultipleOf(7));
		}
	}
}
