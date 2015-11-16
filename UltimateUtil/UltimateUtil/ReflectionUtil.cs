using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UltimateUtil.Fluid;

namespace UltimateUtil
{
	/// <summary>
	/// Various utilities for reflection
	/// </summary>
	public static class ReflectionUtil
	{
		/// <summary>
		/// Returns whether a member has a given attribute
		/// </summary>
		/// <param name="tested">Member to test</param>
		/// <param name="attribute">Type of attribute to look for</param>
		/// <returns>
		/// <c>true</c> if <paramref name="tested"/> has <paramref name="attribute"/> applied to it, 
		/// <c>false</c> if not, or if <paramref name="attribute"/> is not a valid <see cref="Attribute"/>
		/// </returns>
		public static bool HasAttribute(this MemberInfo tested, Type attribute)
		{
			return tested.GetCustomAttributes(attribute, false).Length > 0;
		}
		/// <summary>
		/// Returns whether a member has a given attribute
		/// </summary>
		/// <typeparam name="TAtt">Type of attribute to look for</typeparam>
		/// <param name="tested">Member to test</param>
		/// <returns>
		/// <c>true</c> if <paramref name="tested"/> has <typeparamref name="TAtt"/> applied to it, 
		/// <c>false</c> if not
		/// </returns>
		public static bool HasAttribute<TAtt>(this MemberInfo tested) where TAtt : Attribute
		{
			return tested.HasAttribute(typeof(TAtt));
		}

		/// <summary>
		/// Returns a list of all types in an assembly that have a given attribute applied to them
		/// </summary>
		/// <param name="assem">Assembly to search</param>
		/// <param name="attribute">Type of attribute to look for</param>
		/// <returns>
		/// A list of all types within <paramref name="assem"/> that have <paramref name="attribute"/>
		/// applied to them
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Thrown if <paramref name="attribute"/> does not inherit from <see cref="Attribute"/>
		/// </exception>
		public static List<Type> GetTypesWithAttribute(this Assembly assem, Type attribute)
		{
			if (!attribute.InheritsFrom<Attribute>())
			{
				throw new ArgumentException("Type {0} must inherit from System.Attribute.", nameof(attribute));
			}

			List<Type> res = new List<Type>();
			foreach (Type t in assem.GetTypes())
			{
				if (t.HasAttribute(attribute))
				{
					res.Add(t);
				}
			}

			return res;
		}
		/// <summary>
		/// Returns a list of all types in an assembly that have a given attribute applied to them
		/// </summary>
		/// <typeparam name="TAtt">Type of attribute to look for</typeparam>
		/// <param name="assem">Assembly to search</param>
		/// <returns>
		/// A list of all types within <paramref name="assem"/> that have a <typeparamref name="TAtt"/>
		/// applied to them
		/// </returns>
		public static List<Type> GetTypesWithAttribute<TAtt>(this Assembly assem) where TAtt : Attribute
		{
			return assem.GetTypesWithAttribute(typeof(TAtt));
		}

		/// <summary>
		/// Returns a list of all members in a type that have a given attribute applied to them
		/// </summary>
		/// <param name="type">Type within which to search</param>
		/// <param name="att">Type of attribute to look for</param>
		/// <param name="filters">Types of members to search for</param>
		/// <returns>
		/// A list of all members within <paramref name="type"/> that have <paramref name="att"/> applied
		/// to them
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Thrown if <paramref name="att"/> does not inherit from <see cref="Attribute"/>
		/// </exception>
		public static List<MemberInfo> GetMembersWithAttribute(this Type type, Type att, MemberTypes filters = MemberTypes.All)
		{
			if (!att.InheritsFrom<Attribute>())
			{
				throw new ArgumentException("Type {0} must inherit from System.Attribute.", nameof(att));
			}

			List<MemberInfo> res = new List<MemberInfo>();
			foreach (MemberInfo m in type.GetMembers())
			{
				if (filters.HasFlag(m.MemberType) && m.HasAttribute(att))
				{
					res.Add(m);
				}
			}

			return res;
		}
		/// <summary>
		/// Returns a list of all members in a type that have a given attribute applied to them
		/// </summary>
		/// <typeparam name="TAtt">Type of attribute to look for</typeparam>
		/// <param name="type">Type within which to search</param>
		/// <param name="filters">Types of members to search for</param>
		/// <returns>
		/// A list of all members within <paramref name="type"/> that have <typeparamref name="TAtt"/>
		/// applied to them
		/// </returns>
		public static List<MemberInfo> GetMembersWithAttribute<TAtt>(this Type type, MemberTypes filters = MemberTypes.All)
			where TAtt : Attribute
		{
			return type.GetMembersWithAttribute(typeof(TAtt), filters);
		}

#if GENERIC_DELEGATE_FACTORY
		/// <summary>
		/// Creates a single-argument delegate value from a generic single-argument method.
		/// Not entirely sure how it works, or how to use it.
		/// </summary>
		/// <typeparam name="T">Argument type of the method</typeparam>
		/// <param name="target">Instance of object to create the delegate from</param>
		/// <param name="method">Method within <paramref name="target"/> to create</param>
		/// <returns>A delegate value from the method</returns>
		public static Action<T> CreateDelegate<T>(object target, MethodInfo method)
		{
			Action<T> del = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target, method);

			return del;
		}
		/// <summary>
		/// Creates a single-argument delegate object from a generic single-argument method. Useful when
		/// supplying functional types to generic methods. Not entirely sure how it works.
		/// </summary>
		/// <param name="genericType">Generic type to supply the method</param>
		/// <param name="target">Object instance from which to create the method from. Use null for static methods.</param>
		/// <param name="method">Method to use to create the generic method</param>
		/// <returns>The single-argument delegate form of <paramref name="method"/></returns>
		public static object CreateDelegateGeneric(Type genericType, object target, MethodInfo method)
		{

			MethodInfo createDelegate = typeof(ReflectionUtil).GetMethod(
				nameof(CreateDelegate)).MakeGenericMethod(genericType);

			object del = createDelegate.Invoke(null, new object[] { target, method });

			return del;
		}
#endif

		/// <summary>
		/// Invokes an instance method by string name. Useful for reflected types.
		/// </summary>
		/// <param name="obj">Object instance to invoke the method from. Cannot be null.</param>
		/// <param name="methodName">Name of method to invoke.</param>
		/// <param name="parameters">Parameters to supply the method</param>
		/// <returns>Return value of the method</returns>
		/// <exception cref="ArgumentException">
		/// Thrown if <paramref name="obj"/> is <c>null</c> (indicating a static method), or if
		/// there is no method found by the name of <paramref name="methodName"/>
		/// </exception>
		public static object InvokeMethod(this object obj, string methodName, params object[] parameters)
		{
			if (obj == null)
			{
				throw new ArgumentException("Cannot invoke static methods from here.");
			}

			Type t = obj.GetType();
			MethodInfo method = t.GetMethod(methodName);
			if (method == null)
			{
				throw new ArgumentException("Method not found: " + methodName);
			}

			return method.Invoke(obj, parameters);
		}

		/// <summary>
		/// Invokes a generic method with the given type parameters and method parameters
		/// </summary>
		/// <param name="method">Method to invoke</param>
		/// <param name="instance">Object from which to invoke the method from. Use null if the method is static.</param>
		/// <param name="typeParams">Type parameters when creating the generic method.</param>
		/// <param name="parameters">Value parameters to supply the method.</param>
		/// <returns>The return value of the invoked method</returns>
		public static object InvokeGeneric(this MethodInfo method, object instance, 
			Type[] typeParams, object[] parameters)
		{
			MethodInfo generic = method.MakeGenericMethod(typeParams);
			return generic.Invoke(instance, parameters);
		}

		/// <summary>
		/// Returns a list of all types in a namespace string
		/// </summary>
		/// <param name="assembly">Assembly to search</param>
		/// <param name="ns">Namespace string to look for</param>
		/// <returns>List of all types that are in <paramref name="ns"/>.</returns>
		public static List<Type> TypesInNamespace(Assembly assembly, string ns)
		{
			List<Type> res = new List<Type>();
			foreach (Type t in assembly.GetTypes())
			{
				if (t.Namespace == ns)
				{
					res.Add(t);
				}
			}

			return res;
		}

		/// <summary>
		/// This makes more sense than <see cref="Type.IsAssignableFrom(Type)"/>.
		/// </summary>
		/// <param name="inheriting">Type that is to be tested to be the subclass</param>
		/// <param name="inherited">Type that is to be tested to be the base class</param>
		/// <returns>
		/// <c>true</c> if <paramref name="inheriting"/> inherits from 
		/// <paramref name="inherited"/>, <c>false</c> if not.
		/// </returns>
		public static bool InheritsFrom(this Type inheriting, Type inherited)
		{
			return inherited.IsAssignableFrom(inheriting);
		}

		public static bool InheritsFrom<TBase>(this Type inheriting)
		{
			return inheriting.InheritsFrom(typeof(TBase));
		}

		/// <summary>
		/// Returns whether or not the specified type is <see cref="Nullable{T}"/>.
		/// </summary>
		/// <param name="type">A <see cref="Type"/>.</param>
		/// <returns>True if the specified type is <see cref="Nullable{T}"/>; otherwise, false.</returns>
		/// <remarks>Use <see cref="Nullable.GetUnderlyingType"/> to access the underlying type.</remarks>
		public static bool IsNullableType(this Type type)
		{
			type.ThrowIfNull(nameof(type));

			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

		/// <summary>
		/// Gets the value stored in a private field in a type
		/// </summary>
		/// <param name="type">Type the field is in</param>
		/// <param name="fieldName">Name of field</param>
		/// <param name="instance">Instance of which to get the field from, <c>null</c> if <c>static</c></param>
		/// <returns>The value stored in <paramref name="fieldName"/></returns>
		public static object GetPrivateField(this Type type, string fieldName, object instance = null)
		{
			FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			return field.GetValue(instance);
		}
		/// <summary>
		/// Sets the value stored in a private field in a type
		/// </summary>
		/// <param name="type">Type the field is in</param>
		/// <param name="fieldName">Name of field</param>
		/// <param name="value">Value to set to. Must inherit from the field's type.</param>
		/// <param name="instance">Instance of which to get the field from, <c>null</c> if <c>static</c></param>
		/// <exception cref="ArgumentException">
		/// Thrown if <paramref name="value"/> does not inherit from the field's type.
		/// </exception>
		public static void SetPrivateField(this Type type, string fieldName, object value, object instance = null)
		{
			FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			if (!value.GetType().InheritsFrom(field.FieldType))
			{
				throw new ArgumentException("Type {0} does not inherit from {1}."
					.Fmt(value.GetType().FullName, field.FieldType.FullName), nameof(value));
			}

			field.SetValue(instance, value);
		}

		/// <summary>
		/// Invokes a private method within a type
		/// </summary>
		/// <param name="type">Type the method is in</param>
		/// <param name="methodName">Name of method</param>
		/// <param name="instance">Instance of which to get the method from, or <c>null</c> if <c>static</c></param>
		/// <param name="args">Parameters to supply the method</param>
		/// <returns>The return value of the method, or null if it returns <see cref="void"/>.</returns>
		public static object InvokePrivateMethod(this Type type, string methodName, object instance, params object[] args)
		{
			MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
			return method.Invoke(instance, args);
		}
	}
}
