using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Registries
{
	public class ReflectiveRegistry<TValue, TAtt> : DynamicRegistry<TValue>
		where TValue : class, IRegisterable
		where TAtt : Attribute
	{
		public ReflectiveRegistry(bool autoLoad = false)
		{
			Registry = new Dictionary<string, TValue>();

			if (autoLoad)
			{
				Load(Assembly.GetCallingAssembly());
			}
		}

		public void Load()
		{
			Load(Assembly.GetCallingAssembly());
		}
		public virtual void Load(Assembly assembly)
		{
			foreach (Type t in assembly.GetTypesWithAttribute<TAtt>())
			{
				if (t.InheritsFrom<TValue>())
				{
					TValue inst = Activator.CreateInstance(t) as TValue;
					if (inst != null)
					{
						Register(inst.RegistryName, inst);
					}
				}
			}
		}
	}
}
