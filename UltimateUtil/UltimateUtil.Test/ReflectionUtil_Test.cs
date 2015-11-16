using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class ReflectionUtil_Test
	{
		public static readonly Assembly assembly = Assembly.GetExecutingAssembly();

		[TestMethod, TestCategory("reflection")]
		public void HasAttribute()
		{
			Type ta = typeof(A);

			Assert.IsTrue(ta.HasAttribute<ThingAttribute>());

			FieldInfo f = ta.GetField("value");
			Assert.IsTrue(f.HasAttribute<ThingAttribute>());

			MethodInfo m = ta.GetMethod("MinusTwo");
			Assert.IsFalse(m.HasAttribute<ThingAttribute>());
		}

		[TestMethod, TestCategory("reflection")]
		public void GetTypesWithAttribute()
		{
			List<Type> list = assembly.GetTypesWithAttribute<ThingAttribute>();

			Assert.IsTrue(list.Contains(typeof(A)));
			Assert.IsTrue(list.Contains(typeof(C)));
			Assert.IsFalse(list.Contains(typeof(B)));
		}

		[TestMethod, TestCategory("reflection")]
		public void GetMembersWithAttribute()
		{
			Type ta = typeof(A);
			MethodInfo plus = ta.GetMethod("PlusTwo");
			MethodInfo minus = ta.GetMethod("MinusTwo");
			FieldInfo field = ta.GetField("value");

			List<MemberInfo> list = ta.GetMembersWithAttribute<ThingAttribute>();
			Assert.IsTrue(list.Contains(plus));
			Assert.IsTrue(list.Contains(field));
			Assert.IsFalse(list.Contains(minus));

			list = ta.GetMembersWithAttribute<ThingAttribute>(MemberTypes.Method);
			Assert.IsTrue(list.Contains(plus));
			Assert.IsFalse(list.Contains(field));
			Assert.IsFalse(list.Contains(minus));
		}

		[TestMethod, TestCategory("reflection")]
		public void Invoke()
		{
			A a = new A();

			object obj = a.InvokeMethod("PlusTwo");
			Assert.IsTrue(obj is int);
			Assert.AreEqual(6, (int)obj);

			MethodInfo append = typeof(A).GetMethod("AppendTypeName");
			C c = new C();
			obj = append.InvokeGeneric(a, typeof(C).Once<Type>(), c.Once<object>());
			Assert.IsTrue(obj is string);
			Assert.AreEqual("4_C", obj as string);
			Assert.AreEqual(0.123, c.fancyValue);
		}

		[TestMethod, TestCategory("reflection")]
		public void TypesInNamespace()
		{
			List<Type> types = ReflectionUtil.TypesInNamespace(assembly, "UltimateUtil.Test");
			Assert.IsTrue(types.Contains(typeof(Program)), "Does not have Program.");
			types.Remove(typeof(Program));

			Assert.IsTrue(types.All((t) => t.FullName.Contains("_Test")), "Not all contain _Test");
		}

		[TestMethod, TestCategory("reflection")]
		public void InheritsFrom()
		{
			Type a = typeof(A);
			Type b = typeof(B);
			Type c = typeof(C);

			Assert.IsTrue(b.InheritsFrom(a));
			Assert.IsFalse(c.InheritsFrom(a));
			Assert.IsFalse(a.InheritsFrom(b));
		}

		[TestMethod, TestCategory("reflection")]
		public void IsNullable()
		{
			Type b = typeof(bool);
			Type bn = typeof(bool?);
			Type s = typeof(string);

			Assert.IsFalse(b.IsNullableType());
			Assert.IsTrue(bn.IsNullableType());
			Assert.IsFalse(s.IsNullableType());
		}

		[TestMethod, TestCategory("reflection")]
		public void PrivateAccess()
		{
			Type t = typeof(A);
			A a = new A();

			object obj = t.GetPrivateField("secret", a);
			Assert.IsTrue(obj is string);
			Assert.AreEqual("shh", obj as string);

			t.SetPrivateField("secret", "what", a);
			obj = t.GetPrivateField("secret", a);
			Assert.AreEqual("what", obj as string);

			obj = t.InvokePrivateMethod("manipulate", a);
			Assert.IsNull(obj);
			Assert.AreEqual(-4, a.value);
		}

		[Thing]
		public class A
		{
			[Thing]
			public int value;

			private string secret = "shh";

			public A()
			{ value = 4; }

			public A(int v)
			{ value = v; }

			[Thing]
			public int PlusTwo()
			{ return value + 2; }

			public int MinusTwo()
			{ return value - 2; }

			public string AppendTypeName<T>(T thing)
			{
				Type t = typeof(T);
				if (thing is C)
				{
					C c = thing as C;
					c.fancyValue = 0.123;
				}

				return value.ToString() + "_" + t.Name;
			}

			private void manipulate()
			{
				value = -value;
			}
		}

		public class B : A
		{
			[Thing]
			public string stuff = "mmph";
		}

		[Thing]
		public class C
		{
			public double fancyValue = 3.14;
		}

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method |
			AttributeTargets.Parameter | AttributeTargets.Field, Inherited = false)]
		public class ThingAttribute : Attribute
		{
		}
	}
}
