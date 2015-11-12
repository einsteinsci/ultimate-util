using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Registries
{
	public class DynamicRegistry<TValue> : IDynamicRegistry<TValue>
		where TValue : class, IRegisterable
	{
		public IDictionary<string, TValue> Registry
		{ get; protected set; }

		public TValue this[string key]
		{
			get
			{
				return getItem(key);
			}
		}

		/// <summary>
		/// Internal method called by both indexers.
		/// </summary>
		/// <param name="key">Key of item in registry</param>
		/// <returns>Item registered under <paramref name="key"/></returns>
		protected virtual TValue getItem(string key)
		{
			if (Registry.ContainsKey(key))
			{
				return Registry[key];
			}

			return null;
		}

		public virtual string GetKeyOf(TValue value)
		{
			return Registry.FirstOrDefault((kvp) => kvp.Value == value).Key;
		}
		public virtual void Register(string key, TValue item)
		{
			key.ThrowIfNullOrEmpty(nameof(key));
			item.ThrowIfNull(nameof(item));

			Registry.Add(key, item);
		}
		public virtual bool UnRegister(string key)
		{
			return Registry.Remove(key);
		}

		#region IDynamicRegistry
		void IDynamicRegistry.Register(string key, IRegisterable item)
		{
			Register(key, item as TValue);
		}

		string IDynamicRegistry.GetKeyOf(IRegisterable value)
		{
			return GetKeyOf(value as TValue);
		}

		IRegisterable IDynamicRegistry.this[string key]
		{
			get
			{
				return getItem(key);
			}
		}
		#endregion IDynamicRegistry
	}
}
