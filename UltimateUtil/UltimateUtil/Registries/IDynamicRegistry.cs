using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Registries
{
	/// <summary>
	/// Base interface for dynamic registries of a given type
	/// </summary>
	/// <typeparam name="T">
	/// Type of items stored in the registry; must implement <see cref="IRegisterable"/>.
	/// </typeparam>
	public interface IDynamicRegistry<T>
		where T : IRegisterable
	{
		/// <summary>
		/// Gets all items registered, without keys. Used for <c>foreach</c>.
		/// </summary>
		IEnumerable<T> Items
		{ get; }

		/// <summary>
		/// Indexer for retrieving an item from the registry.
		/// </summary>
		/// <param name="key">Key identifying the registered object</param>
		/// <returns><see cref="T"/> registered under <paramref name="key"/></returns>
		T this[string key]
		{ get; }

		/// <summary>
		/// Method for adding an item to the <see cref="IDynamicRegistry{T}"/>.
		/// </summary>
		/// <param name="key">Key to register under. Usually <see cref="T.RegistryName"/>.</param>
		/// <param name="item">Item to register.</param>
		void Register(string key, T item);

		/// <summary>
		/// Method for removing an item from the <see cref="IDynamicRegistry{T}"/>.
		/// </summary>
		/// <param name="key">Key of item to be removed.</param>
		/// <returns>
		/// <c>true</c> if item was found and removed, false if <paramref name="key"/> was
		/// not found, or the item could not be removed.
		/// </returns>
		bool Unregister(string key);

		/// <summary>
		/// Gets the key for a given object in the registry, the long way.
		/// (Do not return <c><paramref name="value"/>.RegistryName</c>.)
		/// </summary>
		/// <param name="value">Item whose key is to be searched for.</param>
		/// <returns>
		/// The key under which <paramref name="value"/> is registered, 
		/// or <c>null</c> if none is found.
		/// </returns>
		string GetKeyOf(T value);
	}
}
