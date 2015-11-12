using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class ConversionUtil
	{
		public static T To<T>(this IConvertible obj)
		{
			return (T)Convert.ChangeType(obj, typeof(T));
		}

		public static bool TryTo<T>(this IConvertible obj, out T result)
		{
			try
			{
				result = obj.To<T>();
				return true;
			}
			catch
			{
				result = default(T);
				return false;
			}
		}

		public static T ToOrValue<T>(this IConvertible obj, T val)
		{
			T res;
			if (!obj.TryTo<T>(out res))
			{
				res = val;
			}

			return res;
		}
		public static T ToOrDefault<T>(this IConvertible obj)
		{
			return obj.ToOrValue(default(T));
		}
	}
}
