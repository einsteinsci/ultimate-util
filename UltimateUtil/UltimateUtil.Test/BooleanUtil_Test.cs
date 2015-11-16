using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class BooleanUtil_Test
	{
		[TestMethod, TestCategory("boolean")]
		public void FromInteger()
		{
			int n1 = 0, n2 = 1, n3 = 45;
			Assert.IsFalse(n1.ToBool());
			Assert.IsTrue(n2.ToBool());
			Assert.IsTrue(n3.ToBool());

			uint un1 = 0, un2 = 1, un3 = 45;
			Assert.IsFalse(un1.ToBool());
			Assert.IsTrue(un2.ToBool());
			Assert.IsTrue(un3.ToBool());

			long l1 = 0, l2 = 1, l3 = 45;
			Assert.IsFalse(l1.ToBool());
			Assert.IsTrue(l2.ToBool());
			Assert.IsTrue(l3.ToBool());

			ulong ul1 = 0, ul2 = 1, ul3 = 45;
			Assert.IsFalse(ul1.ToBool());
			Assert.IsTrue(ul2.ToBool());
			Assert.IsTrue(ul3.ToBool());

			short s1 = 0, s2 = 1, s3 = 45;
			Assert.IsFalse(s1.ToBool());
			Assert.IsTrue(s2.ToBool());
			Assert.IsTrue(s3.ToBool());

			ushort us1 = 0, us2 = 1, us3 = 45;
			Assert.IsFalse(us1.ToBool());
			Assert.IsTrue(us2.ToBool());
			Assert.IsTrue(us3.ToBool());

			byte b1 = 0, b2 = 1, b3 = 45;
			Assert.IsFalse(b1.ToBool());
			Assert.IsTrue(b2.ToBool());
			Assert.IsTrue(b3.ToBool());

			sbyte sb1 = 0, sb2 = 1, sb3 = 45;
			Assert.IsFalse(sb1.ToBool());
			Assert.IsTrue(sb2.ToBool());
			Assert.IsTrue(sb3.ToBool());
		}

		[TestMethod, TestCategory("boolean")]
		public void ToInteger()
		{
			bool t = true, f = false;

			int n1 = t.ToInt();
			int n2 = f.ToInt();
			Assert.AreEqual(1, n1);
			Assert.AreEqual(0, n2);

			byte b1 = t.ToByte();
			byte b2 = f.ToByte();
			Assert.AreEqual((byte)1, b1);
			Assert.AreEqual((byte)0, b2);
		}

		[TestMethod, TestCategory("boolean")]
		public void NullableToInteger()
		{
			bool? t = true, f = false, n = null;

			int n1 = t.ToInt();
			int n2 = f.ToInt();
			int n3 = n.ToInt();
			Assert.AreEqual(1, n1);
			Assert.AreEqual(-1, n2);
			Assert.AreEqual(0, n3);

			sbyte b1 = t.ToSByte();
			sbyte b2 = f.ToSByte();
			sbyte b3 = n.ToSByte();
			Assert.AreEqual((sbyte)1, b1);
			Assert.AreEqual((sbyte)-1, b2);
			Assert.AreEqual((sbyte)0, b3);
		}

		[TestMethod, TestCategory("boolean")]
		public void ParseLoose()
		{
			Assert.IsTrue(BooleanUtil.ParseLoose("Yes"));
			Assert.IsTrue(BooleanUtil.ParseLoose("y"));
			Assert.IsTrue(BooleanUtil.ParseLoose("TRUE"));
			Assert.IsTrue(BooleanUtil.ParseLoose("1"));

			Assert.IsFalse(BooleanUtil.ParseLoose("No"));
			Assert.IsFalse(BooleanUtil.ParseLoose("n"));
			Assert.IsFalse(BooleanUtil.ParseLoose("FALSE"));
			Assert.IsFalse(BooleanUtil.ParseLoose("0"));
			Assert.IsFalse(BooleanUtil.ParseLoose("-1"));
		}
	}
}
