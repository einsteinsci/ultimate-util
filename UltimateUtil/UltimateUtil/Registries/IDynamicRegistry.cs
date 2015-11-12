using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Registries
{
	public interface IDynamicRegistry
	{
		IRegisterable this[string key]
		{ get; }

		void Register(string key, IRegisterable item);
		bool UnRegister(string key);

		string GetKeyOf(IRegisterable value);
	}

	public interface IDynamicRegistry<T> : IDynamicRegistry 
		where T : IRegisterable
	{
		new T this[string key]
		{ get; }

		void Register(string key, T item);

		string GetKeyOf(T value);
	}
}
