using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	/// <summary>
	/// Various utilities that involve <see cref="Enum"/> types
	/// </summary>
	public static class EnumUtils
	{
		/// <summary>
		/// Gets a description string specified in an <see cref="EnumDescriptionAttribute"/>
		/// applied to the enum value
		/// </summary>
		/// <typeparam name="T">Enum in which to search</typeparam>
		/// <param name="value">Value of enum</param>
		/// <returns>The description applied to <see cref="value"/></returns>
		public static string GetDescription<T>(this T value) where T : struct
		{
			Type t = typeof(T);
			if (!t.InheritsFrom<Enum>())
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

		/// <summary>
		/// Gets all members of an enum via reflection
		/// </summary>
		/// <typeparam name="T">Enum to search</typeparam>
		/// <returns>A <see cref="List{T}"/> of all members in <typeparamref name="T"/></returns>
		public static List<T> GetAllValues<T>() where T : struct
		{
			Type t = typeof(T);
			if (!t.InheritsFrom<Enum>())
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

		/// <summary>
		/// Parses a <see cref="string"/> into a given enum, with the option to ignore case
		/// </summary>
		/// <typeparam name="T">Enum to parse to</typeparam>
		/// <param name="str"><see cref="string"/> to parse</param>
		/// <param name="ignoreCase">Whether to ignore case when parsing</param>
		/// <returns>The value of the enum that <paramref name="str"/> matches</returns>
		public static T ToEnum<T>(this string str, bool ignoreCase = false) where T : struct
		{
			Type t = typeof(T);
			if (!t.InheritsFrom<Enum>())
			{
				throw new ArgumentException("Type " + t.ToString() + " is not an enum.");
			}

			str.ThrowIfNull(nameof(str));
			
			if (str == "")
			{
				throw new ArgumentException("Input string cannot be empty.");
			}

			return (T)Enum.Parse(t, str, ignoreCase);
		}
	}

	/// <summary>
	/// Applies a description to an enum member
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class EnumDescriptionAttribute : Attribute
	{
		/// <summary>
		/// Description applied to the member
		/// </summary>
		public string Description
		{ get; private set; }

		/// <summary>
		/// Applies a description to an enum member
		/// </summary>
		/// <param name="description">Description applied to member</param>
		public EnumDescriptionAttribute(string description)
		{
			Description = description;
		}
	}
}
