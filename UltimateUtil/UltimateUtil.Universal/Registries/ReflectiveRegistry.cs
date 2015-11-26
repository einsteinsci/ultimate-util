using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Registries
{
	/// <summary>
	/// Extension of <see cref="DynamicRegistry{T}"/> with the ability to load
	/// items via reflection with attributes.
	/// </summary>
	/// <typeparam name="TValue">Base class of items registered</typeparam>
	/// <typeparam name="TAtt">Attribute applied to classes to be registered</typeparam>
	public class ReflectiveRegistry<TValue, TAtt> : DynamicRegistry<TValue>
		where TValue : class, IRegisterable
		where TAtt : Attribute
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ReflectiveRegistry{TValue, TAtt}"/>,
		/// with the option to load all items via reflection immediately.
		/// </summary>
		/// <param name="exampleType">
		/// Type whose assembly to auto-load from. Leave <c>null</c> to not auto-load.
		/// </param>
		public ReflectiveRegistry(Type exampleType = null)
		{
			registry = new Dictionary<string, TValue>();

			if (exampleType != null)
			{
				Load(exampleType);
			}
		}

		/// <summary>
		/// Loads all items applied with <typeparamref name="TAtt"/> within a given
		/// <see cref="Assembly"/> via reflection
		/// </summary>
		/// <param name="assembly">Assembly from where to search for registered classes</param>
		public virtual void Load(Assembly assembly)
		{
			foreach (Type t in assembly.GetTypesWithAttribute<TAtt>())
			{
				TryRegisterType(t);
			}
		}
		/// <summary>
		/// Loads all items applied with <typeparamref name="TAtt"/> within the
		/// <see cref="Assembly"/> of a given type, via reflection
		/// </summary>
		/// <param name="exampleType">Type whose assembly to load</param>
		public void Load(Type exampleType)
		{
			Load(exampleType.GetTypeInfo().Assembly);
		}

		/// <summary>
		/// Tries to register a <see cref="Type"/> that inherits from <typeparamref name="TValue"/>,
		/// using <see cref="Activator.CreateInstance(Type)"/>.
		/// </summary>
		/// <param name="t">Type of item to register</param>
		/// <returns>
		/// <c>true</c> if registry was successful, <c>false</c> if there was something 
		/// already registered under the key, or if <paramref name="t"/> does not inherit 
		/// from <typeparamref name="TValue"/>.
		/// </returns>
		public virtual bool TryRegisterType(Type t)
		{
			if (t.InheritsFrom<TValue>())
			{
				TValue inst = Activator.CreateInstance(t) as TValue;
				if (inst != null)
				{
					try
					{
						Register(inst.RegistryName, inst);
						return true;
					}
					catch (ArgumentException)
					{
						return false;
					}
				}
			}

			return false;
		}
		/// <summary>
		/// Tries to register a <see cref="Type"/> that inherits from <typeparamref name="TValue"/>,
		/// using <see cref="Activator.CreateInstance(Type)"/>.
		/// </summary>
		/// <typeparam name="TReg">Type of item to register</typeparam>
		/// <returns>
		/// <c>true</c> if registry was successful, <c>false</c> if there was something already
		/// registered under the key.
		/// </returns>
		public bool TryRegisterType<TReg>() where TReg : TValue
		{
			return TryRegisterType(typeof(TReg));
		}

		/// <summary>
		/// Retrieves the <see cref="Type"/> of the item registered under <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Key of item to search for</param>
		/// <returns>The <see cref="Type"/> of the item, if found.</returns>
		public Type GetRegisteredType(string key)
		{
			if (!registry.ContainsKey(key))
			{
				throw new KeyNotFoundException("No key found by name of {0}.".Fmt(key));
			}

			return this[key].GetType();
		}
	}
}
