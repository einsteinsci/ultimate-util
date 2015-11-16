using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateUtil.Fluid;
using UltimateUtil.Logging;

namespace UltimateUtil.Test
{
	[TestClass]
	public class FluidUtil_Test
	{
		[TestMethod, TestCategory("fluid")]
		public void ForEach()
		{
			string res = "";
			string[] stuff = { "a", "b", "c", "d" };
			stuff.ForEach((s) =>
			{
				res += s;
				return false;
			});

			Assert.AreEqual("abcd", res);
		}

		[TestMethod, TestCategory("fluid")]
		public void ForEachDict()
		{
			string res = "";
			Dictionary<string, string> dict = new Dictionary<string, string>()
			{
				{ "a", "1" },
				{ "b", "2" },
				{ "c", "3" }
			};

			dict.ForEach((k, v) =>
			{
				res += k + "=" + v + ";";
			});
			Assert.AreEqual("a=1;b=2;c=3;", res);

			res = "";
			dict.ForEach((k, v) =>
			{
				res += k + "^" + v + " ";
			});
			Assert.AreEqual("a^1 b^2 c^3 ", res);
		}

		[TestMethod, TestCategory("fluid")]
		public void Cast()
		{
			ITestInterface ti = new A(0.4);

			A a = ti.Cast<A>();
			Assert.AreEqual(0.4, a.value);

			B b = ti.Cast<B>();
			Assert.AreEqual(null, b);
		}

		[TestMethod, TestCategory("fluid")]
		public void Is()
		{
			A a = new A(0.4);
			B b = new B();

			Assert.IsTrue(a.Is<ITestInterface>());
			Assert.IsFalse(b.Is<ITestInterface>());

			ITestInterface ti = a;
			Assert.IsTrue(ti.Is<A>());
		}

		[TestMethod, TestCategory("fluid")]
		public void IsAnyOf()
		{
			Assert.IsTrue((6).IsAnyOf(5, 6, 7, 8));
			Assert.IsTrue("c".IsAnyOf("a", "b", "c", "d"));
			Assert.IsTrue(LogLevel.Info.IsAnyOf(LogLevel.Debug, LogLevel.Info, LogLevel.Interface));
		}

		[TestMethod, TestCategory("fluid")]
		public void MakeList()
		{
			List<int> nums = FluidUtils.List(5, 6, 7, 8);
			Assert.IsTrue(nums.Contains(5));
			Assert.IsTrue(nums.Contains(6));
			Assert.IsTrue(nums.Contains(7));
			Assert.IsTrue(nums.Contains(8));
		}

		[TestMethod, TestCategory("fluid")]
		public void ThrowIfNull()
		{
			B b = new B();
			B n = null;

			try
			{
				b.ThrowIfNull("b");
			}
			catch (ArgumentNullException)
			{
				Assert.Fail("b is not null but threw.");
			}

			try
			{
				n.ThrowIfNull("n");
				Assert.Fail("n is null but did not throw.");
			}
			catch (ArgumentNullException)
			{ }
		}

		[TestMethod, TestCategory("fluid")]
		public void With()
		{
			Button buttonWithSuperLongName = new Button();
			// lots of code in between
			buttonWithSuperLongName.With((b) =>
			{
				b.Width = 35;
				b.Text = "I eat cupcakes";
			});

			Assert.AreEqual(35, buttonWithSuperLongName.Width);
			Assert.AreEqual("I eat cupcakes", buttonWithSuperLongName.Text);
		}

		[TestMethod, TestCategory("fluid")]
		public void WithStruct()
		{
			LinkArea linkArea = new LinkArea();
			// lots of code in between
			FluidUtils.With(ref linkArea, (l) =>
			{
				l.Start = 56;
				l.Length = 112;
				return l;
			});

			Assert.AreEqual(56, linkArea.Start);
			Assert.AreEqual(112, linkArea.Length);
		}

		[TestMethod, TestCategory("fluid")]
		public void IsNullOrDefault()
		{
			int? a = 35;
			int? b = null;
			int? c = 0;

			Assert.IsFalse(a.IsNullOrDefault());
			Assert.IsTrue(b.IsNullOrDefault());
			Assert.IsTrue(c.IsNullOrDefault());
		}

		[TestMethod, TestCategory("fluid")]
		public void IsNull()
		{
			B b = new B();
			B n = null;

			Assert.IsTrue(n.IsNull());
			Assert.IsFalse(b.IsNull());
		}

		[TestMethod, TestCategory("fluid")]
		public void Raise1()
		{
			double stuff = 0;
			int num = 0;

			A a = new A(0.6);
			B b = new B();

			b.RaiseHandlerInt(a, 5);
			Assert.AreNotEqual(0.6, stuff);
			Assert.AreNotEqual(5, num);

			b.handlerInt += (sender, n) =>
			{
				A _a = sender as A;
				stuff = _a.value;
				num = n;
			};

			b.RaiseHandlerInt(a, 5);

			Assert.AreEqual(0.6, stuff);
			Assert.AreEqual(5, num);
		}


		[TestMethod, TestCategory("fluid")]
		public void Raise2()
		{
			double stuff = 0;
			int num = 0;

			A a = new A(0.6);
			B b = new B();
			C c = new C(42);

			b.RaiseHandlerPlain(a, c);
			Assert.AreNotEqual(0.6, stuff);
			Assert.AreNotEqual(42, num);

			b.handlerPlain += (sender, e) =>
			{
				A _a = sender as A;
				stuff = _a.value;

				C _c = e as C;
				num = _c.value;
			};

			b.RaiseHandlerPlain(a, c);

			Assert.AreEqual(0.6, stuff);
			Assert.AreEqual(42, num);
		}

		[TestMethod, TestCategory("fluid")]
		public void Not()
		{
			bool t = true;
			bool f = false;

			Assert.IsTrue(f.Not());
			Assert.IsFalse(t.Not());
		}

		#region test types
		public interface ITestInterface
		{
			int stuff();
		}

		public class A : ITestInterface
		{
			public double value;

			public int stuff()
			{
				return 5;
			}

			public A(double v)
			{
				value = v;
			}
		}

		public class B
		{
			public string str = "stringy";

			public event EventHandler<int> handlerInt;
			public event EventHandler handlerPlain;

			public void RaiseHandlerInt(object sender, int e)
			{
				handlerInt.Raise(sender, e);
			}
			public void RaiseHandlerPlain(object sender, EventArgs e)
			{
				handlerPlain.Raise(sender, e);
			}
		}

		public class C : EventArgs
		{
			public int value;

			public C(int v)
			{
				value = v;
			}

			public override bool Equals(object obj)
			{
				if (obj is C)
				{
					return value == (obj as C).value;
				}

				return false;
			}

			public override int GetHashCode()
			{
				return value;
			}
		}
		#endregion test types
	}
}
