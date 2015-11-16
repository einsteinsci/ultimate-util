using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class EnumUtils_Test
	{
		[TestMethod, TestCategory("enums")]
		public void GetDescription()
		{
			Assert.AreEqual("__two__", E.Two.GetDescription());
			Assert.AreEqual("Three", E.Three.GetDescription());
			Assert.AreEqual("7", ((E)7).GetDescription());
		}

		[TestMethod, TestCategory("enums")]
		public void GetAllValues()
		{
			List<E> list = EnumUtils.GetAllValues<E>();

			Assert.IsTrue(list.Contains(E.One));
			Assert.IsTrue(list.Contains(E.Two));
			Assert.IsTrue(list.Contains(E.Three));
			Assert.IsTrue(list.Contains(E.Four));
			Assert.IsTrue(list.Contains(E.Five));
		}

		[TestMethod, TestCategory("enums")]
		public void ToEnum()
		{
			Assert.AreEqual(E.Three, "Three".ToEnum<E>());
			Assert.AreEqual(E.Four, "foUR".ToEnum<E>(true));
		}

		public enum E
		{
			One = 1,
			[EnumDescription("__two__")]
			Two,
			Three,
			Four,
			Five,
		}
	}
}
