using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class EnumUtils
	{
		public static string GetDescription<T>(this T value) where T : struct
		{
			Type t = typeof(T);
			if (!t.InheritsFrom(typeof(Enum)))
			{
				throw new ArgumentException("Type " + t.ToString() + " is not an enum.");
			}

			FieldInfo field = t.GetField(value.ToString());

			EnumDescriptionAttribute att = field.GetCustomAttribute<EnumDescriptionAttribute>();

			if (att == null)
			{
				return value.ToString();
			}
			else
			{
				return att.Description;
			}
		}

		public static List<T> GetAllValues<T>() where T : struct
		{
			Type t = typeof(T);
			if (!t.InheritsFrom(typeof(Enum)))
			{
				throw new ArgumentException("Type " + t.ToString() + " is not an enum.");
			}

			FieldInfo[] fields = t.GetFields();
			List<T> res = new List<T>();
			foreach (FieldInfo f in fields)
			{
				res.Add((T)f.GetValue(null));
			}

			return res;
		}
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class EnumDescriptionAttribute : Attribute
	{
		public string Description
		{ get; private set; }

		public EnumDescriptionAttribute(string description)
		{
			Description = description;
		}

		public EnumDescriptionAttribute()
		{ }
	}
}
