using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Registries
{
	/// <summary>
	/// Base implementation of <see cref="IDynamicRegistry{T}"/>, useable for
	/// simple registries that must have items registered manually.
	/// </summary>
	/// <typeparam name="T">Type of registered items</typeparam>
	public class DynamicRegistry<T> : IDynamicRegistry<T>
		where T : class, IRegisterable
	{
		/// <summary>
		/// Internal dictionary where registered items are stored
		/// </summary>
		protected IDictionary<string, T> registry
		{ get; set; }

		/// <summary>
		/// Gets a collection of all registered items in the registry
		/// </summary>
		public IEnumerable<T> Items
		{ get { return registry.Values; } }

		/// <summary>
		/// Gets an item from the registry by key.
		/// </summary>
		/// <param name="key">Key the item is registered in</param>
		/// <returns>Item registered under <paramref name="key"/>.</returns>
		/// <exception cref="KeyNotFoundException">
		/// Thrown if there is nothing registered under <paramref name="key"/>.
		/// </exception>
		public T this[string key]
		{
			get
			{
				if (!registry.ContainsKey(key))
				{
					throw new KeyNotFoundException("No key found by name of '{0}'.".Fmt(key));
				}

				return registry[key];
			}
		}

		/// <summary>
		/// Instantiates a new instance of the dynamic registry. Initializes internal dictionary.
		/// </summary>
		public DynamicRegistry()
		{
			registry = new Dictionary<string, T>();
		}

		/// <summary>
		/// Adds an item to the registry under the given key.
		/// </summary>
		/// <param name="key">Key of item to register under</param>
		/// <param name="item">Item to register</param>
		/// <exception cref="ArgumentException">
		/// Thrown if there is already an item registered under <paramref name="key"/>.
		/// </exception>
		public void Register(string key, T item)
		{
			if (registry.ContainsKey(key))
			{
				throw new ArgumentException("Key '{0}' already exists.".Fmt(key), nameof(key));
			}

			registry.Add(key, item);
		}
		/// <summary>
		/// Adds an item to the registry under the given key, if there is nothing
		/// registered under the key.
		/// </summary>
		/// <param name="key">Key of item to register under</param>
		/// <param name="item">Item to register</param>
		/// <returns>
		/// <c>true</c> if registry was successful, <c>false</c> if a key already exists.
		/// </returns>
		public bool RegisterIfNeeded(string key, T item)
		{
			try
			{
				Register(key, item);
				return true;
			}
			catch (ArgumentException)
			{
				return false;
			}
		}

		/// <summary>
		/// Method for removing an item from the <see cref="IDynamicRegistry{T}"/>.
		/// </summary>
		/// <param name="key">Key of item to be removed.</param>
		/// <returns>
		/// <c>true</c> if item was found and removed, false if <paramref name="key"/> was
		/// not found, or the item could not be removed.
		/// </returns>
		public bool Unregister(string key)
		{
			return registry.Remove(key);
		}

		/// <summary>
		/// Gets the key for a given object in the registry, the long way.
		/// (Do not simply return <c><paramref name="value"/>.RegistryName</c>.)
		/// </summary>
		/// <param name="value">Item whose key is to be searched for.</param>
		/// <returns>
		/// The key under which <paramref name="value"/> is registered, 
		/// or <c>null</c> if none is found.
		/// </returns>
		public string GetKeyOf(T value)
		{
			foreach (KeyValuePair<string, T> kvp in registry)
			{
				if (kvp.Value.Equals(value) || kvp.Value == value)
				{
					return kvp.Key;
				}
			}

			return null;
		}

		/// <summary>
		/// Iterates through each item in the registry easily through lambda expressions.
		/// </summary>
		/// <param name="action">Action to apply within <c>foreach</c> loop</param>
		public void ForEach(Action<string, T> action)
		{
			foreach (KeyValuePair<string, T> kvp in registry)
			{
				action(kvp.Key, kvp.Value);
			}
		}

		/// <summary>
		/// Iterates through each item in the registry via lambda expressions, with the option to break.
		/// </summary>
		/// <param name="action">
		/// Action to apply within <c>foreach</c> loop. Return <c>true</c> to break out of
		/// the loop, <c>false</c> to merely continue.
		/// </param>
		public void ForEach(Func<string, T, bool> action)
		{
			foreach (KeyValuePair<string, T> kvp in registry)
			{
				if (action(kvp.Key, kvp.Value))
				{
					break;
				}
			}
		}
	}
}
