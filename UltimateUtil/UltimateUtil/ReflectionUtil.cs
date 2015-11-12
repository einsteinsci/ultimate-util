using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class ReflectionUtil
	{
		public static bool HasAttribute(this MemberInfo tested, Type attribute)
		{
			return tested.GetCustomAttributes(attribute, false).Length > 0;
		}
		public static bool HasAttribute<TAtt>(this MemberInfo tested)
		{
			return tested.HasAttribute(typeof(TAtt));
		}

		/// <summary>
		/// Creates a single-argument delegate value from a generic single-argument method
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
		/// Creates a single-argument delegate object from a generic single-argument method
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

		/// <summary>
		/// Invokes an instance method by name
		/// </summary>
		/// <param name="obj">Object instance to invoke the method from. Cannot be null.</param>
		/// <param name="methodName">Name of method to invoke.</param>
		/// <param name="parameters">Parameters to supply the method</param>
		/// <returns>Return value of the method</returns>
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

		public static object GetPrivateField(this Type type, string fieldName, object instance = null)
		{
			FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			return field.GetValue(instance);
		}
		public static void SetPrivateField(this Type type, string fieldName, object value, object instance = null)
		{
			FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			field.SetValue(instance, value);
		}
		public static object RunPrivateMethod(this Type type, string methodName, object instance, params object[] args)
		{
			MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
			return method.Invoke(instance, args);
		}
	}
}
