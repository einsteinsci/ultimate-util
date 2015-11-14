using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Registries
{
	/// <summary>
	/// Interface for objects registered into an <see cref="IDynamicRegistry"/>,
	/// which are usually singletons.
	/// See also <seealso cref="IDynamicRegistry{T}"/>
	/// </summary>
	public interface IRegisterable
	{
		/// <summary>
		/// Name of registered object, used as its key in the registry
		/// </summary>
		string RegistryName
		{ get; }
	}
}
