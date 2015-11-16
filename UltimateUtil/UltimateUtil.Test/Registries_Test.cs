using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateUtil.Registries;

namespace UltimateUtil.Test
{
	[TestClass]
	public class Registries_Test
	{
		[TestMethod, TestCategory("registries")]
		public void DynamicRegistry_()
		{
			DynamicRegistry<IRegItem> reg = new DynamicRegistry<IRegItem>();
			A a = new A();
			B b = new B();
			C c = new C();

			reg.Register(a.RegistryName, a);
			reg.Register(b.RegistryName, b);
			reg.Register(c.RegistryName, c);

			IRegItem iri = reg["b"];
			Assert.IsNotNull(iri);
			Assert.AreEqual(2, iri.value());

			reg.Unregister(b.RegistryName);
			iri = reg.Items.FirstOrDefault((i) => i.RegistryName == "b");
			Assert.IsNull(iri);
		}

		[TestMethod, TestCategory("registries")]
		public void ReflectiveRegistry_()
		{
			ReflectiveRegistry<IRegItem, RegAttribute> reg = new ReflectiveRegistry<IRegItem, RegAttribute>();
			reg.Load();

			IRegItem iri = reg["b"];
			Assert.IsNotNull(iri);
			Assert.AreEqual(2, iri.value());

			reg.Unregister(iri.RegistryName);
			iri = reg.Items.FirstOrDefault((i) => i.RegistryName == "b");
			Assert.IsNull(iri);
		}

		[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
		public class RegAttribute : Attribute
		{ }

		public interface IRegItem : IRegisterable
		{
			int value();
		}

		[Reg]
		public class A : IRegItem
		{
			public string RegistryName => "a";

			public int value()
			{
				return 1;
			}
		}

		[Reg]
		public class B : IRegItem
		{
			public string RegistryName => "b";

			public int value()
			{
				return 2;
			}
		}

		[Reg]
		public class C : IRegItem
		{
			public string RegistryName => "c";

			public int value()
			{
				return 3;
			}
		}
	}
}
